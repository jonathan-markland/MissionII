﻿
using System;
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
