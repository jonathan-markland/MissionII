
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.GameBoard;
using GameClassLibrary.GameObjects;

namespace MissionIIClassLibrary.GameObjects
{
    public class ManDead : GameObject
    {
        private readonly Action _playerLoseLife;
        private int _whileDeadCount = 0;
        private SpriteInstance SpriteInstance = new SpriteInstance();

        public ManDead(Point topLeftPosition, Action playerLoseLife)
        {
            _playerLoseLife = playerLoseLife;
            _whileDeadCount = Constants.ManDeadDelayCycles;
            SpriteInstance.Traits = MissionIISprites.Dead;
            SpriteInstance.TopLeftPosition = topLeftPosition;
        }

        public override Point TopLeftPosition
        {
            get { return SpriteInstance.TopLeftPosition; }
            set { SpriteInstance.TopLeftPosition = value; }
        }

        public override bool CanBeOverlapped => true;

        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_whileDeadCount > 0)
            {
                if (_whileDeadCount == Constants.ManDeadDelayCycles)
                {
                    MissionIISounds.ManGrunt.Play();
                }
                --_whileDeadCount;
            }
            else
            {
                _playerLoseLife();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(SpriteInstance, 0);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.Extents;
        }

        public override ShotStruct YouHaveBeenShot(bool shotByMan)
        {
            return new ShotStruct(affirmed: false);
        }
    }
}
