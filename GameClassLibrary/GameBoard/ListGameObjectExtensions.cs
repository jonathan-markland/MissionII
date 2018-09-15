
using System.Collections.Generic;
using GameClassLibrary.Math;

namespace GameClassLibrary.GameBoard
{

    public static class ListGameObjectExtensions
    {
        /// <summary>
        /// Returns the maximum width and height of the game objects in the list.
        /// </summary>
        public static Dimensions GetMaxDimensions(this List<GameObject> theList)
        {
            return GetMaxDimensions(theList, 0, 0);
        }



        /// <summary>
        /// Returns the maximum width and height of the game objects in the list.
        /// This includes a user-defined minimum height and width, in case all the
        /// objects are smaller.
        /// </summary>
        public static Dimensions GetMaxDimensions(this List<GameObject> theList, int minWidth, int minHeight)
        {
            int posnWidth = minWidth;
            int posnHeight = minHeight;

            foreach (var obj in theList)
            {
                var objRect = obj.GetBoundingRectangle();
                posnWidth = System.Math.Max(objRect.Width, posnWidth);
                posnHeight = System.Math.Max(objRect.Height, posnHeight);
            }

            return new Dimensions(posnWidth, posnHeight);
        }
    }
}
