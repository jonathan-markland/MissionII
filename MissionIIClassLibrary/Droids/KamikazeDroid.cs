
using System;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Sound;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Droids
{
    public class KamikazeDroid : BaseDroid
    {
        public KamikazeDroid(
            Action manDestroyAction,
            Action<GameObject> manWalksIntoDroidAction,
            Action<GameObject, MovementDeltas> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents,
            Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
            : base(
                  MissionIISprites.Monster4,
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  manWalksIntoDroidAction,
                  startExplosion)
        {
            base.SetIntelligenceProvider(
                Kamikaze.NewKamikaze(this, manDestroyAction, moveAdversaryOnePixel, getManExtents));
        }
    }
}
