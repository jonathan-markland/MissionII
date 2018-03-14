using System;

namespace MissionIIMonoGame
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
                // Intended for when modifying Levels.txt file.
                Console.WriteLine("Game failed to start because of error:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
