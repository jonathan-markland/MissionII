﻿
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class SingleMinded : AbstractIntelligenceProvider
    {
		private readonly Func<Rectangle, FoundDirections> _freeDirectionFinder;
        private readonly Action<Rectangle, MovementDeltas, bool> _fireBullet;

		private int _countDown = 0;
        private int _facingDirection = 0;
        private MovementDeltas _movementDeltas = MovementDeltas.Stationary;



        public SingleMinded(
            Func<Rectangle, FoundDirections> freeDirectionFinder,
            Action<Rectangle, MovementDeltas, bool> fireBullet)
        {
            _fireBullet = fireBullet;
            _freeDirectionFinder = freeDirectionFinder;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            if (Time.CycleCounter.Count32 % Constants.SingleMindedSpeedDivisor == 0)
            {
                if (_countDown > 0)
                {
                    --_countDown;
                    DoMovement(theGameBoard, gameObject);
                }
                else
                {
                    ChooseNewMovement(gameObject.GetBoundingRectangle());
                }
            }
        }



        private void DoMovement(IGameBoard theGameBoard, GameObject gameObject)
        {
            if (!_movementDeltas.IsStationary)
            {
                var hitResult = theGameBoard.MoveAdversaryOnePixel(
                    gameObject, _movementDeltas);

                if ((Time.CycleCounter.Count32 & Constants.SingleMindedFiringCyclesAndMask) == 0)
                {
                    if (!_movementDeltas.IsStationary
                        && Rng.Generator.Next(100) < Constants.SingleMindedFiringProbabilityPercent)
                    {
                        _fireBullet(
                            gameObject.GetBoundingRectangle(), 
                            MovementDeltas.ConvertFromFacingDirection(_facingDirection), false);
                    }
                }

                if (hitResult != CollisionDetection.WallHitTestResult.NothingHit)
                {
                    _countDown = 0;
                }
            }
        }



        private void ChooseNewMovement(Rectangle currentExtents)
        {
            var theRng = Rng.Generator;
            var freeDirections = _freeDirectionFinder(currentExtents);
            if (freeDirections.Count == 0)
            {
                // Can't move.
                _countDown = 0;
                _movementDeltas = MovementDeltas.Stationary;
            }
            else
            {
                _countDown = theRng.Next(Constants.SingleMindedMoveDurationCycles) + Constants.SingleMindedMoveDurationCycles;
                _facingDirection = freeDirections.Choose(theRng.Next(freeDirections.Count));
                _movementDeltas = MovementDeltas.ConvertFromFacingDirection(_facingDirection);
            }
        }



    }
}
