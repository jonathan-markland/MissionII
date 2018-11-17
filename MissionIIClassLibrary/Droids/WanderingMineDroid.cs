
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Sound;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingMineDroid : BaseDroid  // Didn't make it into final release
    {
        public WanderingMineDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action<GameObject> manWalksIntoDroidAction, Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> tryMoveAdversaryOnePixel, Func<Rectangle> getManExtents, Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
            : base(
                  MissionIISprites.Monster5, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  manWalksIntoDroidAction,
                  startExplosion)
        {
            base.SetIntelligenceProvider(
                WanderingMine.NewWanderingMine(this, freeDirectionFinder, manWalksIntoDroidAction,
                      GameClassLibrary.ArtificialIntelligence.Constants.WanderingMineSpeedDivisor, 
                      tryMoveAdversaryOnePixel, getManExtents));
        }
    }
}
