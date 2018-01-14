namespace GameClassLibrary
{
    public static class IDrawingTargetExtensionsForCybertron
    {
        public static void DrawFirstSpriteScreenCentred(this IDrawingTarget drawingTarget, SpriteTraits theSprite)
        {
            drawingTarget.DrawFirstSpriteCentred(
                CybertronGameBoardConstants.ScreenWidth / 2,
                CybertronGameBoardConstants.ScreenHeight / 2,
                theSprite);
        }

        public static void DrawFirstSprite(this IDrawingTarget drawingTarget, SpriteInstance theSprite)
        {
            drawingTarget.DrawSprite(
                CybertronGameBoardConstants.RoomOriginX + theSprite.RoomX,
                CybertronGameBoardConstants.RoomOriginY + theSprite.RoomY,
                theSprite.Traits.GetHostImageObject(0));
        }

        public static void DrawIndexedSprite(this IDrawingTarget drawingTarget, SpriteInstance theSprite, int spriteIndex)
        {
            drawingTarget.DrawSprite(
                CybertronGameBoardConstants.RoomOriginX + theSprite.RoomX,
                CybertronGameBoardConstants.RoomOriginY + theSprite.RoomY,
                theSprite.Traits.GetHostImageObject(spriteIndex));
        }
    }
}
