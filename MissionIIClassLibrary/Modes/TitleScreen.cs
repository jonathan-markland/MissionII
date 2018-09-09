using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class TitleScreen : GameMode
    {
        private const int FireButtonPressEnableTime = (Constants.TitleScreenRollCycles * 3) / 4;
        private int _countDown = Constants.TitleScreenRollCycles;
        private bool _releaseWaiting = true;
        private bool _firstCycle = true;

        public TitleScreen()
        {
        }

        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_firstCycle)
            {
                MissionIISounds.Intro.PlayAsBackgroundMusic();
                _firstCycle = false;
            }

            if (theKeyStates.Fire)
            {
                if (_releaseWaiting || _countDown > FireButtonPressEnableTime) return;
                GameClassLibrary.Modes.GameMode.ActiveMode = new StartNewGame();
            }

            _releaseWaiting = false;

            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                GameClassLibrary.Modes.GameMode.ActiveMode = new HiScore();
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
