using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public static class LevelExpander
    {
        private const int ExpandSize = 5;


        public static void ExpandWallsInWorld(WorldWallData theWorld)
        {
            foreach(var thisLevel in theWorld.Levels)
            {
                foreach(var thisRoom in thisLevel.Rooms)
                {
                    var expandedData = ExpandWalls(thisRoom.FileWallData);
                    // CarveWiderRoutes(expandedData);
                    thisRoom.WallData = expandedData;
                }
            }
        }

        public static WallMatrix ExpandWalls(WallMatrix sourceMatrix)
        {
            // 789       78889
            // 456 ----> 45556
            // 123       45556
            //           45556
            //           12223

            var resultMatrix = new WallMatrix(
                Constants.ClustersHorizonally * ExpandSize,
                Constants.ClustersVertically * ExpandSize);

            var innerWallChar = new WallMatrixChar { WallChar = '@' };

            int destY = 0;

            for(int sourceY=0; sourceY < Constants.SourceFileCharsVertically; sourceY += Constants.ClusterSide)
            {
                int destX = 0;

                for (int sourceX = 0;
                    sourceX < Constants.SourceFileRoomCharsHorizontally;
                    sourceX += Constants.ClusterSide)
                {
                    if (ThreeByThreeIsFullyWall(sourceMatrix, sourceX, sourceY))
                    {
                        PaintFiveByFiveFullyWall(resultMatrix, destX, destY, innerWallChar);
                    }
                    else
                    {
                        PaintExpandThreeByThreeToFiveByFive(sourceMatrix, resultMatrix, sourceX, sourceY + 0, destX, destY + 0);
                        PaintExpandThreeByThreeToFiveByFive(sourceMatrix, resultMatrix, sourceX, sourceY + 1, destX, destY + 1);
                        PaintExpandThreeByThreeToFiveByFive(sourceMatrix, resultMatrix, sourceX, sourceY + 1, destX, destY + 2);
                        PaintExpandThreeByThreeToFiveByFive(sourceMatrix, resultMatrix, sourceX, sourceY + 1, destX, destY + 3);
                        PaintExpandThreeByThreeToFiveByFive(sourceMatrix, resultMatrix, sourceX, sourceY + 2, destX, destY + 4);
                    }
                    destX += ExpandSize;
                }

                destY += ExpandSize;
            }

            return resultMatrix;
        }

        private static void PaintFiveByFiveFullyWall(WallMatrix resultMatrix, int destX, int destY, WallMatrixChar wallChar)
        {
            for(int y=0; y < ExpandSize; y++)
            {
                for (int x = 0; x < ExpandSize; x++)
                {
                    resultMatrix.Write(destX + x, destY + y, wallChar);
                }
            }
        }

        private static bool ThreeByThreeIsFullyWall(WallMatrix sourceMatrix, int x, int y)
        {
            return ThreeAdjacentAreWall(sourceMatrix, x, y+0)
                && ThreeAdjacentAreWall(sourceMatrix, x, y+1)
                && ThreeAdjacentAreWall(sourceMatrix, x, y+2);
        }

        private static bool ThreeAdjacentAreWall(WallMatrix sourceMatrix, int x, int y)
        {
            return sourceMatrix.Read(x+0, y).Wall
                && sourceMatrix.Read(x+1, y).Wall
                && sourceMatrix.Read(x+2, y).Wall;
        }

        public static void PaintExpandThreeByThreeToFiveByFive(
            WallMatrix sourceWallData, WallMatrix destMatrix, 
            int sourceX, int sourceY,
            int destX, int destY)
        {
            var c1 = RemapChar(sourceWallData.Read(sourceX, sourceY));
            var c2 = RemapChar(sourceWallData.Read(sourceX + 1, sourceY));
            var c3 = RemapChar(sourceWallData.Read(sourceX + 2, sourceY));
            destMatrix.Write(destX + 0, destY, c1);
            destMatrix.Write(destX + 1, destY, c2);
            destMatrix.Write(destX + 2, destY, c2);
            destMatrix.Write(destX + 3, destY, c2);
            destMatrix.Write(destX + 4, destY, c3);
        }



        public static WallMatrixChar RemapChar(WallMatrixChar c)
        {
            return new WallMatrixChar { WallChar = (c.Space ? ' ' : '#' ) };
        }



        public static void CarveWiderRoutes(WallMatrix expandedData)
        {
            // TODO: The CarveRun calls could be randomly omitted for artistic effect
            for (int i = 0; i < 25; i++)
            {
                CarveRun(expandedData, i, 0, 0, 1, 25);
                CarveRun(expandedData, i, 24, 0, -1, 25);
            }
            for (int i = 0; i < 25; i++)
            {
                CarveRun(expandedData, 0, i, 1, 0, 25);
                CarveRun(expandedData, 24, i, -1, 0, 25);
            }

            // Carving invalidates the door thickness matching 
            // (potentially), but it will only be because the outermost row
            // is WIDER than inner ones, in a given room.

            int thickCount = ExpandSize;
            CarveDoorIfPresent(expandedData, new Point(0,  0), new MovementDeltas(1, 0), new MovementDeltas( 0, 1), thickCount);
            CarveDoorIfPresent(expandedData, new Point(0, 24), new MovementDeltas(1, 0), new MovementDeltas( 0,-1), thickCount);
            CarveDoorIfPresent(expandedData, new Point(0,  0), new MovementDeltas(0, 1), new MovementDeltas( 1, 0), thickCount);
            CarveDoorIfPresent(expandedData, new Point(24, 0), new MovementDeltas(0, 1), new MovementDeltas(-1, 0), thickCount);
        }



        private static void CarveDoorIfPresent(WallMatrix expandedData, Point point, MovementDeltas withinRow, MovementDeltas betweenRows, int thickCount)
        {
            int x = point.X;
            int y = point.Y;
            for(int i=0; i<25; i++)
            {
                if (expandedData.Read(x,y).Space)
                {
                    CarveDoorHole(expandedData, x, y, betweenRows, thickCount);
                }
                x += withinRow.dx;
                y += withinRow.dy;
            }
        }



        private static void CarveDoorHole(WallMatrix expandedData, int x, int y, MovementDeltas moveDelta, int thickCount)
        {
            while (thickCount > 0)
            {
                expandedData.Write(x, y, new WallMatrixChar { WallChar = ' ' });
                x += moveDelta.dx;
                y += moveDelta.dy;
                --thickCount;
            }
        }



        public static void CarveRun(WallMatrix wallData, int x1, int y1, int dx, int dy, int c)
        {
            var spaceChar = new WallMatrixChar { WallChar = ' ' }; // TODO: sort out constructor
            int runSize = 0;
            while (c > 0)
            {
                var block1 = wallData.Read(x1, y1);
                if (block1.Space)
                {
                    if (runSize > 3) // TODO: This constant could be parameterised for different effects.
                    {
                        wallData.Write(x1 - dx, y1 - dy, spaceChar);
                    }
                    runSize = 0;
                }
                else
                {
                    ++runSize;
                }
                x1 += dx;
                y1 += dy;
                --c;
            }
        }


    }
}
