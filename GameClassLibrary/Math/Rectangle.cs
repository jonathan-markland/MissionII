
namespace GameClassLibrary.Math
{
    public struct Rectangle
    {
        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Rectangle( int left, int top, int width, int height )
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public int Right
        {
            get { return Left + Width; }
        }

        public int Bottom
        {
            get { return Top + Height; }
        }

        public bool Intersects(Rectangle otherRectangle)
        {
            if (Right <= otherRectangle.Left) return false;
            if (Bottom <= otherRectangle.Top) return false;
            if (Left >= otherRectangle.Right) return false;
            if (Top >= otherRectangle.Bottom) return false;
            return true;
        }

        public Point Centre
        {
            get { return new Point((Left + Right) / 2, (Top + Bottom) / 2); }
        }

        public Rectangle Inflate(int n)
        {
            return new Rectangle(Left - n, Top - n, Width + (2 * n), Height + (2 * n));
        }
    }



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
