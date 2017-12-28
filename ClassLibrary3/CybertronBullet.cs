
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
            for (int i = 0; i < Constants.BulletCycles; i++) // TODO: Do inside AdvanceOneCycle()
            {
                var proposedX = Sprite.RoomX + BulletDirection.dx;
                var proposedY = Sprite.RoomY + BulletDirection.dy;

                var hitResult = CollisionDetection.HitsWalls(
                    theGameBoard.CurrentRoomWallData,
                    CybertronGameBoardConstants.TileWidth,
                    CybertronGameBoardConstants.TileHeight,
                    proposedX,
                    proposedY,
                    Sprite.Traits.BoardWidth,
                    Sprite.Traits.BoardHeight);

                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
                {
                    Sprite.RoomX = proposedX;
                    Sprite.RoomY = proposedY;

                    if (CybertronGameStateUpdater.KillThingsIfShot(theGameBoard, this))
                    {
                        theGameBoard.BulletsToRemove.Add(this);
                        return;
                    }
                }
                else // Bullet hit wall or went outside room.
                {
                    theGameBoard.BulletsToRemove.Add(this);
                    return;
                }
            }
        }

        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            // Not handled here.  Bullets killing man happens in AdvanceOneCycle().
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            CybertronScreenPainter.DrawIndexedSprite(Sprite, 0, drawingTarget);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return Sprite.GetBoundingRectangle();
        }

        public override void YouHaveBeenShot(CybertronGameBoard theGameBoard)
        {
            // No action -- bullets cannot be shot.
        }

        public bool IncreasesScore
        {
            get { return _increasesScore; }
        }
    }
}
