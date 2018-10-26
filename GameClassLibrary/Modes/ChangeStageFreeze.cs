
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Modes
{
    /// <summary>
    /// Freeze the display of the current mode for a period, then
    /// change mode.  A sound is played to accompany the freeze.
    /// </summary>
    public static class ChangeStageFreeze
    {
        public static ModeFunctions New(
            uint freezeCycles,
            ModeFunctions previousMode,
            Sound.SoundTraits optionalFreezeSound,
            Func<ModeFunctions> getNextModeFunction)
        {
            optionalFreezeSound?.Play(); // TODO: No, do in AdvanceOneCycle()

            var startTime = Time.CycleSnapshot.Now;

            return new ModeFunctions(

                // -- Advance one cycle --

                keyStates =>
                {
                    if (startTime.HasElapsed(freezeCycles))
                    {
                        GameMode.ActiveMode = getNextModeFunction();
                    }
                },

                // -- Draw --

                drawingTarget =>
                {
                    previousMode.Draw(drawingTarget);
                });
        }
    }
}
