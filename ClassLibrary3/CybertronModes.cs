using System;

namespace GameClassLibrary
{
    public static class CybertronModes
    {
        public static bool HandlePause(
            CybertronKeyStates theKeyStates,
            CybertronGameMode theCurrentModeObject)
        {
            if (theKeyStates.Pause)
            {
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronPauseMode(theCurrentModeObject);
                return true;
            }
            return false;
        }
    }
}