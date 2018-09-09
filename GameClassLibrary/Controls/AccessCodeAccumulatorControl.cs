using System;
using System.Linq;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Controls
{
    public class AccessCodeAccumulatorControl
    {
        private string _accessCode = String.Empty;
        private int _centreX;
        private int _centreY;
        private int _maxLength;
        private Action<string> _onEntryCompleted;
        private Action _onPlayLetterSound;
        private Font _theFont;

        public AccessCodeAccumulatorControl(
            int centreX, int centreY, int maxLength, 
            Action<string> onEntryCompleted, 
            Action onPlayLetterSound, 
            Font theFont)
        {
            _centreX = centreX;
            _centreY = centreY;
            _maxLength = maxLength;
            _onEntryCompleted = onEntryCompleted;
            _onPlayLetterSound = onPlayLetterSound;
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
                        _onPlayLetterSound();
                        return currentString + ch;
                    }
                }
            }

            return currentString;
        }
    }
}
