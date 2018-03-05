using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class GameOver : BaseGameMode
    {
        private int _countDown = Constants.GameOverMessageCycles;

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (_countDown == Constants.GameOverMessageCycles)
            {
                MissionIISounds.GameOver.Play();
            }
            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new TitleScreen();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawFirstSpriteScreenCentred(MissionIISprites.GameOver);
        }
    }
}
