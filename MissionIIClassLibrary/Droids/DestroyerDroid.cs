
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Sound;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Droids
{
    public class DestroyerDroid : BaseDroid
    {
        public DestroyerDroid(
            Action<GameObject> manWalksIntoDroidAction, 
            Action<Rectangle, MovementDeltas, bool> fireBullet, 
            Action<GameObject, MovementDeltas> moveAdversaryOnePixel, 
            Func<Rectangle> getManExtents, 
            Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
            : base(
                  MissionIISprites.Monster3, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  manWalksIntoDroidAction,
                  startExplosion)
        {
            base.SetIntelligenceProvider(
                FiringAttractor.New(this, fireBullet, moveAdversaryOnePixel, getManExtents));
        }

        public override int KillScore
        {
            get { return Constants.DestroyerDroidKillScore; }
        }
    }
}
