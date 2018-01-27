using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissionIIClassLibrary.Math;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class FiringAttractor : AbstractIntelligenceProvider
    {
        private uint _cycleCounter = 0;

        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            ++_cycleCounter;

            if (_cycleCounter % 3 == 0)
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

                if ((_cycleCounter & 7) == 0) // TODO: firing time constant
                {
                    if (!moveDeltas.Stationary
                        && Rng.Generator.Next(100) < 40)
                    {
                        theGameBoard.StartBullet(spriteInstance, moveDeltas, false);
                    }
                }
            }
        }
    }
}
