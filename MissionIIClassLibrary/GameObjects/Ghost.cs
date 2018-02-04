
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.GameObjects
{
    public class Ghost : BaseGameObject
    {
        private int _stunCountDown = 0;
        private int _startCountDown = Constants.GhostStartCycles;
        private SpriteInstance _spriteInstance;
        private ArtificialIntelligence.AbstractIntelligenceProvider _intelligenceProvider;

        public Ghost()
        {
            _intelligenceProvider = new ArtificialIntelligence.Swoop();
            _spriteInstance = new SpriteInstance();
        }

        private bool IsActive
        {
            get { return _startCountDown == 0; }
        }

        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, MissionIIKeyStates theKeyStates)
        {
            if (_startCountDown > 0)
            {
                --_startCountDown;
                if (_startCountDown == 0)
                {
                    _spriteInstance.RoomX = 0;
                    _spriteInstance.RoomY = 0;
                    _spriteInstance.Traits = MissionIISpriteTraits.Ghost;
                    MissionIISounds.Play(MissionIISounds.GhostAppearing);
                }
            }
            else if (_stunCountDown > 0)
            {
                --_stunCountDown;
                if (_stunCountDown == 0)
                {
                    _spriteInstance.Traits = MissionIISpriteTraits.Ghost;
                }
            }
            else
            {
                _intelligenceProvider.AdvanceOneCycle(theGameBoard, _spriteInstance);
            }
        }

        public override void ManWalkedIntoYou(MissionIIGameBoard theGameBoard)
        {
            // The ghost kills the man via its AI.  No action needed here.
        }

        public override void Draw(MissionIIGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            if (IsActive)
            {
                drawingTarget.DrawFirstSprite(_spriteInstance);
            }
        }

        public override bool YouHaveBeenShot(MissionIIGameBoard gameBoard, bool shotByMan)
        {
            if (shotByMan)
            {
                _stunCountDown = Constants.GhostStunnedCycles;
                _spriteInstance.Traits = MissionIISpriteTraits.GhostStunned;
                MissionIISounds.Play(MissionIISounds.StunGhost);
            }
            return true;
        }
    
        public override Rectangle GetBoundingRectangle()
        {
            if (IsActive)
            {
                return _spriteInstance.GetBoundingRectangle();
            }
            else return new Rectangle(-1,-1,0,0); // empty if ghost not started
        }

        public override Point TopLeftPosition
        {
            get { return _spriteInstance.TopLeftPosition; }
            set { _spriteInstance.TopLeftPosition = value; }
        }

        public override bool CanBeOverlapped { get { return false; } }
    }
}
