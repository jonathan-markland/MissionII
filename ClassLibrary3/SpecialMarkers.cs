using System;

namespace GameClassLibrary
{
    public class SpecialMarkers
    {
        private Point? _manStart;
        private Room _manStartRoom;

        public void SetManStart(Room startRoom, Point startPosition)
        {
            if (_manStart.HasValue)
            {
                throw new Exception("More than one man start position already seen (Denoted by 'x').");
            }

            var x = startPosition.X;
            if (x % (Constants.SourceClusterSide) != (Constants.SourceClusterSide / 2))
            {
                throw new Exception($"Man start position (horizontally) must be in the centre of the {Constants.SourceClusterSide}x{Constants.SourceClusterSide} cluster.");
            }

            var y = startPosition.Y;
            if (y % (Constants.SourceClusterSide) != (Constants.SourceClusterSide / 2))
            {
                throw new Exception($"Man start position (vertically) must be in the centre of the {Constants.SourceClusterSide}x{Constants.SourceClusterSide} cluster.");
            }

            _manStart = startPosition;
            _manStartRoom = startRoom;
        }

        public Point ManStart
        {
            get
            {
                return _manStart.Value;
            }
        }

        public Room StartRoom
        {
            get { return _manStartRoom; }
        }
    }
}
