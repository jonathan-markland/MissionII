namespace GameClassLibrary.Walls
{
    public struct WallMatrixChar
    {
        public byte Value;
        public static bool operator==(WallMatrixChar a, WallMatrixChar b) { return a.Value == b.Value; }
        public static bool operator!=(WallMatrixChar a, WallMatrixChar b) { return a.Value != b.Value; }
    }
}
