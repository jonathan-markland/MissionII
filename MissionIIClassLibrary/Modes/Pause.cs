
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public static class Pause
    {
        public static ModeFunctions New(
            ModeFunctions originalMode, MissionIIGameBoard theGameBoard)   // TODO: Avoid passing MissionIIGameBoard
        {
            return PauseWithChangeLevel.New(
                originalMode,
                MissionIISprites.Paused,
                MissionIISounds.PauseMode,
                Constants.ScreenHeight - 146,
                MissionIIFonts.GiantFont,
                MissionIISounds.ManFiring,
                s => theGameBoard.LevelCodeAccepted(s),
                () => Modes.EnteringLevel.New(theGameBoard),
                () => !theGameBoard.DeadManExistsInRoom() && !theGameBoard.Man.IsBeingElectrocuted);
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
