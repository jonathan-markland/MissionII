
using System;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Bouncing : AbstractIntelligenceProvider
    {
        private int _leftLimit;
        private int _rightLimit;
        private int _maxBounceHeightOffFloor;
        private int _movesPerCycle;
        private MovementDeltas _movementDeltas;
        private Action _manDestroyAction;
        private bool _firstCycle;
        private int _floorFoundY;



        public Bouncing(int leftLimit, int rightLimit, int maxBounceHeightOffFloor, int movesPerCycle, Action manDestroyAction, int initialDx)
        {
            _leftLimit = leftLimit;
            _maxBounceHeightOffFloor = maxBounceHeightOffFloor;
            _rightLimit = rightLimit;
            _movesPerCycle = movesPerCycle;
            _manDestroyAction = manDestroyAction;
            _movementDeltas = new MovementDeltas(initialDx, -1);  // Force upwards motion initially.
            _firstCycle = true;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            if (_firstCycle)
            {
                _firstCycle = false;
                _floorFoundY = gameObject.TopLeftPosition.Y;
            }

            for (int i = 0; i < _movesPerCycle; i++)
            {
                var positionBefore = gameObject.TopLeftPosition;

                if (positionBefore.X == _leftLimit && _movementDeltas.dx < 0) { }
                else if (positionBefore.X == _rightLimit && _movementDeltas.dx > 0) { }
                else
                {
                    theGameBoard.MoveAdversaryOnePixel(
                        gameObject,
                        new MovementDeltas(_movementDeltas.dx, 0));
                }

                if (_movementDeltas.dy == -1 && positionBefore.Y == (_floorFoundY - _maxBounceHeightOffFloor)) { }
                else
                {
                    theGameBoard.MoveAdversaryOnePixel(
                        gameObject,
                        new MovementDeltas(0, _movementDeltas.dy));
                }

                var newPosition = gameObject.TopLeftPosition;

                if (positionBefore.X == newPosition.X)
                {
                    _movementDeltas = new MovementDeltas(-_movementDeltas.dx, _movementDeltas.dy);
                }
                if (positionBefore.Y == newPosition.Y)
                {
                    _movementDeltas = new MovementDeltas(_movementDeltas.dx, -_movementDeltas.dy);
                    if (_movementDeltas.dy == -1)
                    {
                        // Switched to moving up, remember where the floor is, in order to limit bounce height.
                        _floorFoundY = newPosition.Y;
                    }
                }

                if (gameObject.Intersects(theGameBoard.GetMan()))
                {
                    _manDestroyAction();
                }
            }
        }
    }
}
