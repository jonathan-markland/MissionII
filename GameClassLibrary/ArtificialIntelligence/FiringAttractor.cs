
using System;
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Walls;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class FiringAttractor
    {
        public static ArtificialIntelligenceFunctions New(
            GameObject gameObject,
            Action<Rectangle, MovementDeltas, bool> fireBullet,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            return new ArtificialIntelligenceFunctions(
                () =>
                {
                    var cycleCount = Time.CycleCounter.Count32;

                    if (cycleCount % Constants.FiringAttractorSpeedDivisor == 0)
                    {
                        var moveDeltas = gameObject.GetBoundingRectangle().GetMovementDeltasToHeadTowards(
                            getManExtents());

                        // We must separate horizontal and vertical movement in order to avoid
                        // things getting 'stuck' on walls because they can't move horizontally
                        // into the wall, but can moe vertically downward.  Trying to do both
                        // directions at once results in rejection of the move, and the
                        // sticking problem.

                        moveAdversaryOnePixel(gameObject, moveDeltas.XComponent);
                        moveAdversaryOnePixel(gameObject, moveDeltas.YComponent);

                        if ((cycleCount & Constants.FiringAttractorFiringCyclesAndMask) == 0)
                        {
                            if (!moveDeltas.IsStationary
                                && Rng.Generator.Next(100) < Constants.AttractorFiringProbabilityPercent)
                            {
                                fireBullet(gameObject.GetBoundingRectangle(), moveDeltas, false);
                            }
                        }
                    }
                });
        }
    }
}
