using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassroomCalLib.ical;
using ClassroomCalLib.util;
using System.Collections.Generic;
using System.Linq;

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
            Assert.IsTrue(ri.LoadRoom("RES-PHAR-129"));
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
            
        }
    }
}
