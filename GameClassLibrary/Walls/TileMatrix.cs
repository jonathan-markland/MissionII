using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Walls
{
    /// <summary>
    /// Tile matrix for a room.  (Immutable).
    /// </summary>
    public class TileMatrix
    {
        protected Tile[] _wallData;
        protected byte[] _styleDeltas;
        protected int _blockCountH;
        protected int _blockCountV;

        public TileMatrix(int blockCountH, int blockCountV)
        {
            var n = blockCountH * blockCountV;
            _wallData = new Tile[n];
            _styleDeltas = new byte[n];
            _blockCountH = blockCountH;
            _blockCountV = blockCountV;
        }

        public int CountH { get { return _blockCountH; } }
        public int CountV { get { return _blockCountV; } }

        public bool Empty { get { return _blockCountH == 0 || _blockCountV == 0; } }

        public Tile Read(Point p)
        {
            return Read(p.X, p.Y);
        }

        public Tile Read(int x, int y)
        {
            if (x >= 0 && x < _blockCountH)
            {
                if (y >= 0 && y < _blockCountV)
                {
                    return _wallData[y * _blockCountH + x];
                }
            }
            throw new Exception("WallMatrix class read outside bounds.");
        }

        public Tile Read(int x, int y, Tile defaultIfOutsideBounds)
        {
            if (x >= 0 && x < _blockCountH)
            {
                if (y >= 0 && y < _blockCountV)
                {
                    return _wallData[y * _blockCountH + x];
                }
            }
            return defaultIfOutsideBounds;
        }

        public byte GetStyleDelta(int x, int y)
        {
            if (x >= 0 && x < _blockCountH)
            {
                if (y >= 0 && y < _blockCountV)
                {
                    return _styleDeltas[y * _blockCountH + x];
                }
            }
            throw new Exception("WallMatrix class read outside bounds.");
        }
    }
}