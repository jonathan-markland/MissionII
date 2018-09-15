
using System;
using GameClassLibrary.ArtificialIntelligence;

namespace MissionIIClassLibrary.Droids
{
    public class BouncingDroid : BaseDroid  // TODO: This is a development test and should be removed.
    {
        public BouncingDroid(Action manDestroyAction)
            : base(
                  MissionIISprites.Monster2,
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new Bouncing(0, 256, 256, 2, manDestroyAction, 1),
                  manDestroyAction
                  )
        {
        }

        public override int KillScore
        {
            get { return Constants.DestroyerDroidKillScore; }
        }
    }
}
