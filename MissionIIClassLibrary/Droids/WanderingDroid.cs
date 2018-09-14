
using System;
using GameClassLibrary.Math;
using GameClassLibrary.ArtificialIntelligence;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingDroid : BaseDroid
    {
        public WanderingDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action manDestroyAction, Action<Rectangle, MovementDeltas, bool> fireBullet)
            : base(
                  MissionIISprites.Monster2, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new SingleMinded(freeDirectionFinder, fireBullet), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.WanderingDroidKillScore; }
        }
    }
}
