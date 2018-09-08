using System;
using System.IO;
using System.Collections.Generic;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public static class LevelFileParser
    {
        public static List<Level> Parse(StreamReader streamReader)
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

            return levelsList;
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
            var rowOfWallMatrices = new List<WriteableTileMatrix>(Constants.RoomsHorizontally);

            var rowOfRooms = new List<Room>(Constants.RoomsHorizontally);

            for (int roomX = 0; roomX < Constants.RoomsHorizontally; ++roomX)
            {
                var m = new WriteableTileMatrix(Constants.SourceFileRoomCharsHorizontally, Constants.SourceFileCharsVertically);
                rowOfWallMatrices.Add(m);
                rowOfRooms.Add(new Room(roomX + 1, roomY, m));
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
                        var x = roomX - 1;
                        var targetRoom = rowOfRooms[x];
                        var sourceString = theSplittings[x];
                        PaintLine(rowOfWallMatrices[x], rowNumber, sourceString);
                        ScanForSpecialMarkers(sourceString, rowNumber, targetRoom, specialMarkers, MissionIITile.Floor);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Error in room-column {roomX}:  " + e.Message);
                    }
                }
            }

            return rowOfRooms;
        }



        public static Tile CharToWallMatrixChar(char ch)
        {
            if (ch == ' ' || ch == 'x') return MissionIITile.Floor;  // 'x' marks man start spot
            if (ch == '#') return MissionIITile.ElectricWall;
            if (ch == '@') return MissionIITile.ElectricWall;
            // NB: There is no specification of any other WallMatrixChar kinds in the source text file.
            throw new Exception($"Cannot recognise character '{ch}' as a valid wall layout character.");
        }



        public static void PaintLine(WriteableTileMatrix targetMatrix, int rowNumber, string thisLine)
        {
            int x = 0;
            foreach(char ch in thisLine)
            {
                targetMatrix.Write(x, rowNumber, CharToWallMatrixChar(ch));
                x++;
            }
        }



        public static void ScanForSpecialMarkers(
            string sourceString, int rowNumber, 
            Room targetRoom, SpecialMarkers specialMarkers, 
            Tile spaceCharValue)
        {
            int x = 0;
            foreach (char ch in sourceString)
            {
                if (ch == 'x')
                {
                    specialMarkers.SetManStartCluster(
                        targetRoom, 
                        new Point(x / Constants.SourceClusterSide, rowNumber / Constants.SourceClusterSide), 
                        spaceCharValue);
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
