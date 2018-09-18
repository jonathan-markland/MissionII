
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class Pause : PauseWithChangeLevel
    {
        public Pause(GameMode originalMode, MissionIIGameBoard theGameBoard)
            : base(
                  originalMode,
                  MissionIISprites.Paused,
                  MissionIISounds.PauseMode,
                  Constants.ScreenHeight - 146,
                  MissionIIFonts.GiantFont,
                  MissionIISounds.ManFiring,
                  s => theGameBoard.LevelCodeAccepted(s),
                  () => new Modes.EnteringLevel(theGameBoard),
                  () => !theGameBoard.Man.IsDead && !theGameBoard.Man.IsBeingElectrocuted)
        {
        }
    }

    
    
    /* A development test of the pause-only facility
    public class Pause : GameClassLibrary.Modes.Pause
    {
         public Pause(GameMode originalMode, MissionIIGameBoard theGameBoard)
            : base(
                  originalMode,
                  MissionIISprites.Paused,
                  MissionIISounds.PauseMode,
                  () => new Modes.EnteringLevel(theGameBoard))
        {
        }

    }*/
}
