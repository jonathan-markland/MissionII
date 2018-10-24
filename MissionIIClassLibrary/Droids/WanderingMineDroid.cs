﻿
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
        public WanderingMineDroid(Func<Rectangle, FoundDirections> freeDirectionFinder, Action manDestroyAction, Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel, Func<Rectangle> getManExtents, Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
            : base(
                  MissionIISprites.Monster5, 
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  manDestroyAction,
                  startExplosion)
        {
            base.SetIntelligenceProvider(
                new WanderingMine(this, freeDirectionFinder, manDestroyAction,
                      GameClassLibrary.ArtificialIntelligence.Constants.WanderingMineSpeedDivisor, 
                      moveAdversaryOnePixel, getManExtents));
        }

        public override int KillScore
        {
            get { return Constants.WanderingMineDroidKillScore; }
        }
    }
}
