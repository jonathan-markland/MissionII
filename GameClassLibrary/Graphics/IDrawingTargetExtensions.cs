
using System.Collections.Generic;
using GameClassLibrary.Math;

namespace GameClassLibrary.Graphics
{
    public static class IDrawingTargetExtensions
    {
        public static void DrawFirstSprite(this IDrawingTarget drawingTarget, SpriteInstance theSprite)
        {
            drawingTarget.DrawSprite(
                theSprite.X,
                theSprite.Y,
                theSprite.Traits.GetHostImageObject(0));
        }



        public static void DrawIndexedSprite(this IDrawingTarget drawingTarget, SpriteInstance theSprite, int spriteIndex)
        {
            drawingTarget.DrawSprite(
                theSprite.X,
                theSprite.Y,
                theSprite.Traits.GetHostImageObject(spriteIndex));
        }



        public static void DrawFirstSpriteCentred(
            this IDrawingTarget drawingTarget, int cx, int cy, SpriteTraits theSprite)
        {
            drawingTarget.DrawSprite(
                cx - theSprite.Width / 2,
                cy - theSprite.Height / 2,
                theSprite.GetHostImageObject(0));
        }


        public static void DrawFirstSpriteScreenCentred(
            this IDrawingTarget drawingTarget, SpriteTraits theSprite)
        {
            drawingTarget.DrawFirstSpriteCentred(Screen.Width / 2, Screen.Height / 2, theSprite);
        }



        public static void DrawFirstSprite(
            this IDrawingTarget drawingTarget, int x, int y, SpriteTraits theSprite)
        {
            drawingTarget.DrawSprite(x, y, theSprite.GetHostImageObject(0));
        }



        public static void DrawNumber(
            this IDrawingTarget drawingTarget, int rightSideX, int topSideY, uint theValue, 
            List<SpriteTraits> theFontSprites)
        {
            System.Diagnostics.Debug.Assert(theFontSprites.Count == 10);
            foreach (var spr in theFontSprites)
            {
                System.Diagnostics.Debug.Assert(spr.ImageCount == 1);
            }

            uint n = theValue;
            do
            {
                var thisDigit = n % 10;
                var thisSprite = theFontSprites[(int)thisDigit];
                rightSideX -= thisSprite.Width;
                drawingTarget.DrawFirstSprite(rightSideX, topSideY, thisSprite);
                n = n / 10;
            }
            while (n != 0);
        }



        public static void DrawText(
            this IDrawingTarget drawingTarget, int x, int topSideY, 
            string theText, Font theFont, TextAlignment textAlignment)
        {
            var destDeltaX = theFont.CharWidth * theFont.ScaleFactorX;
            var srcHeight = theFont.FontSprite.Height;
            var srcCharWidth = theFont.CharWidth;
            var destHeight = srcHeight * theFont.ScaleFactorY;
            var hostImageObject = theFont.FontSprite.GetHostImageObject(0);

            if (textAlignment != TextAlignment.Left)
            {
                var textWidth = theFont.WidthOf(theText);
                if (textAlignment == TextAlignment.Right)
                {
                    x -= textWidth;
                }
                else if (textAlignment == TextAlignment.Centre)
                {
                    x -= textWidth / 2;
                }
            }

            foreach (var ch in theText)
            {
                var charIndex = Font.CharToIndex(ch);
                if (charIndex >= 0)
                {
                    drawingTarget.DrawSpritePieceStretched(
                        charIndex * srcCharWidth, 0, srcCharWidth, srcHeight, 
                        x, topSideY, destDeltaX, destHeight,
                        hostImageObject);
                }
                x += destDeltaX;
            }
        }



        public static void DrawRepeats(
            this IDrawingTarget drawingTarget, int leftX, int topY, int deltaX, int deltaY, 
            uint repeatCount, SpriteTraits theSprite)
        {
            while (repeatCount > 0)
            {
                drawingTarget.DrawFirstSprite(leftX, topY, theSprite);
                leftX += deltaX;
                topY += deltaY;
                --repeatCount;
            }
        }



        public static void DrawTileMatrix(   // TODO: Should this allow huge tile arrays, thus require iterating over a portion only?
            this IDrawingTarget drawingTarget,
            int leftX, int topY, 
            Walls.TileMatrix tileMatrix,
            Point offsetIntoData,
            int renderWidth,
            int renderHeight,
            HostSuppliedSprite[] hostSpritesFortiles)
        {
            var tileWidth = tileMatrix.TileWidth;
            var tileHeight = tileMatrix.TileHeight;

            int startY = offsetIntoData.Y;
            var endY = startY + renderHeight;
            for (int y = startY; y < endY; y++)
            {
                int a = leftX;
                int startX = offsetIntoData.X;
                var endX = startX + renderWidth;
                for (int x = startX; x < endX; x++)
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
