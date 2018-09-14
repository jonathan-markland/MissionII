using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class LevelExit : MissionIIInteractibleObject
    {
        public LevelExit(int roomNumber) 
            : base(new SpriteInstance { Traits = MissionIISprites.LevelExit }, roomNumber)
        {
        }

        public override int CollectionScore => 0;

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            bool carryingEverything = true;
            ((MissionIIGameBoard) theGameBoard).ForEachThingWeHaveToFindOnThisLevel(o => 
            {
                if (!theGameBoard.PlayerInventoryContains(o))
                {
                    carryingEverything = false;
                }
            });
            if (carryingEverything)
            {
                GameClassLibrary.Modes.GameMode.ActiveMode = new Modes.LeavingLevel(theGameBoard);
            }
        }
    }
}
