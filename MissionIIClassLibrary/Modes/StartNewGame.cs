﻿
using System.IO;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class StartNewGame : BaseGameMode
    {
        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            using (var sr = new StreamReader("Resources\\Levels.txt"))
            {
                var loadedWorld = MissionIIClassLibrary.LevelFileParser.Parse(sr);
                MissionIIClassLibrary.LevelFileValidator.ExpectValidPathsInWorld(loadedWorld);
                MissionIIClassLibrary.LevelExpander.ExpandWallsInWorld(loadedWorld, MissionIISpriteTraits.PatternResamplingSprite);
                
                var gameBoard = new MissionIIClassLibrary.MissionIIGameBoard()
                {
                    TheWorldWallData = loadedWorld,
                    BoardWidth = Constants.ScreenWidth,
                    BoardHeight = Constants.ScreenHeight,
                    LevelNumber = Constants.StartLevelNumber,
                    Lives = Constants.InitialLives
                };

                gameBoard.PrepareForNewLevel();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            // This is non-visual
        }
    }
}
