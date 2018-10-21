using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Walls.Clusters
{
    /// <summary>
    /// Provides a way to query a "cluster" (a 3x3 arrangement of tiles).
    /// </summary>
    public class ClusterReader
    {
        //              123
        // Area codes:  456
        //              789

		private ArraySlice2D<Tile> _wallMatrix;
        private Func<Tile, bool> _isFloorFunc;

        protected int _originX;
		protected int _originY;
		protected int _endOffset;
		protected int _innerLength;
		
		
		
		public ClusterReader(
            ArraySlice2D<Tile> wallMatrix, 
            int clusterIndexX, int clusterIndexY, 
            int clusterSide, Func<Tile, bool> isFloorFunc)
		{
            System.Diagnostics.Debug.Assert(clusterSide >= 3);
            System.Diagnostics.Debug.Assert(wallMatrix.CountH % clusterSide == 0);
            System.Diagnostics.Debug.Assert(wallMatrix.CountV % clusterSide == 0);

            _wallMatrix = wallMatrix;
			_originX = clusterIndexX * clusterSide;
			_originY = clusterIndexY * clusterSide;
			_endOffset = clusterSide - 1;
			_innerLength = clusterSide - 2;
            _isFloorFunc = isFloorFunc;
        }
		
		
		
		public bool IsWall(int areaCode)
		{
            return !IsFloor(areaCode);
		}
		
		
		
		public bool IsFloor(int areaCode)
		{
			return _isFloorFunc(Test(areaCode));
		}
		
		
		
		public Tile Test(int areaCode)
		{
			var e = _endOffset;
			
			     if (areaCode == 1)  return Test(0,0);
			else if (areaCode == 3)  return Test(e,0);
			else if (areaCode == 7)  return Test(0,e);
			else if (areaCode == 9)  return Test(e,e);
			else if (areaCode == 5)  return Test(1,1);
			else if (areaCode == 2)  return Test(1,0);
			else if (areaCode == 4)  return Test(0,1);
			else if (areaCode == 6)  return Test(e,1);
			else if (areaCode == 8)  return Test(1,e);
			else throw new Exception($"ClusterCanvas.Test() error:  '{areaCode}' is not a valid area code.");
		}
		
		
		
		private Tile Test(int x, int y)
		{
			return _wallMatrix.At(x + _originX, y + _originY);
		}
    }
}
