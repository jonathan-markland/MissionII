﻿using MissionIIClassLibrary.Math;

namespace MissionIIClassLibrary.Interactibles
{
    public abstract class InteractibleObject : BaseGameObject
    {
        private SpriteInstance Sprite;
        private int _roomNumber;

        public InteractibleObject(SpriteInstance spriteInstance, int roomNumber)
        {
            Sprite = spriteInstance;
            _roomNumber = roomNumber;
        }

        public SpriteTraits SpriteTraits
        {
            get
            {
                return Sprite.Traits;
            }
        }

        /// <summary>
        /// Returns room number for this object, or -1 if picked up.
        /// </summary>
        public int RoomNumber
        {
            get { return _roomNumber; }
        }

        public Point Position
        {
            set
            {
                Sprite.RoomX = value.X;
                Sprite.RoomY = value.Y;
            }
        }

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates theKeyStates)
        {
            // No action required.
        }

        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            theGameBoard.PlayerInventory.Add(this);
            RemoveThisObject(theGameBoard);
            theGameBoard.IncrementScore(CollectionScore);
            CybertronSounds.Play(CybertronSounds.PickUpObject);
        }

        protected void RemoveThisObject(CybertronGameBoard theGameBoard)
        {
            if (_roomNumber != -1)
            {
                _roomNumber = -1; // so will not be added to ObjectsInRoom container again.
                theGameBoard.ObjectsToRemove.Add(this);
            }
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(Sprite, 0);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return Sprite.GetBoundingRectangle(); // Note: applies in every room!
        }

        public override bool YouHaveBeenShot(CybertronGameBoard theGameBoard, bool shotByMan)
        {
            // This cannot be shot (ignore)
            return false;
        }

        public override Point TopLeftPosition
        {
            get { return Sprite.TopLeftPosition; }
            set { Sprite.TopLeftPosition = value; }
        }

        public override bool CanBeOverlapped { get { return true; } }

        public abstract int CollectionScore { get; }
    }
}