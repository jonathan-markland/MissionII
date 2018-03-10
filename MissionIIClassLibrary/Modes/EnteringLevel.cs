using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class EnteringLevel : BaseGameMode
    {
        private MissionIIGameBoard _gameBoard;
        private int _countDown = Constants.EnteringLevelScreenCycles;

        public EnteringLevel(MissionIIGameBoard theGameBoard)
        {
            _gameBoard = theGameBoard;
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (MissionIIModes.HandlePause(theKeyStates, this)) return;
            if (_countDown == Constants.EnteringLevelScreenCycles)
            {
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

            drawingTarget.DrawText(cx, 40, "LEVEL " + _gameBoard.LevelNumber, MissionIISprites.GiantFont, TextAlignment.Centre);
            drawingTarget.DrawText(cx, 110, "FIND THE FOLLOWING ITEMS", MissionIISprites.NarrowFont, TextAlignment.Centre);

            // Show the things you need to find on this level.

            int x = Constants.ScreenWidth / 2;
            int y = 150; // TODO: constant
            int dy = 24; // TODO: constant

            _gameBoard.ForEachThingWeHaveToFindOnThisLevel(
                o =>
                {
                    drawingTarget.DrawFirstSpriteCentred(x, y, o.SpriteTraits);
                    y += dy;
                });
        }
    }
}
