
namespace MissionIIClassLibrary.Modes
{
    public class TitleScreen : GameClassLibrary.Modes.TitleScreenWithCredit
    {
        public TitleScreen()
            : base(
                  Constants.TitleScreenRollCycles,
                  MissionIISprites.TitleScreen,
                  MissionIISounds.Intro,
                  MissionIIFonts.NarrowFont,
                  "BY JONATHAN MARKLAND",
                  () => new StartNewGame(),
                  () => new HiScore())
        {
        }
    }
}
