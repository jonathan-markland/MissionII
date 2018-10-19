using System;
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
        /// <param name="lambdaFunc">Callback function passed top left (x,y) of locations found, relative to tileMatrix top left.</param>
        public static void ForEachEmptyCell(
                        ArrayView2D<Tile> tileMatrix,
                        int tallestWidth,
                        int tallestHeight,
                        int tileWidth,
                        int tileHeight,
                        Func<int,int,bool> lambdaFunc,
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
                        if (!lambdaFunc(x, y)) return;
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
