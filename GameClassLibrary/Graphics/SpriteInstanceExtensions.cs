
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace GameClassLibrary.Graphics
{
    public static class SpriteInstanceExtensions
    {
        /// <summary>
        /// It is advised that the movement is by ONE pixel at a time.
        /// </summary>
        public static CollisionDetection.WallHitTestResult MoveConsideringWallsOnly(
            this SpriteInstance spriteInstance, 
            WallMatrix wallMatrix, 
            int tileWidth, int tileHeight,
            MovementDeltas movementDeltas)
        {
            var proposedX = spriteInstance.RoomX + movementDeltas.dx;
            var proposedY = spriteInstance.RoomY + movementDeltas.dy;

            // First consider both X and Y deltas directly:

            var hitResult = CollisionDetection.HitsWalls(
                wallMatrix, tileWidth, tileHeight,
                proposedX, proposedY,
                spriteInstance.Traits.BoardWidth,
                spriteInstance.Traits.BoardHeight);

            if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
            {
                spriteInstance.RoomX = proposedX;
                spriteInstance.RoomY = proposedY;
            }
            else if (hitResult == CollisionDetection.WallHitTestResult.HitWall)
            {
                // Try X component of motion only:

                hitResult = CollisionDetection.HitsWalls(
                    wallMatrix, tileWidth, tileHeight,
                    proposedX, spriteInstance.RoomY,
                    spriteInstance.Traits.BoardWidth,
                    spriteInstance.Traits.BoardHeight);

                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
                {
                    spriteInstance.RoomX = proposedX;
                    return hitResult; // must have been the Y that stopped the diagonal case working!
                }
                else if (hitResult == CollisionDetection.WallHitTestResult.HitWall)
                {
                    // Try Y component of motion only:

                    hitResult = CollisionDetection.HitsWalls(
                        wallMatrix, tileWidth, tileHeight,
                        spriteInstance.RoomX, proposedY,
                        spriteInstance.Traits.BoardWidth,
                        spriteInstance.Traits.BoardHeight);

                    if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
                    {
                        spriteInstance.RoomY = proposedY;
                        return hitResult; // must have been the X that stopped the diagonal case working!
                    }
                }
            }

            return hitResult;
        }
    }
}
