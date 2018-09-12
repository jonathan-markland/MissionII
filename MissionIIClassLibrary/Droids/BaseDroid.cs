using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary.Droids
{
    public class BaseDroid : MissionIIGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance(); // TODO: private?
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private ArtificialIntelligence.AbstractIntelligenceProvider _intelligenceProvider;

        public BaseDroid(
            SpriteTraits spriteTraits, 
            ArtificialIntelligence.AbstractIntelligenceProvider intelligenceProvider)
        {
            System.Diagnostics.Debug.Assert(spriteTraits != null);
            SpriteInstance.Traits = spriteTraits;
            _intelligenceProvider = intelligenceProvider;
        }

        public override void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates)
        {
            GameClassLibrary.Algorithms.Animation.Animate(
                ref _animationCountdown, ref _imageIndex, AnimationReset, SpriteInstance.Traits.ImageCount);
            _intelligenceProvider.AdvanceOneCycle(theGameBoard, SpriteInstance);
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSpriteRoomRelative(SpriteInstance, _imageIndex);
        }

        public override ShotStruct YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan)
        {
            // TODO: FUTURE: We assume the explosion dimensions match the droid.  We should centre it about the droid.
            theGameBoard.Add(
                new GameObjects.Explosion(
                    SpriteInstance.X,
                    SpriteInstance.Y,
                    MissionIISprites.Explosion));

            theGameBoard.Remove(this);
            return new ShotStruct { Affirmed = true, ScoreIncrease = KillScore };
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.Extents;
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            if (!theGameBoard.ManIsInvincible())
            {
                theGameBoard.Electrocute(ElectrocutionMethod.ByDroid);
            }
            else
            {
                YouHaveBeenShot(theGameBoard, true);
            }
        }

        public override Point TopLeftPosition
        {
            get { return SpriteInstance.TopLeftPosition; }
            set { SpriteInstance.TopLeftPosition = value; }
        }

        public override bool CanBeOverlapped { get { return true; } }
    }
}
