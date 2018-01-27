
namespace MissionIIClassLibrary
{
    public static class MissionIIModes
    {
        public static bool HandlePause(
            MissionIIKeyStates theKeyStates,
            MissionIIGameMode theCurrentModeObject)
        {
            if (theKeyStates.Pause)
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new CybertronPauseMode(theCurrentModeObject);
                MissionIISounds.Play(MissionIISounds.PauseMode);
                return true;
            }
            return false;
        }
    }
}