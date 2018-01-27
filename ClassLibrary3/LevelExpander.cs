
using System.Collections.Generic;
using GameClassLibrary.Math;

namespace GameClassLibrary
{
    public static class LevelExpander
    {
        public const int ExpandSize = 5;


        public static void ExpandWallsInWorld(WorldWallData theWorld)
        {
            foreach(var thisLevel in theWorld.Levels)
            {
                var roomsList = thisLevel.Rooms;

                // Expand all the rooms from the size they are in the source
                // text files to the size they need to be for targetting the
                // screen.

                foreach (var thisRoom in roomsList)
                {
                    thisRoom.WallData = ExpandWallsWithThickPassages.ExpandWalls(thisRoom.FileWallData);
                }

                // Now we know all the rooms, ensure the doorways line up.

                AlignDoorways(roomsList);

                // Now add decorative brickwork via a filter.

                foreach (var thisRoom in roomsList)
                {
                    AddDecorativeBrickwork(thisRoom.WallData);
                }
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
    }
}
