
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
        public WanderingDroid(
            Func<Rectangle, FoundDirections> freeDirectionFinder, 
            Action<Rectangle, MovementDeltas, bool> fireBullet, 
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> tryMoveAdversaryOnePixel)
            : base(MissionIISprites.Monster2)
        {
            base.SetIntelligenceProvider(
                SingleMinded.NewSingleMinded(this, freeDirectionFinder, fireBullet, tryMoveAdversaryOnePixel));
        }
    }
}
