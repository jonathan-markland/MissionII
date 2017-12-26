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
        private MovementDeltas _movementDeltas = new MovementDeltas(0,0);
        private bool _firing = false;

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            if (_countDown > 0)
            {
                --_countDown;

                if (!_movementDeltas.Stationary)
                {
                    CybertronGameStateUpdater.MoveAdversaryOnePixel(
                        theGameBoard,
                        spriteInstance,
                        _movementDeltas);

                    if (_firing && (_countDown & 31) == 0 )
                    {
                        CybertronGameStateUpdater.StartBullet(spriteInstance, _facingDirection, theGameBoard);
                    }
                }
            }
            else
            {
                var theRng = CybertronGameStateUpdater.RandomGenerator;
                _countDown = theRng.Next(50) + 50;
                _facingDirection = theRng.Next(8);
                _movementDeltas = theRng.Next(8) < 1
                    ? new MovementDeltas(0,0)
                    : Business.GetMovementDeltas(_facingDirection);
                _firing = theRng.Next(100) < 20; // TODO: tuneable percentage for agression?
            }
        }
    }
}
