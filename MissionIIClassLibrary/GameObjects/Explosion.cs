using MissionIIClassLibrary.Math;

namespace MissionIIClassLibrary.GameObjects
{
    public class Explosion : BaseGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance();
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private const int ExplosionCountDownReset = 30; // TODO: Put constant elsewhere because we don't know the units
        private int _explosionCountDown = ExplosionCountDownReset;
        private bool _canBeConsideredForMultiKillBonus;

        public Explosion(int roomX, int roomY, SpriteTraits explosionKind, bool canBeConsideredForBonus)
        {
            SpriteInstance.RoomX = roomX;
            SpriteInstance.RoomY = roomY;
            SpriteInstance.Traits = explosionKind;
            _canBeConsideredForMultiKillBonus = canBeConsideredForBonus;
        }

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates theKeyStates)
        {
            if (_explosionCountDown == ExplosionCountDownReset)
            {
                CybertronSounds.Play(CybertronSounds.ExplosionSound);
            }

            if (_explosionCountDown != 0)
            {
                Business.Animate(ref _animationCountdown, ref _imageIndex, AnimationReset, SpriteInstance.Traits.ImageCount);
                --_explosionCountDown;
                if (_explosionCountDown == 0)
                {
                    theGameBoard.ObjectsToRemove.Add(this);  // It gets removed by the framework when we add it to this list.
                }
            }
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(SpriteInstance, _imageIndex);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.GetBoundingRectangle();
        }

        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            // No action required.
        }

        public override bool YouHaveBeenShot(CybertronGameBoard theGameBoard, bool shotByMan)
        {
            // no action required
            return false; // ignore this.
        }

        public override Point TopLeftPosition
        {
            get { return SpriteInstance.TopLeftPosition; }
            set { SpriteInstance.TopLeftPosition = value; }
        }

        public override bool CanBeOverlapped { get { return false; } }

        public void MarkAsUsedForBonus()
        {
            _canBeConsideredForMultiKillBonus = false;
        }

        public bool CanBeConsideredForMultiKillBonus
        {
            get { return _canBeConsideredForMultiKillBonus; }
        }
    }
}