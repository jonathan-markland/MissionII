
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls.Clusters;

namespace GameClassLibrary.Walls
{
    public static class DirectionFinder
    {
        /// <summary>
        /// Find the free directions surrounding an object.
        /// We test a 1-pixel strip just outside the extents of the object given.
        /// All strips that lie in unobstructed space are deemed to be 'free' directions.
        /// </summary>
        /// <param name="objectExtents">Extents of object to consider.</param>
        /// <param name="isSpace">Function to test if rectangle is entirely free of obstructions.</param>
        /// <returns>FoundDirections value contains the free directions, if any.</returns>
        public static FoundDirections GetFreeDirections(
            Rectangle objectExtents, 
            Func<Rectangle, bool> isSpace)
        {
            int countFound = 0;
            int resultMask = 0;

            for (int directionIndex = 0; directionIndex < 8; directionIndex++)
            {
                //
                //     objectExtents       Areas tested (direction numbers indicated).
                //                         - 1,3,5,7 just test corner pixels.
                //
                //                           7<--0-->1
                //        +-----+            ^+-----+^
                //        |#####|            ||#####||
                //        |#####|            6|#####|2
                //        |#####|            ||#####||
                //        +-----+            v+-----+v
                //                           5<--4-->3
                //

                var movementDelta = MovementDeltas.ConvertFromFacingDirection(directionIndex);

                var newWidth = movementDelta.dx == 0 ? objectExtents.Width : 1;
                var newHeight = movementDelta.dy == 0 ? objectExtents.Height : 1;

                var dx = (movementDelta.dx > 0) ? objectExtents.Width : movementDelta.dx;
                var dy = (movementDelta.dy > 0) ? objectExtents.Height : movementDelta.dy;

                if (isSpace(new Rectangle(objectExtents.Left + dx, objectExtents.Top + dy, newWidth, newHeight)))
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
