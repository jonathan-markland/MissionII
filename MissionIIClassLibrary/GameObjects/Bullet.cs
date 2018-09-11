using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

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

        public override void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates)
        {
            for (int i = 0; i < Constants.BulletCycles; i++)
            {
                var proposedX = Sprite.X + BulletDirection.dx;
                var proposedY = Sprite.Y + BulletDirection.dy;

                var hitResult = CollisionDetection.HitsWalls(
                    theGameBoard.CurrentRoomTileMatrix,
                    Constants.TileWidth,
                    Constants.TileHeight,
                    proposedX,
                    proposedY,
                    Sprite.Traits.Width,
                    Sprite.Traits.Height,
                    TileExtensions.IsFloor);

                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
                {
                    Sprite.X = proposedX;
                    Sprite.Y = proposedY;

                    var hitCount = theGameBoard.KillThingsIfShotAndGetHitCount(this);
                    if (hitCount > 0)
                    {
                        theGameBoard.ObjectsToRemove.Add(this);
                        if (_increasesScore && hitCount > 1)
                        {
                            theGameBoard.IncrementScore(Constants.MultiKillWithSingleBulletBonusScore);
                            MissionIISounds.DuoBonus.Play();
                        }
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

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            // Not handled here.  Bullets killing man happens in AdvanceOneCycle().
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSpriteRoomRelative(Sprite, 0);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return Sprite.Extents;
        }

        public override bool YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan)
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
