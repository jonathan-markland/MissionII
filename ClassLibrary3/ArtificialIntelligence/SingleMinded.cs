using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class SingleMinded : AbstractIntelligenceProvider
    {
        private int _countDown = 0;
        private int _facingDirection = 0;
        private MovementDeltas _movementDeltas = new MovementDeltas(0, 0);
        private bool _firing = false;
        private bool _operationEnable = false;

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            _operationEnable = !_operationEnable;  // ie: operate only ever other cycle
            if (_operationEnable)
            {
                if (_countDown > 0)
                {
                    --_countDown;

                    if (!_movementDeltas.Stationary)
                    {
                        var hitResult = CybertronGameStateUpdater.MoveAdversaryOnePixel(
                            theGameBoard,
                            spriteInstance,
                            _movementDeltas);

                        if (_firing && (_countDown & 31) == 0) // TODO: firing time constant
                        {
                            CybertronGameStateUpdater.StartBullet(spriteInstance, _facingDirection, theGameBoard, false);
                        }

                        if (hitResult == CollisionDetection.WallHitTestResult.HitWall)
                        {
                            _countDown = 0;
                        }
                    }
                }
                else
                {
                    var theRng = CybertronGameStateUpdater.RandomGenerator;
                    _countDown = theRng.Next(50) + 50; // TODO: single-minded movement constants
                    _facingDirection = theRng.Next(8);
                    _movementDeltas = theRng.Next(8) < 1
                        ? new MovementDeltas(0, 0)
                        : Business.GetMovementDeltas(_facingDirection);
                    _firing = !_movementDeltas.Stationary && theRng.Next(100) < 20; // TODO: tuneable percentage for agression?
                }
            }
        }
    }
}
