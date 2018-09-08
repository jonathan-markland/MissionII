﻿
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
            int tileWidth, int tileHeight,
            MovementDeltas movementDeltas,
            Func<Tile, bool> isFloorFunc)
        {
            var proposedX = spriteInstance.X + movementDeltas.dx;
            var proposedY = spriteInstance.Y + movementDeltas.dy;

            // First consider both X and Y deltas directly:

            var hitResult = CollisionDetection.HitsWalls(
                wallMatrix, tileWidth, tileHeight,
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
    }
}
