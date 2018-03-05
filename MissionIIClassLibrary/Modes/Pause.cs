using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class Pause : BaseGameMode
    {
        private BaseGameMode _originalMode;
        private bool _keyReleaseSeen;
        private bool _restartGameOnNextRelease;

        public Pause(BaseGameMode originalMode)
        {
            _originalMode = originalMode;
            _keyReleaseSeen = false; // PAUSE key is held at the time this object is created.
            _restartGameOnNextRelease = false;
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
                        MissionIIGameModeSelector.ModeSelector.CurrentMode = _originalMode;
                    }
                }
            }
            else if (theKeyStates.Pause)
            {
                _restartGameOnNextRelease = true;
                _keyReleaseSeen = false;
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            _originalMode.Draw(drawingTarget);
            drawingTarget.DrawFirstSpriteScreenCentred(MissionIISprites.Paused);
        }
    }
}
