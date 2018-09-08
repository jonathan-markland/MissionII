using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls.Clusters;

namespace GameClassLibrary.Walls
{
    public static class DirectionFinder
    {
        public static FoundDirections GetFreeDirections(
            Rectangle currentExtents, 
            TileMatrix currentRoomWallData, 
            int tileWidth, int tileHeight,
            Func<Tile, bool> isFloor)
        {
            int countFound = 0;
            int resultMask = 0;
            for (int directionIndex = 0; directionIndex < 8; directionIndex++)
            {
                var movementDelta = MovementDeltas.ConvertFromFacingDirection(directionIndex);

                var hitResult = CollisionDetection.HitsWalls(
                    currentRoomWallData, tileWidth, tileHeight, 
                    currentExtents.Left, currentExtents.Top, 
                    currentExtents.Width, currentExtents.Height, 
                    isFloor);

                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
                {
                    resultMask |= 1 << directionIndex;
                    ++countFound;
                }
            }
            return new FoundDirections(resultMask, countFound);
        }



        public static int GetDirectionFacingAwayFromWalls(
            TileMatrix fileWallData, Point startCluster, int sourceClusterSide,
            Func<Tile, bool> isFloorFunc)
        {
            var clusterCanvas = new ClusterReader(
                fileWallData, startCluster.X, startCluster.Y, sourceClusterSide, isFloorFunc);

            // Note this is a priority order:
            if (clusterCanvas.IsFloor(8)) return 4; // FACING DOWN
            if (clusterCanvas.IsFloor(6)) return 2; // FACING RIGHT
            if (clusterCanvas.IsFloor(4)) return 6; // FACING LEFT
            if (clusterCanvas.IsFloor(2)) return 0; // FACING UP
            throw new Exception("Cannot establish an exit direction, all sides of cluster have walls.");
        }



    }
}
