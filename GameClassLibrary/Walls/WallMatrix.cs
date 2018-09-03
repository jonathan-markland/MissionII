using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Walls
{
    public class WallMatrix
    {
        protected WallMatrixChar[] _wallData;
        protected byte[] _styleDeltas;
        protected int _blockCountH;
        protected int _blockCountV;

        public WallMatrix(int blockCountH, int blockCountV)
        {
            var n = blockCountH * blockCountV;
            _wallData = new WallMatrixChar[n];
            _styleDeltas = new byte[n];
            _blockCountH = blockCountH;
            _blockCountV = blockCountV;
        }

        public int CountH { get { return _blockCountH; } }
        public int CountV { get { return _blockCountV; } }

        public bool Empty { get { return _blockCountH == 0 || _blockCountV == 0; } }

        public WallMatrixChar Read(Point p)
        {
            return Read(p.X, p.Y);
        }

        public WallMatrixChar Read(int x, int y)
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

        public WallMatrixChar Read(int x, int y, WallMatrixChar defaultIfOutsideBounds)
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