
namespace GameClassLibrary.Time
{
    public struct CycleSnapshot
    {
        public readonly ulong Count64;



        private CycleSnapshot(ulong cycleCount)
        {
            Count64 = cycleCount;
        }



        public static CycleSnapshot Now
        {
            get { return new CycleSnapshot(Time.CycleCounter.Count64); }
        }



        public bool HasElapsed(uint n)
        {
            return (Time.CycleCounter.Count64 - Count64) >= n;
        }
    }
}
