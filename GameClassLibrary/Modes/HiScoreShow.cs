
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Controls.Hiscore;

namespace GameClassLibrary.Modes
{
    public class HiScoreShow : GameMode
    {
        private uint _countDown;
        private readonly uint _screenCycles;
        private readonly HiScoreScreenControl _hiScoreScreenControl;
        private readonly Font _tableFont;
        private readonly Func<GameMode> _getStartNewGameModeFunction;
        private readonly Func<GameMode> _getRollOverModeFunction;
        private readonly SpriteTraits _backgroundSprite;



        public HiScoreShow(
            uint screenCycles,
            SpriteTraits backgroundSprite,
            Font tableFont,
            Func<GameMode> getStartNewGameModeFunction,
            Func<GameMode> getRollOverModeFunction)
        {
            _backgroundSprite = backgroundSprite;
            _getStartNewGameModeFunction = getStartNewGameModeFunction;
            _getRollOverModeFunction = getRollOverModeFunction;
            _tableFont = tableFont;
            _screenCycles = screenCycles;
            _countDown = screenCycles;

            _hiScoreScreenControl = new HiScoreScreenControl(
                new GameClassLibrary.Math.Rectangle(10, 70, 300, 246 - 70),// TODO: screen dimension constants!
                _tableFont,
                null,
                HiScoreEntry.HiScoreTableModel);
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_countDown > 0)
            {
                if (theKeyStates.Fire)
                {
                    if (_countDown < (_screenCycles * 3 / 4))
                    {
                        ActiveMode = _getStartNewGameModeFunction();
                    }
                }
                --_countDown;
            }
            else
            {
                ActiveMode = _getRollOverModeFunction();
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, _backgroundSprite.GetHostImageObject(0));
            _hiScoreScreenControl.DrawScreen(drawingTarget);
        }
    }
}
