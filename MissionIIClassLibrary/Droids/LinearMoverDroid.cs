
using System;
using GameClassLibrary.Math;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Droids
{
    public class LinearMoverDroid : BaseDroid  // TODO: This is a development test and should be removed.
    {
        public LinearMoverDroid(Action manDestroyAction, Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel, Func<Rectangle> getManExtents)
            : base(
                  MissionIISprites.Monster3,
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  // TODO: Demo no longer works since we are using model-space coordinates:
                  manDestroyAction)
        {
            base.SetIntelligenceProvider(
                new LinearMover(this, new Point(100, 50), new Point(200, 50), 1, 
                manDestroyAction, moveAdversaryOnePixel, getManExtents));
        }

        public override int KillScore
        {
            get { return Constants.DestroyerDroidKillScore; }
        }
    }
}
