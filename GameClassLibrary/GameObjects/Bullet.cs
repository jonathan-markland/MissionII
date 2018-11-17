
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.GameObjects
{
    public struct BulletResult
    {
        public readonly uint HitCount;
        public readonly int TotalScoreIncrease;

        public BulletResult(uint hitCount, int totalScoreIncrease)
        {
            HitCount = hitCount;
            TotalScoreIncrease = totalScoreIncrease;
        }
    }


    public class Bullet : GameObject
    {
        private readonly BulletTraits _bulletTraits;
		private readonly MovementDeltas BulletDirection;
		private readonly bool _increasesScore;
		private readonly Func<Rectangle, bool> _isSpace;
		private readonly int _bonusScore;
		private readonly SpriteInstance _spriteInstance;
        private readonly Func<Rectangle, bool, BulletResult> _killThingsInRectangle;
        private readonly Action<int> _incrementPlayerScore;
        private readonly Action<GameObject> _removeObject;

        private bool _firingSoundDone;



        public Bullet(
            BulletTraits bulletTraits,
            Rectangle gameObjectextentsRectangle,
            MovementDeltas bulletDirection, 
            bool increasesScore, 
            int bonusScore,
            Func<Rectangle, bool> isSpace,
            Func<Rectangle, bool, BulletResult> killThingsInRectangle,
            Action<int> incrementPlayerScore,
            Action<GameObject> removeObject)
        {
            _killThingsInRectangle = killThingsInRectangle;
            _removeObject = removeObject;
            _bonusScore = bonusScore;
            _incrementPlayerScore = incrementPlayerScore;
            _removeObject = removeObject;

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
            _isSpace = isSpace;
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
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

                if (_isSpace(
                    new Rectangle(
                        proposedX, proposedY, 
                        _spriteInstance.Traits.Width,
                        _spriteInstance.Traits.Height)))
                {
                    _spriteInstance.X = proposedX;
                    _spriteInstance.Y = proposedY;

                    var bulletResult = _killThingsInRectangle(
                        GetBoundingRectangle(), IncreasesScore);

                    if (bulletResult.HitCount > 0)
                    {
                        _removeObject(this);

                        if (_increasesScore)
                        {
                            if (bulletResult.HitCount > 1)
                            {
                                _incrementPlayerScore(_bonusScore);
                                _bulletTraits.DuoBonus.Play();
                            }
                            _incrementPlayerScore(bulletResult.TotalScoreIncrease);
                        }
                        break;
                    }
                }
                else // Bullet hit wall or went outside room.
                {
                    _removeObject(this);
                    break;
                }
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(_spriteInstance, 0);
        }



        public override Rectangle GetBoundingRectangle()
        {
            return _spriteInstance.Extents;
        }



        public override ShotStruct YouHaveBeenShot(bool shotByMan)
        {
            // No action -- bullets cannot be shot.
			return new ShotStruct(affirmed: false); // ignore this.
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
