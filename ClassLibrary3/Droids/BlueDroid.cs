
namespace GameClassLibrary.Droids
{
    public class BlueDroid : BaseDroid
    {
        public BlueDroid()
            : base(CybertronSpriteTraits.Monster2, new ArtificialIntelligence.SingleMinded())
        {
        }

        public override int KillScore
        {
            get { return CybertronGameBoardConstants.BlueDroidKillScore; }
        }
    }
}
