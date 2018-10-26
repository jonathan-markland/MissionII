
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Controls;

namespace GameClassLibrary.Modes
{
    public static class PauseWithChangeLevel
    {
        public static ModeFunctions New(
            ModeFunctions originalMode,
            SpriteTraits pauseSprite,
            Sound.SoundTraits pauseSound,
            int accessCodeTextTopY,
            Font accessCodesFont,
            Sound.SoundTraits addLetterSound,
            Func<string, bool> tryCode,
            Func<ModeFunctions> getNextModeFunction,
            Func<bool> canChangeLevel)
        {
            bool keyReleaseSeen = false; // PAUSE key is taken as held, at the time this object is created.
            bool restartGameOnNextRelease = false;



            AccessCodeAccumulatorControl accessCodeControl = null;

            if (accessCodesFont != null
                && addLetterSound != null
                && tryCode != null
                && canChangeLevel != null)
            {
                accessCodeControl = new AccessCodeAccumulatorControl(
                    Screen.Width / 2,
                    accessCodeTextTopY,
                    4,
                    accessCode =>
                    {
                        if (tryCode(accessCode))
                        {
                            GameMode.ActiveMode = getNextModeFunction();
                        }
                        else
                        {
                            accessCodeControl?.ClearEntry();
                            pauseSound.Play();
                        }
                    },
                    addLetterSound,
                    accessCodesFont);
            }



            return new ModeFunctions(

                // -- Advance one cycle --

                keyStates =>
                {
                    if (!keyReleaseSeen)
                    {
                        if (!keyStates.Pause)
                        {
                            keyReleaseSeen = true;
                            if (restartGameOnNextRelease)
                            {
                                GameMode.ActiveMode = originalMode;
                            }
                        }
                    }
                    else if (keyStates.Pause)
                    {
                        restartGameOnNextRelease = true;
                        keyReleaseSeen = false;
                    }
                    else if (accessCodeControl != null && canChangeLevel()) // Hint:  Allow pause, but disallow changing to avoid cheating:  eg: Man.IsDead!
                    {
                        accessCodeControl.AdvanceOneCycle(keyStates);
                    }
                },

                // -- Draw --

                drawingTarget =>
                {
                    originalMode.Draw(drawingTarget);
                    drawingTarget.DrawFirstSpriteScreenCentred(pauseSprite);
                    accessCodeControl?.Draw(drawingTarget);
                });
        }
    }
}
