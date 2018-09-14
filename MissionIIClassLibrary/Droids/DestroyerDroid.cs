
using System;

namespace MissionIIClassLibrary.Droids
{
    public class DestroyerDroid : BaseDroid
    {
        public DestroyerDroid(Action manDestroyAction)
            : base(
                  MissionIISprites.Monster3, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new ArtificialIntelligence.FiringAttractor(), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.DestroyerDroidKillScore; }
        }
    }
}
