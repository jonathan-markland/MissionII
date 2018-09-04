
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameClassLibrary.Math; // TODO: move out to GameClssLinbary tests asembly

namespace UnitTestMissionIIClassLibrary
{
    [TestClass]
    public class UnitTestFoundDirections
    {
        #region Test scenario setup

        private static FoundDirections Scenario_1()
        {
            // This scenario returns 1 direction (UP).
            // UP is when bit 0 = 1.
            // The Count==1 because only ONE direction is returned.
            return new FoundDirections(1, 1);
        }

        private static FoundDirections Scenario_8_1()
        {
            // This scenario returns 2 directions (UP and DOWN-RIGHT).
            // UP is when bit 0 = 1.
            // DOWN-RIGHT is when bit 3 = 1.
            // The Count==2 because TWO directions are returned.
            return new FoundDirections(8 | 1, 2);
        }

        private static FoundDirections Scenario_16_8_2()
        {
            // This scenario returns 3 directions (UP and DOWN-RIGHT).
            // UP-RIGHT is when bit 1 = 1.
            // DOWN-RIGHT is when bit 3 = 1.
            // DOWN is when bit 4 = 1.
            // The Count==3 because THREE directions are returned.
            return new FoundDirections(16 | 8 | 2, 3);
        }

        private static FoundDirections Scenario_128()
        {
            // This scenario returns 1 direction (UP-LEFT).
            // UP-LEFT is when bit 7 = 1.
            // The Count==1 because only ONE direction is returned.
            return new FoundDirections(128, 1);
        }

        private static FoundDirections Scenario_128_64_32_16_8_4_2_1()
        {
            // This scenario returns all 8 directions.
            // The Count==8 therefore.
            return new FoundDirections(128 | 64 | 32 | 16 | 8 | 4 | 2 | 1, 8);
        }

        private static FoundDirections Scenario_No_Directions()
        {
            // This scenario returns no directions.
            // The Count==0 therefore.
            return new FoundDirections(0, 0);
        }

        #endregion

        #region Success tests

        [TestMethod]
        public void From_FoundDirections_1_Choose_0_Succeeds()
        {
            Assert.IsTrue(Scenario_1().Choose(0) == 0);
        }

        [TestMethod]
        public void From_All_8_Ways_With_Only_One_Direction_Returned_Choose_Each_In_Turn_Succeeds()
        {
            for (int i = 0; i < 8; i++)
            {
                var fd = new FoundDirections( 1 << i, 1 );
                Assert.IsTrue(fd.Choose(0) == i);
            }
        }

        [TestMethod]
        public void From_FoundDirections_8_1_Choose_0_Succeeds()
        {
            Assert.IsTrue(Scenario_8_1().Choose(0) == 0);
        }

        [TestMethod]
        public void From_FoundDirections_8_1_Choose_1_Succeeds()
        {
            Assert.IsTrue(Scenario_8_1().Choose(1) == 3);
        }

        [TestMethod]
        public void From_FoundDirections_16_8_2_Choose_0_Succeeds()
        {
            Assert.IsTrue(Scenario_16_8_2().Choose(0) == 1);
        }

        [TestMethod]
        public void From_FoundDirections_16_8_2_Choose_1_Succeeds()
        {
            Assert.IsTrue(Scenario_16_8_2().Choose(1) == 3);
        }

        [TestMethod]
        public void From_FoundDirections_16_8_2_Choose_2_Succeeds()
        {
            Assert.IsTrue(Scenario_16_8_2().Choose(2) == 4);
        }

        [TestMethod]
        public void From_FoundDirections_128_Choose_0_Succeeds()
        {
            Assert.IsTrue(Scenario_128().Choose(0) == 7);
        }

        [TestMethod]
        public void From_FoundDirections_128_64_32_16_8_4_2_1_Choose_0_Succeeds()
        {
            Assert.IsTrue(Scenario_128_64_32_16_8_4_2_1().Choose(0) == 0);
        }

        [TestMethod]
        public void From_FoundDirections_128_64_32_16_8_4_2_1_Choose_7_Succeeds()
        {
            Assert.IsTrue(Scenario_128_64_32_16_8_4_2_1().Choose(7) == 7);
        }

        #endregion

        #region Requested index beyond range of available results

        [TestMethod]
        [ExpectedException(typeof(FoundDirectionsException))]
        public void From_FoundDirections_1_Choose_1_Fails()
        {
            Scenario_1().Choose(1);
        }

        [TestMethod]
        [ExpectedException(typeof(FoundDirectionsException))]
        public void From_FoundDirections_8_1_Choose_2_Fails()
        {
            Scenario_8_1().Choose(2);
        }

        [TestMethod]
        [ExpectedException(typeof(FoundDirectionsException))]
        public void From_FoundDirections_16_8_2_Choose_3_Fails()
        {
            Scenario_16_8_2().Choose(3);
        }

        [TestMethod]
        [ExpectedException(typeof(FoundDirectionsException))]
        public void From_FoundDirections_128_Choose_1_Fails()
        {
            Scenario_128().Choose(1);
        }

        [TestMethod]
        [ExpectedException(typeof(FoundDirectionsException))]
        public void From_FoundDirections_128_64_32_16_8_4_2_1_Choose_8_Fails()
        {
            Scenario_128_64_32_16_8_4_2_1().Choose(8);
        }

        [TestMethod]
        [ExpectedException(typeof(FoundDirectionsException))]
        public void From_No_FoundDirections_Choose_0_Fails()
        {
            Scenario_No_Directions().Choose(0);
        }

        [TestMethod]
        [ExpectedException(typeof(FoundDirectionsException))]
        public void From_No_FoundDirections_Choose_1_Fails()
        {
            Scenario_No_Directions().Choose(1);
        }

        #endregion
    }
}
