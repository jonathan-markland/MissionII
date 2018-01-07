using System;
using System.IO;
using System.Collections.Generic;

namespace GameClassLibrary
{
    public static class Constants  // TODO: Location of some of these constants is suspect, in view of general name "Constants"
    {
        public const int BulletCycles = 4;
        public const int GhostStartCycles = 1500;
        public const int GhostStunnedCycles = 200;
        public const int RoomsHorizontally = 4;
        public const int RoomsVertically = 4;
        public const int NumRooms = RoomsHorizontally * RoomsVertically;
        public const int ClustersHorizonally = 5;
        public const int ClustersVertically = 5;
        public const int ClusterSide = 3;
        public const int CharsPerRoomSeparator = 3;
        public const int SourceFileRoomCharsHorizontally = (ClustersHorizonally * ClusterSide);
        public const int SourceFileRowOfRoomCharsHorizontally = SourceFileRoomCharsHorizontally * RoomsHorizontally + (RoomsHorizontally-1) * CharsPerRoomSeparator;
        public const int SourceFileCharsVertically = ClustersVertically * ClusterSide;
        public const int MaxDisplayedLives = 8;
        public const int BulletSpacing = 1;
        public const int InventoryItemSpacing = 4;
        public const int GhostMovementCycles = 2;
        public const int ExclusionZoneAroundMan = 32;
        public const int ManDeadDelayCycles = 100;
        public const int MaxLives = 15;
        public const int NewLifeBoundary = 10000;
        public const int IdealDroidCountPerRoom = 10;
        public const int PositionerShapeSizeMinium = 10; // sort of srbitrary
    }



    public class WorldWallData
    {
        public List<Level> Levels;
    }



    public class Level
    {
        public int LevelNumber;
        public List<Room> Rooms;
    }



    public class Room
    {
        public Room(int x, int y, WallMatrix fileWallData)
        {
            RoomX = x;
            RoomY = y;
            FileWallData = fileWallData;
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
                var roomsList = new List<Room>();

                for (int roomY = 1; roomY <= Constants.RoomsVertically; ++roomY)
                {
                    // TODO:  no more:  ExpectRoomHeader(streamReader, roomX, roomY);
                    roomsList.AddRange(ParseRowOfRooms(streamReader, roomY));
                }

                levelsList.Add(new Level { LevelNumber = nextLevelNumber, Rooms = roomsList });

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



        public static List<Room> ParseRowOfRooms(StreamReader streamReader, int roomY)
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
                    PaintLine(rowOfRooms[roomX-1].FileWallData, rowNumber, theSplittings[roomX-1]);
                }
            }

            return rowOfRooms;
        }



        public static void PaintLine(WallMatrix targetMatrix, int rowNumber, string thisLine)
        {
            int x = 0;
            foreach(char ch in thisLine)
            {
                targetMatrix.Write(x, rowNumber, new WallMatrixChar { WallChar = ch });
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
            return ch == ' ' || ch == '#' || ch == '@';
        }
    }
}
