
namespace MissionIIClassLibrary.Droids
{
    public class GuardianDroid : BaseDroid
    {
        public GuardianDroid()
            : base(MissionIISpriteTraits.Monster4, new ArtificialIntelligence.Guardian())
        {
        }

        public override int KillScore
        {
            get { return Constants.GuardianDroidKillScore; }
        }
    }
}
