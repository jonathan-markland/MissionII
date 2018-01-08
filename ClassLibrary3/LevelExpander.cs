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
                    CarveWiderRoutes(expandedData);
                    SecondBrickIze(expandedData);
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

            int destY = 0;

            for(int sourceY=0; sourceY < Constants.SourceFileCharsVertically; sourceY += Constants.ClusterSide)
            {
                int destX = 0;

                for (int sourceX = 0;
                    sourceX < Constants.SourceFileRoomCharsHorizontally;
                    sourceX += Constants.ClusterSide)
                {
                    PaintExpandThreeByThreeToFiveByFive(sourceMatrix, resultMatrix, sourceX, sourceY + 0, destX, destY + 0);
                    PaintExpandThreeByThreeToFiveByFive(sourceMatrix, resultMatrix, sourceX, sourceY + 1, destX, destY + 1);
                    PaintExpandThreeByThreeToFiveByFive(sourceMatrix, resultMatrix, sourceX, sourceY + 1, destX, destY + 2);
                    PaintExpandThreeByThreeToFiveByFive(sourceMatrix, resultMatrix, sourceX, sourceY + 1, destX, destY + 3);
                    PaintExpandThreeByThreeToFiveByFive(sourceMatrix, resultMatrix, sourceX, sourceY + 2, destX, destY + 4);
                    destX += ExpandSize;
                }

                destY += ExpandSize;
            }

            return resultMatrix;
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
            // All the non-space areas are flagged as Electric when expanding.
            return (c == WallMatrixChar.Space) 
                ? WallMatrixChar.Space 
                : WallMatrixChar.Electric;
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
                if (expandedData.Read(x,y) == WallMatrixChar.Space)
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
                expandedData.Write(x, y, WallMatrixChar.Space);
                x += moveDelta.dx;
                y += moveDelta.dy;
                --thickCount;
            }
        }



        public static void CarveRun(WallMatrix wallData, int x1, int y1, int dx, int dy, int c)
        {
            int runSize = 0;
            while (c > 0)
            {
                var block1 = wallData.Read(x1, y1);
                if (block1 == WallMatrixChar.Space)
                {
                    if (runSize > 3) // TODO: This constant could be parameterised for different effects.
                    {
                        wallData.Write(x1 - dx, y1 - dy, WallMatrixChar.Space);
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



        private static void SecondBrickIze(WallMatrix expandedData)
        {
            // Turn Electric areas into Brick leaving just an Electric outline.

            for (int y = 1; y < 24; ++y)
            {
                for (int x = 1; x < 24; ++x)
                {
                    if (SurroundedByWall8(expandedData, x, y))
                    {
                        expandedData.Write(x, y, WallMatrixChar.Brick);
                    }
                }
            }
        }



        private static bool SurroundedByWall4(WallMatrix wallMatrix, int x, int y) // TODO: We could be arty and call this instead.
        {
            return
                   wallMatrix.Read(x, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x, y + 1) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y) != WallMatrixChar.Space;
        }



        private static bool SurroundedByWall8(WallMatrix wallMatrix, int x, int y)
        {
            return
                   wallMatrix.Read(x, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x, y + 1) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y + 1) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y + 1) != WallMatrixChar.Space;
        }
    }
}
