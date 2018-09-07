namespace GameClassLibrary.Walls
{
    /// <summary>
    /// Represents a kind of tile, which is a user-defined concept.
    /// </summary>
    public struct Tile
    {
        /// <summary>
        /// The index into a user-supplied array of tiles.
        /// </summary>
        public byte VisualIndex;

        public static bool operator==(Tile a, Tile b) { return a.VisualIndex == b.VisualIndex; }
        public static bool operator!=(Tile a, Tile b) { return a.VisualIndex != b.VisualIndex; }
        public static bool operator&(Tile a, Tile b) { return (a.VisualIndex & b.VisualIndex) != 0; }
    }
}
