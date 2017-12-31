using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public abstract class GameObject<T, K>
    {
        public abstract void AdvanceOneCycle(T theGameBoard, K theKeyStates);
        public abstract void Draw(T theGameBoard, IDrawingTarget drawingTarget);
        public abstract Rectangle GetBoundingRectangle();
        public abstract void ManWalkedIntoYou(T theGameBoard);
        public abstract bool YouHaveBeenShot(T theGameBoard);
        public abstract Point TopLeftPosition { get; set; }
        public abstract bool IsSolid { get; }
    }
}
