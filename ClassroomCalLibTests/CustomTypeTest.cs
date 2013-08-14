using System;
using ClassroomCalLib.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassroomCalLibTests
{
    [TestClass]
    public class CustomTypeTest
    {
        [TestMethod]
        public void TestFriendlyOutputExact()
        {
            Assert.AreEqual(new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddSeconds(30)),
                "Busy for 30 seconds");
            Assert.AreEqual(new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddMinutes(30)),
               "Busy for 30 minutes");
            Assert.AreEqual(new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddHours(3)),
               "Busy for 3 hours, and 0 minutes");
            Assert.AreEqual(new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddDays(3)),
               "Busy for 3 days, and 0 hours");
        }

        [TestMethod]
        public void TestFriendOutputFuzzy()
        {
            Assert.AreEqual("Busy for about 30 minutes", new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddMinutes(30).AddSeconds(12)));
            Assert.AreEqual("Busy for about 31 minutes", new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddMinutes(30).AddSeconds(52)));

            //Hours
            Assert.AreEqual("Busy for 3 hours, and 12 minutes",
                new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddHours(3).AddMinutes(12)));
            Assert.AreEqual("Busy for 3 hours, and 52 minutes",
                new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddHours(3).AddMinutes(52)));


            Assert.AreEqual("Busy for 3 days, and 12 hours",
                new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddDays(3).AddHours(12)));
            Assert.AreEqual("Busy for 3 days, and 22 hours",
                new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddDays(3).AddHours(22)));
        }
    }
}
