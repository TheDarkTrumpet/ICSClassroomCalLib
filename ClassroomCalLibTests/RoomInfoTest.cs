using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassroomCalLib.ical;
using ClassroomCalLib.util;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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
        public void TestDefaultURLLoadAll()
        {
            RoomInfo ri = new RoomInfo();
            Assert.IsTrue(ri.LoadAll());
        }

        [TestMethod]
        public void TestFileLoadAll()
        {
            IEnumerable<Room> myRooms = new Room[]{
            new Room
                {
                    RoomNumber = "RES-PHAR-129",
                    FPATHLocation =
                        new ICSPath(
                "../../fixture/RES-PHAR-129.ics")
                }
            }.AsEnumerable();

            RoomInfo ri = new RoomInfo(myRooms);
            Assert.IsTrue(ri.LoadAll("File"));
        }

        [TestMethod]
        public void TestLoadAllOfNoRooms()
        {
            IEnumerable<Room> myRooms = new Room[] {}.AsEnumerable();
            RoomInfo ri = new RoomInfo(myRooms);
            Assert.IsFalse(ri.LoadAll("File"));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestExceptionNullURILoadAll()
        {
            IEnumerable<Room> myRooms = new Room[]{
            new Room
                {
                    RoomNumber = "RES-PHAR-129",
                    FPATHLocation =
                        new ICSPath(
                "../../fixture/RES-PHAR-129.ics")
                }
            }.AsEnumerable();

            RoomInfo ri = new RoomInfo(myRooms);  //Default is URI, should throw an error
            ri.LoadAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestExceptionIncorrectURILoadAll()
        {
            IEnumerable<Room> myRooms = new Room[]{
            new Room
                {
                    RoomNumber = "RES-PHAR-129",
                    FPATHLocation =
                        new ICSPath(
                "../../fixture/RES-PHAR-129.ics")
                }
            }.AsEnumerable();

            RoomInfo ri = new RoomInfo(myRooms);  //Default is URI, should throw an error
            ri.LoadAll("IncorrectLoadOption");
        }

        [TestMethod]
        public void TestLoadOfSingleRoomWithDefaultURI()
        {
            RoomInfo ri = new RoomInfo();
            Assert.IsTrue(ri.LoadRoom("PHAR-129"));
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void TestExceptionSingleLoad()
        {
            RoomInfo ri = new RoomInfo();
            Assert.IsTrue(ri.LoadRoom("omg hax hax"));
        }

        [TestMethod]
        public void TestSerialization()
        {
            var myTestFile = "../../fixture/testserialization.xml";

            File.Delete(myTestFile);
            IEnumerable<Room> myRooms = new Room[]
            {
                new Room
                {
                    RoomNumber = "RES-PHAR-129",
                    FPATHLocation =
                        new ICSPath(
                            "../../fixture/RES-PHAR-129.ics")
                },
                new Room
                {
                    RoomNumber = "RES-PHAR-226",
                    FPATHLocation = new ICSPath("../../fixture/RES-PHAR-226.ics")
                }
            }.AsEnumerable();

            RoomInfo ri = new RoomInfo(myRooms);
            ri.LoadAll("File");
            ri.SerializeToFile(myTestFile);

            Assert.IsTrue(File.Exists(myTestFile));

            // Now load...
            RoomInfo ri2 = new RoomInfo(myRooms);
            ri.DeserializeFromFile(myTestFile);

            //Longer assert...We should have the same output
            Assert.AreEqual(ri.GetRoomByName("RES-PHAR-129").BusyTimes(), ri2.GetRoomByName("RES-PHAR-129").BusyTimes());
            Assert.AreEqual(ri.GetRoomByName("RES-PHAR-226").BusyTimes(), ri2.GetRoomByName("RES-PHAR-226").BusyTimes());

            //Cleanup
            File.Delete(myTestFile);
        }

        [TestMethod]
        public void TestLoadXML()
        {
            Assert.IsTrue(File.Exists("../../../ClassroomCalLib/XML/classrooms.xml"));
        }
    }
}
