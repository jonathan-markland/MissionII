﻿using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public class SpecialMarkers
    {
        private Point? _manStart;
        private int _initialManFacingDirection;

        public void SetManStartCluster(Point startCluster, Tile spaceCharValue, TileMatrix levelTileMatrix)
        {
            if (_manStart.HasValue)
            {
                throw new Exception("More than one man start position already seen (Denoted by 'x').");
            }

            if (startCluster.X < 0 || startCluster.X >= Constants.ClustersHorizontally * Constants.RoomsHorizontally)
            {
                throw new Exception("Man start X position is invalid.");
            }

            if (startCluster.Y < 0 || startCluster.Y >= Constants.ClustersVertically * Constants.RoomsVertically)
            {
                throw new Exception("Man start Y position is invalid.");
            }

            _manStart = startCluster;
            _initialManFacingDirection = DirectionFinder.GetDirectionFacingAwayFromWalls(
                levelTileMatrix, startCluster, Constants.SourceClusterSide, TileExtensions.IsFloor);
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
