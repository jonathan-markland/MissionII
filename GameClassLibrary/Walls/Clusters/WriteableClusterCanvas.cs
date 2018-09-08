using System;

namespace GameClassLibrary.Walls.Clusters
{
    /// <summary>
    /// A "canvas" that supports writing to 3x3 clusters.
    /// </summary>
    public class WriteableClusterCanvas : ClusterReader
    {
        // Area codes:
        // 789       
        // 456 
        // 123       

        private WriteableTileMatrix _writeableWallMatrix;



        public WriteableClusterCanvas(
            WriteableTileMatrix wallMatrix, int clusterIndexX, int clusterIndexY, 
            int clusterSide, Func<Tile, bool> isFloorFunc)
                : base(wallMatrix, clusterIndexX, clusterIndexY, clusterSide, isFloorFunc)
        {
            _writeableWallMatrix = wallMatrix;
        }



        public void Paint(int areaCode, Tile paintChar)
        {
            var e = _endOffset;
            var s = _innerLength;

            if (areaCode == 1) Paint(0, 0, 1, 1, paintChar);
            else if (areaCode == 3) Paint(e, 0, 1, 1, paintChar);
            else if (areaCode == 7) Paint(0, e, 1, 1, paintChar);
            else if (areaCode == 9) Paint(e, e, 1, 1, paintChar);
            else if (areaCode == 5) Paint(1, 1, s, s, paintChar);
            else if (areaCode == 2) Paint(1, 0, s, 1, paintChar);
            else if (areaCode == 4) Paint(0, 1, 1, s, paintChar);
            else if (areaCode == 6) Paint(e, 1, 1, s, paintChar);
            else if (areaCode == 8) Paint(1, e, s, 1, paintChar);
            else throw new Exception($"ClusterCanvas.Paint() error:  '{areaCode}' is not a valid area code.");
        }



        private void Paint(int x, int y, int w, int h, Tile paintChar)
        {
            // TODO:  Could assert area fits.  Not massively important since Write() does bounds checking.

            x += _originX;
            y += _originY;
            var e = x + w;
            var f = y + h;

            while (y < f)
            {
                while (x < e)
                {
                    _writeableWallMatrix.Write(x, y, paintChar);
                    ++x;
                }
                x -= w;
                ++y;
            }
        }
    }
}
