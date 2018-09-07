using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public static class MissionIITile
    {
        public const int SpaceMask = 2;
        public const int BrickMask = 4;
        public const int ElectricMask = 8;

        public static Tile Space = new Tile { VisualIndex = SpaceMask };
        public static Tile Brick = new Tile { VisualIndex = BrickMask };
        public static Tile Electric = new Tile { VisualIndex = ElectricMask };
    }
}
