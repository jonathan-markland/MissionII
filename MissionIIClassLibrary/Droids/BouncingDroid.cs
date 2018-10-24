
using System;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;

namespace MissionIIClassLibrary.Droids
{
    public class BouncingDroid : BaseDroid  // TODO: This is a development test and should be removed.
    {
        public BouncingDroid(
            Action manDestroyAction,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
            : base(
                  MissionIISprites.Monster2,
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  manDestroyAction)
        {
            base.SetIntelligenceProvider(
                new Bouncing(this, 100, 200, 30, 2, manDestroyAction, 1, moveAdversaryOnePixel, getManExtents));
        }

        public override int KillScore
        {
            get { return Constants.DestroyerDroidKillScore; }
        }
    }
}
