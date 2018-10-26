
using GameClassLibrary.Input;

namespace GameClassLibrary.Modes
{
    public class GameMode
    {
        /// <summary>
        /// The currently active game mode.
        /// </summary>
        public static ModeFunctions ActiveMode;

        public static void AdvanceActiveModeOneCycle(KeyStates theKeyStates)
        {
            Time.CycleCounter.IncrementCycleCounter();
            ActiveMode.AdvanceOneCycle(theKeyStates);
        }
    }
}
