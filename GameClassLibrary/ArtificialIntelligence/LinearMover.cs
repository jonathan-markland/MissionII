
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
		private readonly Action _manDestroyAction;

		private bool _headToEnd;



        public LinearMover(
            Point startPoint, Point endPoint, int movesPerCycle, Action manDestroyAction,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            _headToEnd = true;
            _movesPerCycle = movesPerCycle;
            _manDestroyAction = manDestroyAction;
            _moveAdversaryOnePixel = moveAdversaryOnePixel;
            _getManExtents = getManExtents;
        }



        public override void AdvanceOneCycle(GameObject gameObject)
        {
            for (int i = 0; i < _movesPerCycle; i++)
            {
                var currentPosition = gameObject.TopLeftPosition;

                if (currentPosition == _startPoint)
                {
                    _headToEnd = true;
                }
                else if (currentPosition == _endPoint)
                {
                    _headToEnd = false;
                }

                var r = gameObject.GetBoundingRectangle();
                var moveDeltas = new Point(r.Left, r.Top).GetMovementDeltasToHeadTowards(
                    _headToEnd ? _endPoint : _startPoint);

                gameObject.MoveBy(moveDeltas);

                if (gameObject.GetBoundingRectangle().Intersects(_getManExtents()))
                {
                    _manDestroyAction();
                }
            }
        }
    }
}
