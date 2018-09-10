using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public static class IDrawingTargetExtensionsForMissionII
    {

        public static void DrawFirstSpriteRoomRelative(this IDrawingTarget drawingTarget, SpriteInstance theSprite)
        {
            drawingTarget.DrawSprite(
                Constants.RoomOriginX + theSprite.X, // TODO: Should not need to add origin.
                Constants.RoomOriginY + theSprite.Y, // TODO: Should not need to add origin.
                theSprite.Traits.GetHostImageObject(0));
        }

        public static void DrawIndexedSpriteRoomRelative(this IDrawingTarget drawingTarget, SpriteInstance theSprite, int spriteIndex)
        {
            drawingTarget.DrawSprite(
                Constants.RoomOriginX + theSprite.X, // TODO: Should not need to add origin.
                Constants.RoomOriginY + theSprite.Y, // TODO: Should not need to add origin.
                theSprite.Traits.GetHostImageObject(spriteIndex));
        }

    }
}
