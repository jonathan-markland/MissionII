
using System;

namespace GameClassLibrary.Graphics
{
    public struct HostSuppliedSprite
    {
        public int BoardWidth;
        public int BoardHeight;
        public object HostObject;

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
