using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class GameOver : GameMode
    {
        private int _countDown = Constants.GameOverMessageCycles;
        private uint _finalScore;

        public GameOver(uint finalScore)
        {
            _finalScore = finalScore;
        }

        public override void AdvanceOneCycle(KeyStates theKeyStates)
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
                GameClassLibrary.Modes.GameMode.ActiveMode = new HiScore(_finalScore);
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawFirstSpriteScreenCentred(MissionIISprites.GameOver);
        }
    }
}
