﻿using System;
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
            Func<Tile, bool> isSpace)
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
                    isSpace);

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
            Func<Tile, bool> isSpaceFunc)
        {
            var clusterCanvas = new ClusterReader(
                fileWallData, startCluster.X, startCluster.Y, sourceClusterSide, isSpaceFunc);

            // Note this is a priority order:
            if (clusterCanvas.IsSpace(8)) return 4; // FACING DOWN
            if (clusterCanvas.IsSpace(6)) return 2; // FACING RIGHT
            if (clusterCanvas.IsSpace(4)) return 6; // FACING LEFT
            if (clusterCanvas.IsSpace(2)) return 0; // FACING UP
            throw new Exception("Cannot establish an exit direction, all sides of cluster have walls.");
        }



    }
}
