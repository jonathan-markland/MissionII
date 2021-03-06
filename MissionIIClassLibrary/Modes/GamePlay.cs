﻿using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Modes;

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
            _gameBoard.AdvanceOneCycle(theKeyStates); // TODO: pull logic into this class
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            _gameBoard.DrawBoardToTarget(drawingTarget);
        }
    }
}
