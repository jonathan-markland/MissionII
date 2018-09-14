
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public static class GameObjectExtensions // TODO: move back into the librarry
    {
        public static void MoveBy(this GameObject gameObject, MovementDeltas movementDeltas)
        {
            var p = gameObject.TopLeftPosition;
            gameObject.TopLeftPosition = new Point(p.X + movementDeltas.dx, p.Y + movementDeltas.dy);
        }

        public static bool Intersects(this GameObject gameObject, GameObject otherObject)
        {
            return gameObject.GetBoundingRectangle().Intersects(otherObject.GetBoundingRectangle());
        }

        /// <summary>
        /// It is advised that the movement is by ONE pixel at a time.
        /// </summary>
        public static CollisionDetection.WallHitTestResult MoveConsideringWallsOnly(
            this GameObject gameObject,
            TileMatrix wallMatrix,
            MovementDeltas movementDeltas,
            Func<Tile, bool> isFloorFunc)
        {
            var r = gameObject.GetBoundingRectangle();
            var proposedX = r.Left + movementDeltas.dx;
            var proposedY = r.Top + movementDeltas.dy;

            // First consider both X and Y deltas directly:

            var hitResult = CollisionDetection.HitsWalls(
                wallMatrix,
                proposedX, proposedY,
                r.Width,
                r.Height,
                isFloorFunc);

            if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
            {
                gameObject.TopLeftPosition = new Point(proposedX, proposedY);
            }

            return hitResult;
        }



        public static MovementDeltas GetMovementDeltasToHeadTowards(
            this GameObject aggressorSprite,
            GameObject targetSprite)
        {
            var targetCentre = targetSprite.GetBoundingRectangle().Centre;
            var aggressorCentre = aggressorSprite.GetBoundingRectangle().Centre;

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
