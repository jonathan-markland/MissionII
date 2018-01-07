
using System.IO;

namespace GameClassLibrary
{
    public class CybertronTitleScreenMode : CybertronGameMode
    {
        private int _countDown = 400;

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
        private int _countDown = 400;

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

                var cybertronGameBoard = new GameClassLibrary.CybertronGameBoard() // TODO: HACK
                {
                    TheWorldWallData = loadedWorld,
                    BoardWidth = 320,
                    BoardHeight = 256,
                    LevelNumber = 1,
                    RoomNumber = 1,
                    Score = 0,
                    Lives = 3
                };

                GameClassLibrary.CybertronGameStateUpdater.PrepareForNewLevel(cybertronGameBoard);
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
            if (CybertronModes.HandlePause(theKeyStates, this)) return;
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

            int x = 160; // TODO: Find centre
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
            if (CybertronModes.HandlePause(theKeyStates, this)) return;
            GameClassLibrary.CybertronGameStateUpdater.Update(_cybertronGameBoard, theKeyStates); // TODO: pull logic into this class
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            GameClassLibrary.CybertronScreenPainter.DrawBoardToTarget(_cybertronGameBoard, drawingTarget);
        }
    }

    public class CybertronLeavingLevelMode : CybertronGameMode
    {
        private CybertronGameBoard _cybertronGameBoard;
        private int _countDown = 100; // TODO: These literals in this file!!!

        public CybertronLeavingLevelMode(CybertronGameBoard cybertronGameBoard)
        {
            _cybertronGameBoard = cybertronGameBoard;
        }

        public override void AdvanceOneCycle(CybertronKeyStates theKeyStates)
        {
            if (CybertronModes.HandlePause(theKeyStates, this)) return;
            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                var thisLevelNumber = _cybertronGameBoard.LevelNumber;
                ++thisLevelNumber;
                _cybertronGameBoard.LevelNumber = thisLevelNumber;
                GameClassLibrary.CybertronGameStateUpdater.PrepareForNewLevel(_cybertronGameBoard);
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            GameClassLibrary.CybertronScreenPainter.DrawBoardToTarget(_cybertronGameBoard, drawingTarget);
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
            CybertronScreenPainter.DrawFirstSpriteScreenCentred(CybertronSpriteTraits.GameOver, drawingTarget);
        }
    }

    public class CybertronPauseMode : CybertronGameMode
    {
        private CybertronGameMode _originalMode;
        private bool _keyReleaseSeen;
        private bool _restartGameOnNextRelease;

        public CybertronPauseMode(CybertronGameMode originalMode)
        {
            _originalMode = originalMode;
            _keyReleaseSeen = false; // PAUSE key is held at the time this object is created.
            _restartGameOnNextRelease = false;
        }

        public override void AdvanceOneCycle(CybertronKeyStates theKeyStates)
        {
            if (!_keyReleaseSeen)
            {
                if (!theKeyStates.Pause)
                {
                    _keyReleaseSeen = true;
                    if (_restartGameOnNextRelease)
                    {
                        CybertronGameModeSelector.ModeSelector.CurrentMode = _originalMode;
                    }
                }
            }
            else if (theKeyStates.Pause)
            {
                _restartGameOnNextRelease = true;
                _keyReleaseSeen = false;
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            _originalMode.Draw(drawingTarget);
            CybertronScreenPainter.DrawFirstSpriteScreenCentred(CybertronSpriteTraits.Paused, drawingTarget);
        }
    }
}
