using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public abstract class AbstractIntelligenceProvider
    {
        public abstract void AdvanceOneCycle(CybertronGameBoard theGameBoard, SpriteInstance spriteInstance);
    }
}
