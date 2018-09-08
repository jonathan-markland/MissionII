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

    }
}
