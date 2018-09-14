
using System;
using GameClassLibrary.Math;
using GameClassLibrary.ArtificialIntelligence;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingMineDroid : BaseDroid  // Didn't make it into final release
    {
        public WanderingMineDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action manDestroyAction)
            : base(
                  MissionIISprites.Monster5, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new WanderingMine(freeDirectionFinder, manDestroyAction, 
                      GameClassLibrary.ArtificialIntelligence.Constants.WanderingMineSpeedDivisor), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.WanderingMineDroidKillScore; }
        }
    }
}
