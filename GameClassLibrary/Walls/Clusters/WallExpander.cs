
using System;

namespace GameClassLibrary.Walls.Clusters
{
    /// <summary>
    /// In a room tile map, each cluster of 9 tiles (3x3), is expanded
    /// to a cluster of 25 (5x5).
    /// </summary>
    public struct WallExpander
    {
        private int _clustersHorizontally;
        private int _clustersVertically;
        private int _sourceClusterSide;
        private int _destClusterSide;
        private TileMatrix _sourceMatrix;
        private WriteableTileMatrix _destMatrix;
        private Tile _outputFloorTile;
        private Tile _outputWallTile;
        private Func<Tile, bool> _isFloorFunc;



        public WallExpander(
            TileMatrix sourceMatrix,
            int clustersHorizontally, int clustersVertically,
            int sourceClusterSide, int destClusterSide,
            Func<Tile, bool> isFloorFunc,
            Tile outputWallTile,
            Tile outputFloorTile)
        {
            _sourceMatrix = sourceMatrix;
            _destMatrix = null;
            _clustersHorizontally = clustersHorizontally;
            _clustersVertically = clustersVertically;
            _sourceClusterSide = sourceClusterSide;
            _destClusterSide = destClusterSide;
            _isFloorFunc = isFloorFunc;
            _outputFloorTile = outputFloorTile;
            _outputWallTile = outputWallTile;
        }



        public void Paint(WriteableClusterCanvas dstClusterCanvas, int areaCode, bool paintWall)
        {
            dstClusterCanvas.Paint(areaCode, paintWall ? _outputWallTile : _outputFloorTile);
        }



        public WriteableTileMatrix GetExpandedWalls()
        {
            // 789       78889
            // 456 ----> 45556
            // 123       45556
            //           45556
            //           12223

            var destMatrix = new WriteableTileMatrix(
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



        private void ExpandCluster(int x, int y, WriteableTileMatrix destMatrix)
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



        private void SidePiece(WriteableTileMatrix destMatrix, int x, int y, int targetSide, int dx, int dy, int joinSide)
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

            var srcClusterCanvas = new ClusterReader(_sourceMatrix, x, y, _sourceClusterSide, _isFloorFunc);
            var dstClusterCanvas = new WriteableClusterCanvas(destMatrix, x, y, _destClusterSide, _isFloorFunc);

            if (srcClusterCanvas.IsFloor(targetSide)) // If B is unfilled, 
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
                    var srcOtherClusterCanvas = new ClusterReader(_sourceMatrix, otherX, otherY, _sourceClusterSide, _isFloorFunc);
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



        private void CentrePiece(WriteableTileMatrix destMatrix, int x, int y)
        {
            // The level designer specified whether the centres are filled.

            var dstClusterCanvas = new WriteableClusterCanvas(destMatrix, x, y, _destClusterSide, _isFloorFunc);
            var srcClusterCanvas = new ClusterReader(_sourceMatrix, x, y, _sourceClusterSide, _isFloorFunc);

            Paint(dstClusterCanvas, 5, srcClusterCanvas.IsWall(5));
        }



        private void CornerPiece(
            WriteableTileMatrix destMatrix, int x, int y, int targetCorner, 
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

            var dstClusterCanvas = new WriteableClusterCanvas(destMatrix, x, y, _destClusterSide, _isFloorFunc);

            Paint(dstClusterCanvas, targetCorner,
                dstClusterCanvas.IsWall(adjacentSide1)
                || dstClusterCanvas.IsWall(adjacentSide2));
        }
    }
}
