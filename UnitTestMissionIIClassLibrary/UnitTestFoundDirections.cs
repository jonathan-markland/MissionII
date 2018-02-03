using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MissionIIClassLibrary;

namespace UnitTestMissionIIClassLibrary
{
    [TestClass]
    public class UnitTestFoundDirections
    {
        [TestMethod]
        public void FoundDirections_Bit0_Set_Only()
        {
            var fd = new FoundDirections { DirectionsMask = 1, Count = 1 };
            Assert.IsTrue(fd.Choose(0) == 0);
        }

        [TestMethod]
        public void FoundDirections_Bit1_Set_Only()
        {
            var fd = new FoundDirections { DirectionsMask = 2, Count = 1 };
            Assert.IsTrue(fd.Choose(0) == 1);
        }

        [TestMethod]
        public void FoundDirections_Bit2_Set_Only()
        {
            var fd = new FoundDirections { DirectionsMask = 4, Count = 1 };
            Assert.IsTrue(fd.Choose(0) == 2);
        }

        [TestMethod]
        public void FoundDirections_Bit3_Set_Only()
        {
            var fd = new FoundDirections { DirectionsMask = 8, Count = 1 };
            Assert.IsTrue(fd.Choose(0) == 3);
        }

        [TestMethod]
        public void FoundDirections_Bit0_Bit3_Set_Only_Choose_0()
        {
            var fd = new FoundDirections { DirectionsMask = 8 | 1, Count = 2 };
            Assert.IsTrue(fd.Choose(0) == 0);
        }

        [TestMethod]
        public void FoundDirections_Bit0_Bit3_Set_Only_Choose_1()
        {
            var fd = new FoundDirections { DirectionsMask = 8 | 1, Count = 2 };
            Assert.IsTrue(fd.Choose(1) == 3);
        }

        [TestMethod]
        public void FoundDirections_Index_Out_Of_Range_Of_Results()
        {
            var fd = new FoundDirections { DirectionsMask = 1, Count = 1 };
            fd.Choose(1);
        }
    }
}
