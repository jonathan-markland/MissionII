
namespace GameClassLibrary.Interactibles
{
    public class Potion : InteractibleObject
    {
        public Potion(int roomNumber)
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Potion }, roomNumber)
        {
        }

        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            theGameBoard.IncrementLives();
            RemoveThisObject(theGameBoard);
        }
    }
}
