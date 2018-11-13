
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.GameObjects
{
    public class Man : GameObject
    {
        private readonly Action<Rectangle, MovementDeltas, bool> _fireBullet;
        private readonly Action<int> _moveRoomNumberByDelta;
        private readonly Func<Rectangle, CollisionDetection.WallHitTestResult> _hitTest;
        private readonly Action<GameObject, ElectrocutionMethod> _electrocuteMan;
        private readonly Action<GameObject> _killMan;
        private readonly Action _checkManCollidingWithGameObjects;

        private SpriteInstance SpriteInstance = new SpriteInstance();
        private bool _debugInvulnerable = false;
//        private bool _isElectrocuting;
//        private bool _isElectrocutedByWalls;
        private int _facingDirection;
        private int _imageIndex = 0;
        private int _animationCountdown = WalkingAnimationReset;
        private const int WalkingAnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
//        private int _electrocutionCycles = 0;
        private bool _awaitingFireRelease = true;
        private int _invincibleCountDown = 0;
        private int _cyclesMoving = 0;

        public Man(
            Action<Rectangle, MovementDeltas, bool> fireBullet, 
            Action<int> moveRoomNumberByDelta,
            Func<Rectangle, CollisionDetection.WallHitTestResult> hitTest,
            Action<GameObject> killMan,
            Action<GameObject, ElectrocutionMethod> electrocuteMan,
            Action checkManCollidingWithGameObjects)
        {
            _fireBullet = fireBullet;
            _moveRoomNumberByDelta = moveRoomNumberByDelta;
            _hitTest = hitTest;
            _killMan = killMan;
            _electrocuteMan = electrocuteMan;
            _checkManCollidingWithGameObjects = checkManCollidingWithGameObjects;
        }

        public PositionAndDirection Position
        {
            get
            {
                return new PositionAndDirection(
                    new Point(SpriteInstance.X, SpriteInstance.Y),
                    _facingDirection);
            }

            set
            {
                SpriteInstance.X = value.Position.X;
                SpriteInstance.Y = value.Position.Y;
                _facingDirection = value.FacingDirection;
                Standing(value.FacingDirection);
            }
        }

        public override void AdvanceOneCycle(KeyStates keyStates)
        {
            // if (_isElectrocuting || _isElectrocutedByWalls)
            // {
            //     DoElectrocution();
            // }
            // else
            // {
                FireButtonCheck(keyStates);

                int theDirection = keyStates.ToDirectionIndex();
                if (theDirection != -1)
                {
                    DoWalking(keyStates, theDirection);
                }
                else
                {
                    Standing(_facingDirection);
                }

                HandleInvincibility();
            //}
        }

        public void GainInvincibility()
        {
            _invincibleCountDown = Constants.ManInvincibilityCycles;
        }

        private void HandleInvincibility()
        {
            if (_invincibleCountDown > 0)
            {
                --_invincibleCountDown;
            }
        }

//        private void DoElectrocution()
//        {
//            AdvanceAnimation();
//            --_electrocutionCycles;
//            if (_electrocutionCycles == 0)
//            {
//                Die();
//            }
//        }

        private void DoWalking(KeyStates keyStates, int theDirection)
        {
            ++_cyclesMoving;
            _facingDirection = theDirection;
            SpriteInstance.Traits = MissionIISprites.ManWalking[theDirection];
            AdvanceAnimation();
            var movementDeltas = keyStates.ToMovementDeltas();
            var hitResult = this.MoveConsideringWallsOnly(movementDeltas, _hitTest);

            if (!movementDeltas.IsStationary)
            {
                PlayWalkingEffects();
            }

            // Collision between man and walls?

            if (hitResult == CollisionDetection.WallHitTestResult.HitWall)
            {
                Electrocute(ElectrocutionMethod.ByWalls);
            }
            else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomAbove)
            {
                RoomUp();
            }
            else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomBelow)
            {
                RoomDown();
            }
            else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomToLeft)
            {
                RoomLeft();
            }
            else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomToRight)
            {
                RoomRight();
            }
            else
            {
                _checkManCollidingWithGameObjects();
            }
        }

        private void PlayWalkingEffects()
        {
            var c = Constants.FootstepSoundCycles;
            var n = _cyclesMoving % c;
            if (n == 0)
            {
                MissionIISounds.FootStep1.Play();
            }
            else if (n == (c / 2))
            {
                MissionIISounds.FootStep2.Play();
            }
        }

        private void FireButtonCheck(KeyStates keyStates)
        {
            if (keyStates.Fire)
            {
                if (!_awaitingFireRelease)
                {
                    _fireBullet(
                        SpriteInstance.Extents, 
                        MovementDeltas.ConvertFromFacingDirection(_facingDirection) // TODO
                        , true);
                    _awaitingFireRelease = true; // require press-release sequence for firing bullets.
                }
            }
            else
            {
                // Fire button released, do:
                _awaitingFireRelease = false;
            }
        }

        private void AdvanceAnimation()
        {
            GameClassLibrary.Algorithms.Animation.Animate(
                ref _animationCountdown, ref _imageIndex, WalkingAnimationReset, SpriteInstance.Traits.ImageCount);
        }

        private void Standing(int theDirection)
        {
            _animationCountdown = WalkingAnimationReset; // for next time
            SpriteInstance.Traits = MissionIISprites.ManStanding[theDirection];
            _imageIndex = 0;
            _cyclesMoving = 0;
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            if (_invincibleCountDown == 0 || DrawManDuringInvincibility())
            {
                drawingTarget.DrawIndexedSprite(SpriteInstance, _imageIndex);
            }
        }

        private bool DrawManDuringInvincibility()
        {
            var n = (_invincibleCountDown < Constants.ManInvincibilityAlmostOutCycles) ? (16 + 8) : (4 + 2);
            return (_invincibleCountDown & n) > (n / 4);
        }

        public void Alive(int theDirection, int roomX, int roomY) // TODO: refactor to create fresh Man object instead and do all this in the constructor
        {
            _facingDirection = theDirection;
            Standing(theDirection);
            SpriteInstance.X = roomX;
            SpriteInstance.Y = roomY;
        }

        public void Electrocute(ElectrocutionMethod electrocutionMethod)
        {
            if (_debugInvulnerable) return;
            if (!IsInvincible /*&& !_isElectrocuting*/)
            {
                _electrocuteMan(this, electrocutionMethod);
                //                _isElectrocuting = true;
                //                _isElectrocutedByWalls = electrocutionMethod == ElectrocutionMethod.ByWalls;
                //                _electrocutionCycles = ElectrocutionAnimationReset * 5;
                //                SpriteInstance.Traits = MissionIISprites.Electrocution;
                //                _imageIndex = 0;
                //                _animationCountdown = ElectrocutionAnimationReset;
                //                MissionIISounds.Electrocution.Play();
            }
        }

        private void Die()
        {
            if (_debugInvulnerable) return;
            _killMan(this);  // removes this object, replaces with new ManDead
        }

        private void RoomUp()
        {
            MoveRooms(
                -Constants.RoomsHorizontally,
                0, 0,
                0, -SpriteInstance.Traits.Height);
        }

        private void RoomDown()
        {
            MoveRooms(
                Constants.RoomsHorizontally,
                0, 0,
                0, +SpriteInstance.Traits.Height);
        }

        private void RoomLeft()
        {
            MoveRooms(
                -1,
                0, -SpriteInstance.Traits.Width,
                0, 0);
        }

        private void RoomRight()
        {
            MoveRooms(
                +1,
                0, +SpriteInstance.Traits.Width,
                0, 0);
        }

        private void MoveRooms(
            int roomNumberDelta, 
            int deltaRoomWidth, int deltaSpriteWidth, 
            int deltaRoomHeight, int deltaSpriteHeight)
        {
            // Note: We sort of assume all the rooms are the same size!  (Which they are!)
            var roomWidth = Constants.TileWidth * Constants.ClustersHorizontally * Constants.DestClusterSide;
            var roomHeight = Constants.TileHeight * Constants.ClustersVertically * Constants.DestClusterSide;

            SpriteInstance.X += roomWidth * deltaRoomWidth;
            SpriteInstance.X += deltaSpriteWidth;
            SpriteInstance.Y += roomHeight * deltaRoomHeight;
            SpriteInstance.Y += deltaSpriteHeight;

            _moveRoomNumberByDelta(roomNumberDelta);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.Extents;
        }

        public override void ManWalkedIntoYou()
        {
            // No action for self-intersection.
        }

        public override ShotStruct YouHaveBeenShot(bool shotByMan)
        {
            if (!IsInvincible)
            {
                Die(); // No electrocution animation desired here.
            }
			return new ShotStruct(affirmed:true);
        }

        public override Point TopLeftPosition
        {
            get { return SpriteInstance.TopLeftPosition; }
            set { SpriteInstance.TopLeftPosition = value; }
        }

        public override bool CanBeOverlapped { get { return true; } }

//        public bool IsBeingElectrocuted
//        {
//            get { return _isElectrocuting; }
//        }
//
//        public bool IsBeingElectrocutedByWalls
//        {
//            get { return _isElectrocutedByWalls; }
//        }

        public bool IsInvincible
        {
            get
            {
                return _invincibleCountDown > 0;
            }
        }
    }
}


