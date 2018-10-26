using System;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class ArtificialIntelligenceFunctions
    {
        public Action AdvanceOneCycle { get; private set; }

        public ArtificialIntelligenceFunctions(Action advanceOneCycle)
        {
            AdvanceOneCycle = advanceOneCycle;
        }
    }
}
