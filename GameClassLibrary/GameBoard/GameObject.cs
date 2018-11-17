using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.GameBoard
{
    public abstract class GameObject
    {
        // Note - We avoid restricting a GameObject to be a *single* SpriteInstance.

        public abstract Rectangle GetBoundingRectangle();
        public abstract Point TopLeftPosition { get; set; }
        public abstract void AdvanceOneCycle(KeyStates theKeyStates);
        public abstract void Draw(IDrawingTarget drawingTarget);
        public abstract ShotStruct YouHaveBeenShot(bool shotByMan);
    }
}
