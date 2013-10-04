using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ClassroomCalLib.util;

namespace ClassroomCalLib.ical
{
    /// <summary>
    /// RoomInfo is used to initialize and store sets of Rooms.
    /// </summary>
    public class RoomInfo
    {
        private List<Room> myRooms;

        ///<summary>Initiates an instance of RoomInfo</summary>
        ///<param name="pathToXML">Uri to the XML file where room info is stored</param>
        public RoomInfo(Uri pathToXML)
        {
            myRooms = loadRoomInfoFromXML(pathToXML);
        }

        /// <summary>Initiates an instance of RoomInfo</summary>
        /// <param name="roomStructure">List of Rooms</param>
        public RoomInfo(List<Room>roomStructure)
        {
            myRooms = roomStructure;
        }

        /// <summary>Loads each room stored</summary>
        /// <returns>true if each room room is loaded sucessfully, false if one room is not loaded</returns>
        public bool LoadAll()
        {
            bool success = false;
            
            foreach (Room mr in myRooms)
            {
                success = success | mr.Load(mr.Uri);
            }
            return success;
        }

        /// <summary>Loads a given Room</summary>
        /// <param name="room">Room to load</param>
        /// <returns>true if the Room was loaded, false if it wasn't</returns>
        public bool LoadRoomByName(string room)
        {
            Room mr = GetRoomByName(room);
            if (mr == null)
            {
                throw new NullReferenceException("Room not found in the collection of available rooms");
            }

            return false | mr.Load(mr.Uri);
        }

        /// <summary>
        /// Returns the Room matching the input
        /// </summary>
        /// <param name="name">Name of the room to return</param>
        /// <returns>Room</returns>
        public Room GetRoomByName(string name)
        {
            //There should *never* be duplicate rooms
            return myRooms.FirstOrDefault(x => x.RoomNumber == name);
        }

        /// <summary>
        /// Returns a set of all of the ROooms
        /// </summary>
        /// <returns>Set of all Rooms</returns>
        public IEnumerable<Room> GetAllRooms()
        {
            return myRooms;
        }

        /// <summary>
        /// Loads an XML document from a given Uri, then returns a list of Rooms.
        /// Attributes from each Room are obtained from elements in the XML document
        /// </summary>
        /// <param name="fileUri">Uri to the XML document of rooms</param>
        /// <returns>Set of new Rooms as a List</returns>
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

        /// <summary>
        /// Caches all of the Room availabilities to an XML document
        /// </summary>
        /// <param name="FilePath">Name of the cache file to be created</param>
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

        /// <summary>
        /// Caches all of the Room availabilities to an XML document
        /// </summary>
        /// <param name="FilePath">Name of the cache file to be created</param>
        /// <param name="startTime">Starting point of events to cache</param>
        /// <param name="endTime">Ending point of events to cache</param>
        public void SerializeCacheToFile(String FilePath, DateTime startTime, DateTime endTime)
        {
            foreach (Room r in GetAllRooms())
            {
                r.CacheToSimple(startTime, endTime);
            }
            SerializeCacheToFile(FilePath);
        }

        /// <summary>
        /// Deserializes Room information from a file with the given path
        /// </summary>
        /// <param name="FilePath">Path to the file</param>
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
