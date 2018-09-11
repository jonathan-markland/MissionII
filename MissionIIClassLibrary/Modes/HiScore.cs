
namespace MissionIIClassLibrary.Modes
{
    public class HiScore : GameClassLibrary.Modes.HiScore
    {
        public HiScore(uint scoreAchieved)
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
