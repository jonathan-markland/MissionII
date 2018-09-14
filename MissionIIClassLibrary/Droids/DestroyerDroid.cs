
using System;
using GameClassLibrary.Math;
using GameClassLibrary.ArtificialIntelligence;

namespace MissionIIClassLibrary.Droids
{
    public class DestroyerDroid : BaseDroid
    {
        public DestroyerDroid(Action manDestroyAction, Action<Rectangle, MovementDeltas, bool> fireBullet)
            : base(
                  MissionIISprites.Monster3, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new FiringAttractor(fireBullet), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.DestroyerDroidKillScore; }
        }
    }
}
