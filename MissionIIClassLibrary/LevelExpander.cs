
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


        public static void ExpandWallsInWorld(
            WorldWallData theWorld, 
            SpriteTraits wallPatternResamplingSprite) // TODO: Should we avoid dependency on Graphics?  Pass in the resample array instead?
        {
            var levelIndex = 0;

            foreach(var thisLevel in theWorld.Levels)
            {
                var resamplingImageIndex = levelIndex % wallPatternResamplingSprite.ImageCount;
                var roomsList = thisLevel.Rooms;

                // Expand all the rooms from the size they are in the source
                // text files to the size they need to be for targetting the
                // screen.

                foreach (var thisRoom in roomsList)
                {
                    thisRoom.WallData = new WallExpander(
                        thisRoom.FileWallData, 
                        Constants.ClustersHorizontally,
                        Constants.ClustersVertically,
                        Constants.SourceClusterSide,
                        Constants.DestClusterSide).GetExpandedWalls();
                }

                // Now we know all the rooms, ensure the doorways line up.

                AlignDoorways(roomsList);

                // Now add decorative brickwork via a filter, and decorative
                // "style deltas" which are zero based values that can be used
                // to choose alternate sprites on a per-brick basis.

                var resamplingColourData = Business.SpriteToUintArray(
                    wallPatternResamplingSprite.GetHostImageObject(resamplingImageIndex));

                foreach (var thisRoom in roomsList)
                {
                    AddDecorativeBrickwork(thisRoom.WallData);

                    // Set style pattern on the wall squares:

                    SetWallStyleDeltas(
                        thisRoom.WallData,
                        resamplingColourData, 
                        thisRoom.RoomNumber * 8,
                        thisRoom.RoomNumber * 4,
                        128, 
                        true);

                    // Set style pattern on the floor squares:

                    SetWallStyleDeltas(
                        thisRoom.WallData,
                        resamplingColourData,
                        thisRoom.RoomNumber * 4,
                        thisRoom.RoomNumber * 8,
                        128,
                        false);
                }

                ++levelIndex;
            }
        }



        private static void AlignDoorways(List<Room> roomsList)
        {
            for (int y = 0; y < Constants.RoomsVertically; y++)
            {
                for (int x = 0; x < Constants.RoomsHorizontally - 1; x++)
                {
                    AlignDoorwaysGoingLeftRight(
                        roomsList[y * Constants.RoomsHorizontally + x].WallData,
                        roomsList[y * Constants.RoomsHorizontally + x + 1].WallData);
                }
            }

            for (int y=0; y<Constants.RoomsVertically-1; y++)
            {
                for (int x = 0; x < Constants.RoomsHorizontally; x++)
                {
                    AlignDoorwaysGoingUpDown(
                        roomsList[y * Constants.RoomsHorizontally + x].WallData,
                        roomsList[(y+1) * Constants.RoomsHorizontally + x].WallData);
                }
            }
        }



        private static void AlignDoorwaysGoingLeftRight(WallMatrix room1, WallMatrix room2)
        {
            AlignDoorwaysScan(
                room1, new Point(24, 0), 
                room2, new Point(0, 0), 
                new MovementDeltas(0, 1), 
                25);
        }



        private static void AlignDoorwaysGoingUpDown(WallMatrix room1, WallMatrix room2)
        {
            AlignDoorwaysScan(
                room1, new Point(0, 24),
                room2, new Point(0, 0),
                new MovementDeltas(1, 0),
                25);
        }



        private static void AlignDoorwaysScan(
            WallMatrix room1, Point point1,
            WallMatrix room2, Point point2,
            MovementDeltas movementDeltas,
            int blockCount)
        {
            while (blockCount > 0)
            {
                if (   room1.Read(point1) != WallMatrixChar.Space
                    || room2.Read(point2) != WallMatrixChar.Space)
                {
                    room1.Write(point1, WallMatrixChar.Electric);
                    room2.Write(point2, WallMatrixChar.Electric);
                }
                point1 = point1 + movementDeltas;
                point2 = point2 + movementDeltas;
                --blockCount;
            }
        }



        private static void AddDecorativeBrickwork(WallMatrix expandedData)
        {
            // Turn Electric areas into Brick leaving just an Electric outline.

            for (int y = 1; y < 24; ++y)
            {
                for (int x = 1; x < 24; ++x)
                {
                    if (SurroundedByWall8(expandedData, x, y))
                    {
                        expandedData.Write(x, y, WallMatrixChar.Brick);
                    }
                }
            }
        }



        private static bool SurroundedByWall4(WallMatrix wallMatrix, int x, int y) // TODO: We could be arty and call this instead.
        {
            return
                   wallMatrix.Read(x, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x, y + 1) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y) != WallMatrixChar.Space;
        }



        private static bool SurroundedByWall8(WallMatrix wallMatrix, int x, int y)
        {
            return
                   wallMatrix.Read(x, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x, y + 1) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y + 1) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y + 1) != WallMatrixChar.Space;
        }



        private static void SetWallStyleDeltas(
            WallMatrix wallData,
            uint[] resamplingImageArray,
            int logicalOffsetX,
            int logicalOffsetY,
            int sampleThreshold,
            bool doWalls)
        {
            System.Diagnostics.Debug.Assert(resamplingImageArray.Length == 64*64);

            for (int y=0; y < wallData.CountV; y++)
            {
                int cy = (y + logicalOffsetY) & 63;
                for (int x = 0; x < wallData.CountH; x++)
                {
                    if ((wallData.Read(x, y) == WallMatrixChar.Space) ^ doWalls)
                    {
                        int cx = (x + logicalOffsetX) & 63;
                        var greyLevel = Business.ToGreyscale(resamplingImageArray[cy * 64 + cx]);
                        wallData.SetStyleDelta(x, y, (byte)((greyLevel < sampleThreshold) ? 0 : 1));
                    }
                }
            }
        }


    }
}
