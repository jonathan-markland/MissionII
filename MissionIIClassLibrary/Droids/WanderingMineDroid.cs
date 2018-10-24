
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;
using GameClassLibrary.ArtificialIntelligence;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingMineDroid : BaseDroid  // Didn't make it into final release
    {
        public WanderingMineDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action manDestroyAction, Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel, Func<Rectangle> getManExtents)
            : base(
                  MissionIISprites.Monster5, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  manDestroyAction)
        {
            base.SetIntelligenceProvider(
                new WanderingMine(this, freeDirectionFinder, manDestroyAction,
                      GameClassLibrary.ArtificialIntelligence.Constants.WanderingMineSpeedDivisor, 
                      moveAdversaryOnePixel, getManExtents));
        }

        public override int KillScore
        {
            get { return Constants.WanderingMineDroidKillScore; }
        }
    }
}
