
namespace MissionIIClassLibrary
{
    public static class MissionIIModes
    {
        public static bool HandlePause(
            MissionIIKeyStates theKeyStates,
            Modes.BaseGameMode theCurrentModeObject)
        {
            if (theKeyStates.Pause)
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new Modes.Pause(theCurrentModeObject);
                MissionIISounds.Play(MissionIISounds.PauseMode);
                return true;
            }
            return false;
        }
    }
}