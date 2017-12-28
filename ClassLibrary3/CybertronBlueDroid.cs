
namespace GameClassLibrary
{
    public class CybertronBlueDroid : CybertronDroidBase
    {
        public CybertronBlueDroid(int roomX, int roomY)
            : base(roomX, roomY, CybertronSpriteTraits.Monster2, new ArtificialIntelligence.SingleMinded())
        {
        }
    }
}
