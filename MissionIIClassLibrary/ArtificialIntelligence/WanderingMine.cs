
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class WanderingMine : AbstractIntelligenceProvider
    {
        private int _countDown = 0;
        private int _cycleCounter = 0;
        private int _facingDirection = 0;
        private MovementDeltas _movementDeltas = new MovementDeltas(0, 0);
        private Func<Rectangle, FoundDirections> _freeDirectionFinder;
        private Action _manDestroyAction;



        public WanderingMine(Func<Rectangle, FoundDirections> freeDirectionFinder, Action manDestroyAction)
        {
            _manDestroyAction = manDestroyAction;
            _freeDirectionFinder = freeDirectionFinder;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            ++_cycleCounter;
            if ((_cycleCounter % Constants.WanderingMineSpeedDivisor) == 0)
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
            if (!_movementDeltas.Stationary)
            {
                var hitResult = theGameBoard.MoveAdversaryOnePixel(
                    gameObject, _movementDeltas);

                // Check proximity to man, and detonate killing man:

                var detonationRectangle = theGameBoard.GetMan().GetBoundingRectangle().Inflate(5); // TODO: constant
                if (gameObject.GetBoundingRectangle().Intersects(detonationRectangle))
                {
                    _manDestroyAction();
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
                _movementDeltas = new MovementDeltas(0, 0);
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
