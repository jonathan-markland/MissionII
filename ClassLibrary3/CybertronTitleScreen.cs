
using System.IO;

namespace GameClassLibrary
{
    public class CybertronTitleScreenMode : CybertronGameMode
    {
        private int _countDown = 100;

        public override void AdvanceOneCycle(CybertronKeyStates theKeyStates)
        {
            if (_countDown > 0 && !theKeyStates.Fire)
            {
                --_countDown;
            }
            else
            {
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronInstructionsKeysMode();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, CybertronSpriteTraits.TitleScreen.HostImageObjects[0]);
        }
    }

    public class CybertronInstructionsKeysMode : CybertronGameMode
    {
        private int _countDown = 100;

        public override void AdvanceOneCycle(CybertronKeyStates theKeyStates)
        {
            if (theKeyStates.Fire)
            {
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronStartNewGameMode();
            }
            else if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronTitleScreenMode();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, CybertronSpriteTraits.InstructionsKeys.HostImageObjects[0]);
        }
    }

    public class CybertronStartNewGameMode : CybertronGameMode
    {
        public override void AdvanceOneCycle(CybertronKeyStates theKeyStates)
        {
            using (var sr = new StreamReader("Resources\\Levels.txt"))
            {
                var loadedWorld = GameClassLibrary.LevelFileParser.Parse(sr);
                GameClassLibrary.LevelFileValidator.ExpectValidPathsInWorld(loadedWorld);
                GameClassLibrary.LevelExpander.ExpandWallsInWorld(loadedWorld);

                // TODO: PrepareForNewGame() function
                // TODO: PrepareForNewLevel() function

                var cybertronGameBoard = new GameClassLibrary.CybertronGameBoard() // TODO: HACK
                {
                    TheWorldWallData = loadedWorld,
                    BoardWidth = 320,
                    BoardHeight = 256,
                    LevelNumber = 2,
                    RoomNumber = 1,
                    Score = 0,
                    Lives = 3
                };

                // HACKS
                cybertronGameBoard.Key = new GameClassLibrary.CybertronKey(1);
                cybertronGameBoard.Ring = new GameClassLibrary.CybertronRing(4);
                cybertronGameBoard.Gold = new GameClassLibrary.CybertronGold(16);
                cybertronGameBoard.Safe = new GameClassLibrary.CybertronLevelSafe(3);
                cybertronGameBoard.Potion = new GameClassLibrary.CybertronPotion(2);
                cybertronGameBoard.Man.Alive(0, 17, 92);

                GameClassLibrary.CybertronGameStateUpdater.PrepareForNewRoom(cybertronGameBoard);

                CybertronGameModeSelector.ModeSelector.CurrentMode
                    = new CybertronEnteringLevelMode(cybertronGameBoard);
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            // This is non-visual
        }
    }

    public class CybertronEnteringLevelMode : CybertronGameMode
    {
        private CybertronGameBoard _cybertronGameBoard;
        private int _countDown = 150;

        public CybertronEnteringLevelMode(CybertronGameBoard theGameBoard)
        {
            _cybertronGameBoard = theGameBoard;
        }

        public override void AdvanceOneCycle(CybertronKeyStates theKeyStates)
        {
            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                CybertronGameModeSelector.ModeSelector.CurrentMode 
                    = new CybertronGamePlayMode(_cybertronGameBoard);
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, CybertronSpriteTraits.EnteringLevel.HostImageObjects[0]);

            // Show the things you need to find on this level.

            int x = 160; // TODO centre
            int y = 150; // TODO: constant
            int dy = 24; // TODO: constant

            _cybertronGameBoard.ForEachThingWeHaveToFindOnThisLevel(
                o =>
                {
                    CybertronScreenPainter.DrawFirstSpriteCentred(x, y, o.SpriteTraits, drawingTarget);
                    y += dy;
                });
        }
    }

    public class CybertronGamePlayMode : CybertronGameMode
    {
        private CybertronGameBoard _cybertronGameBoard;

        public CybertronGamePlayMode(CybertronGameBoard cybertronGameBoard)
        {
            _cybertronGameBoard = cybertronGameBoard;
        }

        public override void AdvanceOneCycle(CybertronKeyStates theKeyStates)
        {
            GameClassLibrary.CybertronGameStateUpdater.Update(_cybertronGameBoard, theKeyStates); // TODO: pull logic into this class
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            GameClassLibrary.CybertronScreenPainter.DrawBoardToTarget(
                _cybertronGameBoard,
                drawingTarget); // TODO: pull logic into this class
        }
    }

    public class CybertronGameOverMode : CybertronGameMode
    {
        private int _countDown = 100;

        public override void AdvanceOneCycle(CybertronKeyStates theKeyStates)
        {
            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronTitleScreenMode();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, CybertronSpriteTraits.GameOver.HostImageObjects[0]);
        }
    }
}
