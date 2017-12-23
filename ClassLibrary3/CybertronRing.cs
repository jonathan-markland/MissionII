
namespace GameClassLibrary
{
    public class CybertronRing : CybertronObject
    {
        public CybertronRing(int roomX, int roomY, int roomNumber) 
            : base(new SpriteInstance { RoomX = roomX, RoomY = roomY, Traits = CybertronSpriteTraits.Ring }, roomNumber)
        {
        }
    }
}
