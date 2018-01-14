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
            drawingTarget.DrawFirstSprite(0, 8, CybertronSpriteTraits.Score);
            drawingTarget.DrawNumber(140, 8, cybertronGameBoard.Score, theNumbers);

            // Level no, Room no:

            drawingTarget.DrawFirstSprite(210, 8, CybertronSpriteTraits.Room);
            drawingTarget.DrawNumber(320, 8,
                (uint)(cybertronGameBoard.LevelNumber * 100 +
                cybertronGameBoard.RoomNumber), theNumbers);

            // The Room:

            var outlineWallSpriteTraits =
                (cybertronGameBoard.Man.IsBeingElectrocuted)
                ? CybertronSpriteTraits.WallElectric
                : CybertronSpriteTraits.WallBlock1;

            DrawWalls(
                CybertronGameBoardConstants.RoomOriginX, 
                CybertronGameBoardConstants.RoomOriginY,
                CybertronGameBoardConstants.TileWidth,
                CybertronGameBoardConstants.TileHeight, 
                cybertronGameBoard.CurrentRoomWallData,
                outlineWallSpriteTraits,
                CybertronSpriteTraits.WallBlock2,
                drawingTarget);

            // Draw objects in the room:

            cybertronGameBoard.ObjectsInRoom.ForEachDo(o => { o.Draw(cybertronGameBoard, drawingTarget); });

            // Lives:

            int y = 256 - 16;
            drawingTarget.DrawRepeats(0, y, 8, 0, Math.Min(cybertronGameBoard.Lives, Constants.MaxDisplayedLives), CybertronSpriteTraits.Life);

            // Player inventory:

            int x = 320;
            foreach(var carriedObject in cybertronGameBoard.PlayerInventory)
            {
                var spriteTraits = carriedObject.SpriteTraits;
                var spriteWidth = spriteTraits.BoardWidth;
                x -= spriteWidth;
                drawingTarget.DrawFirstSprite(x, y, spriteTraits);
                x -= Constants.InventoryItemSpacing;
            }
        }



        public static void DrawWalls(
            int leftX, int topY, int tileWidth, int tileHeight, 
            WallMatrix wallData, 
            SpriteTraits outlineSpriteTraits,
            SpriteTraits brickSpriteTraits,
            IDrawingTarget drawingTarget)
        {
            for(int y=0; y < wallData.CountV; y++)
            {
                int a = leftX;
                for(int x=0; x < wallData.CountH; x++)
                {
                    var ch = wallData.Read(x, y);
                    if (ch == WallMatrixChar.Electric)
                    {
                        drawingTarget.DrawFirstSprite(leftX, topY, outlineSpriteTraits);
                    }
                    else if (ch != WallMatrixChar.Space)
                    {
                        drawingTarget.DrawFirstSprite(leftX, topY, brickSpriteTraits);
                    }
                    leftX += tileWidth;
                }
                leftX = a;
                topY += tileHeight;
            }
        }
    }
}
