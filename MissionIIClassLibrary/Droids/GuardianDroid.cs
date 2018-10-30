
using System;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Sound;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Droids
{
    public class GuardianDroid : BaseDroid  // Didn't make it into final release
    {
        public GuardianDroid(Action<GameObject> manWalksIntoDroidAction, Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> tryMoveAdversaryOnePixel, Func<Rectangle> getManExtents, Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
            : base(
                  MissionIISprites.Monster4, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  manWalksIntoDroidAction,
                  startExplosion)
        {
            base.SetIntelligenceProvider(
                Guardian.New(this, manWalksIntoDroidAction, tryMoveAdversaryOnePixel, getManExtents));
        }

        public override int KillScore
        {
            get { return Constants.GuardianDroidKillScore; }
        }
    }
}
