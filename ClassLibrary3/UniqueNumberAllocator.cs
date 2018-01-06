﻿using System.Collections.Generic;
using System.Linq;

namespace GameClassLibrary
{



    public static partial class CybertronGameStateUpdater
    {
        public class UniqueNumberAllocator
        {
            private List<int> _theList;

            public UniqueNumberAllocator(int baseNumber, int countOfItems)
            {
                _theList = Enumerable.Range(baseNumber, countOfItems).ToList();
                Business.Shuffle(_theList, RandomGenerator);
            }

            public int Next()
            {
                var resultValue = _theList.Last();
                _theList.RemoveAt(_theList.Count - 1);
                return resultValue;
            }
        }


    }
}
