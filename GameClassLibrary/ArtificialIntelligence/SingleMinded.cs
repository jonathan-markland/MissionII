
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class SingleMinded
    {
        public static ArtificialIntelligenceFunctions New(
            GameObject gameObject,
            Func<Rectangle, FoundDirections> freeDirectionFinder,
            Action<Rectangle, MovementDeltas, bool> fireBullet,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> tryMoveAdversaryOnePixel)
        {
            int countDown = 0;
            int facingDirection = 0;
            var movementDeltas = MovementDeltas.Stationary;

            return new ArtificialIntelligenceFunctions(() => 
            {
                if (Time.CycleCounter.Count32 % Constants.SingleMindedSpeedDivisor == 0)
                {
                    if (countDown > 0)
                    {
                        --countDown;

                        // --- Do movement --

                        if (!movementDeltas.IsStationary)
                        {
                            var hitResult = tryMoveAdversaryOnePixel(gameObject, movementDeltas);

                            if ((Time.CycleCounter.Count32 & Constants.SingleMindedFiringCyclesAndMask) == 0)
                            {
                                if (!movementDeltas.IsStationary
                                    && Rng.Generator.Next(100) < Constants.SingleMindedFiringProbabilityPercent)
                                {
                                    fireBullet(
                                        gameObject.GetBoundingRectangle(),
                                        MovementDeltas.ConvertFromFacingDirection(facingDirection), false);
                                }
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
                        // --- ChooseNewMovement ---

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
                            countDown = theRng.Next(Constants.SingleMindedMoveDurationCycles) + Constants.SingleMindedMoveDurationCycles;
                            facingDirection = freeDirections.Choose(theRng.Next(freeDirections.Count));
                            movementDeltas = MovementDeltas.ConvertFromFacingDirection(facingDirection);
                        }

                        // --- End of ChooseNewMovement ---
                    }
                }
            });
        }
    }
}
