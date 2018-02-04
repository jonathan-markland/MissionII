
using System.Collections.Generic;

namespace GameClassLibrary.Graphics
{
    public static class IDrawingTargetExtensions
    {
        public static void DrawFirstSpriteCentred(this IDrawingTarget drawingTarget, int cx, int cy, SpriteTraits theSprite)
        {
            drawingTarget.DrawSprite(
                cx - theSprite.BoardWidth / 2,
                cy - theSprite.BoardHeight / 2,
                theSprite.GetHostImageObject(0));
        }

        public static void DrawFirstSprite(this IDrawingTarget drawingTarget, int x, int y, SpriteTraits theSprite)
        {
            drawingTarget.DrawSprite(x, y, theSprite.GetHostImageObject(0));
        }

        public static void DrawNumber(this IDrawingTarget drawingTarget, int rightSideX, int topSideY, uint theValue, List<SpriteTraits> theFontSprites)
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
                rightSideX -= thisSprite.BoardWidth;
                drawingTarget.DrawFirstSprite(rightSideX, topSideY, thisSprite);
                n = n / 10;
            }
            while (n != 0);
        }

        public static void DrawText(this IDrawingTarget drawingTarget, int leftSideX, int topSideY, string theText, Font theFont)
        {
            var destDeltaX = theFont.CharWidth * theFont.ScaleFactor;
            var srcHeight = theFont.FontSprite.BoardHeight;
            var srcCharWidth = theFont.CharWidth;
            var destHeight = srcHeight * theFont.ScaleFactor;
            var hostImageObject = theFont.FontSprite.GetHostImageObject(0);

            foreach (var ch in theText)
            {
                var charIndex = theFont.CharToIndex(ch);
                if (charIndex > 0)
                {
                    drawingTarget.DrawSpritePieceStretched(
                        charIndex * srcCharWidth, 0, srcCharWidth, srcHeight, 
                        leftSideX, topSideY, destDeltaX, destHeight,
                        hostImageObject);
                }
                leftSideX += destDeltaX;
            }
        }

        public static void DrawRepeats(this IDrawingTarget drawingTarget, int leftX, int topY, int deltaX, int deltaY, uint repeatCount, SpriteTraits theSprite)
        {
            while (repeatCount > 0)
            {
                drawingTarget.DrawFirstSprite(leftX, topY, theSprite);
                leftX += deltaX;
                topY += deltaY;
                --repeatCount;
            }
        }
    }
}
