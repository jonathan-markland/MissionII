
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary.GameObjects
{
    public class Ghost : GameObject
    {
        private int _stunCountDown = 0;
        private int _startCountDown = Constants.GhostStartCycles;
        private SpriteInstance _spriteInstance;
        private ArtificialIntelligence.AbstractIntelligenceProvider _intelligenceProvider;

        public Ghost(Action manDestroyAction)
        {
            _intelligenceProvider = new ArtificialIntelligence.Swoop(manDestroyAction);
            _spriteInstance = new SpriteInstance();
        }

        private bool IsActive
        {
            get { return _startCountDown == 0; }
        }

        public override void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates)
        {
            if (_startCountDown > 0)
            {
                --_startCountDown;
                if (_startCountDown == 0)
                {
                    var furthestCorner = theGameBoard.GetCornerFurthestAwayFromMan();
                    _spriteInstance.X = furthestCorner.X;
                    _spriteInstance.Y = furthestCorner.Y;
                    _spriteInstance.Traits = MissionIISprites.Ghost;
                    MissionIISounds.GhostAppearing.Play();
                }
            }
            else if (_stunCountDown > 0)
            {
                --_stunCountDown;
                if (_stunCountDown == 0)
                {
                    _spriteInstance.Traits = MissionIISprites.Ghost;
                }
            }
            else
            {
                _intelligenceProvider.AdvanceOneCycle(theGameBoard, this);
            }
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            // The ghost kills the man via its AI.  No action needed here.
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            if (IsActive)
            {
                drawingTarget.DrawFirstSpriteRoomRelative(_spriteInstance);
            }
        }

        public override ShotStruct YouHaveBeenShot(IGameBoard gameBoard, bool shotByMan)
        {
            if (shotByMan)
            {
                _stunCountDown = Constants.GhostStunnedCycles;
                _spriteInstance.Traits = MissionIISprites.GhostStunned;
                MissionIISounds.StunGhost.Play();
            }
            return new ShotStruct { Affirmed = true, ScoreIncrease = 0 };
        }

        public override Rectangle GetBoundingRectangle()
        {
            if (IsActive)
            {
                return _spriteInstance.Extents;
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
