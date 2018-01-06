
namespace GameClassLibrary
{
    public class CybertronLevelSafe : CybertronObject
    {
        public CybertronLevelSafe(int roomNumber) 
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Safe }, roomNumber)
        {
        }


        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
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
                var thisLevelNumber = theGameBoard.LevelNumber;
                ++thisLevelNumber;
                theGameBoard.LevelNumber = thisLevelNumber;
                GameClassLibrary.CybertronGameStateUpdater.PrepareForNewLevel(theGameBoard);
            }
        }
    }
}
