
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public abstract class GameObject<T, K>
    {
        public abstract void AdvanceOneCycle(T theGameBoard, K theKeyStates);
        public abstract void Draw(T theGameBoard, IDrawingTarget drawingTarget);
        public abstract Rectangle GetBoundingRectangle();
        public abstract void ManWalkedIntoYou(T theGameBoard);
        public abstract bool YouHaveBeenShot(T theGameBoard, bool shotByMan);
        public abstract Point TopLeftPosition { get; set; }
        public abstract bool CanBeOverlapped { get; }
    }
}
