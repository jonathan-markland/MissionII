
namespace MissionIIClassLibrary.Modes
{
    public class GameOver : GameClassLibrary.Modes.GameOver
    {
        public GameOver(uint finalScore)
            : base(
                  Constants.GameOverMessageCycles,
                  MissionIISprites.GameOver,
                  MissionIISounds.GameOver,
                  () => new HiScore(finalScore))
        {
        }
    }
}
