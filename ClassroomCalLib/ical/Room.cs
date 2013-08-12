using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDay.iCal;
using ClassroomCalLib.util;
using DDay.iCal.Serialization.iCalendar;

namespace ClassroomCalLib.ical
{
    /**<summary>
     * Class that describes a common room.  Contains the RoomNumber and
     * helper methods that can be used to identify the BusyTimes
     */ 
    public class Room
    {
        public Room() { }

        public Room(String RoomNumber, ICSUri URIToLoad)
        {
            this.RoomNumber = RoomNumber;
            this.Load(URIToLoad);
        }

        public Room(String RoomNumber)
        {
            this.RoomNumber = RoomNumber;
        }

        // Attributes
        public String RoomNumber { get; set; }
        public ICSUri URILocation { get; set; }
        public ICSPath FPATHLocation { get; set; }
        private IICalendarCollection iCalc;
        private IICalendar iCal;
        private List<SimpleEvent> CachedEvents = new List<SimpleEvent>(); 

        public IEnumerable<IFreeBusyEntry> BusyTimes(int minutesFuture)
        {
            return BusyTimes(SystemTime.Now().AddMinutes(minutesFuture));
        }

        public IEnumerable<IFreeBusyEntry> BusyTimes(DateTime DateToGo, DateTime InitialTime=default(DateTime))
        {
            if (iCal != null)
            {
                if (InitialTime == default(DateTime))
                {
                    InitialTime = SystemTime.Now();
                }
                
                IFreeBusy ifb = iCal.GetFreeBusy(
                    new iCalDateTime(InitialTime, "US-Central"),
                    new iCalDateTime(DateToGo, "US-Central"));
                return ifb.Entries.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<SimpleEvent> CacheToSimple(DateTime startDate = default(DateTime), int daysToCache = default(int))
        {
            if (iCal != null)
            {
                var ifb = iCal.Events;
                DateTime endDate = new DateTime();
                CachedEvents.Clear();
                if (daysToCache != default(int) && startDate != default(DateTime))
                {
                    endDate = startDate.AddDays(daysToCache);
                }

                foreach (Event e in ifb)
                {
                    // If we have defaults, equate to True.  If we don't have defaults
                    if (!(daysToCache != default(int) && startDate != default(DateTime)) ||
                        (endDate >= e.Start.ToTimeZone("US-Central").Value &&
                         startDate <= e.End.ToTimeZone("US-Central").Value))
                    {
                        SimpleEvent ne = new SimpleEvent
                        {
                            EventName = e.Name,
                            EventStart = e.Start.ToTimeZone("US-Central").Value,
                            EventStop = e.End.ToTimeZone("US-Central").Value
                        };
                        CachedEvents.Add(ne);
                    }
                }
                return CachedEvents;
            }
            else
            {
                throw new Exception("iCalendar has not been set/loaded");
            }
        }

       public IICalendar GetCalendar()
        {
            return iCal;
        }

        /** <summary>
         * Given a URI/URL, we call iCalendar's LoadFromURI, select the first element in the return, then return true.
         * </summary>
         */
        public bool Load(ICSUri path)
        {
            iCalc = iCalendar.LoadFromUri(path.toURI());
            iCal = iCalc.FirstOrDefault();
            this.URILocation = path;
            return true;            
        }
        
        public bool Load(ICSPath path)
        {
            iCalc = iCalendar.LoadFromFile(path.ToString());
            iCal = iCalc.FirstOrDefault();
            this.FPATHLocation = path;
            return true;            
        }
    }
}
