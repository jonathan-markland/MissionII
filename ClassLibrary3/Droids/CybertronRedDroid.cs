
namespace GameClassLibrary.Droids
{
    public class CybertronRedDroid : CybertronDroidBase
    {
        public CybertronRedDroid()
            : base(CybertronSpriteTraits.Monster1, new ArtificialIntelligence.Attractor())
        {
        }

        public override int KillScore
        {
            get { return CybertronGameBoardConstants.RedDroidKillScore; }
        }
    }
}
