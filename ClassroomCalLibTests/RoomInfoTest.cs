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
        public void TestDefaultURLLoad()
        {
            RoomInfo ri = new RoomInfo();
            Assert.IsTrue(ri.Load());
        }

        [TestMethod]
        public void TestFileLoad()
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
            Assert.IsTrue(ri.Load("File"));
        }

        [TestMethod]
        public void TestLoadOfNoRooms()
        {
            IEnumerable<Room> myRooms = new Room[] {}.AsEnumerable();
            RoomInfo ri = new RoomInfo(myRooms);
            Assert.IsFalse(ri.Load("File"));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestIncorrectLoadOption()
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
            ri.Load();
        }
    }
}
