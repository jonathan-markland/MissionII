
using System;
using GameClassLibrary.Math;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingMineDroid : BaseDroid  // Didn't make it into final release
    {
        public WanderingMineDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action manDestroyAction)
            : base(
                  MissionIISprites.Monster5, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new ArtificialIntelligence.WanderingMine(freeDirectionFinder, manDestroyAction), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.WanderingMineDroidKillScore; }
        }
    }
}
