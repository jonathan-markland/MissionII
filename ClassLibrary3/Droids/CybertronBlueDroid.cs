
namespace GameClassLibrary.Droids
{
    public class CybertronBlueDroid : CybertronDroidBase
    {
        public CybertronBlueDroid()
            : base(CybertronSpriteTraits.Monster2, new ArtificialIntelligence.SingleMinded())
        {
        }

        public override int KillScore
        {
            get { return CybertronGameBoardConstants.BlueDroidKillScore; }
        }
    }
}
