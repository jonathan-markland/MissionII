using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public struct ClusterCanvas
    {
		private WallMatrix _wallMatrix;
		private int _originX;
		private int _originY;
		private int _endOffset;
		private int _innerLength;
		
		
		
		public ClusterCanvas(WallMatrix wallMatrix, int clusterIndexX, int clusterIndexY, int clusterSide)
		{
			_wallMatrix = wallMatrix;
			_originX = clusterIndexX * clusterSide;
			_originY = clusterIndexY * clusterSide;
			_endOffset = clusterSide - 1;
			_innerLength = clusterSide - 2;
		}
		
		
		
		public void Paint(int areaCode, bool paintWall)
		{
			Paint(areaCode, paintWall ? WallMatrixChar.Electric : WallMatrixChar.Space);
		}
		
		
		
		public void Paint(int areaCode, WallMatrixChar paintChar)
		{
			var e = _endOffset;
			var s = _innerLength;
			
			     if (areaCode == 1)  Paint(0,0,1,1, paintChar);
			else if (areaCode == 3)  Paint(e,0,1,1, paintChar);
			else if (areaCode == 7)  Paint(0,e,1,1, paintChar);
			else if (areaCode == 9)  Paint(e,e,1,1, paintChar);
			else if (areaCode == 5)  Paint(1,1,s,s, paintChar);
			else if (areaCode == 2)  Paint(1,0,s,1, paintChar);
			else if (areaCode == 4)  Paint(0,1,1,s, paintChar);
			else if (areaCode == 6)  Paint(e,1,1,s, paintChar);
			else if (areaCode == 8)  Paint(1,e,s,1, paintChar);
			else throw new Exception($"ClusterCanvas.Paint() error:  '{areaCode}' is not a valid area code.");
		}

		
		
		private void paint(int x, int y, int w, int h)
		{
			// TODO:  Could assert area fits.  Not massively important since Write() does bounds checking.
			
			x += _originX;
			y += _originY;
			var e = x + w;
			var f = y + h;
			
			while (y < f)
			{
				while (x < e)
				{
					_wallMatrix.Write(x, y, paintChar);
					++x;
				}
				x -= w;
				++y;
			}
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


/*
   JKL
   MNO
   PQR
         12223
   ABC   45556
   DEF   45556
   GHI   45556
         78889


Filled(2):
- If B is unfilled, 2 will be unfilled.
- Otherwise B is filled ...
  ... except if the 3x3 above exists AND has filled(N) & filled(Q)
      then 2 can be unfilled.

     (respectively for the other sides, 4, 6, 8).

Evaluate the trivial cases AFTER the above

Filled(5) = Filled(2) & Filled(4) & Filled(6) & Filled(8)

Filled(1) = Filled(2) | Filled(4)
... resp. for 3,7,9

*/