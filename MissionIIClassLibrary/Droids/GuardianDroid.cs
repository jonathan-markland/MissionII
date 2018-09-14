
using System;

namespace MissionIIClassLibrary.Droids
{
    public class GuardianDroid : BaseDroid  // Didn't make it into final release
    {
        public GuardianDroid(Action manDestroyAction)
            : base(
                  MissionIISprites.Monster4, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new ArtificialIntelligence.Guardian(manDestroyAction), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.GuardianDroidKillScore; }
        }
    }
}
