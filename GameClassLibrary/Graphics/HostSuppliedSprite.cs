
using System;

namespace GameClassLibrary.Graphics
{
    /// <summary>
    /// Container for a host sprite object, where the type of the object is hidden.
    /// </summary>
    public struct HostSuppliedSprite
    {
        public int BoardWidth { get; private set; }
        public int BoardHeight { get; private set; }
        public object HostObject { get; private set; }

        public HostSuppliedSprite(object hostObject, int w, int h)
        {
            BoardWidth = w;
            BoardHeight = h;
            HostObject = hostObject;
        }

        public uint[] ToArray()
        {
            return ToUintArrayHandler(this);
        }

        /// <summary>
        /// The host environment must provide this, and set this object.
        /// </summary>
        public static Func<uint[], int, int, HostSuppliedSprite> UintArrayToSprite;

        /// <summary>
        /// The host environment must provide this, and set this object.
        /// </summary>
        public static Func<HostSuppliedSprite, uint[]> ToUintArrayHandler;
    }
}
