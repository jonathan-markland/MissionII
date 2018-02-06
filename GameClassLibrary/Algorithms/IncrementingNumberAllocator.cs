namespace GameClassLibrary.Algorithms
{
    public class IncrementingNumberAllocator // For debugging purposes.
    {
        private int _n;

        public IncrementingNumberAllocator(int baseNumber, int countOfItems)
        {
            _n = baseNumber;
        }

        public int Next()
        {
            var resultValue = _n;
            ++_n;
            return resultValue;
        }
    }
}
