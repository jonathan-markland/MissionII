
using GameClassLibrary.Math;

namespace GameClassLibrary.GameBoard
{
    public static class GameBoardExtensions
    {
        public struct BulletResult
        {
            public uint HitCount;
            public int TotalScoreIncrease;
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
                    var shotResult = o.YouHaveBeenShot(gameBoard, increasesScore);
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

            return new BulletResult { HitCount = hitCount, TotalScoreIncrease = scoreDelta };
        }
    }
}
