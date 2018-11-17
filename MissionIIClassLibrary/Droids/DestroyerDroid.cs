
using System;
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Sound;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Droids
{
    public class DestroyerDroid : BaseDroid
    {
        public DestroyerDroid(
            Action<Rectangle, MovementDeltas, bool> fireBullet, 
            Action<GameObject, MovementDeltas> moveAdversaryOnePixel, 
            Func<Rectangle> getManExtents)
            : base(MissionIISprites.Monster3)
        {
            base.SetIntelligenceProvider(
                FiringAttractor.NewFiringAttractor(this, fireBullet, moveAdversaryOnePixel, getManExtents));
        }
    }
}
