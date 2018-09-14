
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public abstract class AbstractIntelligenceProvider
    {
        public abstract void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject);
    }
}
