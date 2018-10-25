
using System;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class LeavingLevel : ChangeStageFreeze
    {
        public LeavingLevel(Func<GameMode> getNextModeFunction)
            : base(
                  Constants.LeavingLevelCycles,
                  ActiveMode,
                  MissionIISounds.LevelExitActivated,
                  getNextModeFunction)
        {
            // No actions
        }
    }
}
