using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public static class LevelExpander
    {
        public static void ExpandWallsInWorld(WorldWallData theWorld)
        {
            foreach(var thisLevel in theWorld.Levels)
            {
                foreach(var thisRoom in thisLevel.Rooms)
                {
                    thisRoom.WallData = ExpandWalls(thisRoom.WallData);
                }
            }
        }



        public static List<string> ExpandWalls(List<string> wallData)
        {
            // 789       78889
            // 456 ----> 45556
            // 123       45556
            //           45556
            //           12223

            var resultList = new List<string>();

            for(int y=0; y<Constants.SourceFileCharsVertically; y += Constants.ClusterSide)
            {
                resultList.Add(ExpandRow(wallData[y]));
                resultList.Add(ExpandRow(wallData[y+1]));
                resultList.Add(ExpandRow(wallData[y+1]));
                resultList.Add(ExpandRow(wallData[y+1]));
                resultList.Add(ExpandRow(wallData[y+2]));
            }

            return resultList;
        }



        public static string ExpandRow(string levelSourceFileRow)
        {
            var resultStr = String.Empty;  // TODO: Garbage optimisation

            for ( int x=0; x < Constants.SourceFileCharsHorizontally; x += Constants.ClusterSide )
            {
                var c1 = RemapChar(levelSourceFileRow[x]);
                var c2 = RemapChar(levelSourceFileRow[x+1]);
                var c3 = RemapChar(levelSourceFileRow[x+2]);
                resultStr += c1 + c2 + c2 + c2 + c3;
            }

            return resultStr;
        }



        public static string RemapChar(char c)
        {
            return c == ' ' ? " " : "#";
        }
    }
}
