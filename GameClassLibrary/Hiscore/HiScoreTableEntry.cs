

namespace GameClassLibrary.Hiscore
{
    public class HiScoreTableEntry
    {
        private uint _score;

        public string Name { get; set; }

        public uint Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                ScoreString = value.ToString(); // garbage reduction
            }
        }

        public bool EditMode { get; set; }

        public string ScoreString { get; private set; }

        public HiScoreTableEntry(string playerName, uint playerScore)
        {
            this.Name = playerName;
            this.Score = playerScore;
            this.EditMode = false;
        }
    }
}
