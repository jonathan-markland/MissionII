
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public static class GameOver
    {
        public static ModeFunctions New(
            uint finalScore)
        {
            return PlacardScreen.New(
                  Constants.GameOverMessageCycles,
                  MissionIISprites.Background2,
                  MissionIIFonts.GiantFont, "GAME OVER",
                  MissionIISounds.GameOver,
                  () =>
                  {
                      if (finalScore > 0
                          && GameClassLibrary.Modes.HiScoreEntry.HiScoreTableModel.CanPlayerEnterTable(finalScore))
                      {
                          return HiScoreEntry.New(finalScore);
                      }
                      return HiScoreShow.New();
                  });
        }
    }
}
