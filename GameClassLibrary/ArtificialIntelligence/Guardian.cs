
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class Guardian
    {
        public static ArtificialIntelligenceFunctions New(
            GameObject gameObject,
            Action<GameObject> manWalksIntoDroidAction,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            int facingDirection = 0;
            var movementDeltas = MovementDeltas.Stationary;

            return new ArtificialIntelligenceFunctions(()=>
            {
                if (movementDeltas.IsStationary)
                {
                    facingDirection = GameClassLibrary.Math.Rng.Generator.Next(8);
                    movementDeltas = MovementDeltas.ConvertFromFacingDirection(facingDirection);
                }
                else
                {
                    var hitResult = moveAdversaryOnePixel(gameObject, movementDeltas);  // TODO: differentiate walls/other droids
                    if (hitResult != CollisionDetection.WallHitTestResult.NothingHit)
                    {
                        if (gameObject.GetBoundingRectangle().Intersects(
                            getManExtents().Inflate(5)))
                        {
                            manWalksIntoDroidAction(gameObject);
                        }
                        else
                        {
                            facingDirection = (facingDirection + 4) & 7;  // TODO: reverse direction function
                            movementDeltas = MovementDeltas.ConvertFromFacingDirection(facingDirection);
                        }
                    }
                }
            });
        }
    }
}
