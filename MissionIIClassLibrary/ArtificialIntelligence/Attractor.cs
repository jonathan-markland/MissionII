
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class Attractor : AbstractIntelligenceProvider
    {
        private bool _operationEnable = false;

        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            _operationEnable = !_operationEnable;  // ie: operate only ever other cycle
            if (_operationEnable)
            {
                var moveDeltas = Business.GetMovementDeltasToHeadTowards(
                    spriteInstance, 
                    theGameBoard.Man.SpriteInstance);

                // We must separate horizontal and vertical movement in order to avoid
                // things getting 'stuck' on walls because they can't move horizontally
                // into the wall, but can moe vertically downward.  Trying to do both
                // directions at once results in rejection of the move, and the
                // sticking problem.

                theGameBoard.MoveAdversaryOnePixel(
                    spriteInstance,
                    new MovementDeltas(moveDeltas.dx, 0));

                theGameBoard.MoveAdversaryOnePixel(
                    spriteInstance,
                    new MovementDeltas(0, moveDeltas.dy));
            }
        }
    }
}
