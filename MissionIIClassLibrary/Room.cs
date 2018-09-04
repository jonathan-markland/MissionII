using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public class Room
    {
        public Room(int x, int y, WallMatrix wallData)
        {
            RoomX = x;
            RoomY = y;
            WallData = wallData;
        }

        /// <summary>
        /// Returns this room's number, unique within the level only.
        /// </summary>
        public int RoomNumber
        {
            get { return RoomX + Constants.RoomsHorizontally * (RoomY - 1); }
        }

        /// <summary>
        /// Room horizontal position on the level.
        /// </summary>
        public int RoomX { get; private set; }
        
        /// <summary>
        /// Room vertical position on th level.
        /// </summary>
        public int RoomY { get; private set; }

        /// <summary>
        /// Wall data for this room.
        /// </summary>
        public WallMatrix WallData { get; private set; }
    }
}
