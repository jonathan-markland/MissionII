using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class InstructionsKeys : BaseGameMode
    {
        private int _countDown = Constants.TitleScreenRollCycles;
        private int _screenIndex = 1;

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (MissionIISpriteTraits.TitleScreen.ImageCount < 2)
            {
                // Cannot rotate any instruction screens.
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new TitleScreen();
                return;
            }

            if (theKeyStates.Fire)
            {
                _screenIndex = 1; // for next time
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new TitleScreen();
            }
            else if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                ++_screenIndex;
                if (_screenIndex >= MissionIISpriteTraits.TitleScreen.ImageCount)
                {
                    _screenIndex = 1;
                }
                _countDown = Constants.TitleScreenRollCycles;
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISpriteTraits.TitleScreen.GetHostImageObject(_screenIndex));
        }
    }
}
