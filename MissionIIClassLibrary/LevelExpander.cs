using System;
using System.Collections.Generic;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Walls.Clusters;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public static class LevelExpander
    {
        public const int ExpandSize = 5;



        public static List<Level> ExpandWallsInWorld(
            List<Level> sourceLevelsList,
            Func<int, uint[]> resamplingPixelArrayProvider)
        {
            var expandedLevelList = new List<Level>();
            var levelIndex = 0;

            foreach (var thisLevel in sourceLevelsList)
            {
                expandedLevelList.Add(ExpandWallsInLevel(thisLevel, resamplingPixelArrayProvider(levelIndex)));
                ++levelIndex;
            }

            return expandedLevelList;
        }



        private static Level ExpandWallsInLevel(
            Level thisLevel, 
            uint[] resamplingColourData)
        {
            // Expand all the rooms from the size they are in the source
            // text files to the size they need to be for targetting the
            // screen.

            var expandedLevelMatrix = GetExpandedLevelTileMatrix(thisLevel.LevelTileMatrix);
            AlignDoorways(expandedLevelMatrix);
            DecorateLevel(expandedLevelMatrix, resamplingColourData);
            return new Level(thisLevel.LevelNumber, expandedLevelMatrix.WholeArea, thisLevel.SpecialMarkers);
        }



        private static WriteableArraySlice2D<Tile> GetExpandedLevelTileMatrix(ArraySlice2D<Tile> levelTileMatrix)
        {
            return new WallExpander(
                    levelTileMatrix,
                    Constants.ClustersHorizontally * Constants.RoomsHorizontally,
                    Constants.ClustersVertically * Constants.RoomsVertically,
                    Constants.SourceClusterSide,
                    Constants.DestClusterSide,
                    TileExtensions.IsFloor,
                    MissionIITile.ElectricWall,
                    MissionIITile.Floor)
                        .GetExpandedWalls();
        }



        private static void DecorateLevel(WriteableArraySlice2D<Tile> levelMatrix, uint[] resamplingColourData)
        {
            int n = 5; // TODO sort out a seed

            AddDecorativeBrickwork(levelMatrix);

            // Set style pattern on the wall squares:

            SetWallStyleDeltas(
                levelMatrix,
                resamplingColourData,
                n * 8,
                n * 4,
                128,
                true);

            // Set style pattern on the floor squares:

            SetWallStyleDeltas(
                levelMatrix,
                resamplingColourData,
                n * 4,
                n * 8,
                128,
                false);
        }



        private static void AlignDoorways(WriteableArraySlice2D<Tile> levelMatrix)
        {
            for (int y = 0; y < Constants.RoomsVertically; y++)
            {
                for (int x = 0; x < Constants.RoomsHorizontally - 1; x++)
                {
                    AlignDoorwaysScan(
                        levelMatrix, 
                        new Point(24, 0) + TopLeftOfRoom(x,y),
                        new Point(0, 0) + TopLeftOfRoom(x+1, y),
                        new MovementDeltas(0, 1),
                        25);
                }
            }

            for (int y=0; y<Constants.RoomsVertically-1; y++)
            {
                for (int x = 0; x < Constants.RoomsHorizontally; x++)
                {
                    AlignDoorwaysScan(
                        levelMatrix,
                        new Point(0, 24) + TopLeftOfRoom(x, y),
                        new Point(0, 0) + TopLeftOfRoom(x, y+1),
                        new MovementDeltas(1, 0),
                        25);
                }
            }
        }



        private static MovementDeltas TopLeftOfRoom(int rx, int ry)
        {
            return new MovementDeltas(
                rx * Constants.ClustersHorizontally * Constants.DestClusterSide,
                ry * Constants.ClustersVertically * Constants.DestClusterSide);
        }

        

        private static void AlignDoorwaysScan(
            WriteableArraySlice2D<Tile> levelMatrix, 
            Point point1,
            Point point2,
            MovementDeltas movementDeltas,
            int blockCount)
        {
            var readableView = levelMatrix.WholeArea;  // TODO:  a bit hackish

            while (blockCount > 0)
            {
                if (   !readableView.At(point1).IsFloor()
                    || !readableView.At(point2).IsFloor())
                {
                    levelMatrix.Write(point1, MissionIITile.ElectricWall);
                    levelMatrix.Write(point2, MissionIITile.ElectricWall);
                }
                point1 = point1 + movementDeltas;
                point2 = point2 + movementDeltas;
                --blockCount;
            }
        }



        private static void AddDecorativeBrickwork(WriteableArraySlice2D<Tile> wallMatrix)
        {
            // Turn Electric areas into Brick leaving just an Electric outline.

            int xe = wallMatrix.CountH - 1;
            int ye = wallMatrix.CountV - 1;

            for (int y = 1; y < ye; ++y)
            {
                for (int x = 1; x < xe; ++x)
                {
                    if (SurroundedByWall8(wallMatrix, x, y))
                    {
                        wallMatrix.Write(x, y, MissionIITile.Wall);
                    }
                }
            }
        }



        private static bool SurroundedByWall4(WriteableArraySlice2D<Tile> wallMatrix, int x, int y) // TODO: We could be arty and call this instead.
        {
            return
                   !wallMatrix.At(x, y - 1).IsFloor()
                && !wallMatrix.At(x, y + 1).IsFloor()
                && !wallMatrix.At(x - 1, y).IsFloor()
                && !wallMatrix.At(x + 1, y).IsFloor();
        }



        private static bool SurroundedByWall8(WriteableArraySlice2D<Tile> wallMatrix, int x, int y)
        {
            return
                   !wallMatrix.At(x, y - 1).IsFloor()
                && !wallMatrix.At(x, y + 1).IsFloor()
                && !wallMatrix.At(x - 1, y).IsFloor()
                && !wallMatrix.At(x + 1, y).IsFloor()
                && !wallMatrix.At(x - 1, y - 1).IsFloor()
                && !wallMatrix.At(x + 1, y - 1).IsFloor()
                && !wallMatrix.At(x - 1, y + 1).IsFloor()
                && !wallMatrix.At(x + 1, y + 1).IsFloor();
        }



        private static void SetWallStyleDeltas(
            WriteableArraySlice2D<Tile> wallMatrix,
            uint[] resamplingImageArray,
            int logicalOffsetX,
            int logicalOffsetY,
            int sampleThreshold,
            bool doWalls)
        {
            System.Diagnostics.Debug.Assert(resamplingImageArray.Length == 64*64);

            for (int y=0; y < wallMatrix.CountV; y++)
            {
                int cy = (y + logicalOffsetY) & 63;
                for (int x = 0; x < wallMatrix.CountH; x++)
                {
                    if ((wallMatrix.At(x, y).IsFloor()) ^ doWalls)
                    {
                        int cx = (x + logicalOffsetX) & 63;
                        var greyLevel = Colour.GetGreyLevel(resamplingImageArray[cy * 64 + cx]);
                        var styleDelta = (byte)((greyLevel < sampleThreshold) ? 0 : 1);
                        var item = (byte)(wallMatrix.At(x, y).VisualIndex | styleDelta);
                        wallMatrix.Write(x, y, new Tile { VisualIndex = item });
                    }
                }
            }
        }


    }
}
