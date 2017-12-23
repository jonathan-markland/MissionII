
namespace GameClassLibrary
{
    public class CybertronGold : CybertronObject
    {
        public CybertronGold(int roomX, int roomY, int roomNumber) 
            : base(new SpriteInstance { RoomX = roomX, RoomY = roomY, Traits = CybertronSpriteTraits.Gold }, roomNumber)
        {
        }
    }
}
