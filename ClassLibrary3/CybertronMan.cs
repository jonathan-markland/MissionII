
namespace GameClassLibrary
{
    public class CybertronMan : CybertronGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance();
        private bool _isDead;
        private bool _isElectrocuting;
        private int _facingDirection = 2;  // TODO: This is the man's initial facing direction.  Sort out properly.z
        private int _imageIndex = 0;
        private int _animationCountdown = WalkingAnimationReset;
        private const int WalkingAnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private const int ElectrocutionAnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private int _electrocutionCycles = 0;
        private bool _awaitingFireRelease = true;

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates keyStates)
        {
            if (_isElectrocuting)
            {
                AdvanceAnimation();
                --_electrocutionCycles;
                if (_electrocutionCycles == 0)
                {
                    Die();
                }
            }
            else if (!_isDead)
            {
                // Collision between man and walls?

                int theDirection = Business.GetDirectionIndex(keyStates);
                if (theDirection != -1)
                {
                    _facingDirection = theDirection;
                    SpriteInstance.Traits = CybertronSpriteTraits.ManWalking[theDirection];
                    AdvanceAnimation();
                    var movementDeltas = Business.GetMovementDeltas(keyStates);

                    var hitResult = CybertronGameStateUpdater.MoveManOnePixel(theGameBoard, movementDeltas);
                    if (hitResult == CollisionDetection.WallHitTestResult.HitWall)
                    {
                        Electrocute();
                        return;
                    }
                    else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomAbove)
                    {
                        RoomUp(theGameBoard);
                        return;
                    }
                    else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomBelow)
                    {
                        RoomDown(theGameBoard);
                        return;
                    }
                    else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomToLeft)
                    {
                        RoomLeft(theGameBoard);
                        return;
                    }
                    else if (hitResult == CollisionDetection.WallHitTestResult.OutsideRoomToRight)
                    {
                        RoomRight(theGameBoard);
                        return;
                    }

                    // Collision between man and room objects?

                    var manRectangle = GetBoundingRectangle();
                    theGameBoard.ForEachDo(roomObject =>
                    {
                        if (manRectangle.Intersects(roomObject.GetBoundingRectangle()))
                        {
                            roomObject.DoManIntersectionAction(theGameBoard);
                            if (_isDead) return false;
                        }
                        return true;
                    });
                }
                else
                {
                    Standing(_facingDirection);
                }

                if (!_isDead && !_isElectrocuting)
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
            }
        }

        private void AdvanceAnimation()
        {
            Business.Animate(ref _animationCountdown, ref _imageIndex, WalkingAnimationReset, SpriteInstance.Traits.HostImageObjects.Count);
        }

        private void Electrocute()
        {
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
            CybertronScreenPainter.DrawIndexedSprite(SpriteInstance, _imageIndex, drawingTarget);
        }

        public void Alive(int theDirection, int roomX, int roomY)
        {
            _isDead = false;
            Standing(theDirection);
            SpriteInstance.RoomX = roomX;
            SpriteInstance.RoomY = roomY;
        }

        public void Die()
        {
            _isDead = true;
            _imageIndex = 0;
            SpriteInstance.Traits = CybertronSpriteTraits.Dead;
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
            if (theGameBoard.DroidsInRoom.Count == 0)
            {
                CybertronGameStateUpdater.IncrementScore(theGameBoard, CybertronGameBoardConstants.RoomClearingBonusScore);
            }
            CybertronGameStateUpdater.PrepareForNewRoom(theGameBoard);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.GetBoundingRectangle();
        }

        public override void DoManIntersectionAction(CybertronGameBoard theGameBoard)
        {
            // No action for self-intersection.
        }
    }
}


