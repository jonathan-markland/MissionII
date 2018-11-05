
using System;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class Attractor
    {
        public static ArtificialIntelligenceFunctions NewAttractor(
            GameObject gameObject,
            Action<GameObject, MovementDeltas> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            return new ArtificialIntelligenceFunctions(
                () =>
                {
                    if ((Time.CycleCounter.Count32 & 1) == 0)
                    {
                        var moveDeltas = gameObject.GetBoundingRectangle().GetMovementDeltasToHeadTowards(getManExtents());
                        moveAdversaryOnePixel(gameObject, moveDeltas);
                    }
                });
        }
    }
}
