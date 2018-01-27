
namespace MissionIIClassLibrary.Droids
{
    public class DestroyerDroid : BaseDroid
    {
        public DestroyerDroid()
            : base(CybertronSpriteTraits.Monster3, new ArtificialIntelligence.FiringAttractor())
        {
        }

        public override int KillScore
        {
            get { return CybertronGameBoardConstants.PinkDroidKillScore; }
        }
    }
}
