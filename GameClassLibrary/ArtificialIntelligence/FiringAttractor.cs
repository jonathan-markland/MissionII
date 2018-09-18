
using System;
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class FiringAttractor : AbstractIntelligenceProvider
    {
        private Action<Rectangle, MovementDeltas, bool> _fireBullet;



        public FiringAttractor(Action<Rectangle, MovementDeltas, bool> fireBullet)
        {
            _fireBullet = fireBullet;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            var cycleCount = Time.CycleCounter.Count32;

            if (cycleCount % Constants.FiringAttractorSpeedDivisor == 0)
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
                    new MovementDeltas(moveDeltas.dx, 0));

                theGameBoard.MoveAdversaryOnePixel(
                    gameObject,
                    new MovementDeltas(0, moveDeltas.dy));

                if ((cycleCount & Constants.FiringAttractorFiringCyclesAndMask) == 0)
                {
                    if (!moveDeltas.Stationary
                        && Rng.Generator.Next(100) < Constants.AttractorFiringProbabilityPercent)
                    {
                        _fireBullet(gameObject.GetBoundingRectangle(), moveDeltas, false);
                    }
                }
            }
        }
    }
}
