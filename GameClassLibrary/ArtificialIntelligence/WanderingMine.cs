
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class WanderingMine : AbstractIntelligenceProvider
    {
        private readonly Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> _moveAdversaryOnePixel;
        private readonly Func<Rectangle> _getManExtents;
        private readonly Func<Rectangle, FoundDirections> _freeDirectionFinder;
        private readonly Action<GameObject> _manWalksIntoDroidAction;
        private readonly int _speedDivisor;
        private readonly GameObject _gameObject;


        private int _countDown = 0;
        private int _facingDirection = 0;
        private MovementDeltas _movementDeltas = MovementDeltas.Stationary;




        public WanderingMine(
            GameObject gameObject,
            Func<Rectangle, FoundDirections> freeDirectionFinder, Action<GameObject> manWalksIntoDroidAction, int speedDivisor,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            _speedDivisor = speedDivisor;
            _manWalksIntoDroidAction = manWalksIntoDroidAction;
            _freeDirectionFinder = freeDirectionFinder;
            _moveAdversaryOnePixel = moveAdversaryOnePixel;
            _getManExtents = getManExtents;
            _gameObject = gameObject;
        }



        public override void AdvanceOneCycle()
        {
            if (Time.CycleCounter.Count32 % _speedDivisor == 0)
            {
                if (_countDown > 0)
                {
                    --_countDown;
                    DoMovement();
                }
                else
                {
                    ChooseNewMovement(_gameObject.GetBoundingRectangle());
                }
            }
        }



        private void DoMovement()
        {
            if (!_movementDeltas.IsStationary)
            {
                var hitResult = _moveAdversaryOnePixel(
                    _gameObject, _movementDeltas);

                // Check proximity to man, and detonate killing man:

                var detonationRectangle = _getManExtents().Inflate(5); // TODO: constant
                if (_gameObject.GetBoundingRectangle().Intersects(detonationRectangle))
                {
                    _manWalksIntoDroidAction(_gameObject);
                    // TODO: Droid (the gameObject) should detonate 
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
                _countDown = theRng.Next(Constants.WanderingMineMoveDurationCycles) + Constants.WanderingMineMoveDurationCycles;
                _facingDirection = freeDirections.Choose(theRng.Next(freeDirections.Count));
                _movementDeltas = MovementDeltas.ConvertFromFacingDirection(_facingDirection);
            }
        }
    }
}
