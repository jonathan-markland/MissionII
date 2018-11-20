using System;
using System.IO;
using System.Collections.Generic;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using System.Linq;

namespace MissionIIClassLibrary
{
    public static class LevelFileParser
    {
        #region New parsing

        public static List<Level> Parse(StreamReader streamReader)
        {
            var levelWidth = Constants.RoomsHorizontally * Constants.ClustersHorizontally * Constants.SourceClusterSide;
            var levelHeight = Constants.RoomsVertically * Constants.ClustersVertically * Constants.SourceClusterSide;

            // Load the file into arrays of char, each with treatment as ArraySlice2D.
            // We don't translate into arrays of Tile yet.

            var listOfLevelsAsCharArrays = ForEachLevelInFileDo(
                streamReader,
                nextLevelNumber =>
                {
                    var wholeOfLevelCharMatrix = new WriteableArraySlice2D<char>(levelWidth, levelHeight);

                    int rowOnLevel = 0;

                    for (int roomY = 0; roomY < Constants.RoomsVertically; ++roomY)
                    {
                        if (streamReader.ReadLine().Length != 0)
                        {
                            throw new Exception("One empty line expected before starting row of rooms.");
                        }

                        for (int rowNumber = 0; rowNumber < Constants.SourceFileRoomCharsVertically; ++rowNumber)
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
                                    PaintLine(wholeOfLevelCharMatrix, roomSideOffset, rowOnLevel, sourceString);
                                }
                                catch (Exception e)
                                {
                                    throw new Exception($"Error in room-column {roomX}:  " + e.Message);
                                }
                            }

                            ++rowOnLevel;
                        }
                    }

                    return wholeOfLevelCharMatrix.WholeArea;
                });

            // Now duplicate the char-arrays rotated 90 degrees each time.
            // This will give later levels some variety.

            var rotatedOnce = Rotate(listOfLevelsAsCharArrays);
            var rotatedTwice = Rotate(rotatedOnce);
            var rotatedThreeTimes = Rotate(rotatedTwice);

            // Concatenate these lists.

            var allCharLevels = listOfLevelsAsCharArrays
                .Concat(rotatedOnce)
                .Concat(rotatedTwice)
                .Concat(rotatedThreeTimes);

            // Translate these to type "Level", which involves finding the man 'x':

            var listOfLevels = new List<Level>();
            int levelNumber = 1;

            foreach (var charLevel in allCharLevels)
            {
                // Find man 'x' position:

                var manPoint = GameClassLibrary.Algorithms.Array2D.FindPositionOf(charLevel, c => (c == 'x'));
                if (manPoint.X == -1)
                {
                    throw new Exception($"Cannot locate 'x' (man start position) in level {levelNumber}");
                }

                // Generate Level:

                var levelTileMatrix = CharToTile(charLevel);

                var specialMarkers = new SpecialMarkers();

                specialMarkers.SetManStartCluster(
                    new Point(manPoint.X / Constants.SourceClusterSide, manPoint.Y / Constants.SourceClusterSide),
                    MissionIITile.Floor,
                    levelTileMatrix);

                var thisLevel = new Level(levelNumber, levelTileMatrix, specialMarkers);

                listOfLevels.Add(thisLevel);

                ++levelNumber;
            }

            return listOfLevels;
        }

        #endregion



        public static List<ArraySlice2D<T>> Rotate<T>(List<ArraySlice2D<T>> sourceList)
        {
            return sourceList.Select(a => GameClassLibrary.Algorithms.Array2D.RotateRight90(a)).ToList();
        }

        public static ArraySlice2D<Tile> CharToTile(ArraySlice2D<char> charLevel)
        {
            return charLevel.ConvertElementsTo(CharToWallMatrixChar);
        }



        public static List<ArraySlice2D<char>> ForEachLevelInFileDo(
            StreamReader streamReader, 
            Func<int, ArraySlice2D<char>> levelReader)
        {
            var levelsList = new List<ArraySlice2D<char>>();
            int nextLevelNumber = 1;

            while (FindNextLevel(streamReader, nextLevelNumber))
            {
                try
                {
                    levelsList.Add(levelReader(nextLevelNumber));
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



        public static void PaintLine(WriteableArraySlice2D<char> targetMatrix, int x, int rowNumber, string thisLine)
        {
            foreach(char ch in thisLine)
            {
                targetMatrix.Write(x, rowNumber, ch);
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
