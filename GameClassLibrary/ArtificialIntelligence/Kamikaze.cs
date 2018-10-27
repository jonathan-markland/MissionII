
using System;
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Walls;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class Kamikaze
    {
        public static ArtificialIntelligenceFunctions New(
            GameObject gameObject,
            Action manDestroyAction,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            return new ArtificialIntelligenceFunctions(
                () =>
                {
                    // if ((Time.CycleCounter.Count32 & 1) == 0)
                    {
                        var moveDeltas = gameObject.GetBoundingRectangle().GetMovementDeltasToHeadTowards(getManExtents());

                        // We must separate horizontal and vertical movement in order to avoid
                        // things getting 'stuck' on walls because they can't move horizontally
                        // into the wall, but can moe vertically downward.  Trying to do both
                        // directions at once results in rejection of the move, and the
                        // sticking problem.

                        moveAdversaryOnePixel(gameObject, moveDeltas.XComponent);
                        moveAdversaryOnePixel(gameObject, moveDeltas.YComponent);  // TODO: Resolve these within the function (one call only).

                        // Check proximity to man, and detonate killing man:

                        var detonationRectangle = getManExtents().Inflate(5); // TODO: constant
                        if (gameObject.GetBoundingRectangle().Intersects(detonationRectangle))
                        {
                            manDestroyAction();
                        }
                    }
                });
        }
    }
}
