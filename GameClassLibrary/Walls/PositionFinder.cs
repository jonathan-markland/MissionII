using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Walls
{
    public static class PositionFinder
    {
        /// <summary>
        /// Empty location finder. Calls the callback for all empty locations.
        /// Intended to allow multiple objects to be positioned in a single pass.
        /// </summary>
        /// <param name="scanArea">Area to scan witin.  Will never report outside this.</param>
        /// <param name="tallestWidth">Width of tallest shape to position.</param>
        /// <param name="tallestHeight">Height of tallest shape to position.</param>
        /// <param name="isSpace">Function to test if rectangle is entirely free of obstructions.</param>
        /// <param name="foundLocationHandler">Handler to be called when a location is found.  Handler can return false to early-terminate this search.</param>
        public static void ForEachEmptyCell(
                        Rectangle scanArea,
                        int tallestWidth,
                        int tallestHeight,
                        Func<Rectangle,bool> isSpace,
                        Func<int,int,bool> foundLocationHandler)
        {
            var roomWidth = scanArea.Width;
            var roomHeight = scanArea.Height;

            var countHorz = (int)roomWidth / tallestWidth;
            var countVert = (int)roomHeight / tallestHeight;

            // Start so that search-space "grid" is centred over the map area:
            var leftX = (roomWidth - (countHorz * tallestWidth)) / 2;
            var topY = (roomHeight - (countVert * tallestHeight)) / 2;

            var y = topY;
            while (countVert > 0)
            {
                var x = leftX;
                var snapshotCountHorz = countHorz;
                while (countHorz > 0)
                {
                    if (isSpace(new Rectangle(x, y, tallestWidth, tallestHeight)))
                    {
                        if (!foundLocationHandler(x, y)) return;
                    }
                    --countHorz;
                    x += tallestWidth;
                }
                countHorz = snapshotCountHorz;
                --countVert;
                y += tallestHeight;
            }
        }
    }
}
