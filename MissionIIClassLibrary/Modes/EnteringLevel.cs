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
                MissionIISounds.Play(MissionIISounds.EnteringLevel);
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
            drawingTarget.DrawSprite(0, 0, MissionIISpriteTraits.EnteringLevel.GetHostImageObject(0));
            drawingTarget.DrawNumber(160, 50, (uint) _gameBoard.LevelNumber, MissionIISpriteTraits.TheNumbers);

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
