
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.GameObjects
{
    public class Man : BaseGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance();
        private bool _debugInvulnerable = false;
        private bool _isDead;
        private bool _isElectrocuting;
        private bool _isElectrocutedByWalls;
        private int _facingDirection;
        private int _imageIndex = 0;
        private int _animationCountdown = WalkingAnimationReset;
        private const int WalkingAnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private const int ElectrocutionAnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private int _electrocutionCycles = 0;
        private bool _awaitingFireRelease = true;
        private int _whileDeadCount = 0;
        private int _invincibleCountDown = 0;
        public int _cyclesMoving = 0;

        public PositionAndDirection Position
        {
            get
            {
                return new PositionAndDirection
                {
                    Position = new Point(SpriteInstance.RoomX, SpriteInstance.RoomY),
                    FacingDirection = _facingDirection
                };
            }
            set
            {
                _isDead = false;
                SpriteInstance.RoomX = value.Position.X;
                SpriteInstance.RoomY = value.Position.Y;
                _facingDirection = value.FacingDirection;
                Standing(value.FacingDirection);
            }
        }

        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, MissionIIKeyStates keyStates)
        {
            if (_isElectrocuting || _isElectrocutedByWalls)
            {
                DoElectrocution();
            }
            else if (!_isDead)
            {
                FireButtonCheck(theGameBoard, keyStates);

                int theDirection = Business.GetDirectionIndex(keyStates);
                if (theDirection != -1)
                {
                    DoWalking(theGameBoard, keyStates, theDirection);
                }
                else
                {
                    Standing(_facingDirection);
                }

                HandleInvincibility();
            }
            else
            {
                DeadHandling(theGameBoard);
            }
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

        private void DoElectrocution()
        {
            AdvanceAnimation();
            --_electrocutionCycles;
            if (_electrocutionCycles == 0)
            {
                Die();
            }
        }

        private void DoWalking(MissionIIGameBoard theGameBoard, MissionIIKeyStates keyStates, int theDirection)
        {
            ++_cyclesMoving;
            _facingDirection = theDirection;
            SpriteInstance.Traits = MissionIISprites.ManWalking[theDirection];
            AdvanceAnimation();
            var movementDeltas = Business.GetMovementDeltas(keyStates);
            var hitResult = theGameBoard.MoveManOnePixel(movementDeltas);

            if (!movementDeltas.Stationary)
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
                RoomUp(theGameBoard);
            }
            else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomBelow)
            {
                RoomDown(theGameBoard);
            }
            else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomToLeft)
            {
                RoomLeft(theGameBoard);
            }
            else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomToRight)
            {
                RoomRight(theGameBoard);
            }
            else
            {
                // Collision between man and room objects?

                var manRectangle = GetBoundingRectangle();
                theGameBoard.ObjectsInRoom.ForEachDo(roomObject =>
                {
                    if (!_isDead && manRectangle.Intersects(roomObject.GetBoundingRectangle()))
                    {
                        roomObject.ManWalkedIntoYou(theGameBoard);
                    }
                });
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

        private void FireButtonCheck(MissionIIGameBoard theGameBoard, MissionIIKeyStates keyStates)
        {
            if (keyStates.Fire)
            {
                if (!_awaitingFireRelease)
                {
                    theGameBoard.StartBullet(SpriteInstance, _facingDirection, true);
                    _awaitingFireRelease = true; // require press-release sequence for firing bullets.
                }
            }
            else
            {
                // Fire button released, do:
                _awaitingFireRelease = false;
            }
        }

        private void DeadHandling(MissionIIGameBoard theGameBoard)
        {
            System.Diagnostics.Debug.Assert(_isDead);
            if (_whileDeadCount > 0)
            {
                --_whileDeadCount;
            }
            else
            {
                theGameBoard.LoseLife();
            }
        }

        private void AdvanceAnimation()
        {
            Business.Animate(ref _animationCountdown, ref _imageIndex, WalkingAnimationReset, SpriteInstance.Traits.ImageCount);
        }

        public void Electrocute(ElectrocutionMethod electrocutionMethod)
        {
            if (_debugInvulnerable) return;
            if (!IsInvincible && !_isDead && !_isElectrocuting)
            {
                _isElectrocuting = true;
                _isElectrocutedByWalls = electrocutionMethod == ElectrocutionMethod.ByWalls;
                _electrocutionCycles = ElectrocutionAnimationReset * 5;
                SpriteInstance.Traits = MissionIISprites.Electrocution;
                _imageIndex = 0;
                _animationCountdown = ElectrocutionAnimationReset;
                MissionIISounds.Electrocution.Play();
            }
        }

        private void Standing(int theDirection)
        {
            _animationCountdown = WalkingAnimationReset; // for next time
            SpriteInstance.Traits = MissionIISprites.ManStanding[theDirection];
            _imageIndex = 0;
            _cyclesMoving = 0;
        }

        public override void Draw(MissionIIGameBoard theGameBoard, IDrawingTarget drawingTarget)
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

        public void Alive(int theDirection, int roomX, int roomY) // TODO: refactor to use the Position property.
        {
            _isDead = false;
            _facingDirection = theDirection;
            Standing(theDirection);
            SpriteInstance.RoomX = roomX;
            SpriteInstance.RoomY = roomY;
        }

        private void Die()
        {
            if (_debugInvulnerable) return;
            if (!_isDead)
            {
                _isElectrocuting = false;
                _isElectrocutedByWalls = false;
                _isDead = true;
                _imageIndex = 0;
                _invincibleCountDown = 0;
                SpriteInstance.Traits = MissionIISprites.Dead;
                // TODO: Sound
                // TODO: Reduce lives.
                _whileDeadCount = Constants.ManDeadDelayCycles;
                MissionIISounds.ManGrunt.Play();
            }
        }

        public bool IsDead
        {
            get { return _isDead; }
        }

        private void RoomUp(MissionIIGameBoard theGameBoard)
        {
            MoveRooms(
                theGameBoard,
                -Constants.RoomsHorizontally,
                0, 0,
                +1, -SpriteInstance.Traits.BoardHeight);
        }

        private void RoomDown(MissionIIGameBoard theGameBoard)
        {
            MoveRooms(
                theGameBoard,
                Constants.RoomsHorizontally,
                0, 0,
                -1, +SpriteInstance.Traits.BoardHeight);
        }

        private void RoomLeft(MissionIIGameBoard theGameBoard)
        {
            MoveRooms(
                theGameBoard,
                -1,
                +1, -SpriteInstance.Traits.BoardWidth,
                0, 0);
        }

        private void RoomRight(MissionIIGameBoard theGameBoard)
        {
            MoveRooms(
                theGameBoard,
                + 1,
                -1, +SpriteInstance.Traits.BoardWidth,
                0, 0);
        }

        private void MoveRooms(
            MissionIIGameBoard theGameBoard, 
            int roomNumberDelta, 
            int deltaRoomWidth, int deltaSpriteWidth, 
            int deltaRoomHeight, int deltaSpriteHeight)
        {
            theGameBoard.MoveRoomNumberByDelta(roomNumberDelta);
            // Note: We sort of assume all the rooms are the same size!  (Which they are!)
            var roomWidth = theGameBoard.CurrentRoomWallData.CountH * Constants.TileWidth; // TODO: Not ideal having these possibly repeated calculations.
            var roomHeight = theGameBoard.CurrentRoomWallData.CountV * Constants.TileHeight; // TODO: Not ideal having these possibly repeated calculations.
            SpriteInstance.RoomX += roomWidth * deltaRoomWidth;
            SpriteInstance.RoomX += deltaSpriteWidth;
            SpriteInstance.RoomY += roomHeight * deltaRoomHeight;
            SpriteInstance.RoomY += deltaSpriteHeight;
            if (! theGameBoard.DroidsExistInRoom)
            {
                theGameBoard.IncrementScore(Constants.RoomClearingBonusScore);
                MissionIISounds.Bonus.Play();
            }
            theGameBoard.PrepareForNewRoom();
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.GetBoundingRectangle();
        }

        public override void ManWalkedIntoYou(MissionIIGameBoard theGameBoard)
        {
            // No action for self-intersection.
        }

        public override bool YouHaveBeenShot(MissionIIGameBoard theGameBoard, bool shotByMan)
        {
            if (!IsInvincible)
            {
                Die(); // No electrocution animation desired here.
            }
            return true;
        }

        public override Point TopLeftPosition
        {
            get { return SpriteInstance.TopLeftPosition; }
            set { SpriteInstance.TopLeftPosition = value; }
        }

        public override bool CanBeOverlapped { get { return true; } }

        public bool IsBeingElectrocuted
        {
            get { return _isElectrocuting; }
        }

        public bool IsBeingElectrocutedByWalls
        {
            get { return _isElectrocutedByWalls; }
        }

        public bool IsInvincible
        {
            get
            {
                return _invincibleCountDown > 0;
            }
        }
    }
}


