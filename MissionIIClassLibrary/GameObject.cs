using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary
{
    public abstract class GameObject // TODO: Move into library AFTER IGameBoard is distilled
    {
        // Note - We avoid restricting a GameObject to be a *single* SpriteInstance.

            // TODO: Can these be abstract properties?
        public abstract Rectangle GetBoundingRectangle();
        public abstract Point TopLeftPosition { get; set; }
        public abstract bool CanBeOverlapped { get; }
        public abstract void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates);
        public abstract void Draw(IDrawingTarget drawingTarget);
        public abstract void ManWalkedIntoYou(IGameBoard theGameBoard);
        public abstract ShotStruct YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan);
        public virtual int KillScore { get { return 0; } }
        public virtual int CollectionScore { get { return 0; } }
    }
}
