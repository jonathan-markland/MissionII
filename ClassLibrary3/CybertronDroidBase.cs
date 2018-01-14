using GameClassLibrary.Math;

namespace GameClassLibrary
{
    public class CybertronDroidBase : CybertronGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance(); // TODO: private?
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private ArtificialIntelligence.AbstractIntelligenceProvider _intelligenceProvider;

        public CybertronDroidBase(
            SpriteTraits spriteTraits, 
            ArtificialIntelligence.AbstractIntelligenceProvider intelligenceProvider)
        {
            SpriteInstance.Traits = spriteTraits;
            _intelligenceProvider = intelligenceProvider;
        }

        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates theKeyStates)
        {
            Business.Animate(ref _animationCountdown, ref _imageIndex, AnimationReset, SpriteInstance.Traits.ImageCount);
            _intelligenceProvider.AdvanceOneCycle(theGameBoard, SpriteInstance);
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(SpriteInstance, _imageIndex);
        }

        public override bool YouHaveBeenShot(CybertronGameBoard theGameBoard)
        {
            // TODO: FUTURE: We assume the explosion dimensions match the droid.  We should centre it about the droid.
            theGameBoard.ObjectsInRoom.Add(
                new CybertronExplosion(
                    SpriteInstance.RoomX,
                    SpriteInstance.RoomY,
                    CybertronSpriteTraits.Explosion));

            theGameBoard.ObjectsToRemove.Add(this);
            return true;
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.GetBoundingRectangle();
        }

        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            theGameBoard.Man.Die();
        }

        public override Point TopLeftPosition
        {
            get { return SpriteInstance.TopLeftPosition; }
            set { SpriteInstance.TopLeftPosition = value; }
        }

        public override bool IsSolid { get { return true; } }
    }
}
