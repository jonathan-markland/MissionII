﻿
using System.IO;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class StartNewGame : GameMode
    {
        public override void AdvanceOneCycle(KeyStates theKeyStates)
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
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            // This is non-visual
        }
    }
}
