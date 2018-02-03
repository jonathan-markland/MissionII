using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameClassLibrary.Math;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class SingleMinded : AbstractIntelligenceProvider
    {
        private int _countDown = 0;
        private int _facingDirection = 0;
        private MovementDeltas _movementDeltas = new MovementDeltas(0, 0);
        private bool _operationEnable = false;

        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            _operationEnable = !_operationEnable;  // ie: operate only ever other cycle
            if (_operationEnable)
            {
                if (_countDown > 0)
                {
                    --_countDown;
                    DoMovement(theGameBoard, spriteInstance);
                }
                else
                {
                    ChooseNewMovement();
                }
            }
        }



        private void DoMovement(MissionIIGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            if (!_movementDeltas.Stationary)
            {
                var hitResult = theGameBoard.MoveAdversaryOnePixel(
                    spriteInstance,
                    _movementDeltas);

                if ((_countDown & 31) == 0) // TODO: firing time constant
                {
                    if (!_movementDeltas.Stationary
                        && Rng.Generator.Next(100) < 20)
                    {
                        theGameBoard.StartBullet(spriteInstance, _facingDirection, false);
                    }
                }

                if (hitResult == CollisionDetection.WallHitTestResult.HitWall)
                {
                    _countDown = 0;
                }
            }
        }



        private void ChooseNewMovement()
        {
            var theRng = Rng.Generator;
            _countDown = theRng.Next(50) + 50; // TODO: single-minded movement constants
            _facingDirection = theRng.Next(8);
            _movementDeltas = theRng.Next(8) < 1
                ? new MovementDeltas(0, 0)
                : Business.GetMovementDeltas(_facingDirection);
        }

    }
}
