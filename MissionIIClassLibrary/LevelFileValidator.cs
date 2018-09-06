using System;
using System.Collections.Generic;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public static class LevelFileValidator
    {
        public static void ExpectValidPathsInWorld(List<Level> levelsList)
        {
            foreach (var thisLevel in levelsList)
            {
                foreach (var thisRoom in thisLevel.Rooms)
                {
                    try
                    {
                        ExpectValidPathsInRoom(thisRoom.WallData);
                        ExpectEdgeDoorwaysMatchOtherRooms(thisRoom, thisLevel.Rooms);
                    }
                    catch(Exception e)
                    {
                        throw new Exception($"{e.Message}  In room ({thisRoom.RoomX},{thisRoom.RoomY}) in level {thisLevel.LevelNumber}.", e);
                    }
                }
            }
        }



        public static void ExpectValidPathsInRoom(TileMatrix fileWallData)
        {
            // Validations for each 3x3 group of tiles individually:

            for(int y = 0; y < Constants.SourceFileCharsVertically - Constants.SourceClusterSide; y += Constants.SourceClusterSide)
            {
                for (int x = 0; x < Constants.SourceFileRoomCharsHorizontally - Constants.SourceClusterSide; x += Constants.SourceClusterSide)
                {
                    ExpectValidThreeByThree(fileWallData, x, y);
                }
            }

            // Validations of the connections BETWEEN the 3x3 groups:

            for (int y = 1; y <= Constants.SourceFileCharsVertically - 2; y += Constants.SourceClusterSide)
            {
                for (int x = 2; x <= Constants.SourceFileRoomCharsHorizontally - 4; x += Constants.SourceClusterSide)
                {
                    ValidateTileConnection(fileWallData, x, y, 1, 0);  // Horizontal connections
                    ValidateTileConnection(fileWallData, y, x, 0, 1);  // Vertical connections
                }
            }
        }



        public static bool BothAreSpaceOrBothAreNotSpace(Tile c1, Tile c2)
        {
            return !((c1 == MissionIITile.Space) ^ (c2 == MissionIITile.Space));
        }



        public static void ValidateTileConnection(TileMatrix fileWallData, int x, int y, int dx, int dy)
        {
            var c1 = fileWallData.TileAt(x, y);
            var c2 = fileWallData.TileAt(x + dx, y + dy);
            if (! BothAreSpaceOrBothAreNotSpace(c1, c2))
            {
                throw new Exception($"Invalid connection between 3 x 3 tiles at ({x},{y}).  Both must be spaces, or both must be wall.");
            }
        }



        public static void ExpectEdgeDoorwaysMatchOtherRooms(Room thisRoom, List<Room> rooms)
        {
            CheckDoorwaysMirrored(thisRoom.RoomX, thisRoom.RoomY,  0, -1, rooms, 1,  0, Constants.SourceClusterSide, 0);  // Along the top / left
            CheckDoorwaysMirrored(thisRoom.RoomX, thisRoom.RoomY,  0,  1, rooms, 1, 14, Constants.SourceClusterSide, 0);  // Along the bottom / right
        }



        private static void CheckDoorwaysMirrored(
            int roomX, int roomY, int roomDx, int roomDy, List<Room> rooms, int startX, int startY, int dx, int dy)
        {
            CheckDoorwaysAlongSide(roomX, roomY, roomDx, roomDy, rooms, startX, startY, dx, dy);
            CheckDoorwaysAlongSide(roomX, roomY, roomDy, roomDx, rooms, startY, startX, dy, dx); // mirror in line y=x
        }



        private static void CheckDoorwaysAlongSide(
            int roomX, int roomY, int roomDx, int roomDy, List<Room> rooms, int startX, int startY, int dx, int dy)
        {
            // - roomDx,roomDy : Where to find the desired adjacent room, relative to the room to check.  One of these will be 0.
            // - startX,startY : char cell to start at, within the room to check.
            // - dx,dy         : Stepping distance to next doorway slot.  One of these will be 0.

            // Reminder: roomDx / roomDy MAY go out of bounds!

            var thisRoom = FindRoom(rooms, roomX, roomY);

            var otherRoomX = (roomX + roomDx);
            var otherRoomY = (roomY + roomDy);

            var otherRoomIsOffMap =
                (otherRoomX <= 0 || otherRoomX > Constants.RoomsHorizontally) ||
                (otherRoomY <= 0 || otherRoomY > Constants.RoomsVertically);

            var otherRoom = otherRoomIsOffMap ? null : FindRoom(rooms, otherRoomX, otherRoomY);

            for (int i=0; i < Constants.ClustersHorizontally; i++) // TODO: collapse H/V constants to one.
            {
                var thisRoomSquare = thisRoom.WallData.TileAt(startX, startY);

                if (otherRoomIsOffMap)
                {
                    if (thisRoomSquare == MissionIITile.Space)
                    {
                        throw new Exception($"No doorway must exist at ({startX},{startY}) because it leads off the map.");
                    }
                }
                else
                {
                    // Reflect to other side for the 'other' room:
                    // Only one of these two will actually calculate a reflection:
                    var travellingVertically = (dx == 0);
                    var travellingHorizontally = (dy == 0);
                    var n = Constants.SourceFileRoomCharsHorizontally - 1;
                    var otherRoomPosX = travellingVertically ? (n - startX) : startX; 
                    var otherRoomPosY = travellingHorizontally ? (n - startY) : startY; 

                    // Fetch corresponding char in adjacent room, and check:
                    var otherRoomSquare = otherRoom.WallData.TileAt(otherRoomPosX, otherRoomPosY);

                    if (! BothAreSpaceOrBothAreNotSpace(thisRoomSquare, otherRoomSquare))
                    {
                        throw new Exception($"Invalid connection " +
                            $"to ({otherRoomPosX},{otherRoomPosY}) in room {otherRoom.RoomX},{otherRoom.RoomY} " +
                            $"from ({startX},{startY}) "
                            );
                    }
                }

                startX += dx;
                startY += dy;
            }
        }



        public static Room FindRoom(List<Room> roomsList, int roomX, int roomY)
        {
            foreach(var thisRoom in roomsList)
            {
                if (thisRoom.RoomX == roomX && thisRoom.RoomY == roomY)
                {
                    return thisRoom;
                }
            }
            return null;
        }



        public static void ExpectValidThreeByThree(TileMatrix fileWallData, int x, int y)
        {
            // 789
            // 456
            // 123

            var c7 = fileWallData.TileAt(x + 0, y + 0);
            var c8 = fileWallData.TileAt(x + 1, y + 0);
            var c9 = fileWallData.TileAt(x + 2, y + 0);
            var c4 = fileWallData.TileAt(x + 0, y + 1);
            var c5 = fileWallData.TileAt(x + 1, y + 1);
            var c6 = fileWallData.TileAt(x + 2, y + 1);
            var c1 = fileWallData.TileAt(x + 0, y + 2);
            var c2 = fileWallData.TileAt(x + 1, y + 2);
            var c3 = fileWallData.TileAt(x + 2, y + 2);

            var spaceIn8246 = (c8 == MissionIITile.Space || c2 == MissionIITile.Space || c4 == MissionIITile.Space || c6 == MissionIITile.Space);

            // Centre square space-check rules.

            if (spaceIn8246 && c5 != MissionIITile.Space)
            {
                throw new Exception($"Character at ({x+1},{y+1}) must be a space!");
            }

            // Relaxed for artistic purposes.
            // if (c5.Space && !spaceIn8246)
            // {
            //     throw new Exception($"Character at ({x+1},{y+1}) cannot be a space without a space above, below, to the left or to the right!");
            // }

            // Corner squares cannot just be spaces.

            if (c7 == MissionIITile.Space && !(c4 == MissionIITile.Space && c5 == MissionIITile.Space && c8 == MissionIITile.Space))
            {
                throw new Exception($"Corner square at ({x},{y}) cannot be a space.");
            }

            if (c9 == MissionIITile.Space && !(c6 == MissionIITile.Space && c5 == MissionIITile.Space && c8 == MissionIITile.Space))
            {
                throw new Exception($"Corner square at ({x+2},{y}) cannot be a space.");
            }

            if (c3 == MissionIITile.Space && !(c2 == MissionIITile.Space && c5 == MissionIITile.Space && c6 == MissionIITile.Space))
            {
                throw new Exception($"Corner square at ({x+2},{y+2}) cannot be a space.");
            }

            if (c1 == MissionIITile.Space && !(c2 == MissionIITile.Space && c5 == MissionIITile.Space && c4 == MissionIITile.Space))
            {
                throw new Exception($"Corner square at ({x},{y+2}) cannot be a space.");
            }
        }

    }
}
