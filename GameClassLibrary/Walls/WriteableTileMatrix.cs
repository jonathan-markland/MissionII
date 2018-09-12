using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Walls
{
    public class WriteableTileMatrix : TileMatrix
    {
        public WriteableTileMatrix(int tileCountH, int tileCountV, int tileWidth, int tileHeight)
            : base(tileCountH, tileCountV, tileWidth, tileHeight)
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
    }
}