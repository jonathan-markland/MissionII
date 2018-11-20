
using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Algorithms
{
    public static class Array2D
    {
        public static Point FindPositionOf<T>(ArraySlice2D<T> sourceArray, Func<T, bool> matchTester)
        {
            var v = sourceArray.CountV;
            var h = sourceArray.CountH;
            for (int y=0; y<v; y++)
            {
                for (int x = 0; x < h; x++)
                {
                    if (matchTester(sourceArray.At(x,y)))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return new Point(-1, -1);
        }

        public static ArraySlice2D<T> RotateRight90<T>(ArraySlice2D<T> sourceArray)
        {
            var sw = sourceArray.CountH;
            var sh = sourceArray.CountV;

            var targetArray = new WriteableArraySlice2D<T>(sh, sw);

            Copy(
                sourceArray, targetArray,
                new Point(0, sh - 1),      
                new MovementDeltas(0, -1), 
                new MovementDeltas(1, 0),  
                new Point(0, 0),
                new MovementDeltas(1, 0),
                new MovementDeltas(0, 1), sh, sw);

            return targetArray.WholeArea;
        }

        private static void Copy<T>(
            ArraySlice2D<T> sourceArray,
            WriteableArraySlice2D<T> targetArray, 
            Point srcp, MovementDeltas srcd1, MovementDeltas srcd2, 
            Point dstp, MovementDeltas dstd1, MovementDeltas dstd2,
            int count1, int count2)
        {
            for (int j = 0; j < count2; j++)
            {
                var d = dstp;
                var s = srcp;
                for (int i = 0; i < count1; i++)
                {
                    targetArray.Write(d, sourceArray.At(s));
                    d += dstd1;
                    s += srcd1;
                }
                dstp += dstd2;
                srcp += srcd2;
            }
        }
    }
}
