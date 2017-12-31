
namespace GameClassLibrary
{
    public class CybertronRedDroid : CybertronDroidBase
    {
        public CybertronRedDroid()
            : base(CybertronSpriteTraits.Monster1, new ArtificialIntelligence.Attractor())
        {
        }
    }
}
