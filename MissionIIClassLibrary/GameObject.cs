
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary
{
    public interface IGameBoard
    {
        
    }

    public abstract class GameObject
    {
        public abstract void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates);
        public abstract void Draw(IDrawingTarget drawingTarget);
        public abstract Rectangle GetBoundingRectangle();
        public abstract void ManWalkedIntoYou(IGameBoard theGameBoard);
        public abstract bool YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan);
        public abstract Point TopLeftPosition { get; set; }
        public abstract bool CanBeOverlapped { get; }
    }
}
