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


/*using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Walls
{
    public static class PositionFinder
    {
        /// <summary>
        /// Empty location finder.
        /// Intended for one-pass-only placement of objects / monsters in room.
        /// </summary>
        /// <param name="tileMatrix">Map data. The search is over this entire area.</param>
        /// <param name="tileWidth">Width of wall block.</param>
        /// <param name="tileHeight">Height of wall block.</param>
        /// <param name="tallestWidth">Width of WIDEST sprite to be positioned.</param>
        /// <param name="tallestHeight">Height of TALLEST sprite to be positioned.</param>
        /// <param name="foundLocationHandler">Callback function passed top left (x,y) of locations found, relative to tileMatrix top left.</param>
        public static void ForEachEmptyCell(
                        ArrayView2D<Tile> tileMatrix,
                        int tallestWidth,
                        int tallestHeight,
                        int tileWidth,
                        int tileHeight,
                        Func<int,int,bool> foundLocationHandler,
                        Func<Tile, bool> isFloor)
        {
            var roomWidth = tileMatrix.CountH * tileWidth;
            var roomHeight = tileMatrix.CountV * tileHeight;

            var countHorz = (int)roomWidth / tallestWidth;
            var countVert = (int)roomHeight / tallestHeight;

            // Start so that search-space net is centred over the map area:
            var leftX = (roomWidth - (countHorz * tallestWidth)) / 2;
            var topY = (roomHeight - (countVert * tallestHeight)) / 2;

            var y = topY;
            while (countVert > 0)
            {
                var x = leftX;
                var snapshotCountHorz = countHorz;
                while (countHorz > 0)
                {
                    if (CollisionDetection.HitsWalls(
                        tileMatrix, x, y, 
                        tallestWidth, tallestHeight,
                        tileWidth, tileHeight, isFloor) == CollisionDetection.WallHitTestResult.NothingHit)
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
*/
