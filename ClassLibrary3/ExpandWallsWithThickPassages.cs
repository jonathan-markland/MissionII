
namespace GameClassLibrary
{
    public static class ExpandWallsWithThickPassages
    {
        public static WallMatrix ExpandWalls(WallMatrix sourceMatrix)
        {
            // 789       78889
            // 456 ----> 45556
            // 123       45556
            //           45556
            //           12223

            var destMatrix = new WallMatrix(
                Constants.ClustersHorizontally * Constants.DestClusterSide,
                Constants.ClustersVertically * Constants.DestClusterSide);

            for (int y = 0; y < Constants.ClustersVertically; ++y)
            {
                for (int x = 0; x < Constants.ClustersHorizontally; ++x)
                {
                    ExpandCluster(sourceMatrix, x, y, destMatrix);
                }
            }

            return destMatrix;
        }



        private static void ExpandCluster(
            WallMatrix sourceMatrix, int x, int y, WallMatrix destMatrix)
        {
            // NB: Order is significant

            SidePiece(sourceMatrix, destMatrix, x, y, 2,  0, -1, 8); // B -> 222  ABOVE  N Q
            SidePiece(sourceMatrix, destMatrix, x, y, 8,  0, +1, 2); // H -> 888  BELOW  N K
            SidePiece(sourceMatrix, destMatrix, x, y, 4, -1,  0, 6); // D -> 444  LEFT   N O
            SidePiece(sourceMatrix, destMatrix, x, y, 6,  1,  0, 4); // F -> 666  RIGHT  N M

            CentrePiece(sourceMatrix, destMatrix, x, y);

            CornerPiece(destMatrix, x, y, 1, 2, 4); // 1 = 2 | 4
            CornerPiece(destMatrix, x, y, 3, 2, 6); // 3 = 2 | 6
            CornerPiece(destMatrix, x, y, 7, 4, 8); // 7 = 4 | 8
            CornerPiece(destMatrix, x, y, 9, 6, 8); // 9 = 6 | 8
        }



        private static void SidePiece(WallMatrix sourceMatrix, WallMatrix destMatrix, int x, int y, int targetSide, int dx, int dy, int joinSide)
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
             *  - Otherwise 2 is filled ...
             *    ... except if the 3x3 above exists AND has filled(N) & filled(Q) & !filled(E)
             *        then 2 can be unfilled.
             *       (respectively for the other sides, 4, 6, 8).
             */

            var srcClusterCanvas = new ClusterCanvas(sourceMatrix, x, y, Constants.SourceClusterSide);
            var dstClusterCanvas = new ClusterCanvas(destMatrix, x, y, Constants.DestClusterSide);

            if (srcClusterCanvas.IsSpace(targetSide)) // If B is unfilled, 
            {
                dstClusterCanvas.Paint(targetSide, false); // 2 will be unfilled.
            }
            else
            {
                var otherX = x + dx;
                var otherY = y + dy;

                if (otherX >= 0 && otherY >= 0 
                    && otherX < Constants.ClustersHorizontally
                    && otherY < Constants.ClustersVertically)   // except if the 3x3 above exists
                {
                    var srcOtherClusterCanvas = new ClusterCanvas(sourceMatrix, otherX, otherY, Constants.SourceClusterSide);
                    dstClusterCanvas.Paint(targetSide,
                        ! (srcOtherClusterCanvas.IsWall(joinSide)
                        && srcOtherClusterCanvas.IsWall(5)
                        && !srcClusterCanvas.IsWall(5)));  // AND has filled(N) & filled(Q) & !filled(E) then 2 can be unfilled.
                }
                else
                {
                    dstClusterCanvas.Paint(targetSide, true);  // Otherwise 2 is filled ...
                }
            }
        }



        private static void CentrePiece(WallMatrix sourceMatrix, WallMatrix destMatrix, int x, int y)
        {
            // The level designer specified whether the centres are filled.

            var dstClusterCanvas = new ClusterCanvas(destMatrix, x, y, Constants.DestClusterSide);
            var srcClusterCanvas = new ClusterCanvas(sourceMatrix, x, y, Constants.SourceClusterSide);

            dstClusterCanvas.Paint(5, srcClusterCanvas.IsWall(5));
        }



        private static void CornerPiece(WallMatrix destMatrix, int x, int y, int targetCorner, int adjacentSide1, int adjacentSide2)
        {
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

            var dstClusterCanvas = new ClusterCanvas(destMatrix, x, y, Constants.DestClusterSide);

            dstClusterCanvas.Paint(targetCorner,
                dstClusterCanvas.IsWall(adjacentSide1)
                || dstClusterCanvas.IsWall(adjacentSide2));
        }
    }
}
