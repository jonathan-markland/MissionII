
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
            // TODO:  Consider how to move to the next level when the man is carrying all the objects necessary.
        }
    }
}
