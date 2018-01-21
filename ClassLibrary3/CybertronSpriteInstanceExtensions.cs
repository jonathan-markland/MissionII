using System.Collections.Generic;
using GameClassLibrary.Math;

namespace GameClassLibrary
{
    public static class CybertronSpriteInstanceExtensions
    {
        public static CollisionDetection.WallHitTestResult MoveSpriteInstanceOnePixelConsideringWallsOnly(
            this SpriteInstance spriteInstance, WallMatrix wallMatrix, MovementDeltas movementDeltas)
        {
            var proposedX = spriteInstance.RoomX + movementDeltas.dx;
            var proposedY = spriteInstance.RoomY + movementDeltas.dy;

            var hitResult = CollisionDetection.HitsWalls(
                    wallMatrix,
                    CybertronGameBoardConstants.TileWidth,
                    CybertronGameBoardConstants.TileHeight,
                    proposedX,
                    proposedY,
                    spriteInstance.Traits.BoardWidth,
                    spriteInstance.Traits.BoardHeight);

            if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
            {
                spriteInstance.RoomX = proposedX;
                spriteInstance.RoomY = proposedY;
            }

            return hitResult;
        }
    }
}
