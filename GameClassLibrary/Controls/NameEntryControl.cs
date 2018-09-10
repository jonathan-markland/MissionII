using System;
using System.Linq;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Controls
{
    public class NameEntryControl
    {
        private static string NextCharString = " ";
        private uint _cycleCounter;
        private string _editString;
        private bool _sustainEditModeUntilButtonsReleased;
        private bool _waitingForRelease;
        private Font _theFont;
        private SpriteTraits _cursorSprite;
        private int _maxLength;
        private Action<string> _onFinalStringSet;
        private bool _addingCharsAllowed;

        public NameEntryControl(
            Font theFont,
            SpriteTraits cursorSprite,
            int maxLength)
        {
            _maxLength = maxLength;
            _theFont = theFont;
            _cursorSprite = cursorSprite;
            _waitingForRelease = true;
            _sustainEditModeUntilButtonsReleased = false;
        }

        public void Start(string textString, Action<string> onFinalStringSet)
        {
            _onFinalStringSet = onFinalStringSet;
            _editString = textString;
            _cycleCounter = 0;
            _addingCharsAllowed = true;
        }

        public bool InEditMode
        {
            get
            {
                return _sustainEditModeUntilButtonsReleased || _addingCharsAllowed;
            }
        }

        public bool AllReleased(KeyStates keyStates)
        {
            return !keyStates.Up && !keyStates.Down && !keyStates.Left && !keyStates.Fire;
        }

        public void AdvanceOneCycle(KeyStates keyStates)
        {
            ++_cycleCounter;

            if (_waitingForRelease && !AllReleased(keyStates)) return;
            _waitingForRelease = false;
            _sustainEditModeUntilButtonsReleased = false;

            if (keyStates.Left) RubOut();
            else if (keyStates.Up) IncrementChar(1);
            else if (keyStates.Down) IncrementChar(-1);
            else if (keyStates.Fire) NextChar();
            else return; // shouldn't be of course!

            _waitingForRelease = true;
        }

        public void Draw(IDrawingTarget drawingTarget, int x, int y)
        {
            drawingTarget.DrawText(x, y, _editString, _theFont, TextAlignment.Left);

            var nameLengthPixels = _theFont.WidthOf(_editString);
            if (_addingCharsAllowed && CursorVisible)
            {
                drawingTarget.DrawFirstSprite(x + nameLengthPixels, y, _cursorSprite);
            }
        }

        private void RubOut()
        {
            if (_editString.Length == 1)
            {
                _editString = " "; // reset to the start state
            }
            else
            {
                _editString = _editString.Substring(0, _editString.Length - 1);
            }
        }

        private void NextChar()
        {
            if (_editString.Length == _maxLength)
            {
                // Last char seen.  Editing done.
                _addingCharsAllowed = false;
                _sustainEditModeUntilButtonsReleased = true;
                _onFinalStringSet?.Invoke(_editString);
            }
            else
            {
                // Add another char
                if (_editString.Length == 0)
                {
                    _editString += NextCharString;
                }
                else
                {
                    _editString += _editString.Last();
                }
            }
        }

        private static char GetNextChar(char ch, int directionDelta)
        {
            var charIndex = CharToIndex(ch) + directionDelta;
            // NB: We use index -1 for SPACE, thus -1..35 is the range
            if (charIndex < -1) return 'Z'; // TODO: fix to be idealistic!
            if (charIndex > 25) return ' '; // TODO: fix to be idealistic!
            return IndexToChar(charIndex);
        }

        public static int CharToIndex(char ch)
        {
            if (ch == ' ') return -1;
            if (ch >= 'A' && ch <= 'Z') return ((int)ch) - 65;
            return -1;
        }

        public static char IndexToChar(int theIndex)
        {
            if (theIndex >= 0 && theIndex <= 25)
            {
                return (char)(theIndex + 65);
            }
            return ' ';
        }

        private void IncrementChar(int directionDelta)
        {
            // Up/Down tweaks the rightmost letter.
            var oldStr = _editString;
            _editString = oldStr.Substring(0, oldStr.Length - 1) + GetNextChar(oldStr.Last(), directionDelta);
        }

        private bool CursorVisible
        {
            get
            {
                var shifted = (_cycleCounter >> 2);
                var masked = shifted & 7;
                var bitpos = 1 << (int)masked;
                return (bitpos & 0xA8) != 0;
            }
        }

    }
}
