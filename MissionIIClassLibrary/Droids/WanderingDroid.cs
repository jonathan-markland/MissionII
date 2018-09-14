
using System;
using GameClassLibrary.Math;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingDroid : BaseDroid
    {
        public WanderingDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action manDestroyAction)
            : base(
                  MissionIISprites.Monster2, 
                  MissionIISprites.Explosion, 
                  new ArtificialIntelligence.SingleMinded(freeDirectionFinder), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.WanderingDroidKillScore; }
        }
    }
}
