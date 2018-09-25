
namespace GameClassLibrary.GameBoard
{
    public struct ShotStruct
    {
        public readonly bool Affirmed;
		public readonly int ScoreIncrease;

		public ShotStruct(bool affirmed)
        {
            Affirmed = affirmed;
            ScoreIncrease = 0;
        }

		public ShotStruct(bool affirmed, int scoreIncrease)
		{
			Affirmed = affirmed;
			ScoreIncrease = scoreIncrease;
		}
    }
}
