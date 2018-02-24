
namespace MissionIIClassLibrary.Droids
{
    public class HomingDroid : BaseDroid
    {
        public HomingDroid()
            : base(MissionIISpriteTraits.Monster1, new ArtificialIntelligence.WanderingMine()) // TODO: revert: Attractor())
        {
        }

        public override int KillScore
        {
            get { return Constants.HomingDroidKillScore; }
        }
    }
}
