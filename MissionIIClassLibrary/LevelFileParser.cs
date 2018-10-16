using System;
using System.IO;
using System.Collections.Generic;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public static class LevelFileParser
    {
        #region New parsing

        public static List<Level> Parse(StreamReader streamReader)
        {
            return ForEachLevelInFileDo(streamReader,
                nextLevelNumber =>
                {
                    var wholeOfLevelMatrix = new WriteableTileMatrix(
                        Constants.RoomsHorizontally * Constants.ClustersHorizontally * Constants.SourceClusterSide,
                        Constants.RoomsVertically * Constants.ClustersVertically * Constants.SourceClusterSide,
                        Constants.TileWidth,
                        Constants.TileHeight);

                    int rowOnLevel = 0;
                    int manX = -1, manY = -1;

                    for (int roomY = 0; roomY < Constants.RoomsVertically; ++roomY)
                    {
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

                            var theSplittings = thisLine.Split(new[] { " | " }, StringSplitOptions.None);
                            if (theSplittings.Length != Constants.RoomsHorizontally)
                            {
                                throw new Exception($"Room definition has invalid number of rooms on the row.  Expected {Constants.RoomsHorizontally}.");
                            }

                            foreach (var str in theSplittings)
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
                                    var sourceString = theSplittings[x];
                                    var roomSideOffset = x * Constants.SourceFileRoomCharsHorizontally;
                                    PaintLine(wholeOfLevelMatrix, roomSideOffset, rowNumber, sourceString);
                                    ForEachSpecialMarker(sourceString, roomSideOffset,
                                        columnOnLevel => {
                                            manX = columnOnLevel;
                                            manY = rowOnLevel;
                                        });
                                }
                                catch (Exception e)
                                {
                                    throw new Exception($"Error in room-column {roomX}:  " + e.Message);
                                }
                            }
                        }

                        ++rowOnLevel;
                    }

                    var specialMarkers = new SpecialMarkers();

                    specialMarkers.SetManStartCluster(
                        new Point(manX / Constants.SourceClusterSide, manY / Constants.SourceClusterSide),
                        MissionIITile.Floor,
                        wholeOfLevelMatrix);

                    return new Level(nextLevelNumber, wholeOfLevelMatrix, specialMarkers);
                });
        }

        #endregion



        public static List<Level> ForEachLevelInFileDo(StreamReader streamReader, Func<int, Level> levelMaker)
        {
            var levelsList = new List<Level>();
            int nextLevelNumber = 1;

            while (FindNextLevel(streamReader, nextLevelNumber))
            {
                try
                {
                    levelsList.Add(levelMaker(nextLevelNumber));
                }
                catch (Exception e)
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



        private static bool IsBlankLine(string textString)
        {
            return String.IsNullOrWhiteSpace(textString);
        }



        public static string GetLevelHeader(int levelNumber)
        {
            return "[Level " + levelNumber + "]";
        }



        public static Tile CharToWallMatrixChar(char ch)
        {
            if (ch == ' ' || ch == 'x') return MissionIITile.Floor;  // 'x' marks man start spot
            if (ch == '#') return MissionIITile.ElectricWall;
            if (ch == '@') return MissionIITile.ElectricWall;
            // NB: There is no specification of any other WallMatrixChar kinds in the source text file.
            throw new Exception($"Cannot recognise character '{ch}' as a valid wall layout character.");
        }



        public static void PaintLine(WriteableTileMatrix targetMatrix, int x, int rowNumber, string thisLine)
        {
            foreach(char ch in thisLine)
            {
                targetMatrix.Write(x, rowNumber, CharToWallMatrixChar(ch));
                x++;
            }
        }



        public static void ForEachSpecialMarker(string sourceString, int x, Action<int> foundAt)
        {
            foreach (char ch in sourceString)
            {
                if (ch == 'x')
                {
                    foundAt(x);
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
