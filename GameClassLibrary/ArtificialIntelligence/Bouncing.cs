
using System;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Bouncing : AbstractIntelligenceProvider
    {
		private readonly int _leftLimit;
		private readonly int _rightLimit;
		private readonly int _maxBounceHeightOffFloor;
		private readonly int _movesPerCycle;
		private readonly Action _manDestroyAction;

		private MovementDeltas _movementDeltas;
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

                if (positionBefore.X == _leftLimit && _movementDeltas.MovingLeft) { }
                else if (positionBefore.X == _rightLimit && _movementDeltas.MovingRight) { }
                else
                {
                    theGameBoard.MoveAdversaryOnePixel(
                        gameObject,
                        _movementDeltas.XComponent);
                }

                if (_movementDeltas.MovingUp && positionBefore.Y == (_floorFoundY - _maxBounceHeightOffFloor)) { }
                else
                {
                    theGameBoard.MoveAdversaryOnePixel(
                        gameObject,
                        _movementDeltas.YComponent);
                }

                var newPosition = gameObject.TopLeftPosition;

                if (positionBefore.X == newPosition.X)
                {
                    _movementDeltas = _movementDeltas.ReverseX;
                }
                if (positionBefore.Y == newPosition.Y)
                {
                    _movementDeltas = _movementDeltas.ReverseY;

                    if (_movementDeltas.MovingUp)
                    {
                        // Remember where the floor is, in order to limit bounce height.
                        _floorFoundY = newPosition.Y;
                    }
                }

                if (gameObject.GetBoundingRectangle().Intersects(
                    theGameBoard.GetManExtentsRectangle()))
                {
                    _manDestroyAction();
                }
            }
        }
    }
}
