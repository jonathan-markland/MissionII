﻿
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Attractor : AbstractIntelligenceProvider
    {
        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            if ((Time.CycleCounter.Count32 & 1) == 0)
            {
                var moveDeltas = gameObject.GetMovementDeltasToHeadTowards(
                    theGameBoard.GetMan());

                // We must separate horizontal and vertical movement in order to avoid
                // things getting 'stuck' on walls because they can't move horizontally
                // into the wall, but can moe vertically downward.  Trying to do both
                // directions at once results in rejection of the move, and the
                // sticking problem.

                theGameBoard.MoveAdversaryOnePixel(
                    gameObject,
                    moveDeltas.XComponent);

                theGameBoard.MoveAdversaryOnePixel(
                    gameObject,
                    moveDeltas.YComponent);
            }
        }
    }
}
