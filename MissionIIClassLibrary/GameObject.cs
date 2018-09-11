
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary
{
    public abstract class GameObject<T>
    {
        public abstract void AdvanceOneCycle(T theGameBoard, KeyStates theKeyStates);
        public abstract void Draw(IDrawingTarget drawingTarget);
        public abstract Rectangle GetBoundingRectangle();
        public abstract void ManWalkedIntoYou(T theGameBoard);
        public abstract bool YouHaveBeenShot(T theGameBoard, bool shotByMan);
        public abstract Point TopLeftPosition { get; set; }
        public abstract bool CanBeOverlapped { get; }
    }
}
