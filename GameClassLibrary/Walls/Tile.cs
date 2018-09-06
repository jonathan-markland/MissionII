namespace GameClassLibrary.Walls
{
    public struct Tile
    {
        public byte VisualIndex;
        public static bool operator==(Tile a, Tile b) { return a.VisualIndex == b.VisualIndex; }
        public static bool operator!=(Tile a, Tile b) { return a.VisualIndex != b.VisualIndex; }
    }
}
