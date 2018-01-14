
using System;

namespace GameClassLibrary
{
    public struct CybertronManPosition
    {
        public Point Position;
        public int FacingDirection;
    }

    public class CybertronMan : CybertronGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance();
        private bool _debugInvulnerable = false;
        private bool _isDead;
        private bool _isElectrocuting;
        private int _facingDirection;
        private int _imageIndex = 0;
        private int _animationCountdown = WalkingAnimationReset;
        private const int WalkingAnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private const int ElectrocutionAnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private int _electrocutionCycles = 0;
        private bool _awaitingFireRelease = true;
        private int _whileDeadCount = 0;

        public CybertronManPosition Position
        {
            get
            {
                return new CybertronManPosition
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

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates keyStates)
        {
            if (_isElectrocuting)
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
            }
            else
            {
                DeadHandling(theGameBoard);
            }
        }

        private void DoElectrocution()
        {
            AdvanceAnimation();
            --_electrocutionCycles;
            if (_electrocutionCycles == 0)
            {
                _isElectrocuting = false;
                Die();
            }
        }

        private void DoWalking(CybertronGameBoard theGameBoard, CybertronKeyStates keyStates, int theDirection)
        {
            _facingDirection = theDirection;
            SpriteInstance.Traits = CybertronSpriteTraits.ManWalking[theDirection];
            AdvanceAnimation();
            var movementDeltas = Business.GetMovementDeltas(keyStates);
            var hitResult = CybertronGameStateUpdater.MoveManOnePixel(theGameBoard, movementDeltas);

            // Collision between man and walls?

            if (hitResult == CollisionDetection.WallHitTestResult.HitWall)
            {
                Electrocute();
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

        private void FireButtonCheck(CybertronGameBoard theGameBoard, CybertronKeyStates keyStates)
        {
            if (keyStates.Fire)
            {
                if (!_awaitingFireRelease)
                {
                    CybertronGameStateUpdater.StartBullet(SpriteInstance, _facingDirection, theGameBoard, true);
                    _awaitingFireRelease = true; // require press-release sequence for firing bullets.
                }
            }
            else
            {
                // Fire button released, do:
                _awaitingFireRelease = false;
            }
        }

        private void DeadHandling(CybertronGameBoard theGameBoard)
        {
            System.Diagnostics.Debug.Assert(_isDead);
            if (_whileDeadCount > 0)
            {
                --_whileDeadCount;
            }
            else
            {
                CybertronGameStateUpdater.LoseLife(theGameBoard);
            }
        }

        private void AdvanceAnimation()
        {
            Business.Animate(ref _animationCountdown, ref _imageIndex, WalkingAnimationReset, SpriteInstance.Traits.ImageCount);
        }

        private void Electrocute()
        {
            if (_debugInvulnerable) return;
            _isElectrocuting = true;
            _electrocutionCycles = ElectrocutionAnimationReset * 5;
            SpriteInstance.Traits = CybertronSpriteTraits.Electrocution;
            _imageIndex = 0;
            _animationCountdown = ElectrocutionAnimationReset;
        }

        private void Standing(int theDirection)
        {
            _animationCountdown = WalkingAnimationReset; // for next time
            SpriteInstance.Traits = CybertronSpriteTraits.ManStanding[theDirection];
            _imageIndex = 0;
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(SpriteInstance, _imageIndex);
        }

        public void Alive(int theDirection, int roomX, int roomY) // TODO: refactor to use the Position property.
        {
            _isDead = false;
            _facingDirection = theDirection;
            Standing(theDirection);
            SpriteInstance.RoomX = roomX;
            SpriteInstance.RoomY = roomY;
        }

        public void Die()
        {
            if (_debugInvulnerable) return;
            if (!_isDead)
            {
                _isDead = true;
                _imageIndex = 0;
                SpriteInstance.Traits = CybertronSpriteTraits.Dead;
                // TODO: Sound
                // TODO: Reduce lives.
                _whileDeadCount = Constants.ManDeadDelayCycles;
            }
        }

        public bool IsDead
        {
            get { return _isDead; }
        }

        private void RoomUp(CybertronGameBoard theGameBoard)
        {
            MoveRooms(
                theGameBoard,
                -Constants.RoomsHorizontally,
                0, 0,
                +1, -SpriteInstance.Traits.BoardHeight);
        }

        private void RoomDown(CybertronGameBoard theGameBoard)
        {
            MoveRooms(
                theGameBoard,
                Constants.RoomsHorizontally,
                0, 0,
                -1, +SpriteInstance.Traits.BoardHeight);
        }

        private void RoomLeft(CybertronGameBoard theGameBoard)
        {
            MoveRooms(
                theGameBoard,
                -1,
                +1, -SpriteInstance.Traits.BoardWidth,
                0, 0);
        }

        private void RoomRight(CybertronGameBoard theGameBoard)
        {
            MoveRooms(
                theGameBoard,
                + 1,
                -1, +SpriteInstance.Traits.BoardWidth,
                0, 0);
        }

        private void MoveRooms(
            CybertronGameBoard theGameBoard, 
            int roomNumberDelta, 
            int deltaRoomWidth, int deltaSpriteWidth, 
            int deltaRoomHeight, int deltaSpriteHeight)
        {
            theGameBoard.RoomNumber += roomNumberDelta;
            // Note: We sort of assume all the rooms are the same size!  (Which they are!)
            var roomWidth = theGameBoard.CurrentRoomWallData.CountH * CybertronGameBoardConstants.TileWidth; // TODO: Not ideal having these possibly repeated calculations.
            var roomHeight = theGameBoard.CurrentRoomWallData.CountV * CybertronGameBoardConstants.TileHeight; // TODO: Not ideal having these possibly repeated calculations.
            SpriteInstance.RoomX += roomWidth * deltaRoomWidth;
            SpriteInstance.RoomX += deltaSpriteWidth;
            SpriteInstance.RoomY += roomHeight * deltaRoomHeight;
            SpriteInstance.RoomY += deltaSpriteHeight;
            if (! theGameBoard.DroidsExistInRoom)
            {
                CybertronGameStateUpdater.IncrementScore(theGameBoard, CybertronGameBoardConstants.RoomClearingBonusScore);
            }
            CybertronGameStateUpdater.PrepareForNewRoom(theGameBoard);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.GetBoundingRectangle();
        }

        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            // No action for self-intersection.
        }

        public override bool YouHaveBeenShot(CybertronGameBoard theGameBoard)
        {
            Die();
            return true;
        }

        public override Point TopLeftPosition
        {
            get { return SpriteInstance.TopLeftPosition; }
            set { SpriteInstance.TopLeftPosition = value; }
        }

        public override bool IsSolid { get { return true; } }

        public bool IsBeingElectrocuted
        {
            get { return _isElectrocuting; }
        }
    }
}


