using GameClassLibrary.Math;
using GameClassLibrary.Input;
using GameClassLibrary.Graphics;
using GameClassLibrary.Sound;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.GameObjects
{
    public class Explosion : GameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance();
        private SoundTraits _explosionSound;
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private const int ExplosionCountDownReset = 30; // TODO: Put constant elsewhere because we don't know the units
        private int _explosionCountDown = ExplosionCountDownReset;



        public Explosion(int x, int y, SpriteTraits explosionKind, SoundTraits explosionSound)
        {
            SpriteInstance.X = x;
            SpriteInstance.Y = y;
            SpriteInstance.Traits = explosionKind;
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
                    ref _animationCountdown, ref _imageIndex, AnimationReset, SpriteInstance.Traits.ImageCount);

                --_explosionCountDown;

                if (_explosionCountDown == 0)
                {
                    theGameBoard.Remove(this);  // It gets removed by the framework when we add it to this list.
                }
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(SpriteInstance, _imageIndex);
        }



        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.Extents;
        }



        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            // No action required.
        }



        public override ShotStruct YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan)
        {
            // no action required
            return new ShotStruct { Affirmed = false }; // ignore this.
        }



        public override Point TopLeftPosition
        {
            get { return SpriteInstance.TopLeftPosition; }
            set { SpriteInstance.TopLeftPosition = value; }
        }



        public override bool CanBeOverlapped
        {
            get { return false; }
        }
    }
}