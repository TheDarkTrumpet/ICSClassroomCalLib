using System;
using System.Collections;
using System.IO;
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
            SystemTime.Now = () => new DateTime(2013, 8, 7, 1, 0, 0);
            Room r = new Room {Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics")), RoomNumber = "RES-PHAR-129"};
            r.Load(r.Uri);
            DateTime nFirst = new DateTime(2013,8,8,15,0,0);
            DateTime nSecond = new DateTime(2013,8,8,17,0,0);
            IEnumerable<SimpleEvent> busyTimes = r.BusyTimes(nFirst, nSecond);

            Assert.AreEqual(busyTimes.Count(), 1);
            Assert.AreEqual(busyTimes.FirstOrDefault().EventStart, new DateTime(2013,8,8,15,30,0));
        }

        [TestMethod]
        public void TestBusyTimesFromMinFuture()
        {
            SystemTime.Now = () => new DateTime(2013, 8, 1, 1, 0, 0);
            Room r = new Room { Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics")), RoomNumber = "RES-PHAR-129" };
            r.Load(r.Uri);
            SystemTime.Now = () => new DateTime(2013, 8, 8, 15, 0, 0);
            IEnumerable<SimpleEvent> busyTimes = r.BusyTimes(120);

            //Assert
            Assert.AreEqual(busyTimes.Count(), 1);
            Assert.AreEqual(busyTimes.FirstOrDefault().EventStart, new DateTime(2013, 8, 8, 15, 30, 0));
        }

        [TestMethod]
        public void TestSimpleCache()
        {
            Room r = new Room { Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics")), RoomNumber = "RES-PHAR-129" };
            r.Load(r.Uri);

            List<SimpleEvent> se;

            SystemTime.Now = () => DateTime.Today; //without this line, this test fails if it runs after another test because of some weird bug
            se = r.CacheToSimple();
            Assert.AreEqual<int>(0, se.Count); //should evaluate to true after 9/4/2013 because that day is the last event in the fixture
            se.Clear();

            se = r.CacheToSimple(new DateTime(2013, 8, 8, 17, 0, 0));
            Assert.AreEqual<int>(2, se.Count);
            se.Clear();

            se = r.CacheToSimple(new DateTime(2013, 8, 26), new DateTime(2013, 9, 4));
            Assert.AreEqual<int>(5, se.Count);
            se.Clear();

            se = r.CacheToSimple(new DateTime(2013, 8, 1), 6);
            Assert.AreEqual<int>(2, se.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TypeUnloadedException))]
        public void TestFreeBusyToStringException()
        {
            Room r = new Room { Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics")), RoomNumber = "RES-PHAR-129" };
            //Missing load function here...
            Assert.AreEqual("", r.FreeBusyToString(new DateTime(2013, 8, 1, 10, 15, 0)));
        }

        [TestMethod]
        public void TestFreeBusyToString()
        {
            SystemTime.Now = () => new DateTime(2013, 8, 1, 1, 0, 0);           
            Room r = new Room { Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics")), RoomNumber = "RES-PHAR-129" };
            r.Load(r.Uri);
            Assert.AreEqual("Busy for 2 hours, and 45 minutes", r.FreeBusyToString(new DateTime(2013, 8, 1, 10, 15, 0)).ToString());
            //This may be best said as "free for about 4 days, and 21 hours
            Assert.AreEqual("Free for 4 days, and 21 hours", r.FreeBusyToString(new DateTime(2013, 8, 1, 13, 15, 0)).ToString());
        }
        
        [TestMethod]
        public void TestGetCalendar()
        {
            Room r = new Room { Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics")), RoomNumber = "RES-PHAR-129" };
            r.Load(r.Uri);
            IICalendar cal = r.GetCalendar();
            Assert.AreEqual<string>("Classroom Renovation Project", cal.Events.First().Summary);
            Assert.AreNotEqual<string>("jhbdvyceri", cal.Events.First().Summary); //makes sure that any arbitrary string matches the summary
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestSimpleCacheException()
        {
            Room r = new Room { Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics")), RoomNumber = "RES-PHAR-129" };
            //r.Load(r.Uri);

            List<SimpleEvent> se = r.CacheToSimple(new DateTime(2013, 8, 8, 17, 0, 0));
        }

        [TestMethod]
        public void TestUri()
        {
            Room r = new Room { Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics")), RoomNumber = "RES-PHAR-129" };
            Assert.AreEqual<Uri>(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics")), r.Uri); //tests getter

            r.Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-226.ics"));
            Assert.AreEqual<Uri>(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-226.ics")), r.Uri); //tests setter
        }
    }
}
