
namespace MissionIIClassLibrary.Droids
{
    public class DestroyerDroid : BaseDroid
    {
        public DestroyerDroid()
            : base(MissionIISpriteTraits.Monster3, new ArtificialIntelligence.FiringAttractor())
        {
        }

        public override int KillScore
        {
            get { return Constants.DestroyerDroidKillScore; }
        }
    }
}
