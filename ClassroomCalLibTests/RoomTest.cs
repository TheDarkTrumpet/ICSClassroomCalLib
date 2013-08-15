using System;
using System.Collections;
using DDay.iCal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassroomCalLib.ical;
using ClassroomCalLib.util;
using System.Collections.Generic;
using System.Linq;

namespace ClassroomCalLibTests
{
    [TestClass]
    public class RoomTest
    {
        [TestMethod]
        public void TestBusyTimes()
        {
            Room r = new Room {FPATHLocation = new ICSPath("../../fixture/RES-PHAR-129.ics"), RoomNumber = "RES-PHAR-129"};
            r.Load(r.FPATHLocation);
            DateTime nFirst = new DateTime(2013,8,8,15,0,0);
            DateTime nSecond = new DateTime(2013,8,8,17,0,0);
            IEnumerable<SimpleEvent> busyTimes = r.BusyTimes(nSecond, nFirst);

            Assert.AreEqual(busyTimes.Count(), 1);
            Assert.AreEqual(busyTimes.FirstOrDefault().EventStart, new DateTime(2013,8,8,15,30,0));
        }

        [TestMethod]
        public void TestBusyTimesFromMinFuture()
        {
            Room r = new Room { FPATHLocation = new ICSPath("../../fixture/RES-PHAR-129.ics"), RoomNumber = "RES-PHAR-129" };
            r.Load(r.FPATHLocation);
            SystemTime.Now = () => new DateTime(2013, 8, 8, 15, 0, 0);
            IEnumerable<SimpleEvent> busyTimes = r.BusyTimes(120);

            //Assert
            Assert.AreEqual(busyTimes.Count(), 1);
            Assert.AreEqual(busyTimes.FirstOrDefault().EventStart, new DateTime(2013, 8, 8, 15, 30, 0));
        }

        [TestMethod]
        public void TestSimpleCache()
        {
            Room r = new Room { FPATHLocation = new ICSPath("../../fixture/RES-PHAR-129.ics"), RoomNumber = "RES-PHAR-129" };
            r.Load(r.FPATHLocation);

            List<SimpleEvent> se = r.CacheToSimple(new DateTime(2013, 8, 8, 17, 0, 0));
            
            //Assert
            Assert.AreEqual(se.Count, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(TypeUnloadedException))]
        public void TestFreeBusyToStringException()
        {
            Room r = new Room { FPATHLocation = new ICSPath("../../fixture/RES-PHAR-129.ics"), RoomNumber = "RES-PHAR-129" };
            //Missing load function here...
            Assert.AreEqual("", r.FreeBusyToString(new DateTime(2013, 8, 1, 10, 15, 0)));
        }

        [TestMethod]
        public void TestFreeBusyToString()
        {
            Room r = new Room { FPATHLocation = new ICSPath("../../fixture/RES-PHAR-129.ics"), RoomNumber = "RES-PHAR-129" };
            r.Load(r.FPATHLocation);
            Assert.AreEqual("Busy for 2 hours, and 45 minutes", r.FreeBusyToString(new DateTime(2013, 8, 1, 10, 15, 0)).ToString());
            //This may be best said as "free for about 4 days, and 21 hours
            Assert.AreEqual("Free for 4 days, and 21 hours", r.FreeBusyToString(new DateTime(2013, 8, 1, 13, 15, 0)).ToString());
        }
    }
}
