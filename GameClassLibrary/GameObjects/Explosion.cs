
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Input;
using GameClassLibrary.Graphics;
using GameClassLibrary.Sound;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.GameObjects
{
    public class Explosion : GameObject
    {
		private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private const int ExplosionCountDownReset = 30; // TODO: Put constant elsewhere because we don't know the units

		private readonly SoundTraits _explosionSound;
        private readonly Action<GameObject> _removeObject;

		private SpriteInstance _spriteInstance = new SpriteInstance();
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
		private int _explosionCountDown = ExplosionCountDownReset;



        public Explosion(int x, int y, SpriteTraits explosionKind, SoundTraits explosionSound, Action<GameObject> removeObject)
        {
            _spriteInstance.X = x;
            _spriteInstance.Y = y;
            _spriteInstance.Traits = explosionKind;
            _explosionSound = explosionSound;
            _removeObject = removeObject;
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_explosionCountDown == ExplosionCountDownReset)
            {
                _explosionSound.Play();
            }

            if (_explosionCountDown != 0)
            {
                GameClassLibrary.Algorithms.Animation.Animate(
                    ref _animationCountdown, ref _imageIndex, AnimationReset, _spriteInstance.Traits.ImageCount);

                --_explosionCountDown;

                if (_explosionCountDown == 0)
                {
                    _removeObject(this);  // It gets removed by the framework when we add it to this list.
                }
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(_spriteInstance, _imageIndex);
        }



        public override Rectangle GetBoundingRectangle()
        {
            return _spriteInstance.Extents;
        }



        public override ShotStruct YouHaveBeenShot(bool shotByMan)
        {
            // no action required
			return new ShotStruct(affirmed:false); // ignore this.
        }



        public override Point TopLeftPosition
        {
            get { return _spriteInstance.TopLeftPosition; }
            set { _spriteInstance.TopLeftPosition = value; }
        }
    }
}