﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public class CybertronGhost : CybertronGameObject
    {
        private int _stunCountDown = 0;
        private int _startCountDown = Constants.GhostStartCycles;
        private SpriteInstance _spriteInstance;
        private ArtificialIntelligence.AbstractIntelligenceProvider _intelligenceProvider;

        public CybertronGhost()
        {
            _intelligenceProvider = new ArtificialIntelligence.Swoop();
            _spriteInstance = new SpriteInstance();
        }

        private bool IsActive
        {
            get { return _startCountDown == 0; }
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
                _intelligenceProvider.AdvanceOneCycle(theGameBoard, _spriteInstance);
            }
        }

        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            // The ghost kills the man via its AI.  No action needed here.
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            if (IsActive)
            {
                CybertronScreenPainter.DrawFirstSprite(_spriteInstance, drawingTarget);
            }
        }

        public override bool YouHaveBeenShot(CybertronGameBoard gameBoard)
        {
            if (_stunCountDown == 0) // multiply-stunning has no effect
            {
                _stunCountDown = Constants.GhostStunnedCycles;
                _spriteInstance.Traits = CybertronSpriteTraits.GhostStunned;
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
    }
}
