
namespace MissionIIClassLibrary.Droids
{
    public class WanderingDroid : BaseDroid
    {
        public WanderingDroid()
            : base(CybertronSpriteTraits.Monster2, new ArtificialIntelligence.SingleMinded())
        {
        }

        public override int KillScore
        {
            get { return CybertronGameBoardConstants.BlueDroidKillScore; }
        }
    }
}
