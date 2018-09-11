using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public abstract class AbstractIntelligenceProvider
    {
        public abstract void AdvanceOneCycle(IGameBoard theGameBoard, SpriteInstance spriteInstance);
    }
}
