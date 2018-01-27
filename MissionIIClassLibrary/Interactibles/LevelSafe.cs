
namespace MissionIIClassLibrary.Interactibles
{
    public class LevelSafe : InteractibleObject
    {
        public LevelSafe(int roomNumber) 
            : base(new SpriteInstance { Traits = MissionIISpriteTraits.Safe }, roomNumber)
        {
        }

        public override int CollectionScore => 0;

        public override void ManWalkedIntoYou(MissionIIGameBoard theGameBoard)
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
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new CybertronLeavingLevelMode(theGameBoard);
                MissionIISounds.Play(MissionIISounds.SafeActivated);
            }
        }
    }
}
