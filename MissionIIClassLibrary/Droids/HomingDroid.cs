
namespace MissionIIClassLibrary.Droids
{
    public class HomingDroid : BaseDroid
    {
        public HomingDroid()
            : base(CybertronSpriteTraits.Monster1, new ArtificialIntelligence.Attractor())
        {
        }

        public override int KillScore
        {
            get { return CybertronGameBoardConstants.RedDroidKillScore; }
        }
    }
}
