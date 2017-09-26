using Gware.Common.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Testing.Common.DataStructures
{
    [TestClass]
    public class CircularBufferTest
    {

        [TestMethod]
        public void WriteCount()
        {
            CircularBuffer<int> buffer = new CircularBuffer<int>(10);
            for (int i = 0; i < 5; i++)
            {
                buffer.Write(i);
            }
            Assert.AreEqual(5, buffer.UnreadCount);
        }

        [TestMethod]
        public void WriteReadCount()
        {
            CircularBuffer<int> buffer = new CircularBuffer<int>(10);
            for (int i = 0; i < 5; i++)
            {
                buffer.Write(i);
            }

            buffer.Read();
            buffer.Read();
            Assert.AreEqual(3, buffer.UnreadCount);
        }
    }
}
