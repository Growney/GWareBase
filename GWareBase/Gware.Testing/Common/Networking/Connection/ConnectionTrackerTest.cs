using Gware.Common.Networking.Connection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Testing.Common.Networking.Connection
{
    [TestClass]
    public class ConnectionTrackerTest
    {
        [TestMethod]
        public void InOrder()
        {
            ConnectionTracker tracker = new ConnectionTracker();
            tracker.UpdateRemoteSequence(1);
            tracker.UpdateRemoteSequence(2);
            tracker.UpdateRemoteSequence(3);
            tracker.UpdateRemoteSequence(4);
            Assert.AreEqual(4,tracker.RemoteSequence);
            Assert.AreEqual((uint)0b0000_0000_0000_1111, tracker.Ack);
        }

        [TestMethod]
        public void Alternating()
        {
            ConnectionTracker tracker = new ConnectionTracker();
            tracker.UpdateRemoteSequence(1);
            tracker.UpdateRemoteSequence(3);
            tracker.UpdateRemoteSequence(5);
            tracker.UpdateRemoteSequence(7);
            Assert.AreEqual(7, tracker.RemoteSequence);
            Assert.AreEqual((uint)0b0000_0000_0101_0101, tracker.Ack);
        }

        [TestMethod]
        public void OverflowGap()
        {
            ConnectionTracker tracker = new ConnectionTracker();
            tracker.UpdateRemoteSequence(0);
            tracker.UpdateRemoteSequence(15);
            tracker.UpdateRemoteSequence(50);
            tracker.UpdateRemoteSequence(53);
            Assert.AreEqual(53, tracker.RemoteSequence);
            Assert.AreEqual((uint)0b0000_0000_0000_1001, tracker.Ack);
        }

        [TestMethod]
        public void ReverseOrder()
        {
            ConnectionTracker tracker = new ConnectionTracker();
            tracker.UpdateRemoteSequence(3);
            tracker.UpdateRemoteSequence(2);
            tracker.UpdateRemoteSequence(1);
            tracker.UpdateRemoteSequence(0);
            Assert.AreEqual(3, tracker.RemoteSequence);
            Assert.AreEqual((uint)0b0000_0000_0000_1111, tracker.Ack);
        }

        [TestMethod]
        public void ReverseOrderAlternating()
        {
            ConnectionTracker tracker = new ConnectionTracker();
            tracker.UpdateRemoteSequence(7);
            tracker.UpdateRemoteSequence(5);
            tracker.UpdateRemoteSequence(3);
            tracker.UpdateRemoteSequence(1);
            Assert.AreEqual(7, tracker.RemoteSequence);
            Assert.AreEqual((uint)0b0000_0000_0101_0101, tracker.Ack);
        }

        [TestMethod]
        public void ReverseOrderGap()
        {
            ConnectionTracker tracker = new ConnectionTracker();
            tracker.UpdateRemoteSequence(53);
            tracker.UpdateRemoteSequence(50);
            tracker.UpdateRemoteSequence(15);
            tracker.UpdateRemoteSequence(12);
            Assert.AreEqual(53, tracker.RemoteSequence);
            Assert.AreEqual((uint)0b0000_0000_0000_1001, tracker.Ack);
        }

        [TestMethod]
        public void InOrderOverflow()
        {
            ConnectionTracker tracker = new ConnectionTracker(null, ushort.MaxValue - 2);
            tracker.UpdateRemoteSequence(ushort.MaxValue - 2);
            tracker.UpdateRemoteSequence(ushort.MaxValue - 1);
            tracker.UpdateRemoteSequence(ushort.MaxValue);
            ushort nextValue = unchecked((ushort)(ushort.MaxValue + (ushort)1));
            tracker.UpdateRemoteSequence(nextValue);
            Assert.AreEqual(0, tracker.RemoteSequence);
            Assert.AreEqual((uint)0b0000_0000_0000_1111, tracker.Ack);
        }

        [TestMethod]
        public void ReverseOverflow()
        {
            ConnectionTracker tracker = new ConnectionTracker(null, ushort.MaxValue - 50);
            tracker.UpdateRemoteSequence(0);
            tracker.UpdateRemoteSequence(ushort.MaxValue);
            tracker.UpdateRemoteSequence(ushort.MaxValue - 1);
            tracker.UpdateRemoteSequence(ushort.MaxValue - 2);
            Assert.AreEqual(0, tracker.RemoteSequence);
            Assert.AreEqual((uint)0b0000_0000_0000_1111, tracker.Ack);
        }
    }
}
