
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public static class MissionIISpriteInstanceExtensions
    {
        public static CollisionDetection.WallHitTestResult MoveSpriteInstanceOnePixelConsideringWallsOnly(
            this SpriteInstance spriteInstance, WallMatrix wallMatrix, MovementDeltas movementDeltas)
        {
            var proposedX = spriteInstance.RoomX + movementDeltas.dx;
            var proposedY = spriteInstance.RoomY + movementDeltas.dy;

            var hitResult = CollisionDetection.HitsWalls(
                    wallMatrix,
                    MissionIIGameBoardConstants.TileWidth,
                    MissionIIGameBoardConstants.TileHeight,
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
