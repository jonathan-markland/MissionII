
using System;
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
            TileMatrix wallMatrix,
            MovementDeltas movementDeltas,
            Func<Tile, bool> isFloorFunc)
        {
            var proposedX = spriteInstance.X + movementDeltas.dx;
            var proposedY = spriteInstance.Y + movementDeltas.dy;

            // First consider both X and Y deltas directly:

            var hitResult = CollisionDetection.HitsWalls(
                wallMatrix,
                proposedX, proposedY,
                spriteInstance.Traits.Width,
                spriteInstance.Traits.Height,
                isFloorFunc);

            if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
            {
                spriteInstance.X = proposedX;
                spriteInstance.Y = proposedY;
            }

            return hitResult;
        }






        public static MovementDeltas GetMovementDeltasToHeadTowards(
            this SpriteInstance aggressorSprite, 
            SpriteInstance targetSprite)
        {
            var targetCentre = targetSprite.Centre;
            var aggressorCentre = aggressorSprite.Centre;

            int dx = 0;
            if (targetCentre.X < aggressorCentre.X) dx = -1;
            if (targetCentre.X > aggressorCentre.X) dx = 1;

            int dy = 0;
            if (targetCentre.Y < aggressorCentre.Y) dy = -1;
            if (targetCentre.Y > aggressorCentre.Y) dy = 1;

            return new MovementDeltas(dx, dy);
        }
    }
}
