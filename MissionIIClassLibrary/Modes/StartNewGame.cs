
using System.IO;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public static class StartNewGame
    {
        public static ModeFunctions New()
        {
            return new ModeFunctions(

                // -- Advance one cycle --

                keyStates =>
                {
                    using (var sr = new StreamReader(Path.Combine("Resources", "Levels.txt")))
                    {
                        var levelsList = MissionIIClassLibrary.LevelFileParser.Parse(sr);
                        MissionIIClassLibrary.LevelFileValidator.ExpectValidPathsInWorld(levelsList);

                        var expandedLevelsList = MissionIIClassLibrary.LevelExpander.ExpandWallsInWorld(
                            levelsList, imageSeed =>
                            {
                                var s = MissionIISprites.PatternResamplingSprite;
                                var imageIndex = imageSeed % s.ImageCount;
                                var imagePixelsArray = s.GetHostImageObject(imageIndex).PixelsToUintArray();
                                return imagePixelsArray;
                            });

                        var loadedWorld = new WorldWallData { Levels = expandedLevelsList };
                        var gameBoard = new MissionIIClassLibrary.MissionIIGameBoard(loadedWorld);

                        gameBoard.PrepareForNewLevel(1);
                    }
                },

                // -- Draw --

                drawingTarget =>
                {
                    // This is non-visual
                });
        }
    }
}
