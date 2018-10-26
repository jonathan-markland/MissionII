
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public static class HiScoreEntry
    {
        public static ModeFunctions New(uint scoreAchieved)
        {
            return GameClassLibrary.Modes.HiScoreEntry.New(Constants.TitleScreenRollCycles,
                  MissionIISprites.HiScoreScreen,
                  MissionIIFonts.WideFont,
                  MissionIIFonts.HiScoreFont,
                  MissionIISprites.Life,
                  scoreAchieved,
                  () => TitleScreen.New(),
                  () => StartNewGame.New(),
                  () => MissionRotatingInstructions.New());
        }

    }
}
