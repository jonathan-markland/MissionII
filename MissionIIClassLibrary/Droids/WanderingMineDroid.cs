
namespace MissionIIClassLibrary.Droids
{
    public class WanderingMineDroid : BaseDroid  // Didn't make it into final release
    {
        public WanderingMineDroid()
            : base(MissionIISpriteTraits.Monster5, new ArtificialIntelligence.WanderingMine())
        {
        }

        public override int KillScore
        {
            get { return Constants.WanderingMineDroidKillScore; }
        }
    }
}
