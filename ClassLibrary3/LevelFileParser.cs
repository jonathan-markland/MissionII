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
        public const int ClustersHorizonally = 5;
        public const int ClustersVertically = 5;
        public const int ClusterSide = 3;
        public const int SourceFileCharsHorizontally = ClustersHorizonally * ClusterSide;
        public const int SourceFileCharsVertically = ClustersVertically * ClusterSide;
        public const int MaxDisplayedLives = 8;
        public const int BulletSpacing = 1;
        public const int InventoryItemSpacing = 4;
        public const int GhostMovementCycles = 2;
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
                    for (int roomX = 1; roomX <= Constants.RoomsHorizontally; ++roomX)
                    {
                        ExpectRoomHeader(streamReader, roomX, roomY);
                        roomsList.Add(ParseRoom(streamReader, roomX, roomY));
                    }
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



        public static Room ParseRoom(StreamReader streamReader, int roomX, int roomY)
        {
            var thisWallData = new WallMatrix(Constants.SourceFileCharsHorizontally, Constants.SourceFileCharsVertically);

            for (int rowNumber = 0; rowNumber < Constants.SourceFileCharsVertically; ++rowNumber)
            {
                var thisLine = streamReader.ReadLine();
                if (thisLine.Length != Constants.SourceFileCharsHorizontally)
                {
                    throw new Exception("Room definition has invalid number of characters on he row:  Expected 15.");
                }
                CheckWallDefinitionCharacters(thisLine);
                PaintLine(thisWallData, rowNumber, thisLine);
            }

            return new Room(roomX, roomY, thisWallData);
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
