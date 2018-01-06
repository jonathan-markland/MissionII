namespace GameClassLibrary
{
    public class CybertronGameModes
    {
        private CybertronGameMode _currentMode;

        public CybertronGameModes()
        {
            CurrentMode = new CybertronTitleScreenMode();
        }

        public CybertronGameMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
            }
        }
    }
}
