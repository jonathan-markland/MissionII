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



        public static Point ClusterToRoomXY(Point clusterPos)
        {
            return new Point(
                (clusterPos.X / Constants.ClustersHorizontally),
                (clusterPos.Y / Constants.ClustersVertically));
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
