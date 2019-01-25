using System;

namespace MissionII
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                using (var game = new GameMain())
                {
                    game.Run();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Game failed because of error:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
