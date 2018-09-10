
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class LeavingLevel : ChangeStageFreeze
    {
        public LeavingLevel(MissionIIGameBoard theGameBoard)
            : base(
                  Constants.LeavingLevelCycles,
                  ActiveMode,
                  MissionIISounds.LevelExitActivated,
                  () =>
                  {
                      var thisLevelNumber = theGameBoard.LevelNumber;
                      ++thisLevelNumber;
                      theGameBoard.LevelNumber = thisLevelNumber;
                      theGameBoard.PrepareForNewLevel();
                      return ActiveMode; // PrepareForNewLevel() just set this
                  })
        {
            // No actions
        }
    }
}
