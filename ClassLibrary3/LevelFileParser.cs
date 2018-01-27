using System;
using System.IO;
using System.Collections.Generic;
using GameClassLibrary.Math;

namespace GameClassLibrary
{
    public static class Constants  // TODO: Location of some of these constants is suspect, in view of general name "Constants"
    {
        public const int FootstepSoundCycles = 24;
        public const int TitleScreenRollCycles = 400;
        public const int GameOverMessageCycles = 100;
        public const int LeavingLevelCycles = 100;
        public const int EnteringLevelScreenCycles = 150;
        public const int InitialLives = 3;
        public const int BulletCycles = 4;
        public const int GhostStartCycles = 1500;
        public const int GhostStunnedCycles = 200;
        public const int RoomsHorizontally = 4;
        public const int RoomsVertically = 4;
        public const int NumRooms = RoomsHorizontally * RoomsVertically;
        public const int ClustersHorizontally = 5;
        public const int ClustersVertically = 5;
        public const int SourceClusterSide = 3;
        public const int DestClusterSide = 5; 
        public const int CharsPerRoomSeparator = 3;
        public const int SourceFileRoomCharsHorizontally = (ClustersHorizontally * SourceClusterSide);
        public const int SourceFileRowOfRoomCharsHorizontally = SourceFileRoomCharsHorizontally * RoomsHorizontally + (RoomsHorizontally-1) * CharsPerRoomSeparator;
        public const int SourceFileCharsVertically = ClustersVertically * SourceClusterSide;
        public const int MaxDisplayedLives = 8;
        public const int BulletSpacing = 1;
        public const int InventoryItemSpacing = 4;
        public const int GhostMovementCycles = 2;
        public const int ExclusionZoneAroundMan = 32;
        public const int ManDeadDelayCycles = 100;
        public const int MaxLives = 15;
        public const int NewLifeBoundary = 10000;
        public const int IdealDroidCountPerRoom = 10;
        public const int PositionerShapeSizeMinimum = 10; // sort of arbitrary
    }



    public class WorldWallData
    {
        public List<Level> Levels;
    }



    public class Level
    {
        private SpecialMarkers _specialMarkers;

        public Level(int levelNumber, List<Room> roomList, SpecialMarkers specialMarkers)
        {
            LevelNumber = levelNumber;
            Rooms = roomList;
            _specialMarkers = specialMarkers;
            if (specialMarkers.StartRoom == null)
            {
                throw new Exception($"Man start position marker 'x' has not been set.");
            }
        }

        public Room ManStartRoom { get { return _specialMarkers.StartRoom; } }
        public Point ManStartCluster { get { return _specialMarkers.ManStart; } }
        public int ManStartFacingDirection { get { return _specialMarkers.InitialManFacingDirection; } }
        public int LevelNumber { get; private set; }
        public List<Room> Rooms { get; private set; }
    }



    public class Room
    {
        public Room(int x, int y, WallMatrix fileWallData)
        {
            RoomX = x;
            RoomY = y;
            FileWallData = fileWallData;
        }

        public int RoomNumber
        {
            get { return RoomX + Constants.RoomsHorizontally * (RoomY - 1); }
        }

        public int RoomX;
        public int RoomY;
        public WallMatrix FileWallData;
        public WallMatrix WallData;
    }


    public static class LevelFileParser
    {
        public static WorldWallData Parse(StreamReader streamReader)
        {
            var levelsList = new List<Level>();
            int nextLevelNumber = 1;

            while (FindNextLevel(streamReader, nextLevelNumber))
            {
                try
                {
                    var roomsList = new List<Room>();
                    var specialMarkers = new SpecialMarkers();

                    for (int roomY = 1; roomY <= Constants.RoomsVertically; ++roomY)
                    {
                        try
                        {
                            // TODO:  no more:  ExpectRoomHeader(streamReader, roomX, roomY);
                            roomsList.AddRange(ParseRowOfRooms(streamReader, roomY, specialMarkers));
                        }
                        catch(Exception e)
                        {
                            throw new Exception($"Error while reading room-row {roomY}:  " + e.Message);
                        }
                    }

                    levelsList.Add(new Level(nextLevelNumber, roomsList, specialMarkers));
                }
                catch(Exception e)
                {
                    throw new Exception($"Error while reading level {nextLevelNumber}:  " + e.Message);
                }

                ++nextLevelNumber;
            }

            return new WorldWallData { Levels = levelsList };
        }



        public static bool FindNextLevel(StreamReader streamReader, int levelNumber)
        {
            var expectedLevelHeader = GetLevelHeader(levelNumber);

            string nextLine;
            while (!streamReader.EndOfStream)
            {
                nextLine = streamReader.ReadLine();
                if (IsBlankLine(nextLine)) continue;
                if (nextLine == expectedLevelHeader) return true;
                throw new Exception("Unexpected content in file: " + nextLine);
            }

            return false; // No header found.
        }



