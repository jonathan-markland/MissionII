
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public static class HiScoreShow
    {
        public static ModeFunctions New()
        {
            return GameClassLibrary.Modes.HiScoreShow.New(
                Constants.TitleScreenRollCycles,
                MissionIISprites.Background,
                MissionIIFonts.GiantFont,
                MissionIIFonts.HiScoreFont,
                () => StartNewGame.New(),
                () => MissionRotatingInstructions.New());
        }
    }
}
