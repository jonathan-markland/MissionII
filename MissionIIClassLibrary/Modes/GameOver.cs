using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class GameOver : BaseGameMode
    {
        private int _countDown = Constants.GameOverMessageCycles;
        private uint _finalScore;

        public GameOver(uint finalScore)
        {
            _finalScore = finalScore;
        }

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
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new HiScoreMode(_finalScore);
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawFirstSpriteScreenCentred(MissionIISprites.GameOver);
        }
    }
}
