
using System;

namespace MissionIIClassLibrary.Droids
{
    public class GuardianDroid : BaseDroid  // Didn't make it into final release
    {
        public GuardianDroid(Action manDestroyAction)
            : base(MissionIISprites.Monster4, MissionIISprites.Explosion, new ArtificialIntelligence.Guardian(), manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.GuardianDroidKillScore; }
        }
    }
}
