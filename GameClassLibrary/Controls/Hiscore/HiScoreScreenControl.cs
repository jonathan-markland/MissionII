
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Controls.Hiscore
{
    public class HiScoreScreenControl
    {
        private const int MaxNameLength = 10;

        private HiScoreScreenModel _theModel;
        private Math.Rectangle _hiScoreScreenDimensions;
        private Controls.NameEntryControl _nameEntryControl;
        private Font _theFont;
        private int _editedRowIndex;


        public HiScoreScreenControl(
            Math.Rectangle hiScoreScreenDimensions, 
            Font theFont, 
            SpriteTraits cursorSprite, 
            HiScoreScreenModel theModel)
        {
            _theModel = theModel;
            _theFont = theFont;
            _nameEntryControl = new Controls.NameEntryControl(theFont, cursorSprite, MaxNameLength);
            _hiScoreScreenDimensions = hiScoreScreenDimensions;
            _editedRowIndex = -1;
        }

        public void ForceEnterScore(uint scoreObtained)
        {
            _editedRowIndex = _theModel.ForceEnterScore(scoreObtained);
            _nameEntryControl.Start("A", newString => { _theModel.SetNameAt(_editedRowIndex, newString); });
        }

        public bool InEditMode
        {
            get
            {
                return _nameEntryControl.InEditMode;
            }
        }

        public void AdvanceOneCycle(KeyStates keyStates)
        {
            _nameEntryControl.AdvanceOneCycle(keyStates);
        }

        public void DrawScreen(IDrawingTarget drawingTarget)
        {
            var nx = _hiScoreScreenDimensions.Left;
            var sx = _hiScoreScreenDimensions.Right;
            var y = _hiScoreScreenDimensions.Top;
            var n = _theModel.NumPlaces;
            var rowSpacing = ((_hiScoreScreenDimensions.Bottom - y) - _theFont.Height) / (n - 1);

            for (int i=0; i<n; i++)
            {
                if (i != _editedRowIndex)
                {
                    drawingTarget.DrawText(nx, y, _theModel.GetNameAt(i), _theFont, TextAlignment.Left);
                }
                else
                {
                    _nameEntryControl.Draw(drawingTarget, nx, y);
                }

                drawingTarget.DrawText(sx, y, _theModel.GetScoreStringAt(i), _theFont, TextAlignment.Right);

                y += rowSpacing;
            }
        }
    }
}
