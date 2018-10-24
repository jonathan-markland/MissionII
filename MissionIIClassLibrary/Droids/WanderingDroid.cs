
using System;
using GameClassLibrary.Math;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Sound;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingDroid : BaseDroid
    {
        public WanderingDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action manDestroyAction, Action<Rectangle, MovementDeltas, bool> fireBullet, Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel, Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
            : base(
                  MissionIISprites.Monster2, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  manDestroyAction,
                  startExplosion)
        {
            base.SetIntelligenceProvider(
                new SingleMinded(this, freeDirectionFinder, fireBullet, moveAdversaryOnePixel));
        }

        public override int KillScore
        {
            get { return Constants.WanderingDroidKillScore; }
        }
    }
}
