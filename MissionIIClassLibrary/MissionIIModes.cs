using GameClassLibrary.Input;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary
{
    public static class MissionIIModes
    {
        public static bool HandlePause(
            MissionIIGameBoard theGameBoard,
            KeyStates theKeyStates,
            ModeFunctions theCurrentModeObject)
        {
            if (theKeyStates.Pause)
            {
                GameClassLibrary.Modes.GameMode.ActiveMode = Modes.Pause.New(theCurrentModeObject, theGameBoard);
                MissionIISounds.PauseMode.Play();
                return true;
            }
            return false;
        }
    }
}