
using System;
using GameClassLibrary.Graphics;

namespace GameClassLibrary.Modes
{
    public static class TitleScreenWithCredit
    {
        public static ModeFunctions New(
            int titleScreenRollCycles,
            SpriteTraits titleScreenBackground,
            Sound.SoundTraits introSound,
            Font fontLarge, string largeMessageText,
            Font font, string creditMessageText,
            Func<ModeFunctions> getStartGameModeObject,
            Func<ModeFunctions> getRollOntoScreenObject)
        {
            var countDown = titleScreenRollCycles;
            var fireButtonPressEnableTime = (titleScreenRollCycles * 3) / 4;
            bool releaseWaiting = true;
            bool firstCycle = true;

            return new ModeFunctions(

                // -- Advance one cycle --

                keyStates =>
                {
                    if (firstCycle)
                    {
                        introSound.PlayAsBackgroundMusic();
                        firstCycle = false;
                    }

                    if (keyStates.Fire)
                    {
                        if (releaseWaiting || countDown > fireButtonPressEnableTime) return;
                        GameMode.ActiveMode = getStartGameModeObject();
                    }

                    releaseWaiting = false;

                    if (countDown > 0)
                    {
                        --countDown;
                    }
                    else if (getRollOntoScreenObject != null)
                    {
                        GameMode.ActiveMode = getRollOntoScreenObject();  // time to leave this mode  eg: a sequence of modes for rolling titles.
                    }
                },

                // -- Draw --

                drawingTarget =>
                {
                    drawingTarget.ClearScreen();
                    drawingTarget.DrawSprite(0, 0, titleScreenBackground.GetHostImageObject(0));
                    drawingTarget.DrawText(160, 100, largeMessageText, fontLarge, TextAlignment.Centre);
                    if (countDown < titleScreenRollCycles / 2)
                    {
                        drawingTarget.DrawText(310, 230, creditMessageText, font, TextAlignment.Right);
                    }
                });
        }
    }
}
