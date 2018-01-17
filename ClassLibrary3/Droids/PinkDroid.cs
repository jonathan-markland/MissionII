
namespace GameClassLibrary.Droids
{
    public class PinkDroid : BaseDroid
    {
        public PinkDroid()
            : base(CybertronSpriteTraits.Monster3, new ArtificialIntelligence.FiringAttractor())
        {
        }

        public override int KillScore
        {
            get { return CybertronGameBoardConstants.PinkDroidKillScore; }
        }
    }
}
