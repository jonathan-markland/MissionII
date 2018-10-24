using System;
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class MissionIIInteractibleObject : InteractibleObject
    {
        private bool _positionedAlready;
        private SpriteInstance Sprite;
        protected int _roomNumber;

        public MissionIIInteractibleObject(SpriteInstance spriteInstance, int roomNumber, Action<InteractibleObject, int> collectObject)
            : base(collectObject)
        {
            _positionedAlready = false;
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

        protected void MarkCollected()
        {
            _roomNumber = -1; // so will not be added to ObjectsInRoom container again.
        }

        public override Rectangle GetBoundingRectangle()
        {
            return Sprite.Extents; // Note: applies in every room!
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            MarkCollected();
            base.ManWalkedIntoYou(theGameBoard);
            MissionIISounds.PickUpObject.Play();
        }

        public override Point TopLeftPosition
        {
            get { return Sprite.TopLeftPosition; }
            set
            {
                _positionedAlready = true;
                Sprite.TopLeftPosition = value;
            }
        }

        public bool PositionedAlready { get { return _positionedAlready; } }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(Sprite, 0);
        }
    }
}
