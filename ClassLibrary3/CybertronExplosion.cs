namespace GameClassLibrary
{
    public class CybertronExplosion : CybertronGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance();
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private const int ExplosionCountDownReset = 30; // TODO: Put constant elsewhere because we don't know the units
        private int _explosionCountDown = ExplosionCountDownReset;

        public CybertronExplosion(int roomX, int roomY, SpriteTraits explosionKind)
        {
            SpriteInstance.RoomX = roomX;
            SpriteInstance.RoomY = roomY;
            SpriteInstance.Traits = explosionKind;
        }

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates theKeyStates)
        {
            if (_explosionCountDown != 0)
            {
                Business.Animate(ref _animationCountdown, ref _imageIndex, AnimationReset, SpriteInstance.Traits.HostImageObjects.Count);
                --_explosionCountDown;
                if (_explosionCountDown == 0)
                {
                    theGameBoard.ExplosionsToRemove.Add(this);  // It gets removed by the framework when we add it to this list.
                }
            }
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            CybertronScreenPainter.DrawIndexedSprite(SpriteInstance, _imageIndex, drawingTarget);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.GetBoundingRectangle();
        }

        public override void DoManIntersectionAction(CybertronGameBoard theGameBoard)
        {
            // No action required.
        }
    }
}