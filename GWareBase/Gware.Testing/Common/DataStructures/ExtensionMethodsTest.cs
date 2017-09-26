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
    public class ExtensionMethodsTest
    {
        [TestMethod]
        public void GetNextPointerNegativeOne()
        {
            int[] array = { 1, 2, 3 };
            Assert.AreEqual(0, array.GetNextPointer(-1));
        }
    
    }
}
