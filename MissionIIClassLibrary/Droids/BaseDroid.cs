
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary.Droids
{
    public class BaseDroid : GameObject
    {
        private SpriteInstance _spriteInstance = new SpriteInstance();
        private SpriteTraits _explosionSpriteTraits;
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private ArtificialIntelligence.AbstractIntelligenceProvider _intelligenceProvider;
        private Action _manDestroyAction;



        public BaseDroid(
            SpriteTraits spriteTraits, 
            SpriteTraits explosionSpriteTraits,
            ArtificialIntelligence.AbstractIntelligenceProvider intelligenceProvider,
            Action manDestroyAction)
        {
            System.Diagnostics.Debug.Assert(spriteTraits != null);
            _spriteInstance.Traits = spriteTraits;
            _explosionSpriteTraits = explosionSpriteTraits;
            _intelligenceProvider = intelligenceProvider;
            _manDestroyAction = manDestroyAction;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates)
        {
            GameClassLibrary.Algorithms.Animation.Animate(
                ref _animationCountdown, ref _imageIndex, AnimationReset, _spriteInstance.Traits.ImageCount);

            _intelligenceProvider.AdvanceOneCycle(theGameBoard, this);
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSpriteRoomRelative(_spriteInstance, _imageIndex);
        }



        public override ShotStruct YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan)
        {
            // TODO: FUTURE: We assume the explosion dimensions match the droid.  We should centre it about the droid.
            theGameBoard.Add(
                new GameObjects.Explosion(
                    _spriteInstance.X,
                    _spriteInstance.Y,
                    _explosionSpriteTraits));

            theGameBoard.Remove(this);

            return new ShotStruct { Affirmed = true, ScoreIncrease = KillScore };
        }



        public override Rectangle GetBoundingRectangle()
        {
            return _spriteInstance.Extents;
        }



        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            if (!theGameBoard.ManIsInvincible())
            {
                _manDestroyAction();
            }
            else
            {
                YouHaveBeenShot(theGameBoard, true);
            }
        }



        public override Point TopLeftPosition
        {
            get { return _spriteInstance.TopLeftPosition; }
            set { _spriteInstance.TopLeftPosition = value; }
        }



        public override bool CanBeOverlapped { get { return true; } }
    }
}
