using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Attractor : AbstractIntelligenceProvider
    {
        private bool _operationEnable = false;

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            _operationEnable = !_operationEnable;  // ie: operate only ever other cycle
            if (_operationEnable)
            {
                var moveDeltas = CybertronGameStateUpdater.GetMovementDeltasToHeadTowards(
                    spriteInstance, 
                    theGameBoard.Man.SpriteInstance);

                // We must separate horizontal and vertical movement in order to avoid
                // things getting 'stuck' on walls because they can't move horizontally
                // into the wall, but can moe vertically downward.  Trying to do both
                // directions at once results in rejection of the move, and the
                // sticking problem.

                CybertronGameStateUpdater.MoveAdversaryOnePixel(
                    theGameBoard,
                    spriteInstance,
                    new MovementDeltas(moveDeltas.dx, 0));

                CybertronGameStateUpdater.MoveAdversaryOnePixel(
                    theGameBoard,
                    spriteInstance,
                    new MovementDeltas(0, moveDeltas.dy));
            }
        }
    }
}
