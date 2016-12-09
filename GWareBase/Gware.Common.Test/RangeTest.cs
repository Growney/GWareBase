using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gware.DataStructures.Common;

namespace Gware.Common.Test
{
    [TestClass]
    public class RangeTest
    {
        #region ----- Constructor -----
        [TestMethod]
        public void ConstructorTestFullRangeDate()
        {
            DateTime start = new DateTime(1990, 4, 19, 12, 0, 0);
            DateTime end = new DateTime(1993,8,15,12,0,0);
            Range<DateTime> test = new Range<DateTime>(start, end);

            Assert.AreEqual<DateTime>(start, test.Start);
            Assert.AreEqual<DateTime>(end, test.End);
        }

        [TestMethod]
        public void ConstructorTestSingleDate()
        {
            DateTime start = new DateTime(1990, 4, 19, 12, 0, 0);
            Range<DateTime> test = new Range<DateTime>(start);

            Assert.AreEqual<DateTime>(start, test.Start);
            Assert.AreEqual<DateTime>(start, test.End);
        }

        [TestMethod]
        public void ConstructorTestFullRangeInt()
        {
            int start = 0;
            int end = 40;
            Range<int> test = new Range<int>(start,end);

            Assert.AreEqual<int>(start, test.Start);
            Assert.AreEqual<int>(end, test.End);
        }

        [TestMethod]
        public void ConstructorTestSingleInt()
        {
            int start = 0;
            Range<int> test = new Range<int>(start);

            Assert.AreEqual<int>(start, test.Start);
        }

        #endregion ----- Constructor -----

        #region ----- Reverse -----
        [TestMethod]
        public void ReverseTestFalseInt()
        {
            Range<int> test = new Range<int>(0,5);

            Assert.IsFalse(test.Reverse);
        }

        [TestMethod]
        public void ReverseTestTrueInt()
        {
            Range<int> test = new Range<int>(5, 0);

            Assert.IsTrue(test.Reverse);
        }

        [TestMethod]
        public void ReverseTestFalseDateTime()
        {
            DateTime start = new DateTime(1990, 4, 19, 12, 0, 0);
            DateTime end = new DateTime(1993, 8, 15, 12, 0, 0);

            Range<DateTime> test = new Range<DateTime>(start, end);

            Assert.IsFalse(test.Reverse);
        }

        [TestMethod]
        public void ReverseTestTrueDateTime()
        {
            DateTime start = new DateTime(1990, 4, 19, 12, 0, 0);
            DateTime end = new DateTime(1993, 8, 15, 12, 0, 0);

            Range<DateTime> test = new Range<DateTime>(end, start);
            
            Assert.IsTrue(test.Reverse);
        }

        [TestMethod]
        public void ReverseTestEqualDateTime()
        {
            DateTime start = new DateTime(1990, 4, 19, 12, 0, 0);

            Range<DateTime> test = new Range<DateTime>(start);

            Assert.IsFalse(test.Reverse);
        }
        #endregion ----- Reverse -----

        #region ----- CompareTo -----

        [TestMethod]
        public void CompareToTestEqualInt()
        {
            Range<int> first = new Range<int>(0,5);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.CompareTo(second) == 0);
        }

        [TestMethod]
        public void CompareToTestLessStartInt()
        {
            Range<int> first = new Range<int>(-5, 5);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void CompareToTestLessEndInt()
        {
            Range<int> first = new Range<int>(0, 4);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void CompareToTestLessBothInt()
        {
            Range<int> first = new Range<int>(-3, 4);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void CompareToTestGreaterStartInt()
        {
            Range<int> first = new Range<int>(1, 5);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.CompareTo(second) > 0);
        }

        [TestMethod]
        public void CompareToTestGreaterEndInt()
        {
            Range<int> first = new Range<int>(0, 6);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.CompareTo(second) > 0);
        }

        [TestMethod]
        public void CompareToTestGreaterBothInt()
        {
            Range<int> first = new Range<int>(1, 6);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.CompareTo(second) > 0);
        }
        #endregion ----- CompareTo -----

        #region ----- Overlaps -----
        [TestMethod]
        public void OverlapsTestEqualInt()
        {
            Range<int> first = new Range<int>(0, 5);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestBeforeInt()
        {
            Range<int> first = new Range<int>(0, 5);
            Range<int> second = new Range<int>(6, 7);

            Assert.IsFalse(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestAfterInt()
        {
            Range<int> first = new Range<int>(6, 7);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsFalse(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestStartInsideInt()
        {
            Range<int> first = new Range<int>(3, 7);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestEndInsideInt()
        {
            Range<int> first = new Range<int>(-2, 3);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestBothInsideInt()
        {
            Range<int> first = new Range<int>(2, 3);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestEncompassInt()
        {
            Range<int> first = new Range<int>(-1, 7);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestEqualReverseInt()
        {
            Range<int> first = new Range<int>(5, 0);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestBeforeReverseInt()
        {
            Range<int> first = new Range<int>(5, 0);
            Range<int> second = new Range<int>(6, 7);

            Assert.IsFalse(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestAfterReverseInt()
        {
            Range<int> first = new Range<int>(7, 6);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsFalse(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestStartInsideReverseInt()
        {
            Range<int> first = new Range<int>(7, 3);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestEndInsideReverseInt()
        {
            Range<int> first = new Range<int>(3, -2);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestBothInsideReverseInt()
        {
            Range<int> first = new Range<int>(3, 2);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.Overlaps(second));
        }
        [TestMethod]
        public void OverlapsTestEncompassReverseInt()
        {
            Range<int> first = new Range<int>(7, -1);
            Range<int> second = new Range<int>(0, 5);

            Assert.IsTrue(first.Overlaps(second));
        }
        

        #endregion ----- Overlaps -----
    }
}
