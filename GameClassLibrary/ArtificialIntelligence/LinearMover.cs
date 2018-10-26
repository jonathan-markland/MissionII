
using System;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class LinearMover
    {
        public static ArtificialIntelligenceFunctions New(
            GameObject gameObject,
            Point startPoint, Point endPoint, int movesPerCycle, Action<GameObject> manWalksIntoDroidAction,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            bool headToEnd = true;

            return new ArtificialIntelligenceFunctions(() => 
            {
                for (int i = 0; i < movesPerCycle; i++)
                {
                    var currentPosition = gameObject.TopLeftPosition;

                    if (currentPosition == startPoint)
                    {
                        headToEnd = true;
                    }
                    else if (currentPosition == endPoint)
                    {
                        headToEnd = false;
                    }

                    var r = gameObject.GetBoundingRectangle();
                    var moveDeltas = new Point(r.Left, r.Top).GetMovementDeltasToHeadTowards(
                        headToEnd ? endPoint : startPoint);

                    gameObject.MoveBy(moveDeltas);

                    if (gameObject.GetBoundingRectangle().Intersects(getManExtents()))
                    {
                        manWalksIntoDroidAction(gameObject);
                    }
                }
            });
        }
    }
}
