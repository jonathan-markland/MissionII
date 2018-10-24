
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Input;
using GameClassLibrary.Graphics;
using GameClassLibrary.Sound;
using GameClassLibrary.GameBoard;
using GameClassLibrary.GameObjects;
using GameClassLibrary.ArtificialIntelligence;

namespace MissionIIClassLibrary.Droids
{
    public class BaseDroid : GameObject
    {
        private readonly Action<GameObject, SpriteTraits, SoundTraits> _startExplosion;
        private readonly Action<GameObject> _manWalksIntoDroidAction;
        private SpriteInstance _spriteInstance = new SpriteInstance();
        private SpriteTraits _explosionSpriteTraits;
        private SoundTraits _explosionSound;
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private AbstractIntelligenceProvider _intelligenceProvider;



        public BaseDroid(
            SpriteTraits spriteTraits,
            SpriteTraits explosionSpriteTraits,
            SoundTraits explosionSound,
            Action<GameObject> manWalksIntoDroidAction,
            Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
        {
            System.Diagnostics.Debug.Assert(spriteTraits != null);
            _spriteInstance.Traits = spriteTraits;
            _explosionSpriteTraits = explosionSpriteTraits;
            _explosionSound = explosionSound;
            _intelligenceProvider = null;
            _manWalksIntoDroidAction = manWalksIntoDroidAction;
            _startExplosion = startExplosion;
        }



        protected void SetIntelligenceProvider(AbstractIntelligenceProvider ai) // not ideal, problem with calling base constructor in derived class because AI object needs to be tied to derived object
        {
            _intelligenceProvider = ai;
        }




        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            GameClassLibrary.Algorithms.Animation.Animate(
                ref _animationCountdown, ref _imageIndex, AnimationReset, _spriteInstance.Traits.ImageCount);

            _intelligenceProvider.AdvanceOneCycle();
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(_spriteInstance, _imageIndex);
        }



        public override ShotStruct YouHaveBeenShot(bool shotByMan)
        {
            _startExplosion(this, _explosionSpriteTraits, _explosionSound);
			return new ShotStruct(affirmed: true, scoreIncrease: KillScore);
        }



        public override Rectangle GetBoundingRectangle()
        {
            return _spriteInstance.Extents;
        }



        public override void ManWalkedIntoYou()
        {
            _manWalksIntoDroidAction(this);
        }



        public override Point TopLeftPosition
        {
            get { return _spriteInstance.TopLeftPosition; }
            set { _spriteInstance.TopLeftPosition = value; }
        }



        public override bool CanBeOverlapped { get { return true; } }
    }
}
