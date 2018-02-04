namespace GameClassLibrary.Math
{
    public struct MovementDeltas
    {
        public MovementDeltas(int dx, int dy) { this.dx = dx; this.dy = dy; }
        public int dx;
        public int dy;


        public bool Stationary { get { return dx == 0 && dy == 0; } }


        private static MovementDeltas[] g_MovementDeltas = new MovementDeltas[]
        {
            new MovementDeltas { dx =  0, dy = -1 }, // up
            new MovementDeltas { dx =  1, dy = -1 }, // up right
            new MovementDeltas { dx =  1, dy =  0 }, // right
            new MovementDeltas { dx =  1, dy =  1 }, // down right
            new MovementDeltas { dx =  0, dy =  1 }, // down
            new MovementDeltas { dx = -1, dy =  1 }, // down left
            new MovementDeltas { dx = -1, dy =  0 }, // left 
            new MovementDeltas { dx = -1, dy = -1 }, // left up
        };


        public static MovementDeltas ConvertFromFacingDirection(int facingDirection)
        {
            return g_MovementDeltas[facingDirection];
        }
    }
}
