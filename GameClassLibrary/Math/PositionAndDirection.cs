
namespace GameClassLibrary.Math
{
    public struct PositionAndDirection
    {
        public PositionAndDirection(Point p, int facingDirection) { Position = p; FacingDirection = facingDirection; }

        public Point Position { get; private set; }
        public int FacingDirection { get; private set; }
    }
}


