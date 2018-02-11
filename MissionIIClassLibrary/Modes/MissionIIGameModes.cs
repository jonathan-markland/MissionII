namespace MissionIIClassLibrary.Modes
{
    public class MissionIIGameModes
    {
        private Modes.BaseGameMode _currentMode;

        public MissionIIGameModes()
        {
            CurrentMode = new Modes.TitleScreen();
        }

        public Modes.BaseGameMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
            }
        }
    }
}
