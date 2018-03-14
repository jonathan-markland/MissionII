using System;
using System.Collections.Generic;

namespace GameClassLibrary.Hiscore
{
    public class HiScoreScreenModel
    {
        private List<HiScoreTableEntryModel> _scoreTable;

        public HiScoreScreenModel(uint lowestScore, uint scoreIncrement)
        {
            _scoreTable = new List<HiScoreTableEntryModel>();
            _scoreTable.Add(new HiScoreTableEntryModel("JUAN", lowestScore + 4 * scoreIncrement));
            _scoreTable.Add(new HiScoreTableEntryModel("DIMITAR", lowestScore + 3 * scoreIncrement));
            _scoreTable.Add(new HiScoreTableEntryModel("LAURA", lowestScore + 2 * scoreIncrement));
            _scoreTable.Add(new HiScoreTableEntryModel("NICO", lowestScore + 1 * scoreIncrement));
            _scoreTable.Add(new HiScoreTableEntryModel("JANE", lowestScore));
            SortScoreTable();
        }

        public int NumPlaces
        {
            get { return _scoreTable.Count; }
        }

        public string GetNameAt(int i)
        {
            return _scoreTable[i].Name;
        }

        public void SetNameAt(int i, string newString)
        {
            _scoreTable[i].Name = newString;
        }

        public string GetScoreStringAt(int i)
        {
            return _scoreTable[i].ScoreString;
        }

        public bool CanPlayerEnterTable(uint scoreObtained)
        {
            // The table is always sorted!
            return scoreObtained > _scoreTable[_scoreTable.Count - 1].Score;
        }

        public int ForceEnterScore(uint scoreObtained)
        {
            var tableRow = _scoreTable[_scoreTable.Count - 1];
            tableRow.Name = String.Empty; // filled on events
            tableRow.Score = scoreObtained;
            SortScoreTable();
            return _scoreTable.IndexOf(tableRow);
        }

        private void SortScoreTable()
        {
            _scoreTable.Sort((x, y) => (x.Score < y.Score) ? 1 : ((x.Score == y.Score) ? 0 : -1));
        }
    }
}
