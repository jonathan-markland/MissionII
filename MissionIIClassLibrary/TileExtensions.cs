using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public static class TileExtensions
    {
        public static bool IsFloor(this Tile t)
        {
            return t.HasFlag(MissionIITile.FloorMask);
        }

        public static bool IsElectric(this Tile t)
        {
            return t.HasFlag(MissionIITile.ElectricWallMask);
        }
    }
}
