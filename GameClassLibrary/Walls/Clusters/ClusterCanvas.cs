using System;

namespace GameClassLibrary.Walls.Clusters
{
    public class ClusterCanvas
    {
        //              123
        // Area codes:  456
        //              789

		private WallMatrix _wallMatrix;
		protected int _originX;
		protected int _originY;
		protected int _endOffset;
		protected int _innerLength;
		
		
		
		public ClusterCanvas(WallMatrix wallMatrix, int clusterIndexX, int clusterIndexY, int clusterSide)
		{
			_wallMatrix = wallMatrix;
			_originX = clusterIndexX * clusterSide;
			_originY = clusterIndexY * clusterSide;
			_endOffset = clusterSide - 1;
			_innerLength = clusterSide - 2;
		}
		
		
		
		public bool IsWall(int areaCode)
		{
			return Test(areaCode) != WallMatrixChar.Space;
		}
		
		
		
		public bool IsSpace(int areaCode)
		{
			return Test(areaCode) == WallMatrixChar.Space;
		}
		
		
		
		public WallMatrixChar Test(int areaCode)
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
			else throw new Exception("ClusterCanvas.Test() error:  '{areaCode}' is not a valid area code.");
		}
		
		
		
		private WallMatrixChar Test(int x, int y)
		{
			return _wallMatrix.Read(x + _originX, y + _originY);
		}
    }
}
