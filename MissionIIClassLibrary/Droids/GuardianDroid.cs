
using System;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary.Droids
{
    public class GuardianDroid : BaseDroid  // Didn't make it into final release
    {
        public GuardianDroid(Action manDestroyAction, Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel, Func<Rectangle> getManExtents)
            : base(
                  MissionIISprites.Monster4, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new Guardian(manDestroyAction, moveAdversaryOnePixel, getManExtents), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.GuardianDroidKillScore; }
        }
    }
}
