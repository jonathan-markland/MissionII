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
                    thisRoom.WallData = ExpandWalls(thisRoom.FileWallData);
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


       /* public static List<string> WidenPassages(List<string> list)
        {
            var height = list.Count;
            var width = list[0].Length;

            for (int y = 1; y < (height-1); y++)
            {
                for (int x = 1; x < (width-1); x++)
                {

                }
            }
        }*/


    }
}
