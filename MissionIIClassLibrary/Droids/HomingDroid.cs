
using System;

namespace MissionIIClassLibrary.Droids
{
    public class HomingDroid : BaseDroid
    {
        public HomingDroid(Action manDestroyAction)
            : base(
                  MissionIISprites.Monster1, 
                  MissionIISprites.Explosion, 
                  MissionIISounds.Explosion,
                  new ArtificialIntelligence.Attractor(), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.HomingDroidKillScore; }
        }
    }
}
