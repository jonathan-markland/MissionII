
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace GameClassLibrary.GameBoard
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Move this game object by the movement delta.
        /// </summary>
        public static void MoveBy(this GameObject gameObject, MovementDeltas movementDeltas)
        {
            var p = gameObject.TopLeftPosition;
            gameObject.TopLeftPosition = new Point(p.X + movementDeltas.dx, p.Y + movementDeltas.dy);
        }



        /// <summary>
        /// Find if this game object intersects the other.
        /// </summary>
        public static bool Intersects(this GameObject gameObject, GameObject otherObject)
        {
            return gameObject.GetBoundingRectangle().Intersects(otherObject.GetBoundingRectangle());
        }



        /// <summary>
        /// It is advised that the movement is by ONE pixel at a time, and allows diagonal movement.
        /// </summary>
        public static CollisionDetection.WallHitTestResult MoveConsideringWallsOnly(
            this GameObject gameObject,
            TileMatrix wallMatrix,
            Rectangle roomArea,
            MovementDeltas movementDeltas,
            Func<Tile, bool> isFloorFunc)
        {
            var r = gameObject.GetBoundingRectangle();
            var proposedX = r.Left + movementDeltas.dx;
            var proposedY = r.Top + movementDeltas.dy;

            var hitResult = CollisionDetection.HitsWalls(
                wallMatrix.WholeArea,
                roomArea,
                proposedX, proposedY,
                r.Width,
                r.Height,
                wallMatrix.TileWidth,
                wallMatrix.TileHeight,
                isFloorFunc);

            if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
            {
                gameObject.TopLeftPosition = new Point(proposedX, proposedY);
            }

            return hitResult;
        }



        /// <summary>
        /// Obtain movement deltas necessary to head towards another object.
        /// </summary>
        public static MovementDeltas GetMovementDeltasToHeadTowards(
            this GameObject aggressorSprite,
            GameObject targetSprite)
        {
            return aggressorSprite.GetBoundingRectangle().Centre.GetMovementDeltasToHeadTowards(
                targetSprite.GetBoundingRectangle().Centre);
        }
    }
}
