
using System.Linq;
using System.Collections.Generic;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class RotatingInstructions : GameMode
    {
        private static char[] _pageSeparator = new char[] { '\v' };
        private static char[] _rowSeparator = new char[] { '\n' };

        private int _countDown;
        private int _pageVisibleCycles;
        private List<List<string>> _listOfPages;



        /// <summary>
        /// Construct new multi-screen instruction pages.
        /// </summary>
        /// <param name="instructionPages">Instruction text using \n between rows, and \v between pages.</param>
        public RotatingInstructions(string instructionPages, int pageVisibleCycles)
        {
            _listOfPages = StringToPages(instructionPages);
            _pageVisibleCycles = pageVisibleCycles;
            _countDown = pageVisibleCycles * _listOfPages.Count;
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (theKeyStates.Fire)
            {
                ActiveMode = new StartNewGame();
            }
            else if (_countDown == 0)
            {
                ActiveMode = new TitleScreen();
            }
            else
            {
                --_countDown;
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISprites.Background.GetHostImageObject(0));

            var theFont = MissionIIFonts.NarrowFont;
            var cx = GameClassLibrary.Graphics.Screen.Width / 2;
            var c = TextAlignment.Centre;

            var pageIndex = _countDown / _pageVisibleCycles;

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
