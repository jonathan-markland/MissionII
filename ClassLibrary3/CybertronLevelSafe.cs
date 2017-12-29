
namespace GameClassLibrary
{
    public class CybertronLevelSafe : CybertronObject
    {
        public CybertronLevelSafe(int roomX, int roomY, int roomNumber) 
            : base(new SpriteInstance { RoomX = roomX, RoomY = roomY, Traits = CybertronSpriteTraits.Safe }, roomNumber)
        {
        }


        public override void ManWalkedIntoYou(CybertronGameBoard theGameBoard)
        {
            // TODO:  Consider how to move to the next level when the man is carrying all the objects necessary.
        }
    }
}
