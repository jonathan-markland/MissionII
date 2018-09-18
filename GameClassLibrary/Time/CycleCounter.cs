
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

        /// <summary>
        /// Only to be called by the root function that handles game cycle updates.
        /// </summary>
        public static void IncrementCycleCounter()
        {
            ++_count64;
        }
    }

    public struct CycleSnapshot
    {
        public readonly ulong Count64;

        private CycleSnapshot(ulong cycleCount)
        {
            Count64 = cycleCount;
        }

        public static CycleSnapshot New
        {
            get { return new CycleSnapshot(Time.CycleCounter.Count64); }
        }

        public bool HasElapsed(uint n)
        {
            return (Time.CycleCounter.Count64 - Count64) >= n;
        }

        public bool FirstCycle
        {
            get { return (Time.CycleCounter.Count64 - Count64) == 0; }
        }
    }
}
