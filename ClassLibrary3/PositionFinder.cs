using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public static class PositionFinder
    {
        /// <summary>
        /// Empty location finder.
        /// Intended for one-pass-only placement of objects / monsters in room.
        /// </summary>
        /// <param name="wallData">Room wall data.</param>
        /// <param name="tileWidth">Width of wall block.</param>
        /// <param name="tileHeight">Height of wall block.</param>
        /// <param name="tallestWidth">Width of WIDEST sprite to be positioned.</param>
        /// <param name="tallestHeight">Height of TALLEST sprite to be positioned.</param>
        /// <param name="lambdaFunc">Callback function passed top left (x,y) of locations found.</param>
        public static void ForEachEmptyCell(
                        WallMatrix wallData,
                        int tileWidth,
                        int tileHeight,
                        int tallestWidth,
                        int tallestHeight,
                        Func<int,int,bool> lambdaFunc)
        {
            var roomWidth = wallData.CountH * tileWidth; // TODO: Not ideal having these possibly repeated calculations.
            var roomHeight = wallData.CountV * tileHeight; // TODO: Not ideal having these possibly repeated calculations.

            var countHorz = (int)roomWidth / tallestWidth;
            var countVert = (int)roomHeight / tallestHeight;

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
                        wallData, tileWidth, tileHeight, x, y, 
                        tallestWidth, tallestHeight) == CollisionDetection.WallHitTestResult.NothingHit)
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
