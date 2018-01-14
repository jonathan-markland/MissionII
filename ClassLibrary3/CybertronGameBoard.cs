using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public class CybertronGameBoard
    {
        public bool IsPaused;
        public int BoardWidth;  // TODO: There are also constants that are used for this.
        public int BoardHeight; // TODO: There are also constants that are used for this.
        public int LevelNumber;
        public int RoomNumber; // one-based
        public uint Score;
        public uint Lives;
        public WorldWallData TheWorldWallData;
        public WallMatrix CurrentRoomWallData;
        public CybertronMan Man = new CybertronMan();
        public SuddenlyReplaceableList<CybertronGameObject> ObjectsInRoom = new SuddenlyReplaceableList<CybertronGameObject>();
        public List<CybertronGameObject> ObjectsToRemove = new List<CybertronGameObject>();
        public List<CybertronObject> PlayerInventory = new List<CybertronObject>();
        public CybertronKey Key;
        public CybertronRing Ring;
        public CybertronGold Gold;
        public CybertronLevelSafe Safe;
        public CybertronPotion Potion;
        public CybertronManPosition ManPositionOnRoomEntry;

        /// <summary>
        /// Returns true if any droids exist in the room.
        /// </summary>
        public bool DroidsExistInRoom
        {
            get
            {
                bool foundDroids = false;
                ObjectsInRoom.ForEachDo(o => 
                {
                    if (o is CybertronDroidBase)
                    {
                        foundDroids = true;
                    }
                });
                return foundDroids;
            }
        }



        public void ForEachThingWeHaveToFindOnThisLevel(Action<CybertronObject> theAction)
        {
            theAction(Key);
            if (LevelNumber > 1)
            {
                theAction(Ring);
            }
            if (LevelNumber > 2)
            {
                theAction(Gold);
            }
        }



        public void DrawBoardToTarget(
            IDrawingTarget drawingTarget)
        {
            // TODO: assumptions about rendering target dimensions here

            drawingTarget.ClearScreen();

            // Score:

            var theNumbers = CybertronSpriteTraits.TheNumbers;
            drawingTarget.DrawFirstSprite(0, 8, CybertronSpriteTraits.Score);
            drawingTarget.DrawNumber(140, 8, Score, theNumbers);

            // Level no, Room no:

            drawingTarget.DrawFirstSprite(210, 8, CybertronSpriteTraits.Room);
            drawingTarget.DrawNumber(
                CybertronGameBoardConstants.ScreenWidth, 8,
                (uint)(LevelNumber * 100 +
                RoomNumber), theNumbers);

            // The Room:

            var outlineWallSpriteTraits =
                (Man.IsBeingElectrocuted)
                ? CybertronSpriteTraits.WallElectric
                : CybertronSpriteTraits.WallBlock1;

            drawingTarget.DrawWalls(
                CybertronGameBoardConstants.RoomOriginX,
                CybertronGameBoardConstants.RoomOriginY,
                CybertronGameBoardConstants.TileWidth,
                CybertronGameBoardConstants.TileHeight,
                CurrentRoomWallData,
                outlineWallSpriteTraits,
                CybertronSpriteTraits.WallBlock2);

            // Draw objects in the room:

            ObjectsInRoom.ForEachDo(o => { o.Draw(this, drawingTarget); });

            // Lives:

            int y = CybertronGameBoardConstants.ScreenHeight - 16;
            drawingTarget.DrawRepeats(0, y, 8, 0, Math.Min(Lives, Constants.MaxDisplayedLives), CybertronSpriteTraits.Life);

            // Player inventory:

            int x = CybertronGameBoardConstants.ScreenWidth - 8;
            foreach (var carriedObject in PlayerInventory)
            {
                var spriteTraits = carriedObject.SpriteTraits;
                var spriteWidth = spriteTraits.BoardWidth;
                x -= spriteWidth;
                drawingTarget.DrawFirstSprite(x, y, spriteTraits);
                x -= Constants.InventoryItemSpacing;
            }
        }
    }
}
