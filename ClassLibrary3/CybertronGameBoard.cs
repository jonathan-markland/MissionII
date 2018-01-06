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

    public class CybertronGameMode: GameMode<CybertronKeyStates>
    {

    }

    public static class CybertronGameModeSelector
    {
        public static CybertronGameModes ModeSelector = new CybertronGameModes();
    }

    public class CybertronGameModes
    {
        private CybertronGameMode _currentMode;

        public CybertronGameModes()
        {
            CurrentMode = new CybertronTitleScreenMode();
        }

        public CybertronGameMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
            }
        }
    }

    public class SuddenlyReplaceableList<T>
    {
        private List<T> _theList;

        public SuddenlyReplaceableList()
        {
            Clear();
        }

        public void Clear()
        {
            _theList = new List<T>();
        }

        public void Add(T objectToAdd)
        {
           _theList.Add(objectToAdd);
        }

        public void RemoveThese(List<T> listOfObjects)
        {
            // Making a fresh list with items removed.
            // Making a fresh list will have a different instance address
            // for the List<T>, thus will terminate any ForEachDo() in operation.
            var newList = new List<T>();
            newList.AddRange(_theList);
            foreach(var o in listOfObjects)
            {
                newList.Remove(o);
            }
            ReplaceWith(newList);
        }

        public void ReplaceWith(List<T> newItems)
        {
            _theList = newItems;
        }

        public void ForEachDo(Action<T> theAction)
        {
            var addressOfOriginalList = _theList;
            // Note: We support the collection being appended while this loop executes.
            var originalListLength = _theList.Count;
            for (int i = 0; i < _theList.Count; i++)
            {
                System.Diagnostics.Debug.Assert(_theList.Count >= originalListLength);
                theAction(_theList[i]);
                // _theList might have been invalidated.
                if (!object.ReferenceEquals(_theList, addressOfOriginalList)) break; // abort because client replaced list during iteration.
            }
        }
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
        public SuddenlyReplaceableList<CybertronGameObject> ObjectsInRoom = new SuddenlyReplaceableList<CybertronGameObject>();
        public List<CybertronGameObject> ObjectsToRemove = new List<CybertronGameObject>();
        public List<CybertronObject> PlayerInventory = new List<CybertronObject>();
        public CybertronKey Key;
        public CybertronRing Ring;
        public CybertronGold Gold;
        public CybertronLevelSafe Safe;
        public CybertronPotion Potion;
        public CybertronManPosition ManPositionOnRoomEntry;

        /// <summary>
        /// Returns true if any droids exist in the room.
        /// </summary>
        public bool DroidsExistInRoom
        {
            get
            {
                bool foundDroids = false;
                ObjectsInRoom.ForEachDo(o => 
                {
                    if (o is CybertronDroidBase)
                    {
                        foundDroids = true;
                    }
                });
                return foundDroids;
            }
        }

        public void ForEachThingWeHaveToFindOnThisLevel(Action<CybertronObject> theAction)
        {
            theAction(Key);
            if (LevelNumber > 1)
            {
                theAction(Ring);
            }
            if (LevelNumber > 2)
            {
                theAction(Gold);
            }
        }
    }
}
