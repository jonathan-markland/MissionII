
namespace GameClassLibrary
{
    public abstract class CybertronObject : CybertronGameObject
    {
        private SpriteInstance Sprite;
        private int _roomNumber;

        public CybertronObject(SpriteInstance spriteInstance, int roomNumber)
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

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates theKeyStates)
        {
            // No action required.
        }

        public override void DoManIntersectionAction(CybertronGameBoard theGameBoard)
        {
            if (theGameBoard.RoomNumber == _roomNumber)
            {
                theGameBoard.PlayerInventory.Add(this);
                _roomNumber = -1; // so will not Draw or action again.
            }
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            if (theGameBoard.RoomNumber == _roomNumber)
            {
                CybertronScreenPainter.DrawIndexedSprite(Sprite, 0, drawingTarget);
            }
        }

        public override Rectangle GetBoundingRectangle()
        {
            return Sprite.GetBoundingRectangle(); // Note: applies in every room!
        }
    }
}
