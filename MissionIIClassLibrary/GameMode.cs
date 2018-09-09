using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary
{
    public class GameMode
    {
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
