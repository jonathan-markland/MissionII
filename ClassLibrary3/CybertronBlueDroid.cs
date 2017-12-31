
namespace GameClassLibrary
{
    public class CybertronBlueDroid : CybertronDroidBase
    {
        public CybertronBlueDroid()
            : base(CybertronSpriteTraits.Monster2, new ArtificialIntelligence.SingleMinded())
        {
        }
    }
}
