
using System;
using GameClassLibrary.Math;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingMineDroid : BaseDroid  // Didn't make it into final release
    {
        public WanderingMineDroid(Func<Rectangle, FoundDirections> freeDirectionFinder)
            : base(MissionIISprites.Monster5, new ArtificialIntelligence.WanderingMine(freeDirectionFinder))
        {
        }

        public override int KillScore
        {
            get { return Constants.WanderingMineDroidKillScore; }
        }
    }
}
