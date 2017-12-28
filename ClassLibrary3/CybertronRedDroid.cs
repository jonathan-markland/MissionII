
namespace GameClassLibrary
{
    public class CybertronRedDroid : CybertronDroidBase
    {
        public CybertronRedDroid(int roomX, int roomY)
            : base(roomX, roomY, CybertronSpriteTraits.Monster1, new ArtificialIntelligence.Attractor())
        {
        }
    }
}
