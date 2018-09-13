
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public abstract class AbstractIntelligenceProvider // TODO: move to library after IGameBoard is distilled.
    {
        public abstract void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject);
    }
}
