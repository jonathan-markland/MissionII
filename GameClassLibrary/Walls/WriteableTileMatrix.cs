using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Walls
{
    public class WriteableTileMatrix
    {
        private Tile[] _theArray;
        private ArrayView2D<Tile> _theMatrix;



        public WriteableTileMatrix(int tileCountH, int tileCountV)
        {
            System.Diagnostics.Debug.Assert(tileCountH >= 0);
            System.Diagnostics.Debug.Assert(tileCountV >= 0);
            System.Diagnostics.Debug.Assert(tileCountH < 10000);
            System.Diagnostics.Debug.Assert(tileCountV < 10000);

            _theArray = new Tile[tileCountH * tileCountV];
            _theMatrix = new ArrayView2D<Tile>(_theArray, tileCountH);
        }



        public int CountH { get { return _theMatrix.CountH; } }
        public int CountV { get { return _theMatrix.CountV; } }
        public Tile At(int x, int y) { return _theMatrix.At(x, y); }
        public ArrayView2D<Tile> WholeArea { get { return _theMatrix; } } 



        public void Write(Point p, Tile ch)
        {
            Write(p.X, p.Y, ch);
        }



        public void Write(int x, int y, Tile ch)
        {
            if (x >= 0 && x < _theMatrix.CountH)
            {
                if (y >= 0 && y < _theMatrix.CountV)
                {
                    _theArray[y * _theMatrix.CountH + x] = ch;
                    return;
                }
            }
            throw new Exception("WriteableTileMatrix class write outside bounds.");
        }
    }
}