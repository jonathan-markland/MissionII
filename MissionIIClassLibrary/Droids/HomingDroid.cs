
using System;
using GameClassLibrary.ArtificialIntelligence;

namespace MissionIIClassLibrary.Droids
{
    public class HomingDroid : BaseDroid
    {
        public HomingDroid(Action manDestroyAction)
            : base(
                  MissionIISprites.Monster1, 
                  MissionIISprites.Explosion, 
                  MissionIISounds.Explosion,
                  new Attractor(), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.HomingDroidKillScore; }
        }
    }
}
