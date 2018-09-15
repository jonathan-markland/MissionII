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

        public static bool operator==(Point p, Point q)
        {
            return p.X == q.X && p.Y == q.Y;
        }

        public static bool operator!=(Point p, Point q)
        {
            return !(p == q);
        }

        public MovementDeltas GetMovementDeltasToHeadTowards(Point targetPoint)
        {
            int dx = 0;
            if (targetPoint.X < X) dx = -1;
            if (targetPoint.X > X) dx = 1;

            int dy = 0;
            if (targetPoint.Y < Y) dy = -1;
            if (targetPoint.Y > Y) dy = 1;

            return new MovementDeltas(dx, dy);
        }

    }
}
