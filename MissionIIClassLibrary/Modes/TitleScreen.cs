using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class TitleScreen : BaseGameMode
    {
        private const int FireButtonPressEnableTime = (Constants.TitleScreenRollCycles * 3) / 4;
        private int _countDown = Constants.TitleScreenRollCycles;
        private bool _releaseWaiting = true;
        private bool _firstCycle = true;

        public TitleScreen()
        {
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (_firstCycle)
            {
                MissionIISounds.Intro.PlayAsBackgroundMusic();
                _firstCycle = false;
            }

            if (theKeyStates.Fire)
            {
                if (_releaseWaiting || _countDown > FireButtonPressEnableTime) return;
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new StartNewGame();
            }

            _releaseWaiting = false;

            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new HiScore();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISprites.TitleScreen.GetHostImageObject(0));
            if (_countDown < Constants.TitleScreenRollCycles / 2)
            {
                drawingTarget.DrawText(310, 230, "BY JONATHAN MARKLAND",
                    MissionIIFonts.NarrowFont, TextAlignment.Right);
            }
        }
    }
}
