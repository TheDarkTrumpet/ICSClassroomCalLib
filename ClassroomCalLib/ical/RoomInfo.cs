using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ClassroomCalLib.util;

namespace ClassroomCalLib.ical
{
    public class RoomInfo
    {
        private IEnumerable<Room> myRooms;
        
        public RoomInfo()
        {
            
        }

        public RoomInfo(string pathToXML)
        {
            myRooms = loadRoomInfoFromXML(new Uri(pathToXML, UriKind.RelativeOrAbsolute));
        }

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

        public IEnumerable<Room> loadRoomInfoFromXML(Uri fileUri)
        {
            XDocument _myXDocument = XDocument.Load(fileUri.ToString());

            return _myXDocument.Root.Elements("Classroom").Select
               (ele => new Room
                  (
                    (string)ele.Element("RoomNumber"),
                    new ICSUri((string)ele.Element("UriLocation"))
                  )
            ).AsEnumerable();
        }


        public void SerializeCacheToFile(String FilePath)
        {
            XElement doc = new XElement("Rooms");
            doc.Add(new XElement("DTCached", DateTime.Now));
            foreach (Room r in GetAllRooms())
            {
                List<XElement> elements = new List<XElement>();
                IEnumerable<SimpleEvent> busyTimes = r.BusyTimes();
                foreach (SimpleEvent e in busyTimes)
                {
                    elements.Add(
                        new XElement("Event",
                            new XElement("EventName", e.EventName),
                            new XElement("StartTime", e.EventStart.ToString()),
                            new XElement("EndTime", e.EventStop.ToString())));
                }
                XElement f = new XElement("Room",
                    new XElement("Room", r.RoomNumber),
                    new XElement("Events", elements));
                doc.Add(f);
            };
            doc.Save(FilePath);
        }

        public void DeserializeCacheFromFile(String FilePath)
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
