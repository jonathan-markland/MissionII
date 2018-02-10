namespace MissionIIClassLibrary
{
    public class MissionIIGameModes
    {
        private Modes.MissionIIGameMode _currentMode;

        public MissionIIGameModes()
        {
            CurrentMode = new Modes.MissionIITitleScreenMode();
        }

        public Modes.MissionIIGameMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
            }
        }
    }
}
