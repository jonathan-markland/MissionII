
using System;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class LinearMover : AbstractIntelligenceProvider
    {
        private readonly Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> _moveAdversaryOnePixel;
        private readonly Func<Rectangle> _getManExtents;
        private readonly Point _startPoint;
		private readonly Point _endPoint;
		private readonly int _movesPerCycle;
		private readonly Action<GameObject> _manWalksIntoDroidAction;
        private readonly GameObject _gameObject;

		private bool _headToEnd;



        public LinearMover(
            GameObject gameObject,
            Point startPoint, Point endPoint, int movesPerCycle, Action<GameObject> manWalksIntoDroidAction,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            _headToEnd = true;
            _movesPerCycle = movesPerCycle;
            _manWalksIntoDroidAction = manWalksIntoDroidAction;
            _moveAdversaryOnePixel = moveAdversaryOnePixel;
            _getManExtents = getManExtents;
            _gameObject = gameObject;
        }



        public override void AdvanceOneCycle()
        {
            for (int i = 0; i < _movesPerCycle; i++)
            {
                var currentPosition = _gameObject.TopLeftPosition;

                if (currentPosition == _startPoint)
                {
                    _headToEnd = true;
                }
                else if (currentPosition == _endPoint)
                {
                    _headToEnd = false;
                }

                var r = _gameObject.GetBoundingRectangle();
                var moveDeltas = new Point(r.Left, r.Top).GetMovementDeltasToHeadTowards(
                    _headToEnd ? _endPoint : _startPoint);

                _gameObject.MoveBy(moveDeltas);

                if (_gameObject.GetBoundingRectangle().Intersects(_getManExtents()))
                {
                    _manWalksIntoDroidAction(_gameObject);
                }
            }
        }
    }
}
