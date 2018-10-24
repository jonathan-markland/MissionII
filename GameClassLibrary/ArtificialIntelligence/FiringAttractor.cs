﻿
using System;
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Walls;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class FiringAttractor : AbstractIntelligenceProvider
    {
        private readonly Action<Rectangle, MovementDeltas, bool> _fireBullet;
        private readonly Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> _moveAdversaryOnePixel;
        private readonly Func<Rectangle> _getManExtents;
        private readonly GameObject _gameObject;



        public FiringAttractor(
            GameObject gameObject,
            Action<Rectangle, MovementDeltas, bool> fireBullet,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            _fireBullet = fireBullet;
            _moveAdversaryOnePixel = moveAdversaryOnePixel;
            _getManExtents = getManExtents;
            _gameObject = gameObject;
        }



        public override void AdvanceOneCycle()
        {
            var cycleCount = Time.CycleCounter.Count32;

            if (cycleCount % Constants.FiringAttractorSpeedDivisor == 0)
            {
                var moveDeltas = _gameObject.GetBoundingRectangle().GetMovementDeltasToHeadTowards(
                    _getManExtents());

                // We must separate horizontal and vertical movement in order to avoid
                // things getting 'stuck' on walls because they can't move horizontally
                // into the wall, but can moe vertically downward.  Trying to do both
                // directions at once results in rejection of the move, and the
                // sticking problem.

                _moveAdversaryOnePixel(_gameObject, moveDeltas.XComponent);
                _moveAdversaryOnePixel(_gameObject, moveDeltas.YComponent);

                if ((cycleCount & Constants.FiringAttractorFiringCyclesAndMask) == 0)
                {
                    if (!moveDeltas.IsStationary
                        && Rng.Generator.Next(100) < Constants.AttractorFiringProbabilityPercent)
                    {
                        _fireBullet(_gameObject.GetBoundingRectangle(), moveDeltas, false);
                    }
                }
            }
        }
    }
}
