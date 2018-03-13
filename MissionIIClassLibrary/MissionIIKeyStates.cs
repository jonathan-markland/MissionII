
namespace MissionIIClassLibrary
{
    public class MissionIIKeyStates
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
    }
}
