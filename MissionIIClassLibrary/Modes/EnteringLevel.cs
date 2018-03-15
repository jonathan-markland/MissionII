using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class EnteringLevel : BaseGameMode
    {
        private MissionIIGameBoard _gameBoard;
        private int _countDown = Constants.EnteringLevelScreenCycles;
        private string _levelAccessCode;

        public EnteringLevel(MissionIIGameBoard theGameBoard)
        {
            _gameBoard = theGameBoard;
            if (ThisLevelHasAccessCode)
            {
                _levelAccessCode = GameClassLibrary.Algorithms.LevelAccessCodes.GetForLevel(_gameBoard.LevelNumber);
            }
        }

        private bool ThisLevelHasAccessCode
        {
            get
            {
                var l = _gameBoard.LevelNumber;
                return (l >= Constants.FirstLevelWithAccessCode && l <= Constants.LastLevelWithAccessCode);
            }
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (MissionIIModes.HandlePause(_gameBoard, theKeyStates, this)) return;
            if (_countDown == Constants.EnteringLevelScreenCycles)
            {
                GameClassLibrary.Sound.SoundTraits.StopBackgroundMusic();
                MissionIISounds.EnteringLevel.Play();
            }
            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode 
                    = new GamePlay(_gameBoard);
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISprites.Background.GetHostImageObject(0));

            var cx = Constants.ScreenWidth / 2;

            drawingTarget.DrawText(cx, 40, "LEVEL " + _gameBoard.LevelNumber, MissionIIFonts.GiantFont, TextAlignment.Centre);

            if (ThisLevelHasAccessCode)
            {
                drawingTarget.DrawText(Constants.ScreenWidth - 10, 50, "ACCESS", MissionIIFonts.NarrowFont, TextAlignment.Right);
                drawingTarget.DrawText(Constants.ScreenWidth - 10, 80, _levelAccessCode, MissionIIFonts.NarrowFont, TextAlignment.Right);
            }

            drawingTarget.DrawText(cx, 110, "FIND THE FOLLOWING ITEMS", MissionIIFonts.NarrowFont, TextAlignment.Centre);

            // Show the things you need to find on this level.

            int x = Constants.ScreenWidth / 2;
            int y = 130; // TODO: constant
            int dy = 24; // TODO: constant

            _gameBoard.ForEachThingWeHaveToFindOnThisLevel(
                o =>
                {
                    drawingTarget.DrawFirstSpriteCentred(x, y, o.SpriteTraits);
                    y += dy;
                });


            drawingTarget.DrawText(cx, 210, "THEN TAKE TO THE SAFE", MissionIIFonts.NarrowFont, TextAlignment.Centre);
            drawingTarget.DrawFirstSpriteCentred(x, 230, MissionIISprites.Safe);
        }
    }
}
