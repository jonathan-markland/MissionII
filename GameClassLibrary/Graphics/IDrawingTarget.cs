﻿namespace GameClassLibrary.Graphics
{
    public interface IDrawingTarget
    {
        void ClearScreen();
        void DrawSprite(int x, int y, HostSuppliedSprite hostSuppliedSprite);
        void DrawSpritePieceStretched(int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh, HostSuppliedSprite hostSuppliedSprite);
    }
}
