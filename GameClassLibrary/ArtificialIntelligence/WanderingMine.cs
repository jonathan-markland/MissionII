
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class WanderingMine
    {
        public static ArtificialIntelligenceFunctions NewWanderingMine(
            GameObject gameObject,
            Func<Rectangle, FoundDirections> freeDirectionFinder, Action<GameObject> manWalksIntoDroidAction, int speedDivisor,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> tryMoveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            int countDown = 0;
            int facingDirection = 0;
            var movementDeltas = MovementDeltas.Stationary;

            return new ArtificialIntelligenceFunctions(() =>
            {
                if (Time.CycleCounter.Count32 % speedDivisor == 0)
                {
                    if (countDown > 0)
                    {
                        // --- Do movement ---

                        --countDown;
                        if (!movementDeltas.IsStationary)
                        {
                            var hitResult = tryMoveAdversaryOnePixel(gameObject, movementDeltas);

                            // Check proximity to man, and detonate killing man:

                            var detonationRectangle = getManExtents().Inflate(5); // TODO: constant
                            if (gameObject.GetBoundingRectangle().Intersects(detonationRectangle))
                            {
                                manWalksIntoDroidAction(gameObject);
                                // TODO: Droid (the gameObject) should detonate 
                            }

                            if (hitResult != CollisionDetection.WallHitTestResult.NothingHit)
                            {
                                countDown = 0;
                            }
                        }

                        // --- End Do movement ---
                    }
                    else
                    {
                        // --- Choose new movement ---

                        var currentExtents = gameObject.GetBoundingRectangle();
                        var theRng = Rng.Generator;
                        var freeDirections = freeDirectionFinder(currentExtents);
                        if (freeDirections.Count == 0)
                        {
                            // Can't move.
                            countDown = 0;
                            movementDeltas = MovementDeltas.Stationary;
                        }
                        else
                        {
                            countDown = theRng.Next(Constants.WanderingMineMoveDurationCycles) + Constants.WanderingMineMoveDurationCycles;
                            facingDirection = freeDirections.Choose(theRng.Next(freeDirections.Count));
                            movementDeltas = MovementDeltas.ConvertFromFacingDirection(facingDirection);
                        }

                        // --- End Choose new movement ---
                    }
                }
            });
        }
    }
}
