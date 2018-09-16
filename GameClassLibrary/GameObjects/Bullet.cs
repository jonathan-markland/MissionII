
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.GameObjects
{
    public class Bullet : GameObject
    {
        private BulletTraits _bulletTraits;
        private SpriteInstance _spriteInstance;
        private MovementDeltas BulletDirection;
        private bool _increasesScore;
        private bool _firingSoundDone;
        private Func<Tile, bool> _isFloor;
        private int _bonusScore;



        public Bullet(
            BulletTraits bulletTraits,
            Rectangle gameObjectextentsRectangle,
            MovementDeltas bulletDirection, 
            bool increasesScore, 
            int bonusScore,
            Func<Tile, bool> isFloor)
        {
            _bonusScore = bonusScore;

            var r = gameObjectextentsRectangle; // convenience
            var spriteTraits = bulletTraits.BulletSpriteTraits;
            var bulletWidth = spriteTraits.Width;
            var bulletHeight = spriteTraits.Height;

            int x, y;

            if (bulletDirection.dx < 0)
            {
                x = (r.Left - bulletWidth) - Constants.BulletSpacing;
            }
            else if (bulletDirection.dx > 0)
            {
                x = r.Left + r.Width + Constants.BulletSpacing;
            }
            else // (bulletDirection.dx == 0)
            {
                x = r.Left + ((r.Width - bulletWidth) / 2);
            }

            if (bulletDirection.dy < 0)
            {
                y = (r.Top - bulletHeight) - Constants.BulletSpacing;
            }
            else if (bulletDirection.dy > 0)
            {
                y = r.Top + r.Height + Constants.BulletSpacing;
            }
            else // (bulletDirection.dy == 0)
            {
                y = r.Top + ((r.Height - bulletHeight) / 2);
            }

            // if (bulletDirection.dx == 0 && bulletDirection.dy == 0)
            // {
            //     return;  // Cannot ascertain a direction away from the source sprite, so do nothing.
            // }


            _firingSoundDone = false;
            _bulletTraits = bulletTraits;
            _spriteInstance = new SpriteInstance { X=x, Y=y, Traits = bulletTraits.BulletSpriteTraits };
            BulletDirection = bulletDirection;
            _increasesScore = increasesScore;
            _isFloor = isFloor;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates)
        {
            if (!_firingSoundDone)
            {
                if (_increasesScore)
                {
                    _bulletTraits.ManFiring.Play();
                }
                else
                {
                    _bulletTraits.AdversaryFiring.Play();
                }
                _firingSoundDone = true;
            }

            for (int i = 0; i < Constants.BulletCycles; i++)
            {
                var proposedX = _spriteInstance.X + BulletDirection.dx;
                var proposedY = _spriteInstance.Y + BulletDirection.dy;

                var hitResult = CollisionDetection.HitsWalls(
                    theGameBoard.GetTileMatrix(),
                    proposedX,
                    proposedY,
                    _spriteInstance.Traits.Width,
                    _spriteInstance.Traits.Height,
                    _isFloor);

                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
                {
                    _spriteInstance.X = proposedX;
                    _spriteInstance.Y = proposedY;

                    var bulletResult = theGameBoard.KillThingsInRectangle(
                        GetBoundingRectangle(), IncreasesScore);

                    if (bulletResult.HitCount > 0)
                    {
                        theGameBoard.Remove(this);

                        if (_increasesScore)
                        {
                            if (bulletResult.HitCount > 1)
                            {
                                theGameBoard.PlayerIncrementScore(_bonusScore);
                                _bulletTraits.DuoBonus.Play();
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
            drawingTarget.DrawIndexedSprite(_spriteInstance, 0);
        }



        public override Rectangle GetBoundingRectangle()
        {
            return _spriteInstance.Extents;
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
            get { return _spriteInstance.TopLeftPosition; }
            set { _spriteInstance.TopLeftPosition = value; }
        }



        public override bool CanBeOverlapped
        {
            get { return false; }
        }
    }
}
