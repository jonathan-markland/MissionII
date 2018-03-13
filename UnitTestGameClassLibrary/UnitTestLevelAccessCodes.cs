using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestGameClassLibrary
{
    [TestClass]
    public class UnitTestLevelAccessCodes
    {
        [TestMethod]
        public void Test_Access_Codes_Are_All_Different()
        {
            var theList = new List<string>();
            for(int i=1; i<=8; i++)
            {
                var thisLevelAccessCode = GameClassLibrary.Algorithms.LevelAccessCodes.GetForLevel(i);
                theList.Add(thisLevelAccessCode);
            }
            Assert.AreEqual(theList.Count, theList.Distinct().ToList().Count);
        }

        [TestMethod]
        public void Test_Access_Code_For_Level_1()
        {
            Assert.AreEqual("LRFU", GameClassLibrary.Algorithms.LevelAccessCodes.GetForLevel(1));
        }

        [TestMethod]
        public void Test_Access_Code_For_Level_2()
        {
            Assert.AreEqual("RFUD", GameClassLibrary.Algorithms.LevelAccessCodes.GetForLevel(2));
        }
    }
}
