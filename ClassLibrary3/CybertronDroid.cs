
namespace GameClassLibrary
{
    public class CybertronDroid : CybertronGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance();
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units

        public CybertronDroid(int roomX, int roomY, SpriteTraits spriteTraits)
        {
            SpriteInstance.RoomX = roomX;
            SpriteInstance.RoomY = roomY;
            SpriteInstance.Traits = spriteTraits;
        }

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates theKeyStates)
        {
            Business.Animate(ref _animationCountdown, ref _imageIndex, AnimationReset, SpriteInstance.Traits.HostImageObjects.Count);
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            CybertronScreenPainter.DrawIndexedSprite(SpriteInstance, _imageIndex, drawingTarget);
        }

        public void CreateYourExplosion(CybertronGameBoard theGameBoard)
        {
            // TODO: FUTURE: We assume the explosion dimensions match the droid.
            theGameBoard.ExplosionsInRoom.Add(new CybertronExplosion(
                SpriteInstance.RoomX,
                SpriteInstance.RoomY,
                CybertronSpriteTraits.Explosion));
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.GetBoundingRectangle();
        }

        public override void DoManIntersectionAction(CybertronGameBoard theGameBoard)
        {
            theGameBoard.Man.Die();
        }
    }
}
