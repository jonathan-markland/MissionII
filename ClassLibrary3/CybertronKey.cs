
namespace GameClassLibrary
{
    public class CybertronKey : CybertronObject
    {
        public CybertronKey(int roomX, int roomY, int roomNumber) 
            : base(new SpriteInstance { RoomX = roomX, RoomY = roomY, Traits = CybertronSpriteTraits.Key }, roomNumber)
        {
        }
    }
}
