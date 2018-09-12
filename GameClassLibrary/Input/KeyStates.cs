
namespace GameClassLibrary.Input
{
    public class KeyStates
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
        public bool Fire;
        public bool Quit;
        public bool Pause;

        public bool AllKeysReleased
        {
            get
            {
                return !Up && !Down && !Left && !Right && !Fire && !Quit && !Pause;
            }
        }

        public int ToDirectionIndex()
        {
            // Clockwise numbering
            // TODO:  Implementation biases certain directions when more than 2 keys held.
            if (Up)
            {
                if (Left) return 7;
                if (Right) return 1;
                return 0;
            }
            if (Down)
            {
                if (Left) return 5;
                if (Right) return 3;
                return 4;
            }
            if (Left) return 6;
            if (Right) return 2;
            return -1; // No keys held.  Cannot determine a direction.
        }

        public Math.MovementDeltas ToMovementDeltas()
        {
            return new Math.MovementDeltas(
                (Left ? -1 : 0) + (Right ? 1 : 0),
                (Up ? -1 : 0) + (Down ? 1 : 0)
            );
        }

    }
}
