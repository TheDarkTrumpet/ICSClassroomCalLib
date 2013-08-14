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
            FriendlyEvent e = new FriendlyEvent();

            Assert.AreEqual(e.DeltaToFriendly("Busy", DateTime.Now, DateTime.Now.AddSeconds(30)),
                "Busy for 30 seconds");
            Assert.AreEqual(e.DeltaToFriendly("Busy", DateTime.Now, DateTime.Now.AddMinutes(30)),
               "Busy for 30 minutes");
            Assert.AreEqual(e.DeltaToFriendly("Busy", DateTime.Now, DateTime.Now.AddHours(3)),
               "Busy for 3 hours, and 0 minutes");
            Assert.AreEqual(e.DeltaToFriendly("Busy", DateTime.Now, DateTime.Now.AddDays(3)),
               "Busy for 3 days, and 0 hours");
        }

        [TestMethod]
        public void TestFriendOutputFuzzy()
        {
            FriendlyEvent e = new FriendlyEvent();
            Assert.AreEqual("Busy for about 30 minutes", e.DeltaToFriendly("Busy", DateTime.Now, DateTime.Now.AddMinutes(30).AddSeconds(12)));
            Assert.AreEqual("Busy for about 31 minutes", e.DeltaToFriendly("Busy", DateTime.Now, DateTime.Now.AddMinutes(30).AddSeconds(52)));

            //Hours
            Assert.AreEqual("Busy for 3 hours, and 12 minutes",
                e.DeltaToFriendly("Busy", DateTime.Now, DateTime.Now.AddHours(3).AddMinutes(12)));
            Assert.AreEqual("Busy for 3 hours, and 52 minutes",
                e.DeltaToFriendly("Busy", DateTime.Now, DateTime.Now.AddHours(3).AddMinutes(52)));


            Assert.AreEqual("Busy for 3 days, and 12 hours",
                e.DeltaToFriendly("Busy", DateTime.Now, DateTime.Now.AddDays(3).AddHours(12)));
            Assert.AreEqual("Busy for 3 days, and 22 hours",
                e.DeltaToFriendly("Busy", DateTime.Now, DateTime.Now.AddDays(3).AddHours(22)));
        }
    }
}
