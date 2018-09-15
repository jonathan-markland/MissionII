
using System;
using GameClassLibrary.Math;
using GameClassLibrary.ArtificialIntelligence;

namespace MissionIIClassLibrary.Droids
{
    public class LinearMoverDroid : BaseDroid  // TODO: This is a development test and should be removed.
    {
        public LinearMoverDroid(Action manDestroyAction)
            : base(
                  MissionIISprites.Monster3,
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new LinearMover(new Point(100, 50), new Point(200, 50), 1, manDestroyAction),
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
