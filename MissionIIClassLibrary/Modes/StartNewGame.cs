
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
                var levelsList = MissionIIClassLibrary.LevelFileParser.Parse(sr);
                MissionIIClassLibrary.LevelFileValidator.ExpectValidPathsInWorld(levelsList);

                var expandedLevelsList = MissionIIClassLibrary.LevelExpander.ExpandWallsInWorld(
                    levelsList, MissionIISprites.PatternResamplingSprite);

                var loadedWorld = new WorldWallData { Levels = expandedLevelsList };
                var gameBoard = new MissionIIClassLibrary.MissionIIGameBoard(loadedWorld);

                gameBoard.PrepareForNewLevel();
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            // This is non-visual
        }
    }
}
