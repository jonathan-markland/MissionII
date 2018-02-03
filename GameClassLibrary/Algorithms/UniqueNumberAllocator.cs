using System.Collections.Generic;
using System.Linq;
using GameClassLibrary.Math;

namespace GameClassLibrary.Algorithms
{
    public class UniqueNumberAllocator
    {
        private List<int> _theList;

        public UniqueNumberAllocator(int baseNumber, int countOfItems)
        {
            _theList = Enumerable.Range(baseNumber, countOfItems).ToList();
            _theList.Shuffle(Rng.Generator);
        }

        public int Next()
        {
            var resultValue = _theList.Last();
            _theList.RemoveAt(_theList.Count - 1);
            return resultValue;
        }
    }
}
