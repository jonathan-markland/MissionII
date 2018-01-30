namespace MissionIIClassLibrary
{
    public static class IDrawingTargetExtensionsForCybertron
    {
        public static void DrawFirstSpriteScreenCentred(this IDrawingTarget drawingTarget, SpriteTraits theSprite)
        {
            drawingTarget.DrawFirstSpriteCentred(
                MissionIIGameBoardConstants.ScreenWidth / 2,
                MissionIIGameBoardConstants.ScreenHeight / 2,
                theSprite);
        }

        public static void DrawFirstSprite(this IDrawingTarget drawingTarget, SpriteInstance theSprite)
        {
            drawingTarget.DrawSprite(
                MissionIIGameBoardConstants.RoomOriginX + theSprite.RoomX,
                MissionIIGameBoardConstants.RoomOriginY + theSprite.RoomY,
                theSprite.Traits.GetHostImageObject(0));
        }

        public static void DrawIndexedSprite(this IDrawingTarget drawingTarget, SpriteInstance theSprite, int spriteIndex)
        {
            drawingTarget.DrawSprite(
                MissionIIGameBoardConstants.RoomOriginX + theSprite.RoomX,
                MissionIIGameBoardConstants.RoomOriginY + theSprite.RoomY,
                theSprite.Traits.GetHostImageObject(spriteIndex));
        }

        public static void DrawWalls(
            this IDrawingTarget drawingTarget,
            int levelNumber,
            int leftX, int topY, int tileWidth, int tileHeight,
            WallMatrix wallData,
            SpriteTraits outlineSpriteTraits,
            SpriteTraits brickSpriteTraits,
            SpriteTraits floorSpriteTraits)
        {
            --levelNumber; // because it's 1-based!

            // TODO: the following are hacks.  We want to do the re-colouring idea.
            // TODO: We also want to have the bricks as 'views' onto a seamlessly
            //       tiled recoloured background.

            var outlineHostSprite = new object[]
            {
                outlineSpriteTraits.GetHostImageObject(levelNumber % outlineSpriteTraits.ImageCount),
                outlineSpriteTraits.GetHostImageObject((levelNumber + 1) % outlineSpriteTraits.ImageCount)
            };

            var brickHostSprite = new object[]
            {
                brickSpriteTraits.GetHostImageObject(levelNumber % brickSpriteTraits.ImageCount),
                brickSpriteTraits.GetHostImageObject((levelNumber + 1) % brickSpriteTraits.ImageCount)
            };

            var floorHostSprite = new object[]
            {
                floorSpriteTraits.GetHostImageObject(levelNumber % floorSpriteTraits.ImageCount),
                floorSpriteTraits.GetHostImageObject((levelNumber + 1) % floorSpriteTraits.ImageCount)
            };

            for (int y = 0; y < wallData.CountV; y++)
            {
                int a = leftX;
                for (int x = 0; x < wallData.CountH; x++)
                {
                    var ch = wallData.Read(x, y);
                    var styleDelta = wallData.GetStyleDelta(x, y);
                    if (ch == WallMatrixChar.Electric) // <-- confusing that this really means draw the wall in either normal or electric state
                    {
                        drawingTarget.DrawSprite(leftX, topY, outlineHostSprite[styleDelta]);
                    }
                    else if (ch != WallMatrixChar.Space)
                    {
                        drawingTarget.DrawSprite(leftX, topY, brickHostSprite[styleDelta]);
                    }
                    else
                    {
                        drawingTarget.DrawSprite(leftX, topY, floorHostSprite[styleDelta]);
                    }
                    leftX += tileWidth;
                }
                leftX = a;
                topY += tileHeight;
            }
        }
    }
}
