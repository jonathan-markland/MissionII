using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Walls
{
    public class WriteableWallMatrix : TileMatrix
    {
        public WriteableWallMatrix(int blockCountH, int blockCountV)
            : base(blockCountH, blockCountV)
        {
        }



        public void Write(Point p, Tile ch)
        {
            Write(p.X, p.Y, ch);
        }



        public void Write(int x, int y, Tile ch)
        {
            if (x >= 0 && x < _blockCountH)
            {
                if (y >= 0 && y < _blockCountV)
                {
                    _wallData[y * _blockCountH + x] = ch;
                    return;
                }
            }
            throw new Exception("WallMatrix class write outside bounds.");
        }



        public void SetStyleDelta(int x, int y, byte styleDelta)
        {
            if (x >= 0 && x < _blockCountH)
            {
                if (y >= 0 && y < _blockCountV)
                {
                    _styleDeltas[y * _blockCountH + x] = styleDelta;
                    return;
                }
            }
            throw new Exception("WallMatrix class write outside bounds.");
        }
    }
}