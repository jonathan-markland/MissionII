
using System;
using System.Collections.Generic;
using System.Linq;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Modes
{
    public class RotatingInstructions : GameMode
    {
        private static char[] _pageSeparator = new char[] { '\v' };
        private static char[] _rowSeparator = new char[] { '\n' };

        private int _countDown;
        private int _initialCycles;
        private int _pageVisibleCycles;
        private List<List<string>> _listOfPages;
        private SpriteTraits _backgroundSprite;
        private Font _theFont;
        private Func<GameMode> _getStartGameMode;
        private Func<GameMode> _getNextModeFunction;



        /// <summary>
        /// Construct new multi-screen instruction pages.
        /// </summary>
        /// <param name="instructionPages">Instruction text using \n between rows, and \v between pages.</param>
        public RotatingInstructions(
            SpriteTraits backgroundSprite,
            Font theFont,
            string instructionPages, 
            int pageVisibleCycles,
            Func<GameMode> getStartGameMode,
            Func<GameMode> getNextModeFunction)
        {
            _theFont = theFont;
            _backgroundSprite = backgroundSprite;
            _listOfPages = StringToPages(instructionPages);
            _pageVisibleCycles = pageVisibleCycles;
            _initialCycles = pageVisibleCycles * _listOfPages.Count;
            _countDown = pageVisibleCycles * _listOfPages.Count;
            _getNextModeFunction = getNextModeFunction;
            _getStartGameMode = getStartGameMode;
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (theKeyStates.Fire)
            {
                ActiveMode = _getStartGameMode();
            }
            else if (_countDown == 0)
            {
                ActiveMode = _getNextModeFunction();
            }
            else
            {
                --_countDown;
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, _backgroundSprite.GetHostImageObject(0));

            var theFont = _theFont;
            var cx = Screen.Width / 2;
            var c = TextAlignment.Centre;
            var pageIndex = (_initialCycles - _countDown) / _pageVisibleCycles;

            if (pageIndex < _listOfPages.Count)
            {
                var thisPage = _listOfPages[pageIndex];
                int y = (Screen.Height - thisPage.Count * theFont.Height * 2) / 2;
                foreach (var msgText in thisPage)
                {
                    if (msgText.Length > 0)
                    {
                        drawingTarget.DrawText(cx, y, msgText, theFont, c);
                    }
                    y += theFont.Height * 2;
                }
            }
        }



        private static List<List<string>> StringToPages(string instructionPages)
        {
            var listOfPages = new List<List<string>>();
            var thePages = instructionPages.Split(_pageSeparator, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var page in thePages)
            {
                var theLines = page.Split(_rowSeparator).ToList();
                listOfPages.Add(theLines);
            }
            return listOfPages;
        }
    }
}
