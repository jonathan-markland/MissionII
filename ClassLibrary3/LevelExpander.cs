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
                PaintExpandedRow(sourceMatrix, resultMatrix, sourceY+0, destY+0);
                PaintExpandedRow(sourceMatrix, resultMatrix, sourceY+1, destY+1);
                PaintExpandedRow(sourceMatrix, resultMatrix, sourceY+1, destY+2);
                PaintExpandedRow(sourceMatrix, resultMatrix, sourceY+1, destY+3);
                PaintExpandedRow(sourceMatrix, resultMatrix, sourceY+2, destY+4);
                destY += ExpandSize;
            }

            return resultMatrix;
        }



        public static void PaintExpandedRow(WallMatrix sourceWallData, WallMatrix destMatrix, int sourceY, int destY)
        {
            int destX = 0;

            for ( int sourceX=0; sourceX < Constants.SourceFileCharsHorizontally; sourceX += Constants.ClusterSide )
            {
                var c1 = RemapChar(sourceWallData.Read(sourceX, sourceY));
                var c2 = RemapChar(sourceWallData.Read(sourceX + 1, sourceY));
                var c3 = RemapChar(sourceWallData.Read(sourceX + 2, sourceY));
                destMatrix.Write(destX + 0, destY, c1);
                destMatrix.Write(destX + 1, destY, c2);
                destMatrix.Write(destX + 2, destY, c2);
                destMatrix.Write(destX + 3, destY, c2);
                destMatrix.Write(destX + 4, destY, c3);
                destX += ExpandSize;
            }
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
