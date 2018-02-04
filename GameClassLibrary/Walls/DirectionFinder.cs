using GameClassLibrary.Math;

namespace GameClassLibrary.Walls
{
    public static class DirectionFinder
    {
        public static FoundDirections GetFreeDirections(
            Rectangle currentExtents, 
            WallMatrix currentRoomWallData, 
            int tileWidth, int tileHeight)
        {
            int countFound = 0;
            int resultMask = 0;
            for (int directionIndex = 0; directionIndex < 8; directionIndex++)
            {
                var movementDelta = Business.GetMovementDeltas(directionIndex);
                var hitResult = CollisionDetection.HitsWalls(currentRoomWallData, tileWidth, tileHeight, currentExtents.Left, currentExtents.Top, currentExtents.Width, currentExtents.Height);
                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
                {
                    resultMask |= 1 << directionIndex;
                    ++countFound;
                }
            }
            return new FoundDirections { Count = countFound, DirectionsMask = resultMask };
        }
    }
}