        public static string ReadLineAfterAnyEmpties(StreamReader streamReader)
        {
            string nextLine;
            for (; ; )
            {
                nextLine = streamReader.ReadLine();
                if (!IsBlankLine(nextLine)) break;
            }
            return nextLine;
        }



        public static void ExpectRoomHeader(StreamReader streamReader, int roomX, int roomY)
        {
            var nextLine = ReadLineAfterAnyEmpties(streamReader);

            var roomHeader = GetRoomHeader(roomX, roomY);
            if (nextLine != roomHeader)
            {
                throw new Exception("Missing room header.  Expected: " + roomHeader);
            }
        }



        private static bool IsBlankLine(string textString)
        {
            return String.IsNullOrWhiteSpace(textString);
        }



        public static string GetLevelHeader(int levelNumber)
        {
            return "[Level " + levelNumber + "]";
        }



        public static string GetRoomHeader(int roomX, int roomY)
        {
            return "[Room " + roomX + "," + roomY + "]";
        }



        public static List<Room> ParseRowOfRooms(StreamReader streamReader, int roomY, SpecialMarkers specialMarkers)
        {
            var rowOfRooms = new List<Room>(Constants.RoomsHorizontally);
            for (int roomX = 0; roomX < Constants.RoomsHorizontally; ++roomX)
            {
                rowOfRooms.Add(new Room(roomX + 1, roomY, 
                    new WallMatrix(Constants.SourceFileRoomCharsHorizontally, Constants.SourceFileCharsVertically)));
            }

            if (streamReader.ReadLine().Length != 0)
            {
                throw new Exception("One empty line expected before starting row of rooms.");
            }

            for (int rowNumber = 0; rowNumber < Constants.SourceFileCharsVertically; ++rowNumber)
            {
                var thisLine = streamReader.ReadLine();
                if (thisLine.Length != Constants.SourceFileRowOfRoomCharsHorizontally)
                {
                    throw new Exception($"Room-row definition has invalid number of characters on the row:  Expected {Constants.SourceFileRoomCharsHorizontally}.");
                }

                var theSplittings = thisLine.Split(new [] { " | " }, StringSplitOptions.None);
                if (theSplittings.Length != Constants.RoomsHorizontally)
                {
                    throw new Exception($"Room definition has invalid number of rooms on the row.  Expected {Constants.RoomsHorizontally}.");
                }

                foreach(var str in theSplittings)
                {
                    if (str.Length != Constants.SourceFileRoomCharsHorizontally)
                    {
                        throw new Exception($"Room definition has invalid number of rooms on the row.  Expected {Constants.RoomsHorizontally}.");
                    }

                    CheckWallDefinitionCharacters(str);
                }

                for (int roomX = 1; roomX <= Constants.RoomsHorizontally; ++roomX)
                {
                    try
                    {
                        var targetRoom = rowOfRooms[roomX - 1];
                        var sourceString = theSplittings[roomX - 1];
                        PaintLine(targetRoom.FileWallData, rowNumber, sourceString);
                        ScanForSpecialMarkers(sourceString, rowNumber, targetRoom, specialMarkers);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Error in room-column {roomX}:  " + e.Message);
                    }
                }
            }

            return rowOfRooms;
        }



        public static WallMatrixChar CharToWallMatrixChar(char ch)
        {
            if (ch == ' ' || ch == 'x') return WallMatrixChar.Space;  // 'x' marks man start spot
            if (ch == '#') return WallMatrixChar.Electric;
            if (ch == '@') return WallMatrixChar.Electric;
            // NB: There is no specification of any other WallMatrixChar kinds in the source text file.
            throw new Exception($"Cannot recognise character '{ch}' as a valid wall layout character.");
        }



        public static void PaintLine(WallMatrix targetMatrix, int rowNumber, string thisLine)
        {
            int x = 0;
            foreach(char ch in thisLine)
            {
                targetMatrix.Write(x, rowNumber, CharToWallMatrixChar(ch));
                x++;
            }
        }



        public static void ScanForSpecialMarkers(string sourceString, int rowNumber, Room targetRoom, SpecialMarkers specialMarkers)
        {
            int x = 0;
            foreach (char ch in sourceString)
            {
                if (ch == 'x')
                {
                    specialMarkers.SetManStartCluster(
                        targetRoom, 
                        new Point(x / Constants.SourceClusterSide, rowNumber / Constants.SourceClusterSide));
                }
                x++;
            }
        }



        public static void CheckWallDefinitionCharacters(string str)
        {
            foreach (char ch in str)
            {
                if (!IsValidWallDefinitionChar(ch))
                {
                    throw new Exception("Invalid character in wall definition: " + ch);
                }
            }
        }



        public static bool IsValidWallDefinitionChar(char ch)
        {
            return ch == ' ' || ch == '#' || ch == '@' || ch == 'x';
        }
    }
}
