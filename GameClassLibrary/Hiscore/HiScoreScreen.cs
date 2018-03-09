using GameClassLibrary.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace GameClassLibrary.Hiscore
{
    public class HiScoreScreen
    {
        private List<HiScoreTableEntry> _scoreTable;
        private const int NumPlaces = 5;
        private const int MaxNameLength = 10;
        private HiScoreScreenDimensions _hiScoreScreenDimensions;
        private bool _waitingForRelease;
        private uint _cycleCounter;
        private Font _theFont;
        private bool _sustainEditModeUntilButtonsReleased;

        public HiScoreScreen(HiScoreScreenDimensions hiScoreScreenDimensions, Font theFont)
        {
            _theFont = theFont;
            _waitingForRelease = true;
            _hiScoreScreenDimensions = hiScoreScreenDimensions;
            _scoreTable = new List<HiScoreTableEntry>();
            _scoreTable.Add(new HiScoreTableEntry("JONATHAN", 100000));
            _scoreTable.Add(new HiScoreTableEntry("IAN", 80000));
            _scoreTable.Add(new HiScoreTableEntry("FIDELIS", 60000));
            _scoreTable.Add(new HiScoreTableEntry("NAEEM", 40000));
            _scoreTable.Add(new HiScoreTableEntry("BOB", 2000));
            _sustainEditModeUntilButtonsReleased = false;
        }

        public bool CanPlayerEnterTable(uint scoreObtained)
        {
            return scoreObtained > _scoreTable[NumPlaces - 1].Score;
        }

        public void ForceEnterScore(uint scoreObtained)
        {
            var tableRow = _scoreTable[NumPlaces - 1];
            tableRow.Name = " "; // required for IncrementChar() / DecrementChar()!
            tableRow.Score = scoreObtained;
            tableRow.EditMode = true;
            _scoreTable.Sort((x,y) => (x.Score < y.Score) ? 1 : ((x.Score == y.Score) ? 0 : -1));
            _waitingForRelease = true; // opportunistic.
            _cycleCounter = 0;
        }

        public bool InEditMode
        {
            get
            {
                return _sustainEditModeUntilButtonsReleased || _scoreTable.Any(x => x.EditMode);
            }
        }

        public void AdvanceOneCycle(HiScoreTableKeyStates keyStates)
        {
            ++_cycleCounter;

            if (_waitingForRelease && !keyStates.AllReleased) return;
            _waitingForRelease = false;
            _sustainEditModeUntilButtonsReleased = false;

            // We don't store which of the entries is in edit mode.

            foreach (var tableEntry in _scoreTable)
            {
                if (!tableEntry.EditMode) continue;
                if (keyStates.Left) RubOut(tableEntry);
                else if (keyStates.Up) IncrementChar(tableEntry, 1);
                else if (keyStates.Down) IncrementChar(tableEntry, -1);
                else if (keyStates.Fire) NextChar(tableEntry);
                else continue; // shouldn't be of course!
                _waitingForRelease = true;
            }
        }

        private void RubOut(HiScoreTableEntry tableEntry)
        {
            if (tableEntry.Name.Length == 1)
            {
                tableEntry.Name = " "; // reset to the start state
            }
            else
            {
                tableEntry.Name = tableEntry.Name.Substring(0, tableEntry.Name.Length - 1);
            }
        }

        private void NextChar(HiScoreTableEntry tableEntry)
        {
            if (tableEntry.Name.Length == MaxNameLength)
            {
                // Last char seen.  Editing done.
                tableEntry.EditMode = false;
                _sustainEditModeUntilButtonsReleased = true;
            }
            else
            {
                tableEntry.Name = tableEntry.Name + " "; // move onto next char, defaulting to space.
            }
        }

        private static char GetNextChar(char ch, int directionDelta)
        {
            var charIndex = Font.CharToIndex(ch) + directionDelta;
            // NB: We use index -1 for SPACE, thus -1..35 is the range
            if (charIndex < -1) return 'Z'; // TODO: fix to be idealistic!
            if (charIndex > 35) return ' '; // TODO: fix to be idealistic!
            return Font.IndexToChar(charIndex);
        }

        private void IncrementChar(HiScoreTableEntry tableEntry, int directionDelta)
        {
            var oldStr = tableEntry.Name;
            tableEntry.Name = oldStr.Substring(0, oldStr.Length - 1) + GetNextChar(oldStr.Last(), directionDelta);
        }

        /// <summary>
        /// Draw the score table.
        /// Intended to be called as part of a greater drawing routine.
        /// </summary>
        public void DrawScreen(IDrawingTarget drawingTarget)
        {
            var nx = _hiScoreScreenDimensions.NamesLeftX;
            var sx = _hiScoreScreenDimensions.ScoresRightX;
            var y = _hiScoreScreenDimensions.TopEdgeY;
            var rowSpacing = ((_hiScoreScreenDimensions.BottomEdgeY - y) - _theFont.Height) / (NumPlaces - 1);
            foreach(var tableEntry in _scoreTable)
            {
                drawingTarget.DrawText(nx, y, tableEntry.Name, _theFont);
                drawingTarget.DrawText(sx - 100, y, tableEntry.ScoreString, _theFont); // TODO: right align and remove "- 100"
                y += rowSpacing;
            }
        }
    }
}
