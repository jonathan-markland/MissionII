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
        public WallMatrix CurrentRoomWallData;
        public CybertronMan Man = new CybertronMan();
        public List<CybertronGameObject> ObjectsInRoom = new List<CybertronGameObject>();
        public List<CybertronGameObject> ObjectsToRemove = new List<CybertronGameObject>();
        public List<CybertronObject> PlayerInventory = new List<CybertronObject>();
        public CybertronKey Key;
        public CybertronRing Ring;
        public CybertronGold Gold;
        
        /// <summary>
        /// Returns true if any droids exist in the room.
        /// </summary>
        public bool DroidsExistInRoom
        {
            get
            {
                foreach (var theObject in ObjectsInRoom)
                {
                    if (theObject is CybertronDroidBase)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Iterate all game objects in the room and call the callback.
        /// The callback must return 'true' to continue, or 'false' to terminate.
        /// </summary>
        /// <returns>true if enumeration succeeded normally.  false if callback stopped enumeration.</returns>
        public bool ForEachDo(Func<CybertronGameObject, bool> theAction)
        {
            // Note: We support the collection being appended while this loop executes.
            var n = ObjectsInRoom.Count;
            for (int i=0; i<n; i++)
            { 
                if (!theAction(ObjectsInRoom[i])) return false;
            }
            return theAction(Man); // TODO: remove
        }
    }
}
