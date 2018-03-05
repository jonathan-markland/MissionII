using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class TitleScreen : BaseGameMode
    {
        private int _countDown = Constants.TitleScreenRollCycles;
        private bool _releaseWaiting = true;
        private bool _firstCycle = true;

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (_firstCycle)
            {
                MissionIISounds.Intro.Play();
                _firstCycle = false;
            }

            if (theKeyStates.Fire)
            {
                if (_releaseWaiting) return;
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new StartNewGame();
            }

            _releaseWaiting = false;

            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new InstructionsKeys();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISprites.TitleScreen.GetHostImageObject(0));
        }
    }
}
