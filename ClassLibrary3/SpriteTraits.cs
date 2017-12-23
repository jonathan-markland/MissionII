using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public class SpriteTraits
    {
        public SpriteTraits(int boardWidth, int boardHeight, List<object> hostImageObjects, GameTimeSpan timeBetweenFrames)
        {
            if (hostImageObjects.Count > 1)
            {
                if (timeBetweenFrames.Milliseconds == 0)
                {
                    throw new Exception("Cannot register animated SpriteTraits with zero time between frames!");
                }
            }
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            HostImageObjects = hostImageObjects;
            TimeBetweenFrames = timeBetweenFrames;
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
        public GameTimeSpan TimeBetweenFrames { get; private set; }
    }
}
