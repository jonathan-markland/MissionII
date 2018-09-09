
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class MissionIIGameModes
    {
        private GameMode _currentMode;

        public MissionIIGameModes()
        {
            CurrentMode = new Modes.TitleScreen();
        }

        public GameMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
            }
        }
    }
}
