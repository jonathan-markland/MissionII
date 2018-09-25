using System;
using System.Linq;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Controls
{
    public class AccessCodeAccumulatorControl
    {
        private readonly int _centreX;
		private readonly int _centreY;
		private readonly int _maxLength;
		private readonly Action<string> _onEntryCompleted;
		private readonly Sound.SoundTraits _playLetterSound;
		private readonly Font _theFont;

		private string _accessCode = String.Empty;



        public AccessCodeAccumulatorControl(
            int centreX, int centreY, int maxLength, 
            Action<string> onEntryCompleted,
            Sound.SoundTraits playLetterSound, 
            Font theFont)
        {
            _centreX = centreX;
            _centreY = centreY;
            _maxLength = maxLength;
            _onEntryCompleted = onEntryCompleted;
            _playLetterSound = playLetterSound;
            _theFont = theFont;
        }



        public void ClearEntry()
        {
            _accessCode = String.Empty;
        }

        

		public void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_accessCode.Length == _maxLength)
            {
                if (theKeyStates.AllKeysReleased)
                {
                    _onEntryCompleted(_accessCode);
                }
            }
            else
            {
                _accessCode = AccumulateAccessCode(
                    _accessCode,
                    theKeyStates.Up,
                    theKeyStates.Down,
                    theKeyStates.Left,
                    theKeyStates.Right,
                    theKeyStates.Fire);
            }
        }



        public void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawText(
                _centreX, _centreY, _accessCode, _theFont, TextAlignment.Centre);
        }



        private string AccumulateAccessCode(string currentString, bool up, bool down, bool left, bool right, bool fire)
        {
            if (currentString.Length >= _maxLength)
            {
                return String.Empty;
            }
            else
            {
                char ch = ' ';
                if (up) ch = 'U';
                else if (down) ch = 'D';
                else if (left) ch = 'L';
                else if (right) ch = 'R';
                else if (fire) ch = 'F';
                if (ch != ' ')
                {
                    if (currentString.Length == 0 || currentString.Last() != ch)
                    {
                        _playLetterSound.Play();
                        return currentString + ch;
                    }
                }
            }

            return currentString;
        }
    }
}
