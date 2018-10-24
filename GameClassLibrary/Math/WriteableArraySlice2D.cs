﻿using System;

namespace GameClassLibrary.Math
{
    public struct WriteableArraySlice2D<T>
    {
        private T[] _theArray;
        private ArraySlice2D<T> _theMatrix;



        public WriteableArraySlice2D(int tileCountH, int tileCountV)
        {
            System.Diagnostics.Debug.Assert(tileCountH >= 0);
            System.Diagnostics.Debug.Assert(tileCountV >= 0);
            System.Diagnostics.Debug.Assert(tileCountH < 10000);
            System.Diagnostics.Debug.Assert(tileCountV < 10000);

            _theArray = new T[tileCountH * tileCountV];
            _theMatrix = new ArraySlice2D<T>(_theArray, tileCountH);
        }



        public int CountH { get { return _theMatrix.CountH; } }
        public int CountV { get { return _theMatrix.CountV; } }
        public T At(int x, int y) { return _theMatrix.At(x, y); }
        public ArraySlice2D<T> WholeArea { get { return _theMatrix; } } 



        public void Write(Point p, T ch)
        {
            Write(p.X, p.Y, ch);
        }



        public void Write(int x, int y, T ch)
        {
            if (x >= 0 && x < _theMatrix.CountH)
            {
                if (y >= 0 && y < _theMatrix.CountV)
                {
                    _theArray[_theMatrix.OriginArrayOffset + y * _theMatrix.CountH + x] = ch;
                    return;
                }
            }

            throw new Exception("WriteableTileMatrix class write outside bounds.");
        }
    }
}