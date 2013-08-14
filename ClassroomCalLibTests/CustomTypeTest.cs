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
            Assert.AreEqual("Busy for 30 seconds", new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddSeconds(30)).ToString());
            Assert.AreEqual("Busy for 30 minutes", new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddMinutes(30)).ToString());
            Assert.AreEqual("Busy for 3 hours, and 0 minutes", new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddHours(3)).ToString());
            Assert.AreEqual("Busy for 3 days, and 0 hours", new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddDays(3)).ToString());
        }

        [TestMethod]
        public void TestFriendOutputFuzzy()
        {
            Assert.AreEqual("Busy for about 30 minutes", new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddMinutes(30).AddSeconds(12)).ToString());
            Assert.AreEqual("Busy for about 31 minutes", new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddMinutes(30).AddSeconds(52)).ToString());

            //Hours
            Assert.AreEqual("Busy for 3 hours, and 12 minutes",
                new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddHours(3).AddMinutes(12)).ToString());
            Assert.AreEqual("Busy for 3 hours, and 52 minutes",
                new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddHours(3).AddMinutes(52)).ToString());


            Assert.AreEqual("Busy for 3 days, and 12 hours",
                new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddDays(3).AddHours(12)).ToString());
            Assert.AreEqual("Busy for 3 days, and 22 hours",
                new FriendlyEvent("Busy", DateTime.Now, DateTime.Now.AddDays(3).AddHours(22)).ToString());
        }
    }
}
