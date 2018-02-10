
namespace MissionIIClassLibrary
{
    public static class MissionIIModes
    {
        public static bool HandlePause(
            MissionIIKeyStates theKeyStates,
            Modes.MissionIIGameMode theCurrentModeObject)
        {
            if (theKeyStates.Pause)
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new Modes.MissionIIPauseMode(theCurrentModeObject);
                MissionIISounds.Play(MissionIISounds.PauseMode);
                return true;
            }
            return false;
        }
    }
}