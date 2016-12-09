using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Gware.Common.Test
{
    [TestClass]
    public class HelperMethodsTest
    {
        #region ----- Overlap -----

        [TestMethod]
        public void OverlapTestEqualInt()
        {
            Assert.IsTrue(HelperMethods.Overlap<int>(0,5,0,5));
        }
        [TestMethod]
        public void OverlapTestBeforeInt()
        {
            Assert.IsFalse(HelperMethods.Overlap<int>(0, 5, 6, 7));
        }
        [TestMethod]
        public void OverlapTestAfterInt()
        {
            Assert.IsFalse(HelperMethods.Overlap<int>(6, 7, 0, 5));
        }
        [TestMethod]
        public void OverlapTestStartInsideInt()
        {
            Assert.IsTrue(HelperMethods.Overlap<int>(3, 7, 0, 5));
        }
        [TestMethod]
        public void OverlapTestEndInsideInt()
        {
            Assert.IsTrue(HelperMethods.Overlap<int>(-2, 3, 0, 5));
        }
        [TestMethod]
        public void OverlapTestBothInsideInt()
        {
            Assert.IsTrue(HelperMethods.Overlap<int>(2, 3, 0, 5));
        }
        [TestMethod]
        public void OverlapTestEncompassInt()
        {
            Assert.IsTrue(HelperMethods.Overlap<int>(-1, 7, 0, 5));
        }

        #endregion ----- Overlap
    }
}
