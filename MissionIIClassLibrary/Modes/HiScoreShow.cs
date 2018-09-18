
namespace MissionIIClassLibrary.Modes
{
    public class HiScoreShow : GameClassLibrary.Modes.HiScoreShow
    {
        public HiScoreShow()
            : base(
                  Constants.TitleScreenRollCycles,
                  MissionIISprites.HiScoreScreen,
                  MissionIIFonts.HiScoreFont,
                  () => new StartNewGame(),
                  () => new MissionRotatingInstructions())
        {
        }
    }
}
