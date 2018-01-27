﻿
namespace MissionIIClassLibrary
{
    public static class CollisionDetection
    {
        public enum WallHitTestResult
        {
            NothingHit,
            OutsideRoomToLeft,
            OutsideRoomToRight,
            OutsideRoomAbove,
            OutsideRoomBelow,
            HitWall
        }

        public static WallHitTestResult HitsWalls(
            WallMatrix wallData,
            int tileWidth,
            int tileHeight,
            int objectX,
            int objectY,
            int objectWidth,
            int objectHeight)
        {
            if (wallData.Empty) return WallHitTestResult.HitWall; // Required.

            var roomWidth = wallData.CountH * tileWidth; // TODO: Not ideal having these possibly repeated calculations.
            var roomHeight = wallData.CountV * tileHeight; // TODO: Not ideal having these possibly repeated calculations.

            // Non-inclusive bottom right corner of object:
            var objectX2 = objectX + objectWidth;
            var objectY2 = objectY + objectHeight;

            // Fully or partially off-screen is treated as a collision:
            if (objectX2 > roomWidth) return WallHitTestResult.OutsideRoomToRight;
            if (objectY2 > roomHeight) return WallHitTestResult.OutsideRoomBelow;
            if (objectX < 0) return WallHitTestResult.OutsideRoomToLeft;
            if (objectY < 0) return WallHitTestResult.OutsideRoomAbove;

            // Lies completely on-screen.

            // Calculate coverage area in room block coordinates:
            int cX = objectX / tileWidth;
            int cY = objectY / tileHeight;
            int cx2 = (objectX2 + (tileWidth - 1)) / tileWidth; // non-inclusive
            int cy2 = (objectY2 + (tileHeight - 1)) / tileHeight; // non-inclusive

            for (int y=cY; y<cy2; y++)
            {
                for (int x = cX; x < cx2; x++)
                {
                    if (wallData.Read(x, y) != WallMatrixChar.Space)
                    {
                        return WallHitTestResult.HitWall;  // hit wall block  // NB: If indexing fails, all rows MUST be the same length!
                    }
                }
            }

            return WallHitTestResult.NothingHit;
        }
    }
}