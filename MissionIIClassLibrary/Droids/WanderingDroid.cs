
using System;
using GameClassLibrary.Math;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingDroid : BaseDroid
    {
        public WanderingDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action manDestroyAction, Action<Rectangle, MovementDeltas, bool> fireBullet, Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel)
            : base(
                  MissionIISprites.Monster2, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  new SingleMinded(freeDirectionFinder, fireBullet, moveAdversaryOnePixel), 
                  manDestroyAction)
        {
        }

        public override int KillScore
        {
            get { return Constants.WanderingDroidKillScore; }
        }
    }
}
