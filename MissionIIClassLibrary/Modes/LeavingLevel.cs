using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class LeavingLevel : GameMode
    {
        private MissionIIGameBoard _gameBoard;
        private int _countDown = Constants.LeavingLevelCycles;

        public LeavingLevel(MissionIIGameBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (MissionIIModes.HandlePause(_gameBoard, theKeyStates, this)) return;
            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                var thisLevelNumber = _gameBoard.LevelNumber;
                ++thisLevelNumber;
                _gameBoard.LevelNumber = thisLevelNumber;
                _gameBoard.PrepareForNewLevel();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            _gameBoard.DrawBoardToTarget(drawingTarget);
        }
    }
}
