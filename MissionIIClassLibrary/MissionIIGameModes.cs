namespace MissionIIClassLibrary
{
    public class MissionIIGameModes
    {
        private MissionIIGameMode _currentMode;

        public MissionIIGameModes()
        {
            CurrentMode = new MissionIITitleScreenMode();
        }

        public MissionIIGameMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
            }
        }
    }
}
