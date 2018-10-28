
using System;
using System.Collections.Generic;
using System.Linq;
using GameClassLibrary.Graphics;

namespace GameClassLibrary.Modes
{
    public static class RotatingInstructions
    {
        private static char[] _pageSeparator = new char[] { '\v' };
        private static char[] _rowSeparator = new char[] { '\n' };



        public static ModeFunctions New(
            SpriteTraits backgroundSprite,
            Font theFont,
            string instructionPages,
            int pageVisibleCycles,
            Func<ModeFunctions> getStartGameMode,
            Func<ModeFunctions> getNextModeFunction)
        {
            var listOfPages = StringToPages(instructionPages);

            int initialCycles = pageVisibleCycles * listOfPages.Count;
            int countDown = pageVisibleCycles * listOfPages.Count;

            return new ModeFunctions(

                // -- Advance one cycle --

                keyStates => 
                {
                    if (keyStates.Fire)
                    {
                        GameMode.ActiveMode = getStartGameMode();
                    }
                    else if (countDown == 0)
                    {
                        GameMode.ActiveMode = getNextModeFunction();
                    }
                    else
                    {
                        --countDown;
                    }
                },

                // -- Draw --

                drawingTarget => 
                {
                    drawingTarget.ClearScreen();
                    drawingTarget.DrawSprite(0, 0, backgroundSprite.GetHostImageObject(0));

                    var cx = Screen.Width / 2;
                    var c = TextAlignment.Centre;
                    var pageIndex = (initialCycles - countDown) / pageVisibleCycles;

                    if (pageIndex < listOfPages.Count)
                    {
                        var thisPage = listOfPages[pageIndex];
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
                });
        }

        

        private static List<List<string>> StringToPages(string instructionPages)  // TODO: MOVE OUT?
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
