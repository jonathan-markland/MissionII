﻿using System.Collections.Generic;
using System.Linq;

namespace GameClassLibrary
{
    public class UniqueNumberAllocator
    {
        private List<int> _theList;

        public UniqueNumberAllocator(int baseNumber, int countOfItems)
        {
            _theList = Enumerable.Range(baseNumber, countOfItems).ToList();
            Business.Shuffle(_theList, CybertronGameStateUpdater.RandomGenerator);
        }

        public int Next()
        {
            var resultValue = _theList.Last();
            _theList.RemoveAt(_theList.Count - 1);
            return resultValue;
        }
    }

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
