using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary.Modes
{
    public class GamePlay : GameMode
    {
        private MissionIIGameBoard _gameBoard;

        public GamePlay(MissionIIGameBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (MissionIIModes.HandlePause(_gameBoard, theKeyStates, this)) return;
            _gameBoard.Update(theKeyStates); // TODO: pull logic into this class
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            _gameBoard.DrawBoardToTarget(drawingTarget);
        }
    }
}
