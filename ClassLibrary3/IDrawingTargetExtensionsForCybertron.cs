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

        public static void DrawWalls(
            this IDrawingTarget drawingTarget,
            int levelNumber,
            int leftX, int topY, int tileWidth, int tileHeight,
            WallMatrix wallData,
            SpriteTraits outlineSpriteTraits,
            SpriteTraits brickSpriteTraits)
        {
            --levelNumber; // because it's 1-based!
            var outlineIndex = outlineSpriteTraits.GetHostImageObject(levelNumber % outlineSpriteTraits.ImageCount);
            var brickIndex = brickSpriteTraits.GetHostImageObject(levelNumber % brickSpriteTraits.ImageCount);

            for (int y = 0; y < wallData.CountV; y++)
            {
                int a = leftX;
                for (int x = 0; x < wallData.CountH; x++)
                {
                    var ch = wallData.Read(x, y);
                    if (ch == WallMatrixChar.Electric) // <-- confusing that this really means draw the wall in either normal or electric state
                    {
                        drawingTarget.DrawSprite(leftX, topY, outlineIndex);
                    }
                    else if (ch != WallMatrixChar.Space)
                    {
                        drawingTarget.DrawSprite(leftX, topY, brickIndex);
                    }
                    leftX += tileWidth;
                }
                leftX = a;
                topY += tileHeight;
            }
        }
    }
}
