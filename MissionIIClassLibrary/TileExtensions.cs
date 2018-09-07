using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public static class TileExtensions
    {
        public static bool IsSpace(this Tile t)
        {
            return t.HasFlag(MissionIITile.SpaceMask);
        }

        public static bool IsElectric(this Tile t)
        {
            return t.HasFlag(MissionIITile.ElectricMask);
        }
    }
}
