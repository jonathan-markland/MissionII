
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class Pause : PauseWithChangeLevel
    {
        public Pause(GameMode originalMode, MissionIIGameBoard theGameBoard)
            : base(
                  originalMode,
                  Constants.ScreenHeight - 60,
                  MissionIISprites.Paused,
                  MissionIISounds.PauseMode,
                  MissionIISounds.ManFiring,
                  MissionIIFonts.GiantFont,
                  s => theGameBoard.LevelCodeAccepted(s),
                  () => new Modes.EnteringLevel(theGameBoard),
                  () => !theGameBoard.Man.IsDead && !theGameBoard.Man.IsBeingElectrocuted)
        {
        }
    }
}
