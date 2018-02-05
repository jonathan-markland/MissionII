using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class SingleMinded : AbstractIntelligenceProvider
    {
        private int _countDown = 0;
        private int _cycleCounter = 0;
        private int _facingDirection = 0;
        private MovementDeltas _movementDeltas = new MovementDeltas(0, 0);



        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            ++_cycleCounter;
            if ((_cycleCounter & 1) == 0)
            {
                if (_countDown > 0)
                {
                    --_countDown;
                    DoMovement(theGameBoard, spriteInstance);
                }
                else
                {
                    ChooseNewMovement(theGameBoard, spriteInstance.GetBoundingRectangle());
                }
            }
        }



        private void DoMovement(MissionIIGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            if (!_movementDeltas.Stationary)
            {
                var hitResult = theGameBoard.MoveAdversaryOnePixel( 
                    spriteInstance, _movementDeltas);

                if ((_cycleCounter & 31) == 0) // TODO: firing time constant
                {
                    if (!_movementDeltas.Stationary
                        && Rng.Generator.Next(100) < 20)
                    {
                        theGameBoard.StartBullet(spriteInstance, _facingDirection, false);
                    }
                }

                if (hitResult != CollisionDetection.WallHitTestResult.NothingHit)
                {
                    _countDown = 0;
                }
            }
        }



        private void ChooseNewMovement(MissionIIGameBoard theGameBoard, Rectangle currentExtents)
        {
            // TODO: single-minded movement constants
            var theRng = Rng.Generator;
            var freeDirections = theGameBoard.GetFreeDirections(currentExtents);
            if (freeDirections.Count == 0)
            {
                _countDown = 10; // Can't move.  Time to next decision.
                _movementDeltas = new MovementDeltas(0, 0);
            }
            else
            {
                _countDown = theRng.Next(50) + 50;
                _facingDirection = freeDirections.Choose(theRng.Next(freeDirections.Count));
                _movementDeltas = MovementDeltas.ConvertFromFacingDirection(_facingDirection);
            }
        }



    }
}
