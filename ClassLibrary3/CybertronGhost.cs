using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public class CybertronGhost : CybertronGameObject
    {
        private int _startCountDown = 0;
        private int _stunCountDown = 0;
        private bool _killedMan = false;
        private SpriteInstance _spriteInstance;

        public CybertronGhost()
        {
            _spriteInstance = new SpriteInstance();
        }

        public void NotifyNewRoom()
        {
            _startCountDown = Constants.GhostStartCycles;
            _stunCountDown = 0;
            _killedMan = false;
        }

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates theKeyStates)
        {
            if (_startCountDown > 0)
            {
                --_startCountDown;
                if (_startCountDown == 0)
                {
                    _spriteInstance.RoomX = 0;
                    _spriteInstance.RoomY = 0;
                    _spriteInstance.Traits = CybertronSpriteTraits.Ghost;
                }
            }
            else if (_stunCountDown > 0)
            {
                --_stunCountDown;
                if (_stunCountDown == 0)
                {
                    _spriteInstance.Traits = CybertronSpriteTraits.Ghost;
                }
            }
            else
            {
                var moveDeltas = CybertronGameStateUpdater.GetMovementDeltasToHeadTowards(
                    _spriteInstance,
                    theGameBoard.Man.SpriteInstance);

                CybertronGameStateUpdater.MoveAdversaryOnePixelUnchecked(
                    _spriteInstance,
                    moveDeltas);

                if (_spriteInstance.Intersects(theGameBoard.Man.SpriteInstance))
                {
                    KillMan(theGameBoard);
                }
            }
        }

        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            KillMan(theGameBoard); // This is not as important as the other call site.
        }

        private void KillMan(CybertronGameBoard theGameBoard)
        {
            if (_stunCountDown == 0 && !_killedMan)
            {
                theGameBoard.Man.Die();
                _killedMan = true; // only make the call once per room.
            }
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            if (_startCountDown == 0)
            {
                CybertronScreenPainter.DrawFirstSprite(_spriteInstance, drawingTarget);
            }
        }

        public void Stunned()
        {
            if (_stunCountDown == 0) // multiply-stunning has no effect
            {
                _stunCountDown = Constants.GhostStunnedCycles;
                _spriteInstance.Traits = CybertronSpriteTraits.GhostStunned;
            }
        }

        public override Rectangle GetBoundingRectangle()
        {
            if (_startCountDown == 0)
            {
                return _spriteInstance.GetBoundingRectangle();
            }
            else return new Rectangle(); // empty if ghost not started
        }
    }
}
