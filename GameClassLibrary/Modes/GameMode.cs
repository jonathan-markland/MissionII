using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Modes
{
    public class GameMode
    {
        /// <summary>
        /// The currently active game mode.
        /// </summary>
        public static GameMode ActiveMode;

        public virtual void AdvanceOneCycle(KeyStates theKeyStates)
        {
            // No base action
        }

        public virtual void Draw(IDrawingTarget drawingTarget)
        {
            // No base action
        }
    }
}
