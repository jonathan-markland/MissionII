
using System;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class Pause : BaseGameMode
    {
        private BaseGameMode _originalMode;
        private bool _keyReleaseSeen;
        private bool _restartGameOnNextRelease;
        private Controls.AccessCodeAccumulatorControl _accessCodeControl;
        private MissionIIGameBoard _theGameBoard;

        public Pause(BaseGameMode originalMode, MissionIIGameBoard theGameBoard)
        {
            _theGameBoard = theGameBoard;
            _originalMode = originalMode;
            _keyReleaseSeen = false; // PAUSE key is held at the time this object is created.
            _restartGameOnNextRelease = false;
            _accessCodeControl = new Controls.AccessCodeAccumulatorControl(
                Constants.ScreenWidth / 2,
                Constants.ScreenHeight - 60,
                4,
                OnAccessCodeEntered,
                MissionIISounds.ManFiring.Play);
        }

        private void OnAccessCodeEntered(string accessCode)
        {
            if (!_theGameBoard.LevelCodeAccepted(accessCode))
            {
                _accessCodeControl.ClearEntry();
                MissionIISounds.PauseMode.Play();
            }
            else
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new Modes.EnteringLevel(_theGameBoard);
            }
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
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
            else
            {
                _accessCodeControl.AdvanceOneCycle(theKeyStates);
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            _originalMode.Draw(drawingTarget);
            drawingTarget.DrawFirstSpriteScreenCentred(MissionIISprites.Paused);
            _accessCodeControl.Draw(drawingTarget);
        }

        private void ExitToOriginalMode()
        {
            MissionIIGameModeSelector.ModeSelector.CurrentMode = _originalMode;
        }
    }
}
