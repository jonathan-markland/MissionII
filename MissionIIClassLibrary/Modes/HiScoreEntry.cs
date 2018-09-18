
namespace MissionIIClassLibrary.Modes
{
    public class HiScoreEntry : GameClassLibrary.Modes.HiScoreEntry
    {
        public HiScoreEntry(uint scoreAchieved)
            : base(
                  Constants.TitleScreenRollCycles,
                  MissionIISprites.HiScoreScreen,
                  MissionIIFonts.WideFont,
                  MissionIIFonts.HiScoreFont,
                  MissionIISprites.Life,
                  scoreAchieved,
                  () => new TitleScreen(),
                  () => new StartNewGame(),
                  () => new MissionRotatingInstructions())
        {
        }
    }
}
