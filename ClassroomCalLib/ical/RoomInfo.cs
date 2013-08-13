using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ClassroomCalLib.util;
using DDay.iCal;

namespace ClassroomCalLib.ical
{
    public class RoomInfo
    {
        private IEnumerable<Room> myRooms = new Room[]{
            new Room
                {
                    RoomNumber = "RES-PHAR-129",
                    URILocation =
                        new ICSUri(
                "http://email.uiowa.edu/owa/calendar/1e6dbdb8e4814563ab994449345e95d4@iowa.uiowa.edu/5a824eb9eed54decbcc295e54102622c11906123505361280052/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-226",
                    URILocation =
                        new ICSUri(
                "http://email.uiowa.edu/owa/calendar/9357b43b5d12499eb6401e018008d778@iowa.uiowa.edu/10f19757292f4032940035bd6bf2c9278625850918483163074/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-S538",
                    URILocation =
                        new ICSUri(
                "http://email.uiowa.edu/owa/calendar/6360779ea39448228938d8af2ea1fdb0@iowa.uiowa.edu/4faef19495c34caebd3d1150ef4f0e8412999831582725087249/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-S543",
                    URILocation =
                        new ICSUri(
                "http://email.uiowa.edu/owa/calendar/a240763f0c3e42f5bdf7abbfed04bcf9@iowa.uiowa.edu/8229b275c732441998e33566a87d99e18120430139117729276/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-S545",
                    URILocation =
                        new ICSUri(
                "http://email.uiowa.edu/owa/calendar/40b5b7aa4292485288df207b10b67df1@iowa.uiowa.edu/dc27ae284f1c4776808b9e43b03faaae4944822449095139734/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-S552",
                    URILocation =
                        new ICSUri(
                "http://email.uiowa.edu/owa/calendar/1e2f1b0998fe4005bdd71fabed0d10d9@iowa.uiowa.edu/42deb627faf9417ba6345c3d723aad678591608703955087174/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-SOPH-100A",
                    URILocation =
                        new ICSUri(
                "http://email.uiowa.edu/owa/calendar/5dbd85075a2c4f1282f26f83fe4a5441@iowa.uiowa.edu/c79da2e03cf64036aa7489a9672a9cd15569082608891722194/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-XOPH-100B",
                    URILocation =
                        new ICSUri(
                "http://email.uiowa.edu/owa/calendar/dfca5bf180484ed589e55cc3a8e6f239@iowa.uiowa.edu/5fa0fb650082495e9e6add737cb304fc1655299616341240835/calendar.ics")
                }
        }.AsEnumerable();

        public RoomInfo() { }
        public RoomInfo(IEnumerable<Room>roomStructure)
        {
            myRooms = roomStructure;
        }

        public bool LoadAll(string type="URI")
        {
            bool success = false;
            foreach (Room mr in myRooms)
            {
                success = success | _GenericLoader(mr, type);
            }
            return success;
        }

        public bool LoadRoom(string room, string type = "URI")
        {
            Room mr = GetRoomByName(room);
            if (mr == null)
            {
                throw new NullReferenceException("Room not found in the collection of available rooms");
            }

            return _GenericLoader(mr, type);
        }

        private bool _GenericLoader(Room RoomToLoad, string type)
        {
            bool success = false;
            if (type == "URI")
            {
                success = success | RoomToLoad.Load(RoomToLoad.URILocation);
            }
            else if (type == "File")
            {
                success = success | RoomToLoad.Load(RoomToLoad.FPATHLocation);
            }
            else
            {
                throw new ArgumentException("Acceptable types of URI and File are allowed");
            }
            return success;
        }

        public Room GetRoomByName(string name)
        {
            //There should *never* be duplicate rooms
            return myRooms.FirstOrDefault(x => x.RoomNumber == name);
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return myRooms;
        }


        public void SerializeToFile(String FilePath)
        {
            XElement doc = new XElement("Rooms");

            foreach (Room r in GetAllRooms())
            {
                List<XElement> elements = new List<XElement>();
                IEnumerable<SimpleEvent> busyTimes = r.BusyTimes(new DateTime(2013, 8, 14, 17, 0, 0),
                    new DateTime(2013, 8, 8, 17, 0, 0));
                foreach (SimpleEvent e in busyTimes)
                {
                    elements.Add(
                        new XElement("Event",
                            new XElement("EventName", e.EventName),
                            new XElement("StartTime", e.EventStart.ToLongTimeString()),
                            new XElement("EndTime", e.EventStop.ToLongTimeString())));
                }
                XElement f = new XElement("Room",
                    new XElement("Room", r.RoomNumber),
                    new XElement("CachedTime", DateTime.Now),
                    new XElement("Events", elements));
                doc.Add(f);
            }
            doc.Save(FilePath);
        }

        public void DeserializeFromFile(String FilePath)
        {
            XElement doc = XElement.Load(FilePath);
            foreach (XElement xe in doc.Elements("Room"))
            {
                Room myRoom = GetRoomByName(xe.Element("Room").Value);
                var xe2 = xe.Element("Events").Elements("Event");
                List<SimpleEvent> events = new List<SimpleEvent>();

                myRoom.RoomNumber = xe.Element("Room").Value;

                foreach (XElement xe3 in xe2)
                {
                    DateTime EStart = DateTime.Parse(xe3.Element("StartTime").Value);
                    DateTime EStop = DateTime.Parse(xe3.Element("EndTime").Value);


                    events.Add(new SimpleEvent
                    {
                        EventName = xe3.Element("EventName").Value,
                        EventStart = EStart,
                        EventStop = EStop,
                        Status = "Busy"
                    });
                }
                myRoom.setCache(events);
            }
        }
    }
}
