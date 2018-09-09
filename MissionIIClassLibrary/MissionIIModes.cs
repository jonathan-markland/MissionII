using GameClassLibrary.Input;

namespace MissionIIClassLibrary
{
    public static class MissionIIModes
    {
        public static bool HandlePause(
            MissionIIGameBoard theGameBoard,
            KeyStates theKeyStates,
            Modes.BaseGameMode theCurrentModeObject)
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