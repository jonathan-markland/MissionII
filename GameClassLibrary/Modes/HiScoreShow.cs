
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Controls.Hiscore;

namespace GameClassLibrary.Modes
{
    public static class HiScoreShow
    {
        public static ModeFunctions New(
            uint screenCycles,
            SpriteTraits backgroundSprite,
            Font tableFont,
            Func<ModeFunctions> getStartNewGameModeFunction,
            Func<ModeFunctions> getRollOverModeFunction)
        {
            uint countDown = screenCycles;

            var hiScoreScreenControl = new HiScoreScreenControl(
                new GameClassLibrary.Math.Rectangle(10, 70, 300, 246 - 70),// TODO: screen dimension constants!
                tableFont,
                null,
                HiScoreEntry.HiScoreTableModel);

            return new ModeFunctions(

                // -- Advance one cycle --

                keyStates =>
                {
                    if (countDown > 0)
                    {
                        if (keyStates.Fire)
                        {
                            if (countDown < (screenCycles * 3 / 4))
                            {
                                GameMode.ActiveMode = getStartNewGameModeFunction();
                            }
                        }
                        --countDown;
                    }
                    else
                    {
                        GameMode.ActiveMode = getRollOverModeFunction();
                    }
                },

                // -- Draw --

                drawingTarget =>
                {
                    drawingTarget.ClearScreen();
                    drawingTarget.DrawSprite(0, 0, backgroundSprite.GetHostImageObject(0));
                    hiScoreScreenControl.DrawScreen(drawingTarget);
                });
        }
    }
}
