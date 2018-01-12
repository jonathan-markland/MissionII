using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public static class ExpandWallsWithThinPassages
    {
        private const int ExpandSize = LevelExpander.ExpandSize;



        public static WallMatrix ExpandWalls(WallMatrix sourceMatrix)
        {
            // 789       78889
            // 456 ----> 45556
            // 123       45556
            //           45556
            //           12223

            var destMatrix = new WallMatrix(
                Constants.ClustersHorizonally * ExpandSize,
                Constants.ClustersVertically * ExpandSize);

            int destY = 0;

            for (int sourceY = 0; sourceY < Constants.SourceFileCharsVertically; sourceY += Constants.ClusterSide)
            {
                int destX = 0;

                for (int sourceX = 0;
                    sourceX < Constants.SourceFileRoomCharsHorizontally;
                    sourceX += Constants.ClusterSide)
                {
                    PaintExpandThreeByThreeToFiveByFive(sourceMatrix, destMatrix, sourceX, sourceY + 0, destX, destY + 0);
                    PaintExpandThreeByThreeToFiveByFive(sourceMatrix, destMatrix, sourceX, sourceY + 1, destX, destY + 1);
                    PaintExpandThreeByThreeToFiveByFive(sourceMatrix, destMatrix, sourceX, sourceY + 1, destX, destY + 2);
                    PaintExpandThreeByThreeToFiveByFive(sourceMatrix, destMatrix, sourceX, sourceY + 1, destX, destY + 3);
                    PaintExpandThreeByThreeToFiveByFive(sourceMatrix, destMatrix, sourceX, sourceY + 2, destX, destY + 4);
                    destX += ExpandSize;
                }

                destY += ExpandSize;
            }

            return destMatrix;
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
    }
}
