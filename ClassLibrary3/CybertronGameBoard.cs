﻿using System;
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

        // public List<CybertronBullet> BulletsInRoom = new List<CybertronBullet>();
        // public List<CybertronDroidBase> DroidsInRoom = new List<CybertronDroidBase>();
        // public List<CybertronExplosion> ExplosionsInRoom = new List<CybertronExplosion>();
        // public List<CybertronExplosion> ExplosionsToRemove = new List<CybertronExplosion>();
        // public List<CybertronBullet> BulletsToRemove = new List<CybertronBullet>();
        // public List<CybertronDroidBase> DroidsToRemove = new List<CybertronDroidBase>();
        public CybertronGhost Ghost = new CybertronGhost();
        // TODO: List<??> Inventory;   // What's carried.    TODO: Paint inventory    TODO: Manage list when collecting an item.
        public List<CybertronObject> PlayerInventory = new List<CybertronObject>();
        public CybertronKey Key;
        public CybertronRing Ring;
        public CybertronGold Gold;

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
            if (!theAction(Ghost)) return false; // TODO: remove

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
