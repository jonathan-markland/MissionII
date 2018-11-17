
using System;
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class Kamikaze
    {
        public static ArtificialIntelligenceFunctions NewKamikaze(
            GameObject gameObject,
            Action manDestroyAction,
            Action<GameObject, MovementDeltas> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            return new ArtificialIntelligenceFunctions(
                () =>
                {
                    var moveDeltas = gameObject.GetBoundingRectangle().GetMovementDeltasToHeadTowards(getManExtents());
                    moveAdversaryOnePixel(gameObject, moveDeltas);

                    // Check proximity to man, and detonate killing man:

                    var detonationRectangle = getManExtents().Inflate(5); // TODO: constant
                    if (gameObject.GetBoundingRectangle().Intersects(detonationRectangle))
                    {
                        manDestroyAction();
                    }
                });
        }
    }
}
