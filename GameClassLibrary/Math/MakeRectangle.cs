
namespace GameClassLibrary.Math
{
    public static class MakeRectangle
    {
        public static Rectangle CentredInArea(int hostWidth, int hostHeight, int areaWidth, int areaHeight)
        {
            var resultRect = new Rectangle((hostWidth - areaWidth) / 2, (hostHeight - areaHeight) / 2, areaWidth, areaHeight);
            return resultRect;
        }

        public static Rectangle GetSquarePixelsProjectionArea(int hostWidth, int hostHeight, int sourceWidth, int sourceHeight)
        {
            int sourceSidePixels = System.Math.Max(sourceWidth, sourceHeight); // consider as a square
            int smallestDimension = System.Math.Min(hostWidth, hostHeight);
            int multiplier = System.Math.Max(smallestDimension / sourceSidePixels, 1);
            return CentredInArea(hostWidth, hostHeight, multiplier * sourceWidth, multiplier * sourceHeight);
        }

        public static Rectangle GetBestFitProjectionArea(int hostWidth, int hostHeight, int sourceWidth, int sourceHeight)
        {
            // Expand width to target width, and see if height then fits:
            var newHeight = (hostWidth * sourceHeight) / sourceWidth;
            if (newHeight <= hostHeight)
            {
                return CentredInArea(hostWidth, hostHeight, hostWidth, newHeight);
            }
            else
            {
                var newWidth = (hostHeight * sourceWidth) / sourceHeight;
                return CentredInArea(hostWidth, hostHeight, newWidth, hostHeight);
            }
        }
    }
}
