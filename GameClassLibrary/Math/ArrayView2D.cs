using System;

namespace GameClassLibrary.Math
{
    public struct ArrayView2D<T>
    {
        private T[] _theArray;
        private int _originOffset;
        private int _elemCountH;
        private int _elemCountV;
        private int _rowStrafe;



        /// <summary>
        /// Construct ArrayView2D to cover the whole area of the array.
        /// </summary>
        public ArrayView2D(T[] theArray, int countH)
        {
            if (countH != 0)
            {
                if (theArray.Length % countH != 0)
                {
                    throw new Exception("ArrayView2D array length does not match proposed row width");
                }
                _theArray = theArray;
                _originOffset = 0;
                _rowStrafe = countH;
                _elemCountH = countH;
                _elemCountV = theArray.Length / countH;
            }
            else
            {
                _theArray = theArray;
                _originOffset = 0;
                _rowStrafe = 0;
                _elemCountH = 0;
                _elemCountV = 0;
            }
        }



        /// <summary>
        /// Construct ArrayView2D that is sub-area of another ArrayView2D.
        /// </summary>
        /// <param name="otherView">The other array view.</param>
        /// <param name="x">Legal location within other array view.</param>
        /// <param name="y">Legal location within other array view.</param>
        /// <param name="viewWidth">Width of desired sub-view, allowing 0.</param>
        /// <param name="viewHeight">Height of desired sub-view, allowing 0.</param>
        public ArrayView2D(ArrayView2D<T> otherView, int x, int y, int viewWidth, int viewHeight)
        {
            // TODO: clipping validation.  Revert to 0*0 if disjoint.
            _theArray = otherView._theArray;
            _rowStrafe = otherView._rowStrafe;
            _originOffset = otherView._originOffset + y * _rowStrafe + x;
            _elemCountH = viewWidth;
            _elemCountV = viewHeight;
        }



        /// <summary>
        /// The number of elements horizontally in this view.
        /// </summary>
        public int CountH { get { return _elemCountH; } }

        /// <summary>
        /// The number of elements vertically in this view.
        /// </summary>
        public int CountV { get { return _elemCountV; } }

        /// <summary>
        /// Query if the matrix has no size.
        /// </summary>
        public bool Empty { get { return _elemCountH == 0 || _elemCountV == 0; } }

        /// <summary>
        /// Return the element at the position given.
        /// </summary>
        public T At(int x, int y)
        {
            if (x >= 0 && x < _elemCountH)
            {
                if (y >= 0 && y < _elemCountV)
                {
                    var i = _originOffset + y * _rowStrafe + x;
                    return _theArray[i];
                }
            }
            throw new Exception("ArrayView2D class read outside bounds of view.");
        }

        /// <summary>
        /// Return the element at the position given.
        /// </summary>
        public T At(Point p)
        {
            return At(p.X, p.Y);
        }

        /// <summary>
        /// Return the element at the position given, or return the default if
        /// the position is outside the bounds of this matrix.
        /// </summary>
        public T At(int x, int y, T defaultIfOutsideBounds)
        {
            if (x >= 0 && x < _elemCountH)
            {
                if (y >= 0 && y < _elemCountV)
                {
                    var i = _originOffset + y * _rowStrafe + x;
                    return _theArray[i];
                }
            }
            return defaultIfOutsideBounds;
        }
    }
}
