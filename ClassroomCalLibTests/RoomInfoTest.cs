using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassroomCalLib.ical;

namespace ClassroomCalLibTests
{
    [TestClass]
    public class RoomInfoTest
    {
        [TestMethod]
        /**<summary>
         * This test will go through all the defaults we have, and load each URL.
         * This, by far, is the longest running test we have.</summary>
         */
        public void TestDefaultURLLoad()
        {
            RoomInfo ri = new RoomInfo();
            Assert.IsTrue(ri.Load());
        }
    }
}
