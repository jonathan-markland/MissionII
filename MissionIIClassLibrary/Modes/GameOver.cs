
namespace MissionIIClassLibrary.Modes
{
    public class GameOver : GameClassLibrary.Modes.PlacardScreen
    {
        public GameOver(uint finalScore)
            : base(
                  Constants.GameOverMessageCycles,
                  MissionIISprites.GameOver,
                  MissionIISounds.GameOver,
                  () =>
                  {
                      if(finalScore > 0 
                          && GameClassLibrary.Modes.HiScoreEntry.HiScoreTableModel.CanPlayerEnterTable(finalScore))
                      {
                          return new HiScoreEntry(finalScore);
                      }
                      return new HiScoreShow();
                  })
        {
        }
    }
}
