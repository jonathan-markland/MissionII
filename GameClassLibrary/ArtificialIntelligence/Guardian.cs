﻿
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Guardian: AbstractIntelligenceProvider
    {
		private readonly Action _manDestroyAction;

		private int _facingDirection = 0;
        private MovementDeltas _movementDeltas = MovementDeltas.Stationary;



        public Guardian(Action manDestroyAction)
        {
            _manDestroyAction = manDestroyAction;
        }


        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            if (_movementDeltas.IsStationary)
            {
                _facingDirection = GameClassLibrary.Math.Rng.Generator.Next(8);
                _movementDeltas = MovementDeltas.ConvertFromFacingDirection(_facingDirection);
            }
            else
            {
                var hitResult = theGameBoard.MoveAdversaryOnePixel(gameObject, _movementDeltas);  // TODO: differentiate walls/other droids
                if (hitResult != CollisionDetection.WallHitTestResult.NothingHit)
                {
                    if (gameObject.GetBoundingRectangle().Intersects(
                        theGameBoard.GetMan().GetBoundingRectangle().Inflate(5)))
                    {
                        _manDestroyAction();
                    }
                    else
                    {
                        _facingDirection = (_facingDirection + 4) & 7;  // TODO: reverse direction function
                        _movementDeltas = MovementDeltas.ConvertFromFacingDirection(_facingDirection);
                    }
                }
            }
        }
    }
}
