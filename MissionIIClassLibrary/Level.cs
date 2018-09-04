using System;
using System.Collections.Generic;
using GameClassLibrary.Math;

namespace MissionIIClassLibrary
{
    public class Level
    {
        private SpecialMarkers _specialMarkers;

        public Level(int levelNumber, List<Room> roomList, SpecialMarkers specialMarkers)
        {
            LevelNumber = levelNumber;
            Rooms = roomList;
            _specialMarkers = specialMarkers;
            if (specialMarkers.StartRoom == null)
            {
                throw new Exception($"Man start position marker 'x' has not been set.");
            }
        }

        public Room ManStartRoom { get { return _specialMarkers.StartRoom; } }
        public Point ManStartCluster { get { return _specialMarkers.ManStart; } }
        public int ManStartFacingDirection { get { return _specialMarkers.InitialManFacingDirection; } }
        public int LevelNumber { get; private set; }
        public List<Room> Rooms { get; private set; }
        public SpecialMarkers SpecialMarkers { get { return _specialMarkers; } }
    }
}
