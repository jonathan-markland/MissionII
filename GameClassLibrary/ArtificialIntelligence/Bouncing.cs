
using System;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class Bouncing
    {
        public static ArtificialIntelligenceFunctions NewBouncing(
            GameObject gameObject,
            int leftLimit, int rightLimit, int maxBounceHeightOffFloor, int movesPerCycle,
            Action<GameObject> manWalksIntoDroidAction, int initialDx,
            Action<GameObject, MovementDeltas> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            var movementDeltas = new MovementDeltas(initialDx, -1);  // Force upwards motion initially.
            bool firstCycle = true;
            int floorFoundY = 0;

            return new ArtificialIntelligenceFunctions(
                () =>
                {
                    if (firstCycle)
                    {
                        firstCycle = false;
                        floorFoundY = gameObject.TopLeftPosition.Y;
                    }

                    for (int i = 0; i < movesPerCycle; i++)
                    {
                        var positionBefore = gameObject.TopLeftPosition;

                        if (positionBefore.X == leftLimit && movementDeltas.MovingLeft) { }
                        else if (positionBefore.X == rightLimit && movementDeltas.MovingRight) { }
                        else
                        {
                            moveAdversaryOnePixel(gameObject, movementDeltas.XComponent);
                        }

                        if (movementDeltas.MovingUp && positionBefore.Y == (floorFoundY - maxBounceHeightOffFloor)) { }
                        else
                        {
                            moveAdversaryOnePixel(gameObject, movementDeltas.YComponent);
                        }

                        var newPosition = gameObject.TopLeftPosition;

                        if (positionBefore.X == newPosition.X)
                        {
                            movementDeltas = movementDeltas.ReverseX;
                        }
                        if (positionBefore.Y == newPosition.Y)
                        {
                            movementDeltas = movementDeltas.ReverseY;

                            if (movementDeltas.MovingUp)
                            {
                                // Remember where the floor is, in order to limit bounce height.
                                floorFoundY = newPosition.Y;
                            }
                        }

                        if (gameObject.GetBoundingRectangle().Intersects(getManExtents()))
                        {
                            manWalksIntoDroidAction(gameObject);
                        }
                    }
                });
        }
    }
}
