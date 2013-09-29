using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDay.iCal;
using ClassroomCalLib.util;

namespace ClassroomCalLib.ical
{
    /**<summary>
     * Class that describes a common room.  Contains the RoomNumber and
     * helper methods that can be used to identify the BusyTimes
     */ 
    public class Room
    {
        public Room() { }

        public Room(String RoomNumber, Uri URIToLoad)
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
        public Uri Uri { get; set; }
        private IICalendarCollection iCalc;
        private IICalendar iCal;

        private List<SimpleEvent> CachedEvents = new List<SimpleEvent>();
        private Boolean CacheLoaded = false;

        public FriendlyEvent FreeBusyToString(DateTime dt = default(DateTime))
        {
            if (dt == default(DateTime))
            {
                dt = SystemTime.Now();
            }

            SimpleEvent SoonestEvent = BusyTimes().Where(x => (dt <= x.EventStop) || (dt <= x.EventStart)).OrderBy(x => x.EventStart).FirstOrDefault();

            if (SoonestEvent == null)
            {
                //We really shouldn't get here, it basically means we have no cache at all, so we can't determine how free or busy we are.
                return new FriendlyEvent("Free for an unknown period of time");
            } else if (dt <= SoonestEvent.EventStart) //Currently we are Free
            {
                return new FriendlyEvent("Free", dt, SoonestEvent.EventStart);
            } else if (dt <= SoonestEvent.EventStop) //Currently busy
            {
                return new FriendlyEvent("Busy", dt, SoonestEvent.EventStop);
            }
            else
            {
                throw new Exception("FreeBusyString reached a point it shouldn't have.");
            }
        }

        public IEnumerable<SimpleEvent> BusyTimes(int minutesFuture)
        {
            return BusyTimes(SystemTime.Now().AddMinutes(minutesFuture));
        }

        public IEnumerable<SimpleEvent> BusyTimes(DateTime DateToGo, DateTime InitialTime=default(DateTime))
        {
            var BusyTimes = this.BusyTimes();
            
            if (InitialTime == default(DateTime))
                {
                    InitialTime = SystemTime.Now();
                }

            return BusyTimes.Where(x => x.EventStart <= DateToGo && x.EventStop >= InitialTime);
        }

        public IEnumerable<SimpleEvent> BusyTimes()
        {
            //If no iCal object, then we haven't loaded.  If no cache, then there's nothing to query from and we got here incorrectly
            if ((iCal == null || !iCal.IsLoaded) && !CacheLoaded) { throw new TypeUnloadedException("You must call the Load() method, or set a cache, prior to this function"); }

            if ((iCal != null && iCal.IsLoaded) && !CacheLoaded)
            {
                CacheToSimple();
            }
            
            return CachedEvents;
        } 

        public List<SimpleEvent> CacheToSimple(DateTime startDate = default(DateTime), DateTime endDate = default(DateTime))
        {
            if (iCal != null)
            {
                var ifb = iCal.Events;
                
                CachedEvents.Clear();
                if (endDate == default(DateTime) && startDate != default(DateTime))
                {
                    endDate = startDate.AddDays(7);  //Default cache is 1 week in the future
                }

                foreach (Event e in ifb)
                {
                    // If we have defaults, equate to True.  If we don't have defaults
                    if (!(startDate != default(DateTime) && endDate != default(DateTime)) ||
                        (endDate >= e.Start.ToTimeZone("US-Central").Value &&
                         startDate <= e.End.ToTimeZone("US-Central").Value))
                    {
                        SimpleEvent ne = new SimpleEvent
                        {
                            EventName = e.Summary,
                            EventStart = e.Start.ToTimeZone("US-Central").Value,
                            EventStop = e.End.ToTimeZone("US-Central").Value
                        };
                        CachedEvents.Add(ne);
                    }
                }
                CacheLoaded = true;
                return CachedEvents;
            }
            else
            {
                throw new Exception("iCalendar has not been set/loaded");
            }
        }

        public void setCache(List<SimpleEvent> events)
        {
            CachedEvents = events;
            CacheLoaded = true;
        }

       public IICalendar GetCalendar()
        {
            return iCal;
        }

        /** <summary>
         * Given a URI/URL, we call iCalendar's LoadFromURI, select the first element in the return, then return true.
         * </summary>
         */

        public bool Load(Uri path)
        {
            iCalc = iCalendar.LoadFromUri(path);
            iCal = iCalc.FirstOrDefault();
            this.Uri = path;
            return true;
        }
    }
}
