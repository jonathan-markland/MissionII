namespace GameClassLibrary
{
    public struct MovementDeltas
    {
        public MovementDeltas(int dx, int dy) { this.dx = dx; this.dy = dy; }
        public int dx;
        public int dy;

        public bool Stationary { get { return dx == 0 && dy == 0; } }
    }
}
