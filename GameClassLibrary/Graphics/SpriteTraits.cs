
using System;
using System.Collections.Generic;

namespace GameClassLibrary.Graphics
{
    public class SpriteTraits
    {
        private List<HostSuppliedSprite> _hostImageObjects;
        private static Func<string, HostSuppliedSprite> _hostSpriteSupplier;

        /// <summary>
        /// Must be initialised by the hosting environment.
        /// Allows a client to obtain a host sprite object, given a sprite name.
        /// </summary>
        public static void InitSpriteSupplier(Func<string, HostSuppliedSprite> hostSpriteSupplier)
        {
            _hostSpriteSupplier = hostSpriteSupplier;
        }

        public SpriteTraits(string spriteName, int imageCount)
        {
            // When there are multiple images:
            // - We append "_1", "_2" .. etc to the spriteName
            //   and request those names from the host.
            // - We must also make sure all images in the set are the same size!
            // When it's just a single image, we just load that only by the name given.

            int boardWidth = 0;
            int boardHeight = 0;
            var hostImageObjects = new List<HostSuppliedSprite>();

            for (int i = 1; i <= imageCount; i++)
            {
                var thisHostImageInfo = _hostSpriteSupplier((imageCount == 1) ? spriteName : spriteName + "_" + i);
                if (i == 1)
                {
                    boardWidth = thisHostImageInfo.Width;
                    boardHeight = thisHostImageInfo.Height;
                }
                else
                {
                    if (boardWidth != thisHostImageInfo.Width)
                    {
                        throw new Exception("Sprite widths don't match in the file set for '" + spriteName + "'.");
                    }
                    if (boardHeight != thisHostImageInfo.Height)
                    {
                        throw new Exception("Sprite heights don't match in the file set for '" + spriteName + "'.");
                    }
                }
                hostImageObjects.Add(thisHostImageInfo);
            }

            Width = boardWidth;
            Height = boardHeight;
            _hostImageObjects = hostImageObjects;
        }
        
        /// <summary>
        /// A copy of the width is stored here, to remind the game engine.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// A copy of the height is stored here, to remind the game engine.
        /// </summary>
        public int Height { get; private set;  }

        /// <summary>
        /// Retrieves the image object at the given index.  Only the host
        /// knows the actual format.
        /// </summary>
        public HostSuppliedSprite GetHostImageObject(int n)
        {
            return _hostImageObjects[n];
        }

        /// <summary>
        /// Returns the number of host image objects associated with this sprite.
        /// </summary>
        public int ImageCount {  get { return _hostImageObjects.Count; } }
    }
}
