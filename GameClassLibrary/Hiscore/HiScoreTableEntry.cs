

namespace GameClassLibrary.Hiscore
{
    public class HiScoreTableEntry
    {
        private uint _score;

        public string Name { get; set; }

        public string ScoreString { get; private set; }

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

        public HiScoreTableEntry(string playerName, uint playerScore)
        {
            Name = playerName;
            Score = playerScore;
        }
    }
}
