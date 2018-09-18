
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Controls.Hiscore;

namespace GameClassLibrary.Modes
{
    public class HiScoreEntry : GameMode
    {
        public static HiScoreScreenModel HiScoreTableModel;  // TODO: reconsider

        private uint _countDown;
        private readonly uint _screenCycles;
        private readonly HiScoreScreenControl _hiScoreScreenControl;
        private readonly Font _enterNameFont;
        private readonly Font _tableFont;
        private readonly SpriteTraits _cursorSprite;
        private readonly Func<GameMode> _getTitleScreenModeFunction;
        private readonly Func<GameMode> _getStartNewGameModeFunction;
        private readonly Func<GameMode> _getRollOverModeFunction;
        private readonly SpriteTraits _backgroundSprite;



        /// <summary>
        /// Construction must be called once on program start up.
        /// </summary>
        public static void StaticInit(uint initialLowestHiScore, uint initialHiScoresIncrement)
        {
            HiScoreTableModel =
                new HiScoreScreenModel(
                    initialLowestHiScore,
                    initialHiScoresIncrement);
        }



        /// <summary>
        /// Constructor for finishing a game when scoreAchieved > 0.
        /// If scoreAchieved == 0, we just show the screen.
        /// </summary>
        public HiScoreEntry(
            uint screenCycles,
            SpriteTraits backgroundSprite,
            Font enterNameFont,
            Font tableFont,
            SpriteTraits cursorSprite,
            uint scoreAchieved,
            Func<GameMode> getTitleScreenModeFunction,
            Func<GameMode> getStartNewGameModeFunction,
            Func<GameMode> getRollOverModeFunction)
        {
            _backgroundSprite = backgroundSprite;
            _getTitleScreenModeFunction = getTitleScreenModeFunction;
            _getStartNewGameModeFunction = getStartNewGameModeFunction;
            _getRollOverModeFunction = getRollOverModeFunction;
            _tableFont = tableFont;
            _enterNameFont = enterNameFont;
            _cursorSprite = cursorSprite;
            _screenCycles = screenCycles;
            _countDown = screenCycles;

            _hiScoreScreenControl = new HiScoreScreenControl(
                new GameClassLibrary.Math.Rectangle(10, 70, 300, 246 - 70),// TODO: screen dimension constants!
                _tableFont,
                _cursorSprite,
                HiScoreTableModel);

            _hiScoreScreenControl.ForceEnterScore(scoreAchieved);
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_hiScoreScreenControl.InEditMode)
            {
                _hiScoreScreenControl.AdvanceOneCycle(theKeyStates);
            }
            else
            {
                var hiScoreShow = new HiScoreShow(
                        _screenCycles, _backgroundSprite, _tableFont,
                        _getStartNewGameModeFunction, _getTitleScreenModeFunction);

                ActiveMode = new ChangeStageFreeze(
                    _screenCycles,
                    hiScoreShow,
                    null,
                    _getTitleScreenModeFunction);
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, _backgroundSprite.GetHostImageObject(0));
            _hiScoreScreenControl.DrawScreen(drawingTarget);
            drawingTarget.DrawText(
                Screen.Width / 2, 56, "ENTER YOUR NAME", _enterNameFont, TextAlignment.Centre);
        }
    }
}
