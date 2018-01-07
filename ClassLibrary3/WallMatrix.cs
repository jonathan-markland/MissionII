using System;

namespace GameClassLibrary
{
    public class WallMatrix
    {
        private WallMatrixChar[] _wallData;
        private int _blockCountH;
        private int _blockCountV;

        public WallMatrix(int blockCountH, int blockCountV)
        {
            _wallData = new WallMatrixChar[blockCountH*blockCountV];
            _blockCountH = blockCountH;
            _blockCountV = blockCountV;
        }

        public int CountH { get { return _blockCountH; } }
        public int CountV { get { return _blockCountV; } }

        public bool Empty { get { return _blockCountH == 0 || _blockCountV == 0; } }

        public WallMatrixChar Read(int x, int y)
        {
            if (x >= 0 && x < _blockCountH)
            {
                if (y >=0 && y < _blockCountV)
                {
                    return _wallData[y * _blockCountH + x];
                }
            }
            throw new Exception("WallMatrix class read outside bounds.");
        }

        public void Write(int x, int y, WallMatrixChar ch)
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

        public WallMatrix Clone()
        {
            var newMatrix = new WallMatrix(_blockCountH, _blockCountV);
            var i = 0;
            foreach(var ch in _wallData)
            {
                newMatrix._wallData[i] = ch;
            }
            return newMatrix;
        }
    }

    public struct WallMatrixChar // TODO: refactor with constructor.
    {
        public bool Space { get { return WallChar == ' '; } }
        public bool Wall { get { return WallChar != ' '; } }
        public bool Wall1 { get { return WallChar == '#'; } }
        public bool Wall2 { get { return WallChar == '@'; } }
        public char WallChar;
    }
}
