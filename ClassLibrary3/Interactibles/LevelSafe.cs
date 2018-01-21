
namespace GameClassLibrary.Interactibles
{
    public class LevelSafe : InteractibleObject
    {
        public LevelSafe(int roomNumber) 
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
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronLeavingLevelMode(theGameBoard);
                CybertronSounds.Play(CybertronSounds.SafeActivated);
            }
        }
    }
}
