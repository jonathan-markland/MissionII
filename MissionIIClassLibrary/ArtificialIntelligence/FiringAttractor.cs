
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class FiringAttractor : AbstractIntelligenceProvider
    {
        private uint _cycleCounter = 0;

        public override void AdvanceOneCycle(IGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            ++_cycleCounter;

            if (_cycleCounter % Constants.FiringAttractorSpeedDivisor == 0)
            {
                var moveDeltas = Business.GetMovementDeltasToHeadTowards(
                    spriteInstance,
                    theGameBoard.ManSpriteInstance());

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

                if ((_cycleCounter & Constants.FiringAttractorFiringCyclesAndMask) == 0)
                {
                    if (!moveDeltas.Stationary
                        && Rng.Generator.Next(100) < Constants.AttractorFiringProbabilityPercent)
                    {
                        theGameBoard.StartBullet(spriteInstance, moveDeltas, false);
                    }
                }
            }
        }
    }
}
