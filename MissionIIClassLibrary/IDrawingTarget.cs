namespace MissionIIClassLibrary
{
    public interface IDrawingTarget
    {
        void ClearScreen();
        void DrawSprite(int x, int y, object hostImageObject);
        void DrawSpritePieceStretched(int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh, object hostImageObject);
    }
}
