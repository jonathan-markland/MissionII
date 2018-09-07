using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public static class TileExtensions
    {
        public static bool IsSpace(this Tile t)
        {
            return t.HasFlag(2);
        }
    }
}
