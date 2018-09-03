using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public class Room
    {
        public Room(int x, int y, WallMatrix fileWallData)
        {
            RoomX = x;
            RoomY = y;
            FileWallData = fileWallData;
        }

        public int RoomNumber
        {
            get { return RoomX + Constants.RoomsHorizontally * (RoomY - 1); }
        }

        public int RoomX { get; private set; }
        public int RoomY { get; private set; }
        public WallMatrix FileWallData { get; private set; }
        public WallMatrix WallData { get; private set; }
    }
}
