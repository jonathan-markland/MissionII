
namespace GameClassLibrary.Droids
{
    public class RedDroid : BaseDroid
    {
        public RedDroid()
            : base(CybertronSpriteTraits.Monster1, new ArtificialIntelligence.Attractor())
        {
        }

        public override int KillScore
        {
            get { return CybertronGameBoardConstants.RedDroidKillScore; }
        }
    }
}
