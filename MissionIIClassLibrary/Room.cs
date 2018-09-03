using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public class Room
    {
        public Room(int x, int y, WriteableWallMatrix fileWallData)
        {
            RoomX = x;
            RoomY = y;
            FileWallData = fileWallData;
        }

        public int RoomNumber
        {
            get { return RoomX + Constants.RoomsHorizontally * (RoomY - 1); }
        }

        public int RoomX;
        public int RoomY;
        public WriteableWallMatrix FileWallData;
        public WriteableWallMatrix WallData;
    }
}
