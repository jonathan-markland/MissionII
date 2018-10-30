
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
            Action<GameObject, MovementDeltas> moveAdversaryOnePixel,
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

                        moveAdversaryOnePixel(gameObject, moveDeltas);

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
