
using System;
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Swoop : AbstractIntelligenceProvider
    {
        private readonly Action _manDestroyAction;



        public Swoop(Action manDestroyAction)
        {
            _manDestroyAction = manDestroyAction;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            for (int i = 0; i < Constants.SwoopMovementCycles; i++)
            {
                var moveDeltas = gameObject.GetBoundingRectangle().GetMovementDeltasToHeadTowards(
                    theGameBoard.GetManExtentsRectangle());

                gameObject.MoveBy(moveDeltas);

                if (gameObject.GetBoundingRectangle().Intersects(
                    theGameBoard.GetManExtentsRectangle()))
                {
                    _manDestroyAction();
                }
            }
        }
    }
}
