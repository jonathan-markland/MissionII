namespace GameClassLibrary.Math
{
    public struct MovementDeltas
    {
        public MovementDeltas(int dx, int dy) { this.dx = dx; this.dy = dy; }

        public int dx { get; private set; }
        public int dy { get; private set; }



        public bool IsStationary 
		{ 
			get { return dx == 0 && dy == 0; } 
		}



        public static MovementDeltas Stationary
        {
            get { return new MovementDeltas(0, 0); }
        }



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



        public bool MovingLeft { get { return dx < 0; } }
        public bool MovingRight { get { return dx > 0; } }
        public bool MovingUp { get { return dy < 0; } }
        public bool MovingDown { get { return dy > 0; } }



        public MovementDeltas XComponent
        {
            get { return new MovementDeltas(dx, 0); }
        }

        public MovementDeltas YComponent
        {
            get { return new MovementDeltas(0, dy); }
        }



        public MovementDeltas ReverseX
        {
            get { return new MovementDeltas(-dx, dy); }
        }

        public MovementDeltas ReverseY
        {
            get { return new MovementDeltas(dx, -dy); }
        }



        public MovementDeltas ReflectYX
        {
            get { return new MovementDeltas(dy, dx); }
        }

    }
}
