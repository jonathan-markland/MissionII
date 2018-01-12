using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public static class LevelExpander
    {
        public const int ExpandSize = 5;


        public static void ExpandWallsInWorld(WorldWallData theWorld)
        {
            foreach(var thisLevel in theWorld.Levels)
            {
                foreach(var thisRoom in thisLevel.Rooms)
                {
                    var expandedData = ExpandWallsWithThickPassages.ExpandWalls(thisRoom.FileWallData);
                    //CarveDoorways(expandedData);
                    SecondBrickIze(expandedData);
                    thisRoom.WallData = expandedData;
                }
            }
        }



        public static void CarveDoorways(WallMatrix expandedData)
        {
            // Carving invalidates the door thickness matching 
            // (potentially), but it will only be because the outermost row
            // is WIDER than inner ones, in a given room.

            int thickCount = ExpandSize;
            CarveDoorIfPresent(expandedData, new Point(0,  0), new MovementDeltas(1, 0), new MovementDeltas( 0, 1), thickCount);
            CarveDoorIfPresent(expandedData, new Point(0, 24), new MovementDeltas(1, 0), new MovementDeltas( 0,-1), thickCount);
            CarveDoorIfPresent(expandedData, new Point(0,  0), new MovementDeltas(0, 1), new MovementDeltas( 1, 0), thickCount);
            CarveDoorIfPresent(expandedData, new Point(24, 0), new MovementDeltas(0, 1), new MovementDeltas(-1, 0), thickCount);
        }



        private static void CarveDoorIfPresent(WallMatrix expandedData, Point point, MovementDeltas withinRow, MovementDeltas betweenRows, int thickCount)
        {
            int x = point.X;
            int y = point.Y;
            for(int i=0; i<25; i++)
            {
                if (expandedData.Read(x,y) == WallMatrixChar.Space)
                {
                    CarveDoorHole(expandedData, x, y, betweenRows, thickCount);
                }
                x += withinRow.dx;
                y += withinRow.dy;
            }
        }



        private static void CarveDoorHole(WallMatrix expandedData, int x, int y, MovementDeltas moveDelta, int thickCount)
        {
            while (thickCount > 0)
            {
                expandedData.Write(x, y, WallMatrixChar.Space);
                x += moveDelta.dx;
                y += moveDelta.dy;
                --thickCount;
            }
        }



        private static void SecondBrickIze(WallMatrix expandedData)
        {
            // Turn Electric areas into Brick leaving just an Electric outline.

            for (int y = 1; y < 24; ++y)
            {
                for (int x = 1; x < 24; ++x)
                {
                    if (SurroundedByWall8(expandedData, x, y))
                    {
                        expandedData.Write(x, y, WallMatrixChar.Brick);
                    }
                }
            }
        }



        private static bool SurroundedByWall4(WallMatrix wallMatrix, int x, int y) // TODO: We could be arty and call this instead.
        {
            return
                   wallMatrix.Read(x, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x, y + 1) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y) != WallMatrixChar.Space;
        }



        private static bool SurroundedByWall8(WallMatrix wallMatrix, int x, int y)
        {
            return
                   wallMatrix.Read(x, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x, y + 1) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y - 1) != WallMatrixChar.Space
                && wallMatrix.Read(x - 1, y + 1) != WallMatrixChar.Space
                && wallMatrix.Read(x + 1, y + 1) != WallMatrixChar.Space;
        }
    }
}
