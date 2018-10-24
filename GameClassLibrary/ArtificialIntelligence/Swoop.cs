
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Swoop : AbstractIntelligenceProvider
    {
        private readonly Action _manDestroyAction;
        private readonly Func<Rectangle> _getManExtents;
        private readonly GameObject _gameObject;


        public Swoop(
            GameObject gameObject,
            Action manDestroyAction,
            Func<Rectangle> getManExtents)
        {
            _manDestroyAction = manDestroyAction;
            _getManExtents = getManExtents;
            _gameObject = gameObject;
        }



        public override void AdvanceOneCycle()
        {
            for (int i = 0; i < Constants.SwoopMovementCycles; i++)
            {
                var moveDeltas = _gameObject.GetBoundingRectangle().GetMovementDeltasToHeadTowards(_getManExtents());

                _gameObject.MoveBy(moveDeltas);

                if (_gameObject.GetBoundingRectangle().Intersects(_getManExtents()))
                {
                    _manDestroyAction();
                }
            }
        }
    }
}
