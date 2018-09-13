using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary.GameObjects
{
    public class Bullet : GameObject
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
                    theGameBoard.GetTileMatrix(),
                    proposedX,
                    proposedY,
                    Sprite.Traits.Width,
                    Sprite.Traits.Height,
                    TileExtensions.IsFloor);

                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
                {
                    Sprite.X = proposedX;
                    Sprite.Y = proposedY;

                    var bulletResult = theGameBoard.KillThingsIfShotAndGetHitCount(this);
                    if (bulletResult.HitCount > 0)
                    {
                        theGameBoard.Remove(this);
                        if (_increasesScore)
                        {
                            if (bulletResult.HitCount > 1)
                            {
                                theGameBoard.PlayerIncrementScore(Constants.MultiKillWithSingleBulletBonusScore);
                                MissionIISounds.DuoBonus.Play();
                            }
                            theGameBoard.PlayerIncrementScore(bulletResult.TotalScoreIncrease);
                        }
                        break;
                    }
                }
                else // Bullet hit wall or went outside room.
                {
                    theGameBoard.Remove(this);
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

        public override ShotStruct YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan)
        {
            // No action -- bullets cannot be shot.
            return new ShotStruct { Affirmed = false };
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
