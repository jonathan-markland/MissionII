
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Controls;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public class Pause : GameMode
    {
        private GameMode _originalMode;
        private bool _keyReleaseSeen;
        private bool _restartGameOnNextRelease;
        private AccessCodeAccumulatorControl _accessCodeControl;
        private MissionIIGameBoard _theGameBoard;

        public Pause(GameMode originalMode, MissionIIGameBoard theGameBoard)
        {
            _theGameBoard = theGameBoard;
            _originalMode = originalMode;
            _keyReleaseSeen = false; // PAUSE key is held at the time this object is created.
            _restartGameOnNextRelease = false;
            _accessCodeControl = new AccessCodeAccumulatorControl(
                Constants.ScreenWidth / 2,
                Constants.ScreenHeight - 60,
                4,
                OnAccessCodeEntered,
                MissionIISounds.ManFiring.Play,
                MissionIIClassLibrary.MissionIIFonts.GiantFont);
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
                ActiveMode = new Modes.EnteringLevel(_theGameBoard);
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
            else if (!_theGameBoard.Man.IsDead && !_theGameBoard.Man.IsBeingElectrocuted) // anti-cheating!
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
            GameClassLibrary.Modes.GameMode.ActiveMode = _originalMode;
        }
    }
}
