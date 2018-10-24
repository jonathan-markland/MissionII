
using System;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Sound;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Droids
{
    public class HomingDroid : BaseDroid
    {
        public HomingDroid(
            Action manDestroyAction, 
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents,
            Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
            : base(
                  MissionIISprites.Monster1, 
                  MissionIISprites.Explosion, 
                  MissionIISounds.Explosion,
                  manDestroyAction,
                  startExplosion)
        {
            base.SetIntelligenceProvider(
                new Attractor(this, moveAdversaryOnePixel, getManExtents));
        }

        public override int KillScore
        {
            get { return Constants.HomingDroidKillScore; }
        }
    }
}
