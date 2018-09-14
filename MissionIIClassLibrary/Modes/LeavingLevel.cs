
using GameClassLibrary.Modes;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Modes
{
    public class LeavingLevel : ChangeStageFreeze
    {
        public LeavingLevel(IGameBoard theGameBoard)
            : base(
                  Constants.LeavingLevelCycles,
                  ActiveMode,
                  MissionIISounds.LevelExitActivated,
                  () =>
                  {
                      var thisLevelNumber = theGameBoard.GetLevelNumber();
                      ++thisLevelNumber;
                      theGameBoard.PrepareForNewLevel(thisLevelNumber);
                      return ActiveMode; // PrepareForNewLevel() just set this
                  })
        {
            // No actions
        }
    }
}
