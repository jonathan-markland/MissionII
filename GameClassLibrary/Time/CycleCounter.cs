
namespace GameClassLibrary.Time
{
	public static class CycleCounter
    {
        private static ulong _count64 = 0;



        /// <summary>
        /// Current value of the cycle counter.
        /// Incremented by 1 per game cycle.
        /// </summary>
        public static uint Count32
        {
            get { return (uint) _count64; }
        }



        public static uint Count64
        {
            get { return (uint)_count64; }
        }



        public static bool Every(int n)
        {
            return Count32 % n == 0;
        }



        public static bool EveryOtherCycle()
        {
            return (Count32 & 1) == 0;
        }



        /// <summary>
        /// Only to be called by the root function that handles game cycle updates.
        /// </summary>
        public static void IncrementCycleCounter()
        {
            ++_count64;
        }
    }
}
