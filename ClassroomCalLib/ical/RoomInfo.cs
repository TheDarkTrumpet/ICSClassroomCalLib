﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ClassroomCalLib.util;

namespace ClassroomCalLib.ical
{
    public class RoomInfo
    {
        private List<Room> myRooms;

        public RoomInfo(Uri pathToXML)
        {
            myRooms = loadRoomInfoFromXML(pathToXML);
        }

        public RoomInfo(List<Room>roomStructure)
        {
            myRooms = roomStructure;
        }

        public bool LoadAll()
        {
            bool success = false;
            
            foreach (Room mr in myRooms)
            {
                success = success | mr.Load(mr.Uri);
            }
            return success;
        }

        public bool LoadRoomByName(string room)
        {
            Room mr = GetRoomByName(room);
            if (mr == null)
            {
                throw new NullReferenceException("Room not found in the collection of available rooms");
            }

            return false | mr.Load(mr.Uri);
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

        public List<Room> loadRoomInfoFromXML(Uri fileUri)
        {
            XDocument myXDocument = XDocument.Load(fileUri.ToString());

            return myXDocument.Root.Elements("Classroom").Select
               (ele => new Room
                  {
                    RoomNumber = ele.Element("RoomNumber").Value,
                    Uri = new Uri(ele.Element("URI").Value)
                  }
            ).ToList();
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
