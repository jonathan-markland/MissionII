
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class WanderingMine : AbstractIntelligenceProvider
    {
        private int _countDown = 0;
        private int _cycleCounter = 0;
        private int _facingDirection = 0;
        private MovementDeltas _movementDeltas = new MovementDeltas(0, 0);



        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            ++_cycleCounter;
            if ((_cycleCounter % Constants.WanderingMineSpeedDivisor) == 0)
            {
                if (_countDown > 0)
                {
                    --_countDown;
                    DoMovement(theGameBoard, spriteInstance);
                }
                else
                {
                    ChooseNewMovement(theGameBoard, spriteInstance.Extents);
                }
            }
        }



        private void DoMovement(MissionIIGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            if (!_movementDeltas.Stationary)
            {
                var hitResult = theGameBoard.MoveAdversaryOnePixel(
                    spriteInstance, _movementDeltas);

                // TODO: Check proximity to man, and detonate killing man.

                var detonationRectangle = theGameBoard.Man.GetBoundingRectangle().Inflate(5); // TODO: constant
                if (spriteInstance.Intersects(detonationRectangle))
                {
                    theGameBoard.Man.Electrocute(ElectrocutionMethod.ByDroid);
                    // TODO: Droid should detonate, but AI classes have no reference to the droid object!  Fix that.
                }

                if (hitResult != CollisionDetection.WallHitTestResult.NothingHit)
                {
                    _countDown = 0;
                }
            }
        }



        private void ChooseNewMovement(MissionIIGameBoard theGameBoard, Rectangle currentExtents)
        {
            // TODO: Libraryize this function:  See also SingleMinded.
            // TODO: single-minded movement constants
            var theRng = Rng.Generator;
            var freeDirections = theGameBoard.GetFreeDirections(currentExtents);
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
