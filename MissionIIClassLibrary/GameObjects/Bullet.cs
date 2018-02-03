using GameClassLibrary.Math;

namespace MissionIIClassLibrary.GameObjects
{
    public class Bullet : BaseGameObject
    {
        public SpriteInstance Sprite;
        public MovementDeltas BulletDirection;
        public bool _increasesScore;

        public Bullet(SpriteInstance theSprite, MovementDeltas bulletDirection, bool increasesScore)
        {
            Sprite = theSprite;
            BulletDirection = bulletDirection;
            _increasesScore = increasesScore;
        }

        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, MissionIIKeyStates theKeyStates)
        {
            for (int i = 0; i < Constants.BulletCycles; i++)
            {
                var proposedX = Sprite.RoomX + BulletDirection.dx;
                var proposedY = Sprite.RoomY + BulletDirection.dy;

                var hitResult = CollisionDetection.HitsWalls(
                    theGameBoard.CurrentRoomWallData,
                    MissionIIGameBoardConstants.TileWidth,
                    MissionIIGameBoardConstants.TileHeight,
                    proposedX,
                    proposedY,
                    Sprite.Traits.BoardWidth,
                    Sprite.Traits.BoardHeight);

                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
                {
                    Sprite.RoomX = proposedX;
                    Sprite.RoomY = proposedY;

                    if (theGameBoard.KillThingsIfShot(this))
                    {
                        theGameBoard.ObjectsToRemove.Add(this);
                        break;
                    }
                }
                else // Bullet hit wall or went outside room.
                {
                    theGameBoard.ObjectsToRemove.Add(this);
                    break;
                }
            }
        }

        public override void ManWalkedIntoYou(MissionIIGameBoard theGameBoard)
        {
            // Not handled here.  Bullets killing man happens in AdvanceOneCycle().
        }

        public override void Draw(MissionIIGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(Sprite, 0);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return Sprite.GetBoundingRectangle();
        }

        public override bool YouHaveBeenShot(MissionIIGameBoard theGameBoard, bool shotByMan)
        {
            // No action -- bullets cannot be shot.
            return false;
        }

        public bool IncreasesScore
        {
            get { return _increasesScore; }
        }

        public override Point TopLeftPosition
        {
            get { return Sprite.TopLeftPosition; }
            set { Sprite.TopLeftPosition = value; }
        }

        public override bool CanBeOverlapped { get { return false; } }
    }
}
