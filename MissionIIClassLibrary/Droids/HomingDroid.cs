
using System;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Sound;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Droids
{
    public class HomingDroid : BaseDroid
    {
        public HomingDroid(
            Action<GameObject, MovementDeltas> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents,
            Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
            : base(
                  MissionIISprites.Monster1, 
                  MissionIISprites.Explosion, 
                  MissionIISounds.Explosion,
                  startExplosion)
        {
            base.SetIntelligenceProvider(
                Attractor.NewAttractor(this, moveAdversaryOnePixel, getManExtents));
        }
    }
}
