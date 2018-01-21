﻿using System;
using GameClassLibrary.Math;

namespace GameClassLibrary
{
    public class SpecialMarkers
    {
        private Point? _manStart;
        private Room _manStartRoom;
        private int _initialManFacingDirection;

        public void SetManStartCluster(Room startRoom, Point startCluster)
        {
            if (_manStart.HasValue)
            {
                throw new Exception("More than one man start position already seen (Denoted by 'x').");
            }
            if (startCluster.X < 0 || startCluster.X >= Constants.ClustersHorizontally)
            {
                throw new Exception("Man start X position is invalid.");
            }
            if (startCluster.Y < 0 || startCluster.Y >= Constants.ClustersVertically)
            {
                throw new Exception("Man start Y position is invalid.");
            }
            _manStart = startCluster;
            _manStartRoom = startRoom;
            _initialManFacingDirection = Business.GetDirectionFacingAwayFromWalls(startRoom.FileWallData, startCluster);
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

        public int InitialManFacingDirection
        {
            get { return _initialManFacingDirection; }
        }
    }
}