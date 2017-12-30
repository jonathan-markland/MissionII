
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
            _roomNumber = -1; // so will not be added to ObjectsInRoom container again.
            theGameBoard.ObjectsToRemove.Add(this);
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            CybertronScreenPainter.DrawIndexedSprite(Sprite, 0, drawingTarget);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return Sprite.GetBoundingRectangle(); // Note: applies in every room!
        }

        public override bool YouHaveBeenShot(CybertronGameBoard theGameBoard)
        {
            // This cannot be shot (ignore)
            return false;
        }
    }
}
