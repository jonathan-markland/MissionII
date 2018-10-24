using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.GameBoard
{
    public abstract class GameObject // TODO: Move into library AFTER IGameBoard is distilled
    {
        // Note - We avoid restricting a GameObject to be a *single* SpriteInstance.

        public abstract Rectangle GetBoundingRectangle();
        public abstract Point TopLeftPosition { get; set; }
        public abstract bool CanBeOverlapped { get; }
        public abstract void AdvanceOneCycle(KeyStates theKeyStates);
        public abstract void Draw(IDrawingTarget drawingTarget);
        public abstract void ManWalkedIntoYou();
        public abstract ShotStruct YouHaveBeenShot(bool shotByMan);
        public virtual int KillScore { get { return 0; } }
        public virtual int CollectionScore { get { return 0; } }
    }
}
