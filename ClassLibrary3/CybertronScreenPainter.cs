using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public static class CybertronScreenPainter
    {
        public static void DrawBoardToTarget(
            CybertronGameBoard cybertronGameBoard, 
            IDrawingTarget drawingTarget)
        {
            // TODO: assumptions about rendering target dimensions here

            drawingTarget.ClearScreen();

            // Score:

            var theNumbers = CybertronSpriteTraits.TheNumbers;
            DrawFirstSprite(0, 8, CybertronSpriteTraits.Score, drawingTarget);
            DrawNumber(140, 8, cybertronGameBoard.Score, theNumbers, drawingTarget);

            // Level no, Room no:

            DrawFirstSprite(210, 8, CybertronSpriteTraits.Room, drawingTarget);
            DrawNumber(320, 8,
                (uint)(cybertronGameBoard.LevelNumber * 100 +
                cybertronGameBoard.RoomNumber), theNumbers, drawingTarget);

            // The Room:

            DrawWalls(
                CybertronGameBoardConstants.RoomOriginX, 
                CybertronGameBoardConstants.RoomOriginY,
                CybertronGameBoardConstants.TileWidth,
                CybertronGameBoardConstants.TileHeight, 
                cybertronGameBoard.CurrentRoomWallData, drawingTarget);

            // Draw objects in the room:

            cybertronGameBoard.ForEachDo(o => { o.Draw(cybertronGameBoard, drawingTarget); return true; });

            // Lives:

            int y = 256 - 16;
            DrawRepeats(0, y, 8, 0, Math.Min(cybertronGameBoard.Lives, Constants.MaxDisplayedLives), CybertronSpriteTraits.Life, drawingTarget);

            // Player inventory:

            int x = 320;
            foreach(var carriedObject in cybertronGameBoard.PlayerInventory)
            {
                var spriteTraits = carriedObject.SpriteTraits;
                var spriteWidth = spriteTraits.BoardWidth;
                x -= spriteWidth;
                DrawFirstSprite(x, y, spriteTraits, drawingTarget);
                x -= Constants.InventoryItemSpacing;
            }
        }

        public static void DrawFirstSprite(int x, int y, SpriteTraits theSprite, IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawSprite(x, y, theSprite.HostImageObjects[0]);
        }

        public static void DrawFirstSprite(SpriteInstance theSprite, IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawSprite(
                CybertronGameBoardConstants.RoomOriginX + theSprite.RoomX,
                CybertronGameBoardConstants.RoomOriginY + theSprite.RoomY, 
                theSprite.Traits.HostImageObjects[0]);
        }

        public static void DrawIndexedSprite(SpriteInstance theSprite, int spriteIndex, IDrawingTarget drawingTarget)
        {
            drawingTarget.DrawSprite(
                CybertronGameBoardConstants.RoomOriginX + theSprite.RoomX,
                CybertronGameBoardConstants.RoomOriginY + theSprite.RoomY,
                theSprite.Traits.HostImageObjects[spriteIndex]);
        }


        public static void DrawWalls(int leftX, int topY, int tileWidth, int tileHeight, WallMatrix wallData, IDrawingTarget drawingTarget)
        {
            for(int y=0; y < wallData.CountV; y++)
            {
                int a = leftX;
                for(int x=0; x < wallData.CountH; x++)
                {
                    var ch = wallData.Read(x, y);
                    if (ch.Wall)
                    {
                        DrawFirstSprite(leftX, topY, CybertronSpriteTraits.WallBlock, drawingTarget);
                    }
                    leftX += tileWidth;
                }
                leftX = a;
                topY += tileHeight;
            }
        }



        public static void DrawNumber(int rightSideX, int topSideY, uint theValue, List<SpriteTraits> theFontSprites, IDrawingTarget drawingTarget)
        {
            System.Diagnostics.Debug.Assert(theFontSprites.Count == 10);
            foreach(var spr in theFontSprites)
            {
                System.Diagnostics.Debug.Assert(spr.HostImageObjects.Count == 1);
            }

            uint n = theValue;
            do
            {
                var thisDigit = n % 10;
                var thisSprite = theFontSprites[(int)thisDigit];
                rightSideX -= thisSprite.BoardWidth;
                DrawFirstSprite(rightSideX, topSideY, thisSprite, drawingTarget);
                n = n / 10;
            }
            while (n != 0);
        }



        public static void DrawRepeats(int leftX, int topY, int deltaX, int deltaY, uint repeatCount, SpriteTraits theSprite, IDrawingTarget drawingTarget)
        {
            while(repeatCount > 0)
            {
                DrawFirstSprite(leftX, topY, theSprite, drawingTarget);
                leftX += deltaX;
                topY += deltaY;
                --repeatCount;
            }
        }



    }
}
