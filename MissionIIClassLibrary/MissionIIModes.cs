using GameClassLibrary.Input;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary
{
    public static class MissionIIModes
    {
        public static bool HandlePause(
            MissionIIGameBoard theGameBoard,
            KeyStates theKeyStates,
            GameMode theCurrentModeObject)
        {
            if (theKeyStates.Pause)
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new Modes.Pause(theCurrentModeObject, theGameBoard);
                MissionIISounds.PauseMode.Play();
                return true;
            }
            return false;
        }
    }
}