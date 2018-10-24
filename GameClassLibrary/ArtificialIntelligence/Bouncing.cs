
using System;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Bouncing : AbstractIntelligenceProvider
    {
        private readonly Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> _moveAdversaryOnePixel;
        private readonly Func<Rectangle> _getManExtents;

        private readonly int _leftLimit;
		private readonly int _rightLimit;
		private readonly int _maxBounceHeightOffFloor;
		private readonly int _movesPerCycle;
		private readonly Action<GameObject> _manWalksIntoDroidAction;
        private readonly GameObject _gameObject;

		private MovementDeltas _movementDeltas;
        private bool _firstCycle;
        private int _floorFoundY;



        public Bouncing(
            GameObject gameObject,
            int leftLimit, int rightLimit, int maxBounceHeightOffFloor, int movesPerCycle,
            Action<GameObject> manWalksIntoDroidAction, int initialDx,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            _leftLimit = leftLimit;
            _maxBounceHeightOffFloor = maxBounceHeightOffFloor;
            _rightLimit = rightLimit;
            _movesPerCycle = movesPerCycle;
            _manWalksIntoDroidAction = manWalksIntoDroidAction;
            _movementDeltas = new MovementDeltas(initialDx, -1);  // Force upwards motion initially.
            _firstCycle = true;
            _moveAdversaryOnePixel = moveAdversaryOnePixel;
            _getManExtents = getManExtents;
            _gameObject = gameObject;
        }



        public override void AdvanceOneCycle()
        {
            if (_firstCycle)
            {
                _firstCycle = false;
                _floorFoundY = _gameObject.TopLeftPosition.Y;
            }

            for (int i = 0; i < _movesPerCycle; i++)
            {
                var positionBefore = _gameObject.TopLeftPosition;

                if (positionBefore.X == _leftLimit && _movementDeltas.MovingLeft) { }
                else if (positionBefore.X == _rightLimit && _movementDeltas.MovingRight) { }
                else
                {
                    _moveAdversaryOnePixel(_gameObject, _movementDeltas.XComponent);
                }

                if (_movementDeltas.MovingUp && positionBefore.Y == (_floorFoundY - _maxBounceHeightOffFloor)) { }
                else
                {
                    _moveAdversaryOnePixel(_gameObject, _movementDeltas.YComponent);
                }

                var newPosition = _gameObject.TopLeftPosition;

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

                if (_gameObject.GetBoundingRectangle().Intersects(_getManExtents()))
                {
                    _manWalksIntoDroidAction(_gameObject);
                }
            }
        }
    }
}
