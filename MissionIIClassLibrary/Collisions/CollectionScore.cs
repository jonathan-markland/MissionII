using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionIIClassLibrary.Collisions
{
    public static class CollectionScore
    {
        public static int Get(object o)
        {
            if (o is Interactibles.Key) return Constants.KeyCollectionScore;
            if (o is Interactibles.Gold) return Constants.GoldCollectionScore;
            if (o is Interactibles.Ring) return Constants.RingCollectionScore;
            if (o is Interactibles.InvincibilityAmulet) return Constants.InvincibilityAmuletScore;
            return 0;
        }
    }
}
