using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Controls
{
    public class AccessCodeAccumulatorControl
    {
        private string _accessCode = String.Empty;
        private int _centreX;
        private int _centreY;
        private int _maxLength;
        private Action<string> _onEntryCompleted;
        private Action _onPlayLetterSound;

        public AccessCodeAccumulatorControl(int centreX, int centreY, int maxLength, Action<string> onEntryCompleted, Action onPlayLetterSound)
        {
            _centreX = centreX;
            _centreY = centreY;
            _maxLength = maxLength;
            _onEntryCompleted = onEntryCompleted;
            _onPlayLetterSound = onPlayLetterSound;
        }

        public void ClearEntry()
        {
            _accessCode = String.Empty;
        }

        public void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
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
                _centreX, _centreY, _accessCode, MissionIISprites.GiantFont, TextAlignment.Centre);
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
