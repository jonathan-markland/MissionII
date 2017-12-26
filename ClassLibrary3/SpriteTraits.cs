using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public class SpriteTraits
    {
        public SpriteTraits(int boardWidth, int boardHeight, List<object> hostImageObjects)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            HostImageObjects = hostImageObjects;
        }
        
        /// <summary>
        /// A copy of the width is stored here, to remind the game engine.
        /// </summary>
        public int BoardWidth { get; private set; }

        /// <summary>
        /// A copy of the height is stored here, to remind the game engine.
        /// </summary>
        public int BoardHeight { get; private set;  }

        public List<object> HostImageObjects { get; private set; }
    }
}
