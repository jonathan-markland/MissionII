using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public static class MissionIITile
    {
        public const int FloorMask = 2;
        public const int WallMask = 4;
        public const int ElectricWallMask = 8;

        public static Tile Floor = new Tile { VisualIndex = FloorMask };
        public static Tile Wall = new Tile { VisualIndex = WallMask };
        public static Tile ElectricWall = new Tile { VisualIndex = ElectricWallMask };
    }
}
