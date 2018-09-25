
using System;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class LinearMover : AbstractIntelligenceProvider
    {
		private readonly Point _startPoint;
		private readonly Point _endPoint;
		private readonly int _movesPerCycle;
		private readonly Action _manDestroyAction;

		private bool _headToEnd;



        public LinearMover(Point startPoint, Point endPoint, int movesPerCycle, Action manDestroyAction)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            _headToEnd = true;
            _movesPerCycle = movesPerCycle;
            _manDestroyAction = manDestroyAction;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
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

                if (gameObject.Intersects(theGameBoard.GetMan()))
                {
                    _manDestroyAction();
                }
            }
        }
    }
}
