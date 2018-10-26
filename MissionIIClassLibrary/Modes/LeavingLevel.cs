
using System;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public static class LeavingLevel
    {
        public static ModeFunctions New(Func<ModeFunctions> getNextModeFunction)
        {
            return ChangeStageFreeze.New(
                  Constants.LeavingLevelCycles,
                  GameMode.ActiveMode,
                  MissionIISounds.LevelExitActivated,
                  getNextModeFunction);
        }
    }
}
