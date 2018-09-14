using GameClassLibrary.Math;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class MissionIIInteractibleObject : InteractibleObject
    {
        private SpriteInstance Sprite;
        private int _roomNumber;

        public MissionIIInteractibleObject(SpriteInstance spriteInstance, int roomNumber)
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

        public override Rectangle GetBoundingRectangle()
        {
            return Sprite.Extents; // Note: applies in every room!
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            _roomNumber = -1; // so will not be added to ObjectsInRoom container again.
            base.ManWalkedIntoYou(theGameBoard);
            MissionIISounds.PickUpObject.Play();
        }

        public override Point TopLeftPosition
        {
            get { return Sprite.TopLeftPosition; }
            set { Sprite.TopLeftPosition = value; }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSpriteRoomRelative(Sprite, 0);
        }
    }
}
