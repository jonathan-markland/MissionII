
using System;
using GameClassLibrary.Graphics;

namespace GameClassLibrary.Modes
{
    /// <summary>
    /// Shows a placard sprite for a period, with an accompanying sound.
    /// Then move to the next mode using the given function.
    /// </summary>
    public static class PlacardScreen
    {
        public static ModeFunctions New(
            int placardCycles,
            SpriteTraits placardSprite,
            Font largeFont, string messageText,
            Sound.SoundTraits placardSound,
            Func<ModeFunctions> getNextModeFunction)
        {
            var countDown = placardCycles;
            bool firstCycle = true;

            return new ModeFunctions(

                // -- Advance one cycle --

                keyStates =>
                {
                    if (firstCycle)
                    {
                        firstCycle = false;
                        placardSound.Play();
                    }

                    if (countDown > 0)
                    {
                        --countDown;
                    }
                    else
                    {
                        GameMode.ActiveMode = getNextModeFunction();
                    }
                },

                // -- Draw --

                drawingTarget =>
                {
                    drawingTarget.ClearScreen();
                    drawingTarget.DrawFirstSpriteScreenCentred(placardSprite);
                    drawingTarget.DrawText(Screen.Width / 2, 100, messageText, largeFont, TextAlignment.Centre);
                });
        }
    }
}
