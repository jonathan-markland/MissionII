﻿using GameClassLibrary.Math;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Droids
{
    public class BaseDroid : BaseGameObject
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

        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, MissionIIKeyStates theKeyStates)
        {
            Business.Animate(ref _animationCountdown, ref _imageIndex, AnimationReset, SpriteInstance.Traits.ImageCount);
            _intelligenceProvider.AdvanceOneCycle(theGameBoard, SpriteInstance);
        }

        public override void Draw(MissionIIGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(SpriteInstance, _imageIndex);
        }

        public override bool YouHaveBeenShot(MissionIIGameBoard theGameBoard, bool shotByMan)
        {
            // TODO: FUTURE: We assume the explosion dimensions match the droid.  We should centre it about the droid.
            theGameBoard.ObjectsInRoom.Add(
                new GameObjects.Explosion(
                    SpriteInstance.X,
                    SpriteInstance.Y,
                    MissionIISprites.Explosion,
                    shotByMan));

            theGameBoard.ObjectsToRemove.Add(this);
            return true;
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.GetBoundingRectangle();
        }

        public override void ManWalkedIntoYou(MissionIIGameBoard theGameBoard)
        {
            if (!theGameBoard.Man.IsInvincible)
            {
                theGameBoard.Man.Electrocute(ElectrocutionMethod.ByDroid);
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
