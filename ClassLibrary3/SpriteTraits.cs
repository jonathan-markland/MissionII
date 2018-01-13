using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public class SpriteTraits
    {
        private List<object> _hostImageObjects;

        public SpriteTraits(int boardWidth, int boardHeight, List<object> hostImageObjects)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            _hostImageObjects = hostImageObjects;
        }
        
        /// <summary>
        /// A copy of the width is stored here, to remind the game engine.
        /// </summary>
        public int BoardWidth { get; private set; }

        /// <summary>
        /// A copy of the height is stored here, to remind the game engine.
        /// </summary>
        public int BoardHeight { get; private set;  }

        /// <summary>
        /// Retrieves the image object at the given index.  Only the host
        /// knows the actual format.
        /// </summary>
        public object GetHostImageObject(int n)
        {
            return _hostImageObjects[n];
        }

        /// <summary>
        /// Returns the number of host image objects associated with this sprite.
        /// </summary>
        public int ImageCount {  get { return _hostImageObjects.Count; } }
    }
}
