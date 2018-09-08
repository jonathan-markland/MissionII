
using System;

namespace GameClassLibrary.Graphics
{
    /// <summary>
    /// Container for a host sprite object, where the type of the object is hidden.
    /// </summary>
    public struct HostSuppliedSprite
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public object HostObject { get; private set; }

        /// <summary>
        /// Initialisation function that must be called by the hosting environment
        /// before instances of this class can be created.
        /// </summary>
        public static void InitialisationByHost(
            Func<uint[], int, int, object> uintArrayToSprite,
            Func<object, uint[]> toUintArrayHandler)
        {
            UintArrayToSprite = uintArrayToSprite;
            ToUintArrayHandler = toUintArrayHandler;
        }

        /// <summary>
        /// Constructor called by a host environment to create a link
        /// to a sprite object held by the host.  Only the host knows
        /// the format of the sprite object.
        /// </summary>
        public HostSuppliedSprite(object hostObject, int w, int h)
        {
            Width = w;
            Height = h;
            HostObject = hostObject;
        }

        /// <summary>
        /// Constructor called to create a sprite image from a pixels array,
        /// and dimensions.  The host environment is called to create an
        /// underlying image object, which is linked to from this object.
        /// </summary>
        public HostSuppliedSprite(uint[] pixelsArray, int w, int h)
        {
            if (w >= 0
                && h >= 0
                && w <= 10000 // just overflow prevention
                && h <= 10000 // just overflow prevention
                && (w * h) == pixelsArray.Length)
            {
                Width = w;
                Height = h;
                HostObject = UintArrayToSprite(pixelsArray, w, h);
            }
            else throw new Exception("Invalid sprite dimensions.");
        }

        /// <summary>
        /// Requests the host to return an array of uint values that give
        /// the ARGB values for each pixel of the image, from top left to
        /// bottom right, rows primary.
        /// </summary>
        public uint[] PixelsToUintArray()
        {
            return ToUintArrayHandler(HostObject);
        }

        /// <summary>
        /// The host environment must provide this, and set this object.
        /// </summary>
        private static Func<uint[], int, int, object> UintArrayToSprite;

        /// <summary>
        /// The host environment must provide this, and set this object.
        /// </summary>
        private static Func<object, uint[]> ToUintArrayHandler;
    }
}
