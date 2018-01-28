
namespace MissionIIClassLibrary.Droids
{
    public class HomingDroid : BaseDroid
    {
        public HomingDroid()
            : base(MissionIISpriteTraits.Monster1, new ArtificialIntelligence.Attractor())
        {
        }

        public override int KillScore
        {
            get { return MissionIIGameBoardConstants.HomingDroidKillScore; }
        }
    }
}
