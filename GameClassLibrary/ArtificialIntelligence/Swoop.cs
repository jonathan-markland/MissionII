
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



        public Swoop(
            Action manDestroyAction,
            Func<Rectangle> getManExtents)
        {
            _manDestroyAction = manDestroyAction;
            _getManExtents = getManExtents;
        }



        public override void AdvanceOneCycle(GameObject gameObject)
        {
            for (int i = 0; i < Constants.SwoopMovementCycles; i++)
            {
                var moveDeltas = gameObject.GetBoundingRectangle().GetMovementDeltasToHeadTowards(_getManExtents());

                gameObject.MoveBy(moveDeltas);

                if (gameObject.GetBoundingRectangle().Intersects(_getManExtents()))
                {
                    _manDestroyAction();
                }
            }
        }
    }
}
