using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class LevelExit : InteractibleObject
    {
        public LevelExit(int roomNumber) 
            : base(new SpriteInstance { Traits = MissionIISprites.LevelExit }, roomNumber)
        {
        }

        public override int CollectionScore => 0;

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            bool carryingEverything = true;
            theGameBoard.ForEachThingWeHaveToFindOnThisLevel(o => 
            {
                if (!theGameBoard.PlayerInventory.Contains(o))
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
