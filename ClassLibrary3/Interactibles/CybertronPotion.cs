
namespace GameClassLibrary.Interactibles
{
    public class CybertronPotion : CybertronObject
    {
        public CybertronPotion(int roomNumber)
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Potion }, roomNumber)
        {
        }

        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            CybertronGameStateUpdater.IncrementLives(theGameBoard);
            RemoveThisObject(theGameBoard);
        }
    }
}
