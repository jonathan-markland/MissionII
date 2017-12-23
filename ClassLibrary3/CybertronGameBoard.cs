using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public abstract class CybertronGameObject: GameObject<CybertronGameBoard, CybertronKeyStates>
    {
    }

    public class CybertronGameBoard
    {
        public bool IsPaused;
        public int BoardWidth;
        public int BoardHeight;
        public int LevelNumber;
        public int RoomNumber; // one-based
        public uint Score;
        public uint Lives;
        public WorldWallData TheWorldWallData;
        public List<string> CurrentRoomWallData;
        public CybertronMan Man = new CybertronMan();
        public List<CybertronBullet> BulletsInRoom = new List<CybertronBullet>();
        public List<CybertronDroid> DroidsInRoom = new List<CybertronDroid>();
        public List<CybertronObject> ObjectsInRoom = new List<CybertronObject>();
        public List<CybertronExplosion> ExplosionsInRoom = new List<CybertronExplosion>();
        public List<CybertronExplosion> ExplosionsToRemove = new List<CybertronExplosion>();
        public CybertronGhost Ghost;
        // TODO: List<??> Inventory;   // What's carried.    TODO: Paint inventory    TODO: Manage list when collecting an item.
        public List<CybertronObject> PlayerInventory = new List<CybertronObject>();
        public CybertronKey Key;
        public CybertronRing Ring;
        public CybertronGold Gold;

        /// <summary>
        /// Iterate all game objects in the room and call the callback.
        /// The callback must return 'true' to continue, or 'false' to terminate.
        /// </summary>
        /// <returns>true if enumeration succeeded normally.  false if callback stopped enumeration.</returns>
        public bool ForEachDo(Func<CybertronGameObject, bool> theAction)
        {
            foreach (var theDroid in DroidsInRoom)
            {
                if (!theAction(theDroid)) return false;
            }
            foreach (var theExplosion in ExplosionsInRoom)
            {
                if (!theAction(theExplosion)) return false;
            }
            if (Ghost != null) // TODO: Will we use null at all?
            {
                if (!theAction(Ghost)) return false;
            }
            foreach (var theObject in ObjectsInRoom)
            {
                if (!theAction(theObject)) return false;
            }
            foreach (var theBullet in BulletsInRoom)
            {
                if (!theAction(theBullet)) return false;
            }
            return theAction(Man);
        }
    }
}
