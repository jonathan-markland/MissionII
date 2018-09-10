
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Modes
{
    /// <summary>
    /// Shows a placard sprite for a period, with an accompanying sound.
    /// Then move to the next mode using the given function.
    /// </summary>
    public class PlacardScreen : GameMode
    {
        private int _countDown;
        private bool _firstCycle = true;
        private Sound.SoundTraits _placardSound;
        private SpriteTraits _placardSprite;
        private Func<GameMode> _getNextModeFunction;



        public PlacardScreen(
            int placardCycles,
            SpriteTraits placardSprite,
            Sound.SoundTraits placardSound,
            Func<GameMode> getNextModeFunction)
        {
            _getNextModeFunction = getNextModeFunction;
            _placardSprite = placardSprite;
            _placardSound = placardSound;
            _countDown = placardCycles;
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_firstCycle)
            {
                _firstCycle = false;
                _placardSound.Play();
            }

            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                ActiveMode = _getNextModeFunction();
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawFirstSpriteScreenCentred(_placardSprite);
        }
    }
}
