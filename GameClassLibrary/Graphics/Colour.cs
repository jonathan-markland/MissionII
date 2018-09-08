using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary.Graphics
{
    public static class Colour
    {
        public static byte GetGreyLevel(uint rgbValue)
        {
            var r = (rgbValue >> 16) & 0xFF;
            var g = (rgbValue >> 8) & 0xFF;
            var b = (rgbValue) & 0xFF;

            var rn = r * 87;
            var gn = g * 141;
            var bn = b * 28;

            return (byte)((rn + gn + bn) >> 8);
        }

        public static uint ExpandToGreyScaleArgb(byte v)
        {
            return 0xFF000000 | (uint)(v << 16) | (uint)(v << 8) | v;
        }

        public static uint ToGreyscale(uint rgbValue)
        {
            return ExpandToGreyScaleArgb(GetGreyLevel(rgbValue));
        }

        public static int GetColourWheelBlueValue(int i)
        {
            i &= 255;
            if (i < 43) return i * 6;  // good enough approx for ramp up to 255
            if (i < 129) return 255;
            if (i < 171) return (171 - i) * 6;
            return 0;
        }

        public static int GetColourWheelGreenValue(int i)
        {
            return GetColourWheelBlueValue(i - 85);
        }

        public static int GetColourWheelRedValue(int i)
        {
            return GetColourWheelBlueValue(i + 85);
        }

        public static uint GetWheelColourAsPackedValue(int i)
        {
            var r = GetColourWheelRedValue(i);
            var g = GetColourWheelGreenValue(i);
            var b = GetColourWheelBlueValue(i);
            return 0xFF000000 | (uint)(r << 16) | (uint)(g << 8) | (uint)b;
        }

        public static void ToGreyscale(uint[] uintArray)
        {
            var n = uintArray.Length;
            for (int i = 0; i < n; i++)
            {
                uintArray[i] = ToGreyscale(uintArray[i]);
            }
        }

        public static void ReplaceWithThreshold(uint[] uintArray, uint highColour, uint lowColour)
        {
            var n = uintArray.Length;
            for(int i=0; i<n; i++)
            {
                var v = uintArray[i] & 255; // assume greyscale, only sample the least significant byte
                uint r = 0;
                if (v == 0)
                {
                    r = 0;
                }
                else if (v < 128)
                {
                    r = lowColour;
                }
                else
                {
                    r = highColour;
                }
                uintArray[i] = r;
            }
        }
    }
}
