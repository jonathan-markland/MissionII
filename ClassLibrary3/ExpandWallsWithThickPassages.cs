using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public static class ExpandWallsWithThickPassages
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
                    ExpandCluster(sourceMatrix, sourceX, sourceY, destMatrix, destX, destY);
                    destX += ExpandSize;
                }

                destY += ExpandSize;
            }

            return destMatrix;
        }



        private static void ExpandCluster(
            WallMatrix sourceMatrix, int sourceX, int sourceY, 
            WallMatrix destMatrix, int destX, int destY)
        {
            /*
             *  JKL
             *  MNO
             *  PQR
             *        12223
             *  ABC   45556
             *  DEF   45556
             *  GHI   45556
             *        78889
             *
             *
             *  Filled(2)=
             *  - If B is unfilled, 2 will be unfilled.
             *  - Otherwise B is filled ...
             *    ... except if the 3x3 above exists AND has filled(N) & filled(Q)
             *        then 2 can be unfilled.
             *       (respectively for the other sides, 4, 6, 8).
             */

            SidePiece(sourceMatrix, destMatrix, 1, 0, 1, 0, 3, 1, 0, -1, 1, 2); // B -> 222  ABOVE  N Q
            SidePiece(sourceMatrix, destMatrix, 1, 2, 1, 4, 3, 1, 0, +1, 1, 0); // H -> 888  BELOW  N K
            SidePiece(sourceMatrix, destMatrix, 0, 1, 0, 1, 1, 3, -1, 0, 2, 1); // D -> 444  LEFT   N O
            SidePiece(sourceMatrix, destMatrix, 2, 1, 4, 1, 1, 3, +1, 0, 0, 1); // F -> 666  RIGHT  N M

            /*
             *   Evaluate the trivial cases AFTER the above
             *   
             *   Filled(5) = Filled(2) & Filled(4) & Filled(6) & Filled(8)
             */

            CentrePiece(sourceMatrix, destMatrix); // 2, 0, 0, 2, 4, 2, 2, 4);

            /*
             *   12223
             *   45556 
             *   45556 
             *   45556 
             *   78889 
             *
             *   Filled(1) = Filled(2) | Filled(4)
             *   ...resp. for 3, 7, 9
             */

            CornerPiece(sourceMatrix, destMatrix, 0, 0, 2, 0, 0, 2); // 1 = 2 | 4
            CornerPiece(sourceMatrix, destMatrix, 4, 0, 2, 0, 4, 2); // 3 = 2 | 6
            CornerPiece(sourceMatrix, destMatrix, 0, 4, 0, 2, 2, 4); // 7 = 4 | 8
            CornerPiece(sourceMatrix, destMatrix, 4, 4, 4, 2, 2, 4); // 9 = 6 | 8
        }



        private static void SidePiece(
            WallMatrix sourceMatrix, WallMatrix destMatrix, 
            int sourceSideX, int sourceSideY, 
            int destSideX, int destSideY, int destSideWidth, int destSideHeight, 
            int deltaToAdjacentClusterX, int deltaToAdjacentClusterY, 
            int adjacentClusterX, int adjacentClusterY)
        {
            // B -> 222  ABOVE  N Q



        }



        private static void CentrePiece(WallMatrix sourceMatrix, WallMatrix destMatrix)
        {

        }



        private static void CornerPiece(
            WallMatrix sourceMatrix, WallMatrix destMatrix,
            int cornerX, int cornerY,
            int side1X, int side1Y,
            int size2X, int size2Y)
        {

        }



    }
}
