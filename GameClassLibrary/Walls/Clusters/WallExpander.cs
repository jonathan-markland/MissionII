
namespace GameClassLibrary.Walls.Clusters
{
    /// <summary>
    /// In a room tile map consisting of n*m 3*3 tile clusters,
    /// this returns an equivalent map with the tiles expanded to 5*5.
    /// </summary>
    public struct WallExpander
    {
        private int _clustersHorizontally;
        private int _clustersVertically;
        private int _sourceClusterSide;
        private int _destClusterSide;
        private WallMatrix _sourceMatrix;
        private WriteableWallMatrix _destMatrix;



        public WallExpander(
            WallMatrix sourceMatrix,
            int clustersHorizontally, int clustersVertically,
            int sourceClusterSide, int destClusterSide)
        {
            _sourceMatrix = sourceMatrix;
            _destMatrix = null;
            _clustersHorizontally = clustersHorizontally;
            _clustersVertically = clustersVertically;
            _sourceClusterSide = sourceClusterSide;
            _destClusterSide = destClusterSide;
        }



        public static void Paint(WriteableClusterCanvas dstClusterCanvas, int areaCode, bool paintWall)
        {
            dstClusterCanvas.Paint(areaCode, paintWall ? WallMatrixChar.Electric : WallMatrixChar.Space);
        }



        public WriteableWallMatrix GetExpandedWalls()
        {
            // 789       78889
            // 456 ----> 45556
            // 123       45556
            //           45556
            //           12223

            var destMatrix = new WriteableWallMatrix(
                _clustersHorizontally * _destClusterSide,
                _clustersVertically * _destClusterSide);

            _destMatrix = destMatrix;

            for (int y = 0; y < _clustersVertically; ++y)
            {
                for (int x = 0; x < _clustersHorizontally; ++x)
                {
                    ExpandCluster(x, y, destMatrix);
                }
            }

            return destMatrix;
        }



        private void ExpandCluster(int x, int y, WriteableWallMatrix destMatrix)
        {
            // NB: Order is significant

            SidePiece(destMatrix, x, y, 2,  0, -1, 8); // B -> 222  ABOVE  N Q
            SidePiece(destMatrix, x, y, 8,  0, +1, 2); // H -> 888  BELOW  N K
            SidePiece(destMatrix, x, y, 4, -1,  0, 6); // D -> 444  LEFT   N O
            SidePiece(destMatrix, x, y, 6,  1,  0, 4); // F -> 666  RIGHT  N M

            CentrePiece(destMatrix, x, y);

            CornerPiece(destMatrix, x, y, 1, 2, 4); // 1 = 2 | 4
            CornerPiece(destMatrix, x, y, 3, 2, 6); // 3 = 2 | 6
            CornerPiece(destMatrix, x, y, 7, 4, 8); // 7 = 4 | 8
            CornerPiece(destMatrix, x, y, 9, 6, 8); // 9 = 6 | 8
        }



        private void SidePiece(WriteableWallMatrix destMatrix, int x, int y, int targetSide, int dx, int dy, int joinSide)
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

            var srcClusterCanvas = new ClusterReader(_sourceMatrix, x, y, _sourceClusterSide);
            var dstClusterCanvas = new WriteableClusterCanvas(destMatrix, x, y, _destClusterSide);

            if (srcClusterCanvas.IsSpace(targetSide)) // If B is unfilled, 
            {
                Paint(dstClusterCanvas, targetSide, false); // 2 will be unfilled.
            }
            else
            {
                var otherX = x + dx;
                var otherY = y + dy;

                if (otherX >= 0 && otherY >= 0 
                    && otherX < _clustersHorizontally
                    && otherY < _clustersVertically)   // except if the 3x3 above exists
                {
                    var srcOtherClusterCanvas = new ClusterReader(_sourceMatrix, otherX, otherY, _sourceClusterSide);
                    Paint(dstClusterCanvas, targetSide,
                        ! (srcOtherClusterCanvas.IsWall(joinSide)
                        && srcOtherClusterCanvas.IsWall(5)
                        && !srcClusterCanvas.IsWall(5)));  // AND has filled(N) & filled(Q) & !filled(E) then 2 can be unfilled.
                }
                else
                {
                    Paint(dstClusterCanvas, targetSide, true);  // Otherwise 2 is filled ...
                }
            }
        }



        private void CentrePiece(WriteableWallMatrix destMatrix, int x, int y)
        {
            // The level designer specified whether the centres are filled.

            var dstClusterCanvas = new WriteableClusterCanvas(destMatrix, x, y, _destClusterSide);
            var srcClusterCanvas = new ClusterReader(_sourceMatrix, x, y, _sourceClusterSide);

            Paint(dstClusterCanvas, 5, srcClusterCanvas.IsWall(5));
        }



        private void CornerPiece(
            WriteableWallMatrix destMatrix, int x, int y, int targetCorner, 
            int adjacentSide1, int adjacentSide2)
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

            var dstClusterCanvas = new WriteableClusterCanvas(destMatrix, x, y, _destClusterSide);

            Paint(dstClusterCanvas, targetCorner,
                dstClusterCanvas.IsWall(adjacentSide1)
                || dstClusterCanvas.IsWall(adjacentSide2));
        }
    }
}
