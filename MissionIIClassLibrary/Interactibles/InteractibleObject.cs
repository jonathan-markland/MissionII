using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

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

        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, KeyStates theKeyStates)
        {
            // No action required.
        }

        public override void ManWalkedIntoYou(MissionIIGameBoard theGameBoard)
        {
            theGameBoard.PlayerInventory.Add(this);
            RemoveThisObject(theGameBoard);
            theGameBoard.IncrementScore(CollectionScore);
            MissionIISounds.PickUpObject.Play();
        }

        protected void RemoveThisObject(MissionIIGameBoard theGameBoard)
        {
            if (_roomNumber != -1)
            {
                _roomNumber = -1; // so will not be added to ObjectsInRoom container again.
                theGameBoard.ObjectsToRemove.Add(this);
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSpriteRoomRelative(Sprite, 0);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return Sprite.Extents; // Note: applies in every room!
        }

        public override bool YouHaveBeenShot(MissionIIGameBoard theGameBoard, bool shotByMan)
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
