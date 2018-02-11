using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class LeavingLevel : BaseGameMode
    {
        private MissionIIGameBoard _gameBoard;
        private int _countDown = Constants.LeavingLevelCycles;

        public LeavingLevel(MissionIIGameBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (MissionIIModes.HandlePause(theKeyStates, this)) return;
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
