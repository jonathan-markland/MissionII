﻿using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary.GameObjects
{
    public class Explosion : BaseGameObject
    {
        public SpriteInstance SpriteInstance = new SpriteInstance();
        private int _imageIndex = 0;
        private int _animationCountdown = AnimationReset;
        private const int AnimationReset = 10; // TODO: Put constant elsewhere because we don't know the units
        private const int ExplosionCountDownReset = 30; // TODO: Put constant elsewhere because we don't know the units
        private int _explosionCountDown = ExplosionCountDownReset;
        private bool _canBeConsideredForMultiKillBonus;

        public Explosion(int roomX, int roomY, SpriteTraits explosionKind, bool canBeConsideredForBonus)
        {
            SpriteInstance.X = roomX;
            SpriteInstance.Y = roomY;
            SpriteInstance.Traits = explosionKind;
            _canBeConsideredForMultiKillBonus = canBeConsideredForBonus;
        }

        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, KeyStates theKeyStates)
        {
            if (_explosionCountDown == ExplosionCountDownReset)
            {
                MissionIISounds.Explosion.Play();
            }

            if (_explosionCountDown != 0)
            {
                Business.Animate(ref _animationCountdown, ref _imageIndex, AnimationReset, SpriteInstance.Traits.ImageCount);
                --_explosionCountDown;
                if (_explosionCountDown == 0)
                {
                    theGameBoard.ObjectsToRemove.Add(this);  // It gets removed by the framework when we add it to this list.
                }
            }
        }

        public override void Draw(MissionIIGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawIndexedSpriteRoomRelative(SpriteInstance, _imageIndex);
        }

        public override Rectangle GetBoundingRectangle()
        {
            return SpriteInstance.Extents;
        }

        public override void ManWalkedIntoYou(MissionIIGameBoard theGameBoard)
        {
            // No action required.
        }

        public override bool YouHaveBeenShot(MissionIIGameBoard theGameBoard, bool shotByMan)
        {
            // no action required
            return false; // ignore this.
        }

        public override Point TopLeftPosition
        {
            get { return SpriteInstance.TopLeftPosition; }
            set { SpriteInstance.TopLeftPosition = value; }
        }

        public override bool CanBeOverlapped { get { return false; } }

        public void MarkAsUsedForBonus()
        {
            _canBeConsideredForMultiKillBonus = false;
        }

        public bool CanBeConsideredForMultiKillBonus
        {
            get { return _canBeConsideredForMultiKillBonus; }
        }
    }
}