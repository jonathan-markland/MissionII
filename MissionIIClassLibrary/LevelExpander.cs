
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
            SpriteTraits wallPatternResamplingSprite) // TODO: Should we avoid dependency on Graphics?  Pass in the resample array instead?
        {
            var expandedLevelList = new List<Level>();
            var levelIndex = 0;

            foreach (var thisLevel in sourceLevelsList)
            {
                var resamplingImageIndex = levelIndex % wallPatternResamplingSprite.ImageCount;
                expandedLevelList.Add(ExpandWallsInLevel(resamplingImageIndex, thisLevel, wallPatternResamplingSprite));
                ++levelIndex;
            }

            return expandedLevelList;
        }



        private static Level ExpandWallsInLevel(
            int resamplingImageIndex, 
            Level thisLevel, 
            SpriteTraits wallPatternResamplingSprite)
        {
            // Expand all the rooms from the size they are in the source
            // text files to the size they need to be for targetting the
            // screen.

            var roomsList = thisLevel.Rooms;
            var expandedRoomMatrices = GetListOfExpandedRoomMatrices(roomsList);

            // Now we know all the rooms, ensure the doorways line up.

            AlignDoorways(expandedRoomMatrices);

            // Now add decorative brickwork via a filter, and decorative
            // "style deltas" which are zero based values that can be used
            // to choose alternate sprites on a per-brick basis.

            var resamplingColourData =
                wallPatternResamplingSprite.GetHostImageObject(resamplingImageIndex).ToArray();

            var newRoomsList = new List<Room>();

            for (int i = 0; i < roomsList.Count; i++)
            {
                newRoomsList.Add(GetDecoratedRoom(expandedRoomMatrices[i], roomsList[i], resamplingColourData));
            }

            return new Level(thisLevel.LevelNumber, newRoomsList, thisLevel.SpecialMarkers);
        }



        private static List<WriteableWallMatrix> GetListOfExpandedRoomMatrices(List<Room> roomsList)
        {
            var listOfWriteableRoomMatrices = new List<WriteableWallMatrix>();

            foreach (var thisRoom in roomsList)
            {
                listOfWriteableRoomMatrices.Add(
                    new WallExpander(
                        thisRoom.WallData,
                        Constants.ClustersHorizontally,
                        Constants.ClustersVertically,
                        Constants.SourceClusterSide,
                        Constants.DestClusterSide,
                        MissionIITile.Electric,
                        MissionIITile.Space,
                        MissionIIGameBoard.IsSpace)
                            .GetExpandedWalls());
            }

            return listOfWriteableRoomMatrices;
        }



        private static Room GetDecoratedRoom(WriteableWallMatrix thisMatrix, Room sourceRoom, uint[] resamplingColourData)
        {
            var n = sourceRoom.RoomNumber;

            AddDecorativeBrickwork(thisMatrix);

            // Set style pattern on the wall squares:

            SetWallStyleDeltas(
                thisMatrix,
                resamplingColourData,
                n * 8,
                n * 4,
                128,
                true);

            // Set style pattern on the floor squares:

            SetWallStyleDeltas(
                thisMatrix,
                resamplingColourData,
                n * 4,
                n * 8,
                128,
                false);

            return new Room(sourceRoom.RoomX, sourceRoom.RoomY, thisMatrix);
        }



        private static void AlignDoorways(List<WriteableWallMatrix> matricesInRoomOrder)
        {
            for (int y = 0; y < Constants.RoomsVertically; y++)
            {
                for (int x = 0; x < Constants.RoomsHorizontally - 1; x++)
                {
                    AlignDoorwaysGoingLeftRight(
                        matricesInRoomOrder[y * Constants.RoomsHorizontally + x],
                        matricesInRoomOrder[y * Constants.RoomsHorizontally + x + 1]);
                }
            }

            for (int y=0; y<Constants.RoomsVertically-1; y++)
            {
                for (int x = 0; x < Constants.RoomsHorizontally; x++)
                {
                    AlignDoorwaysGoingUpDown(
                        matricesInRoomOrder[y * Constants.RoomsHorizontally + x],
                        matricesInRoomOrder[(y+1) * Constants.RoomsHorizontally + x]);
                }
            }
        }



        private static void AlignDoorwaysGoingLeftRight(WriteableWallMatrix roomMatrix1, WriteableWallMatrix roomMatrix2)
        {
            AlignDoorwaysScan(
                roomMatrix1, new Point(24, 0), 
                roomMatrix2, new Point(0, 0), 
                new MovementDeltas(0, 1), 
                25);
        }



        private static void AlignDoorwaysGoingUpDown(WriteableWallMatrix roomMatrix1, WriteableWallMatrix roomMatrix2)
        {
            AlignDoorwaysScan(
                roomMatrix1, new Point(0, 24),
                roomMatrix2, new Point(0, 0),
                new MovementDeltas(1, 0),
                25);
        }



        private static void AlignDoorwaysScan(
            WriteableWallMatrix roomMatrix1, Point point1,
            WriteableWallMatrix roomMatrix2, Point point2,
            MovementDeltas movementDeltas,
            int blockCount)
        {
            while (blockCount > 0)
            {
                if (   roomMatrix1.Read(point1) != MissionIITile.Space
                    || roomMatrix2.Read(point2) != MissionIITile.Space)
                {
                    roomMatrix1.Write(point1, MissionIITile.Electric);
                    roomMatrix2.Write(point2, MissionIITile.Electric);
                }
                point1 = point1 + movementDeltas;
                point2 = point2 + movementDeltas;
                --blockCount;
            }
        }



        private static void AddDecorativeBrickwork(WriteableWallMatrix wallMatrix)
        {
            // Turn Electric areas into Brick leaving just an Electric outline.

            for (int y = 1; y < 24; ++y)
            {
                for (int x = 1; x < 24; ++x)
                {
                    if (SurroundedByWall8(wallMatrix, x, y))
                    {
                        wallMatrix.Write(x, y, MissionIITile.Brick);
                    }
                }
            }
        }



        private static bool SurroundedByWall4(WriteableWallMatrix wallMatrix, int x, int y) // TODO: We could be arty and call this instead.
        {
            return
                   wallMatrix.Read(x, y - 1) != MissionIITile.Space
                && wallMatrix.Read(x, y + 1) != MissionIITile.Space
                && wallMatrix.Read(x - 1, y) != MissionIITile.Space
                && wallMatrix.Read(x + 1, y) != MissionIITile.Space;
        }



        private static bool SurroundedByWall8(WriteableWallMatrix wallMatrix, int x, int y)
        {
            return
                   wallMatrix.Read(x, y - 1) != MissionIITile.Space
                && wallMatrix.Read(x, y + 1) != MissionIITile.Space
                && wallMatrix.Read(x - 1, y) != MissionIITile.Space
                && wallMatrix.Read(x + 1, y) != MissionIITile.Space
                && wallMatrix.Read(x - 1, y - 1) != MissionIITile.Space
                && wallMatrix.Read(x + 1, y - 1) != MissionIITile.Space
                && wallMatrix.Read(x - 1, y + 1) != MissionIITile.Space
                && wallMatrix.Read(x + 1, y + 1) != MissionIITile.Space;
        }



        private static void SetWallStyleDeltas(
            WriteableWallMatrix wallMatrix,
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
                    if ((wallMatrix.Read(x, y) == MissionIITile.Space) ^ doWalls)
                    {
                        int cx = (x + logicalOffsetX) & 63;
                        var greyLevel = Colour.GetGreyLevel(resamplingImageArray[cy * 64 + cx]);
                        wallMatrix.SetStyleDelta(x, y, (byte)((greyLevel < sampleThreshold) ? 0 : 1));
                    }
                }
            }
        }


    }
}
