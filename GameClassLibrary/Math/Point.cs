namespace GameClassLibrary.Math
{
    public struct Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point( int x, int y )
        {
            X = x;
            Y = y;
        }

        public static Point operator+(Point p, MovementDeltas md)
        {
            return new Point(p.X + md.dx, p.Y + md.dy);
        }
    }
}
