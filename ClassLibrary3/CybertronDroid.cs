
namespace GameClassLibrary
{
    public class CybertronDroid : CybertronGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance();
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private ArtificialIntelligence.AbstractIntelligenceProvider _intelligenceProvider;

        public CybertronDroid(
            int roomX, 
            int roomY, 
            SpriteTraits spriteTraits, 
            ArtificialIntelligence.AbstractIntelligenceProvider intelligenceProvider)
        {
            SpriteInstance.RoomX = roomX;
            SpriteInstance.RoomY = roomY;
            SpriteInstance.Traits = spriteTraits;
            _intelligenceProvider = intelligenceProvider;
        }

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates theKeyStates)
        {
            Business.Animate(ref _animationCountdown, ref _imageIndex, AnimationReset, SpriteInstance.Traits.HostImageObjects.Count);
            _intelligenceProvider.AdvanceOneCycle(theGameBoard, SpriteInstance);
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            CybertronScreenPainter.DrawIndexedSprite(SpriteInstance, _imageIndex, drawingTarget);
        }

        public void CreateYourExplosion(CybertronGameBoard theGameBoard)
        {
            // TODO: FUTURE: We assume the explosion dimensions match the droid.
            theGameBoard.ExplosionsInRoom.Add(new CybertronExplosion(
                SpriteInstance.RoomX,
                SpriteInstance.RoomY,
                CybertronSpriteTraits.Explosion));
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.GetBoundingRectangle();
        }

        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            theGameBoard.Man.Die();
        }
    }
}
