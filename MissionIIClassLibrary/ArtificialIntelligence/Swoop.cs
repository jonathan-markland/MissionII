
using System;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class Swoop : AbstractIntelligenceProvider
    {
        private Action _manDestroyAction;



        public Swoop(Action manDestroyAction)
        {
            _manDestroyAction = manDestroyAction;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            for (int i = 0; i < Constants.GhostMovementCycles; i++)
            {
                var moveDeltas = gameObject.GetMovementDeltasToHeadTowards(
                    theGameBoard.GetMan());

                gameObject.MoveBy(moveDeltas);

                if (gameObject.Intersects(theGameBoard.GetMan()))
                {
                    _manDestroyAction();
                }
            }
        }
    }
}
