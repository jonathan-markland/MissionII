
namespace MissionIIClassLibrary.Droids
{
    public class HomingDroid : BaseDroid
    {
        public HomingDroid()
            : base(MissionIISprites.Monster1, new ArtificialIntelligence.Attractor())
        {
        }

        public override int KillScore
        {
            get { return Constants.HomingDroidKillScore; }
        }
    }
}
