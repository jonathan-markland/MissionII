using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary.GameObjects
{
    public class Explosion : MissionIIGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance();
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private const int ExplosionCountDownReset = 30; // TODO: Put constant elsewhere because we don't know the units
        private int _explosionCountDown = ExplosionCountDownReset;

        public Explosion(int roomX, int roomY, SpriteTraits explosionKind)
        {
            SpriteInstance.X = roomX;
            SpriteInstance.Y = roomY;
            SpriteInstance.Traits = explosionKind;
        }

        public override void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates)
        {
            if (_explosionCountDown == ExplosionCountDownReset)
            {
                MissionIISounds.Explosion.Play();
            }

            if (_explosionCountDown != 0)
            {
                Business.Animate(ref _animationCountdown, ref _imageIndex, AnimationReset, SpriteInstance.Traits.ImageCount);
                --_explosionCountDown;
                if (_explosionCountDown == 0)
                {
                    theGameBoard.Remove(this);  // It gets removed by the framework when we add it to this list.
                }
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSpriteRoomRelative(SpriteInstance, _imageIndex);
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

        public override bool CanBeOverlapped { get { return false; } }
    }
}