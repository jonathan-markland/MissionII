
using GameClassLibrary.Math;

namespace GameClassLibrary.GameBoard
{
    public static class GameBoardExtensions
    {
        public struct BulletResult
        {
            public readonly uint HitCount;
			public readonly int TotalScoreIncrease;

			public BulletResult(uint hitCount, int totalScoreIncrease)
			{
				HitCount = hitCount;
				TotalScoreIncrease = totalScoreIncrease;
			}
        }



        public static BulletResult KillThingsInRectangle(
            this IGameBoard gameBoard,
            Rectangle bulletRectangle,
            bool increasesScore)
        {
            int scoreDelta = 0;
            uint hitCount = 0;

            gameBoard.ForEachObjectInPlayDo<GameObject>(o =>
            {
                if (o.GetBoundingRectangle().Intersects(bulletRectangle))
                {
                    var shotResult = o.YouHaveBeenShot(increasesScore);
                    if (shotResult.Affirmed)
                    {
                        if (increasesScore)
                        {
                            scoreDelta += shotResult.ScoreIncrease;
                        }
                        ++hitCount;
                    }
                }
            });

			return new BulletResult(hitCount, scoreDelta);
        }
    }
}
