﻿
namespace GameClassLibrary
{
    public class CybertronBullet : CybertronGameObject
    {
        public SpriteInstance Sprite;
        public MovementDeltas BulletDirection;

        public CybertronBullet(SpriteInstance theSprite, MovementDeltas bulletDirection)
        {
            Sprite = theSprite;
            BulletDirection = bulletDirection;
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
    }
}
