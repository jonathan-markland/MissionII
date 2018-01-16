
using System.IO;

namespace GameClassLibrary
{
    public class CybertronTitleScreenMode : CybertronGameMode
    {
        private int _countDown = Constants.TitleScreenRollCycles;
        private bool _releaseWaiting = true;

        public override void AdvanceOneCycle(CybertronKeyStates theKeyStates)
        {
            if (theKeyStates.Fire)
            {
                if (_releaseWaiting) return;
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronStartNewGameMode();
            }

            _releaseWaiting = false;

            if (_countDown > 0)
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
            drawingTarget.DrawSprite(0, 0, CybertronSpriteTraits.TitleScreen.GetHostImageObject(0));
        }
    }

    public class CybertronInstructionsKeysMode : CybertronGameMode
    {
        private int _countDown = Constants.TitleScreenRollCycles;
        private int _screenIndex = 1;

        public override void AdvanceOneCycle(CybertronKeyStates theKeyStates)
        {
            if (CybertronSpriteTraits.TitleScreen.ImageCount < 2)
            {
                // Cannot rotate any instruction screens.
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronTitleScreenMode();
                return;
            }

            if (theKeyStates.Fire)
            {
                _screenIndex = 1; // for next time
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronTitleScreenMode();
            }
            else if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                ++_screenIndex;
                if (_screenIndex >= CybertronSpriteTraits.TitleScreen.ImageCount)
                {
                    _screenIndex = 1;
                }
                _countDown = Constants.TitleScreenRollCycles;
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, CybertronSpriteTraits.TitleScreen.GetHostImageObject(_screenIndex));
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

                var cybertronGameBoard = new GameClassLibrary.CybertronGameBoard()
                {
                    TheWorldWallData = loadedWorld,
                    BoardWidth = CybertronGameBoardConstants.ScreenWidth,
                    BoardHeight = CybertronGameBoardConstants.ScreenHeight,
                    LevelNumber = 1,
                    Lives = Constants.InitialLives
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
        private int _countDown = Constants.EnteringLevelScreenCycles;

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
            drawingTarget.DrawSprite(0, 0, CybertronSpriteTraits.EnteringLevel.GetHostImageObject(0));

            // Show the things you need to find on this level.

            int x = CybertronGameBoardConstants.ScreenWidth / 2;
            int y = 150; // TODO: constant
            int dy = 24; // TODO: constant

            _cybertronGameBoard.ForEachThingWeHaveToFindOnThisLevel(
                o =>
                {
                    drawingTarget.DrawFirstSpriteCentred(x, y, o.SpriteTraits);
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
            _cybertronGameBoard.DrawBoardToTarget(drawingTarget);
        }
    }

    public class CybertronLeavingLevelMode : CybertronGameMode
    {
        private CybertronGameBoard _cybertronGameBoard;
        private int _countDown = Constants.LeavingLevelCycles;

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
            _cybertronGameBoard.DrawBoardToTarget(drawingTarget);
        }
    }

    public class CybertronGameOverMode : CybertronGameMode
    {
        private int _countDown = Constants.GameOverMessageCycles;

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
            drawingTarget.DrawFirstSpriteScreenCentred(CybertronSpriteTraits.GameOver);
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
            drawingTarget.DrawFirstSpriteScreenCentred(CybertronSpriteTraits.Paused);
        }
    }
}
