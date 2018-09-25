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

		private SpriteInstance _spriteInstance = new SpriteInstance();
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
		private int _explosionCountDown = ExplosionCountDownReset;



        public Explosion(int x, int y, SpriteTraits explosionKind, SoundTraits explosionSound)
        {
            _spriteInstance.X = x;
            _spriteInstance.Y = y;
            _spriteInstance.Traits = explosionKind;
            _explosionSound = explosionSound;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates)
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
                    theGameBoard.Remove(this);  // It gets removed by the framework when we add it to this list.
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



        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            // No action required.
        }



        public override ShotStruct YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan)
        {
            // no action required
			return new ShotStruct(affirmed:false); // ignore this.
        }



        public override Point TopLeftPosition
        {
            get { return _spriteInstance.TopLeftPosition; }
            set { _spriteInstance.TopLeftPosition = value; }
        }



        public override bool CanBeOverlapped
        {
            get { return false; }
        }
    }
}