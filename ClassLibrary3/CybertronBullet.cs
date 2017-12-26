
namespace GameClassLibrary
{
    public class CybertronBullet : CybertronGameObject
    {
        public SpriteInstance Sprite;
        public MovementDeltas BulletDirection;
        public bool _increasesScore;

        public CybertronBullet(SpriteInstance theSprite, MovementDeltas bulletDirection, bool increasesScore)
        {
            Sprite = theSprite;
            BulletDirection = bulletDirection;
            _increasesScore = increasesScore;
        }

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates theKeyStates)
        {
            // Not handled here.  Handled at global level.
        }

        public override void DoManIntersectionAction(CybertronGameBoard theGameBoard)
        {
            // Not handled here.  Handled at global level.  // TODO: could this be handled here?
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            CybertronScreenPainter.DrawIndexedSprite(Sprite, 0, drawingTarget);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return Sprite.GetBoundingRectangle();
        }

        public bool IncreasesScore
        {
            get { return _increasesScore; }
        }
    }
}
