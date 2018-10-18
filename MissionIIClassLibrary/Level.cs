using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public class Level
    {
        private SpecialMarkers _specialMarkers;



        public Level(int levelNumber, TileMatrix wholeOfLevelMatrix, SpecialMarkers specialMarkers)
        {
            LevelNumber = levelNumber;
            LevelTileMatrix = wholeOfLevelMatrix;
            _specialMarkers = specialMarkers;
            if (!specialMarkers.ManStartFound)
            {
                throw new Exception($"Man start position marker 'x' has not been set.");
            }
        }



        /// <summary>
        /// The room number is 1-based.
        /// </summary>
        public static int ClusterToRoomNumber(Point clusterPos)
        {
            return
                (clusterPos.Y / Constants.ClustersVertically) * Constants.RoomsHorizontally
                + (clusterPos.X / Constants.ClustersHorizontally) + 1;
        }



        public int ManStartRoomNumber
        {
            get { return ClusterToRoomNumber(ManStartCluster); }
        }

        public Point ManStartCluster
        {
            get { return _specialMarkers.ManStartCluster; }
        }

        public int ManStartFacingDirection
        {
            get { return _specialMarkers.InitialManFacingDirection; }
        }

        public int LevelNumber
        {
            get; private set;
        }

        public TileMatrix LevelTileMatrix
        {
            get; private set;
        }

        public SpecialMarkers SpecialMarkers
        {
            get { return _specialMarkers; }
        }
    }
}
