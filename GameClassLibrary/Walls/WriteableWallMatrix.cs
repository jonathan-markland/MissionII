using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Walls
{
    public class WriteableTileMatrix : TileMatrix
    {
        public WriteableTileMatrix(int blockCountH, int blockCountV)
            : base(blockCountH, blockCountV)
        {
        }



        public void Write(Point p, Tile ch)
        {
            Write(p.X, p.Y, ch);
        }



        public void Write(int x, int y, Tile ch)
        {
            if (x >= 0 && x < _countH)
            {
                if (y >= 0 && y < _countV)
                {
                    _theTiles[y * _countH + x] = ch;
                    return;
                }
            }
            throw new Exception("TileMatrix class write outside bounds.");
        }



        public void SetStyleDelta(int x, int y, byte styleDelta)
        {
            if (x >= 0 && x < _countH)
            {
                if (y >= 0 && y < _countV)
                {
                    _styleDeltas[y * _countH + x] = styleDelta;
                    return;
                }
            }
            throw new Exception("TileMatrix class write outside bounds.");
        }
    }
}