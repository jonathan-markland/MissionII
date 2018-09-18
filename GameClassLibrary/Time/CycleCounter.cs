
namespace GameClassLibrary.Time
{
    public static class CycleCounter
    {
        private static uint _count32 = 0;

        /// <summary>
        /// Current value of the cycle counter.
        /// Incremented by 1 per game cycle.
        /// </summary>
        public static uint Count32
        {
            get { return _count32; }
        }

        /// <summary>
        /// Only to be called by the root function that handles game cycle updates.
        /// </summary>
        public static void IncrementCycleCounter()
        {
            ++_count32;
        }
    }
}
