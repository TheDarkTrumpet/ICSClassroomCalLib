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
            IEnumerable<IFreeBusyEntry> busyTimes = r.BusyTimes(nSecond, nFirst);

            Assert.AreEqual(busyTimes.Count(), 1);
            Assert.AreEqual(busyTimes.FirstOrDefault().Status.ToString(), "Busy");
            Assert.AreEqual(busyTimes.FirstOrDefault().StartTime, new DateTime(2013,8,8,15,30,0));
        }

        [TestMethod]
        public void TestBusyTimesFromMinFuture()
        {
            Room r = new Room { FPATHLocation = new ICSPath("../../fixture/RES-PHAR-129.ics"), RoomNumber = "RES-PHAR-129" };
            r.Load(r.FPATHLocation);
            SystemTime.Now = () => new DateTime(2013, 8, 8, 15, 0, 0);
            IEnumerable<IFreeBusyEntry> busyTimes = r.BusyTimes(120);

            //Assert
            Assert.AreEqual(busyTimes.Count(), 1);
            Assert.AreEqual(busyTimes.FirstOrDefault().Status.ToString(), "Busy");
            Assert.AreEqual(busyTimes.FirstOrDefault().StartTime, new DateTime(2013, 8, 8, 15, 30, 0));
        }
    }
}
