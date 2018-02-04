
using System.IO;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public class MissionIITitleScreenMode : MissionIIGameMode
    {
        private int _countDown = Constants.TitleScreenRollCycles;
        private bool _releaseWaiting = true;
        private bool _firstCycle = true;

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (_firstCycle)
            {
                MissionIISounds.Play(MissionIISounds.Intro);
                _firstCycle = false;
            }

            if (theKeyStates.Fire)
            {
                if (_releaseWaiting) return;
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new MissionIIStartNewGameMode();
            }

            _releaseWaiting = false;

            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new MissionIIInstructionsKeysMode();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISpriteTraits.TitleScreen.GetHostImageObject(0));
        }
    }

    public class MissionIIInstructionsKeysMode : MissionIIGameMode
    {
        private int _countDown = Constants.TitleScreenRollCycles;
        private int _screenIndex = 1;

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (MissionIISpriteTraits.TitleScreen.ImageCount < 2)
            {
                // Cannot rotate any instruction screens.
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new MissionIITitleScreenMode();
                return;
            }

            if (theKeyStates.Fire)
            {
                _screenIndex = 1; // for next time
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new MissionIITitleScreenMode();
            }
            else if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                ++_screenIndex;
                if (_screenIndex >= MissionIISpriteTraits.TitleScreen.ImageCount)
                {
                    _screenIndex = 1;
                }
                _countDown = Constants.TitleScreenRollCycles;
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISpriteTraits.TitleScreen.GetHostImageObject(_screenIndex));
        }
    }

    public class MissionIIStartNewGameMode : MissionIIGameMode
    {
        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            using (var sr = new StreamReader("Resources\\Levels.txt"))
            {
                var loadedWorld = MissionIIClassLibrary.LevelFileParser.Parse(sr);
                MissionIIClassLibrary.LevelFileValidator.ExpectValidPathsInWorld(loadedWorld);
                MissionIIClassLibrary.LevelExpander.ExpandWallsInWorld(loadedWorld, MissionIISpriteTraits.PatternResamplingSprite);
                
                var cybertronGameBoard = new MissionIIClassLibrary.MissionIIGameBoard()
                {
                    TheWorldWallData = loadedWorld,
                    BoardWidth = Constants.ScreenWidth,
                    BoardHeight = Constants.ScreenHeight,
                    LevelNumber = Constants.StartLevelNumber,
                    Lives = Constants.InitialLives
                };

                cybertronGameBoard.PrepareForNewLevel();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            // This is non-visual
        }
    }

    public class MissionIIEnteringLevelMode : MissionIIGameMode
    {
        private MissionIIGameBoard _cybertronGameBoard;
        private int _countDown = Constants.EnteringLevelScreenCycles;

        public MissionIIEnteringLevelMode(MissionIIGameBoard theGameBoard)
        {
            _cybertronGameBoard = theGameBoard;
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (MissionIIModes.HandlePause(theKeyStates, this)) return;
            if (_countDown == Constants.EnteringLevelScreenCycles)
            {
                MissionIISounds.Play(MissionIISounds.EnteringLevel);
            }
            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode 
                    = new MissionIIGamePlayMode(_cybertronGameBoard);
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISpriteTraits.EnteringLevel.GetHostImageObject(0));

            // Show the things you need to find on this level.

            int x = Constants.ScreenWidth / 2;
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

    public class MissionIIGamePlayMode : MissionIIGameMode
    {
        private MissionIIGameBoard _cybertronGameBoard;

        public MissionIIGamePlayMode(MissionIIGameBoard cybertronGameBoard)
        {
            _cybertronGameBoard = cybertronGameBoard;
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (MissionIIModes.HandlePause(theKeyStates, this)) return;
            _cybertronGameBoard.Update(theKeyStates); // TODO: pull logic into this class
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            _cybertronGameBoard.DrawBoardToTarget(drawingTarget);
        }
    }

    public class MissionIILeavingLevelMode : MissionIIGameMode
    {
        private MissionIIGameBoard _cybertronGameBoard;
        private int _countDown = Constants.LeavingLevelCycles;

        public MissionIILeavingLevelMode(MissionIIGameBoard cybertronGameBoard)
        {
            _cybertronGameBoard = cybertronGameBoard;
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (MissionIIModes.HandlePause(theKeyStates, this)) return;
            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                var thisLevelNumber = _cybertronGameBoard.LevelNumber;
                ++thisLevelNumber;
                _cybertronGameBoard.LevelNumber = thisLevelNumber;
                _cybertronGameBoard.PrepareForNewLevel();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            _cybertronGameBoard.DrawBoardToTarget(drawingTarget);
        }
    }

    public class MissionIIGameOverMode : MissionIIGameMode
    {
        private int _countDown = Constants.GameOverMessageCycles;

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (_countDown == Constants.GameOverMessageCycles)
            {
                MissionIISounds.Play(MissionIISounds.GameOver);
            }
            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new MissionIITitleScreenMode();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawFirstSpriteScreenCentred(MissionIISpriteTraits.GameOver);
        }
    }

    public class MissionIIPauseMode : MissionIIGameMode
    {
        private MissionIIGameMode _originalMode;
        private bool _keyReleaseSeen;
        private bool _restartGameOnNextRelease;

        public MissionIIPauseMode(MissionIIGameMode originalMode)
        {
            _originalMode = originalMode;
            _keyReleaseSeen = false; // PAUSE key is held at the time this object is created.
            _restartGameOnNextRelease = false;
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (!_keyReleaseSeen)
            {
                if (!theKeyStates.Pause)
                {
                    _keyReleaseSeen = true;
                    if (_restartGameOnNextRelease)
                    {
                        MissionIIGameModeSelector.ModeSelector.CurrentMode = _originalMode;
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
            drawingTarget.DrawFirstSpriteScreenCentred(MissionIISpriteTraits.Paused);
        }
    }
}
