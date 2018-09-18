
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Controls;

namespace GameClassLibrary.Modes
{
    public class PauseWithChangeLevel : GameMode
    {
        private readonly GameMode _originalMode;
        private bool _keyReleaseSeen;
        private bool _restartGameOnNextRelease;
        private readonly AccessCodeAccumulatorControl _accessCodeControl;
        private readonly Sound.SoundTraits _pauseSound;
        private readonly Func<string, bool> _tryCode;
        private readonly Func<GameMode> _getNextModeFunction;
        private readonly Func<bool> _canChangeLevel;
        private readonly SpriteTraits _pauseSprite;



        public PauseWithChangeLevel(
            GameMode originalMode, 
            SpriteTraits pauseSprite,
            Sound.SoundTraits pauseSound,
            int accessCodeTextTopY,
            Font accessCodesFont,
            Sound.SoundTraits addLetterSound,
            Func<string, bool> tryCode,
            Func<GameMode> getNextModeFunction,
            Func<bool> canChangeLevel)
        {
            _pauseSprite = pauseSprite;
            _canChangeLevel = canChangeLevel;
            _getNextModeFunction = getNextModeFunction;
            _tryCode = tryCode;
            _pauseSound = pauseSound;
            _originalMode = originalMode;
            _keyReleaseSeen = false; // PAUSE key is taken as held, at the time this object is created.
            _restartGameOnNextRelease = false;

            if (accessCodesFont != null
                && addLetterSound != null
                && tryCode != null
                && canChangeLevel != null)
            {
                _accessCodeControl = new AccessCodeAccumulatorControl(
                    Screen.Width / 2,
                    accessCodeTextTopY,
                    4,
                    OnAccessCodeEntered,
                    addLetterSound,
                    accessCodesFont);
            }
        }



        private void OnAccessCodeEntered(string accessCode)
        {
            if (_tryCode(accessCode))
            {
                ActiveMode = _getNextModeFunction();
            }
            else
            { 
                _accessCodeControl?.ClearEntry();
                _pauseSound.Play();
            }
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (!_keyReleaseSeen)
            {
                if (!theKeyStates.Pause)
                {
                    _keyReleaseSeen = true;
                    if (_restartGameOnNextRelease)
                    {
                        ExitToOriginalMode();
                    }
                }
            }
            else if (theKeyStates.Pause)
            {
                _restartGameOnNextRelease = true;
                _keyReleaseSeen = false;
            }
            else if (_accessCodeControl != null && _canChangeLevel()) // Hint:  Allow pause, but disallow changing to avoid cheating:  eg: Man.IsDead!
            {
                _accessCodeControl.AdvanceOneCycle(theKeyStates);
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            _originalMode.Draw(drawingTarget);
            drawingTarget.DrawFirstSpriteScreenCentred(_pauseSprite);
            _accessCodeControl?.Draw(drawingTarget);
        }



        private void ExitToOriginalMode()
        {
            ActiveMode = _originalMode;
        }
    }
}
