﻿
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
        public WanderingDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action<GameObject> manWalksIntoDroidAction, Action<Rectangle, MovementDeltas, bool> fireBullet, Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel, Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
            : base(
                  MissionIISprites.Monster2, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  manWalksIntoDroidAction,
                  startExplosion)
        {
            base.SetIntelligenceProvider(
                SingleMinded.New(this, freeDirectionFinder, fireBullet, moveAdversaryOnePixel));
        }

        public override int KillScore
        {
            get { return Constants.WanderingDroidKillScore; }
        }
    }
}
