
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public static class TitleScreen
    {
        public static ModeFunctions New()
        {
            return GameClassLibrary.Modes.TitleScreenWithCredit.New(
                Constants.TitleScreenRollCycles,
                  MissionIISprites.Background,
                  MissionIISounds.Intro,
                  MissionIIFonts.GiantFont,
                  "SECOND MISSION",
                  MissionIIFonts.NarrowFont,
                  "A RETRO GAME BY JONATHAN MARKLAND",
                  () => StartNewGame.New(),
                  () => HiScoreShow.New());
        }
    }
}
