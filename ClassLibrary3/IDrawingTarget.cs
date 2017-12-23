
namespace GameClassLibrary
{
    public interface IDrawingTarget
    {
        void ClearScreen();
        void DrawSprite(int x, int y, object hostImageObject);
    }
}
