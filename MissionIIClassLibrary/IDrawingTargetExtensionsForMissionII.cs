using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public static class IDrawingTargetExtensionsForMissionII
    {
        public static void DrawFirstSpriteScreenCentred(this IDrawingTarget drawingTarget, SpriteTraits theSprite)
        {
            drawingTarget.DrawFirstSpriteCentred(
                Constants.ScreenWidth / 2,
                Constants.ScreenHeight / 2,
                theSprite);
        }

        public static void DrawFirstSprite(this IDrawingTarget drawingTarget, SpriteInstance theSprite)
        {
            drawingTarget.DrawSprite(
                Constants.RoomOriginX + theSprite.X, // TODO: Should not need to add origin.
                Constants.RoomOriginY + theSprite.Y, // TODO: Should not need to add origin.
                theSprite.Traits.GetHostImageObject(0));
        }

        public static void DrawIndexedSprite(this IDrawingTarget drawingTarget, SpriteInstance theSprite, int spriteIndex)
        {
            drawingTarget.DrawSprite(
                Constants.RoomOriginX + theSprite.X, // TODO: Should not need to add origin.
                Constants.RoomOriginY + theSprite.Y, // TODO: Should not need to add origin.
                theSprite.Traits.GetHostImageObject(spriteIndex));
        }

        public static void DrawTileMatrix(
            this IDrawingTarget drawingTarget,
            int leftX, int topY, int tileWidth, int tileHeight,
            TileMatrix tileMatrix,
            HostSuppliedSprite[] hostSpritesFortiles)
        {
            for (int y = 0; y < tileMatrix.CountV; y++)
            {
                int a = leftX;
                for (int x = 0; x < tileMatrix.CountH; x++)
                {
                    var t = tileMatrix.TileAt(x, y).VisualIndex;
                    drawingTarget.DrawSprite(leftX, topY, hostSpritesFortiles[t]);
                    leftX += tileWidth;
                }
                leftX = a;
                topY += tileHeight;
            }
        }
    }
}
