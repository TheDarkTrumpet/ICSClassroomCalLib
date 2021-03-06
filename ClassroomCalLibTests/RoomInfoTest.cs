﻿using System;
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
         *</summary>
         */ 
        public void TestXMLLoadAll()
        {
            RoomInfo ri = new RoomInfo(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/classrooms.xml")));
            //RoomInfo ri = new RoomInfo();
            Assert.IsTrue(ri.LoadAll());
        }

        [TestMethod]
        public void TestICSFileLoadAll()
        {
            List<Room> myRooms = new Room[]{
            new Room
                {
                    RoomNumber = "RES-PHAR-129",
                    Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics"))
                }
            }.ToList();

            RoomInfo ri = new RoomInfo(myRooms);
            Assert.IsTrue(ri.LoadAll());
        }

        [TestMethod]
        public void TestLoadAllOfNoRooms()
        {
            List<Room> myRooms = new Room[] {}.ToList();
            RoomInfo ri = new RoomInfo(myRooms);
            Assert.IsFalse(ri.LoadAll());
        }

        [TestMethod]
        public void TestLoadOfSingleRoomWithDefaultURI()
        {
            RoomInfo ri = new RoomInfo(new Uri("../../fixture/classrooms.xml", UriKind.Relative));
            Assert.IsTrue(ri.LoadRoomByName("PHAR-129"));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestExceptionSingleLoad()
        {
            RoomInfo ri = new RoomInfo(new Uri("../../fixture/classrooms.xml", UriKind.Relative));
            Assert.IsTrue(ri.LoadRoomByName("omg hax hax"));
        } 

        [TestMethod]
        public void TestSerialization()
        {
            var myTestFile = "../../fixture/testserialization.xml";

            File.Delete(myTestFile);
            List<Room> myRooms = new Room[]
            {
                new Room
                {
                    RoomNumber = "RES-PHAR-129",
                    Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics"))
                },
                new Room
                {
                    RoomNumber = "RES-PHAR-226",
                    Uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/RES-PHAR-129.ics"))
                }
            }.ToList();

            RoomInfo ri = new RoomInfo(myRooms);
            ri.LoadAll();
            ri.SerializeCacheToFile(myTestFile);

            Assert.IsTrue(File.Exists(myTestFile));

            // Now load...
            RoomInfo ri2 = new RoomInfo(myRooms);
            ri.DeserializeCacheFromFile(myTestFile);

            //Longer assert...We should have the same output
            Assert.AreEqual(ri.GetRoomByName("RES-PHAR-129").BusyTimes(), ri2.GetRoomByName("RES-PHAR-129").BusyTimes());
            Assert.AreEqual(ri.GetRoomByName("RES-PHAR-226").BusyTimes(), ri2.GetRoomByName("RES-PHAR-226").BusyTimes());

            //Cleanup
            File.Delete(myTestFile);
        }

        [TestMethod]
        public void TestLoadXML()
        {
            //Assert.IsTrue(File.Exists("../../fixture/classrooms.xml"));
            Assert.IsTrue(File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "../../fixture/classrooms.xml")));
        }
    }
}
