﻿using GameClassLibrary.Walls;
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

        public static void DrawWalls(
            this IDrawingTarget drawingTarget,
            int leftX, int topY, int tileWidth, int tileHeight,
            TileMatrix wallData, // TODO: Don't really want to re-map this on the fly.  Pre-calculate.
            WallAndFloorHostSprites hostSprites) // TODO: pass array of sprite indices
        {
            for (int y = 0; y < wallData.CountV; y++)
            {
                int a = leftX;
                for (int x = 0; x < wallData.CountH; x++)
                {
                    var ch = wallData.TileAt(x, y);
                    var styleDelta = wallData.GetStyleDelta(x, y); // TODO: Don't want this here.
                    if (ch == MissionIITile.Electric) // <-- confusing that this really means draw the wall in either normal or electric state
                    {
                        drawingTarget.DrawSprite(leftX, topY, hostSprites.OutlineBricks[styleDelta]);
                    }
                    else if (!ch.IsSpace())
                    {
                        drawingTarget.DrawSprite(leftX, topY, hostSprites.FillerBricks[styleDelta]);
                    }
                    else
                    {
                        drawingTarget.DrawSprite(leftX, topY, hostSprites.FloorBricks[styleDelta]);
                    }
                    leftX += tileWidth;
                }
                leftX = a;
                topY += tileHeight;
            }
        }
    }
}
