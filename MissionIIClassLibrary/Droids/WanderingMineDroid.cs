
using System;
using GameClassLibrary.Math;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingMineDroid : BaseDroid  // Didn't make it into final release
    {
        public WanderingMineDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action manDestroyAction)
            : base(MissionIISprites.Monster5, MissionIISprites.Explosion, new ArtificialIntelligence.WanderingMine(freeDirectionFinder), manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.WanderingMineDroidKillScore; }
        }
    }
}
