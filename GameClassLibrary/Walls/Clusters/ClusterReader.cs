using System;

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

		private TileMatrix _wallMatrix;
        private Func<Tile, bool> _isSpaceFunc;

        protected int _originX;
		protected int _originY;
		protected int _endOffset;
		protected int _innerLength;
		
		
		
		public ClusterReader(
            TileMatrix wallMatrix, 
            int clusterIndexX, int clusterIndexY, 
            int clusterSide, Func<Tile, bool> isSpaceFunc)
		{
			_wallMatrix = wallMatrix;
			_originX = clusterIndexX * clusterSide;
			_originY = clusterIndexY * clusterSide;
			_endOffset = clusterSide - 1;
			_innerLength = clusterSide - 2;
            _isSpaceFunc = isSpaceFunc;
        }
		
		
		
		public bool IsWall(int areaCode)
		{
            return !IsSpace(areaCode);
		}
		
		
		
		public bool IsSpace(int areaCode)
		{
			return _isSpaceFunc(Test(areaCode));
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
			return _wallMatrix.TileAt(x + _originX, y + _originY);
		}
    }
}
