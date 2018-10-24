
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;
using GameClassLibrary.ArtificialIntelligence;

namespace MissionIIClassLibrary.Droids
{
    public class DestroyerDroid : BaseDroid
    {
        public DestroyerDroid(Action manDestroyAction, Action<Rectangle, MovementDeltas, bool> fireBullet, Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel, Func<Rectangle> getManExtents)
            : base(
                  MissionIISprites.Monster3, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new FiringAttractor(fireBullet, moveAdversaryOnePixel, getManExtents), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.DestroyerDroidKillScore; }
        }
    }
}
