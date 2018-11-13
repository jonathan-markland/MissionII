
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.GameObjects
{
    public class ManElectrocuted : GameObject
    {
        private const int ElectrocutionAnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units

        private readonly Action<GameObject> _killMan;
        private readonly bool _isElectrocutedByWalls;
        private int _electrocutionCycles = ElectrocutionAnimationReset * 5;
        private SpriteInstance SpriteInstance = new SpriteInstance();
        private int _animationCountdown = ElectrocutionAnimationReset;
        private int _imageIndex = 0;

        public ManElectrocuted(
            Point topLeftPosition,
            ElectrocutionMethod electrocutionMethod,
            Action<GameObject> killMan)
        {
            _killMan = killMan;
            _isElectrocutedByWalls = (electrocutionMethod == ElectrocutionMethod.ByWalls);
            SpriteInstance.Traits = MissionIISprites.Electrocution;
            SpriteInstance.TopLeftPosition = topLeftPosition;
            _imageIndex = 0;
            MissionIISounds.Electrocution.Play();
        }

        public override Point TopLeftPosition
        {
            get { return SpriteInstance.TopLeftPosition; }
            set { SpriteInstance.TopLeftPosition = value; }
        }

        public override bool CanBeOverlapped => true;

        private void AdvanceAnimation()
        {
            GameClassLibrary.Algorithms.Animation.Animate(
                ref _animationCountdown, ref _imageIndex, ElectrocutionAnimationReset, SpriteInstance.Traits.ImageCount);
        }

        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            AdvanceAnimation();
            --_electrocutionCycles;
            if (_electrocutionCycles == 0)
            {
                _killMan(this);
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSprite(SpriteInstance, _imageIndex);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.Extents;
        }

        public override void ManWalkedIntoYou()
        {
            // Cannot happen
        }

        public override ShotStruct YouHaveBeenShot(bool shotByMan)
        {
            return new ShotStruct(affirmed: false);
        }
    }
}
