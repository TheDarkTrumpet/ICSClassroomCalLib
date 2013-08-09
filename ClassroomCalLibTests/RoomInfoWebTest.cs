using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassroomCalLib.ical;

namespace ClassroomCalLibTests
{
    [TestClass]
    public class RoomInfoWebTest
    {
        [TestMethod]
        public void TestURLLoad()
        {
            RoomInfoWeb ri = new RoomInfoWeb();

            ri.Load();
        }
    }
}
