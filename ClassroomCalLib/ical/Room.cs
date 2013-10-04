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
     * </summary>
     */
    public class Room
    {
        /**<summary>
         * Initiates a new instance of the Room class with given information
         * </summary>
         */
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

        ///<summary>Returns a list of busy times for this room from now until n minutes</summary>
        ///<param name="minutesFuture">Number of minutes into the future to look</param>
        ///<returns>Returns list of busy times</returns>
        public IEnumerable<SimpleEvent> BusyTimes(int minutesFuture)
        {
            return BusyTimes(SystemTime.Now().AddMinutes(minutesFuture));
        }

        public IEnumerable<SimpleEvent> BusyTimes(DateTime InitialTime, DateTime DateToGo)
        {
            var BusyTimes = this.BusyTimes();
  
            return BusyTimes.Where(x => x.EventStart <= DateToGo && x.EventStop > InitialTime);
        }

        public IEnumerable<SimpleEvent> BusyTimes(DateTime DateInFuture)
        {
            var InitialTime = SystemTime.Now();
            return BusyTimes(InitialTime, DateInFuture);
        }

        ///<summary>Returns a list of busy times for this room</summary>
        ///<returns>Returns list of busy times</returns>
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

        /// <summary>
        /// Caches Room events in the next seven days
        /// </summary>
        /// <returns>List of SimpleEvents</returns>
        public List<SimpleEvent> CacheToSimple()
        {
            return _cacheToSimple(SystemTime.Now(), SystemTime.Now().AddDays(7));
        } 

        /// <summary>
        /// Caches Room events of seven days, starting with a given day
        /// </summary>
        /// <param name="startDate">First day of events to cache</param>
        /// <returns>List of SimpleEvents</returns>
        public List<SimpleEvent> CacheToSimple(DateTime startDate)
        {
            return _cacheToSimple(startDate, startDate.AddDays(7));
        }

        /// <summary>
        /// Caches Room events in the given range of days
        /// </summary>
        /// <param name="startDate">First day of events to cache</param>
        /// <param name="endDate">End day of events to cache</param>
        /// <returns>List of SimpleEvents</returns>
        public List<SimpleEvent> CacheToSimple(DateTime startDate, DateTime endDate)
        {
            return _cacheToSimple(startDate, endDate);
        }

        /// <summary>
        /// Caches Room events, starting on the given date, for a given amount of days
        /// </summary>
        /// <param name="startDate">First day of events to cache</param>
        /// <param name="numDays">Number of days worth of events</param>
        /// <returns>List of SimpleEvents</returns>
        public List<SimpleEvent> CacheToSimple(DateTime startDate, int numDays)
        {
            return _cacheToSimple(startDate, startDate.AddDays(numDays));
        }

        private List<SimpleEvent> _cacheToSimple(DateTime startDate, DateTime endDate)
        {
            if (iCal != null)
            {
                var ifb = iCal.Events;
                
                CachedEvents.Clear();
                if (endDate == default(DateTime) && startDate != default(DateTime))
                {
                    endDate = startDate.AddDays(7);  //Default cache is 1 week in the future
                }

                IList<Occurrence> occurrences;

                if (endDate == default(DateTime) && startDate == default(DateTime))
                {
                    occurrences = iCal.GetOccurrences(startDate.AddYears(-200), endDate.AddYears(200));
                }
                else
                {
                    occurrences = iCal.GetOccurrences(startDate, endDate);
                }

                foreach (Occurrence e in occurrences)
                {
                    IRecurringComponent rc = e.Source as IRecurringComponent;
                    SimpleEvent ne = new SimpleEvent
                        {
                            EventName = rc.Summary,
                            EventStart = e.Period.StartTime.ToTimeZone("US-Central").Value,
                            EventStop = e.Period.EndTime.ToTimeZone("US-Central").Value
                        };
                        CachedEvents.Add(ne);
                }
                CacheLoaded = true;
                return CachedEvents;
            }
            else
            {
                throw new Exception("iCalendar has not been set/loaded");
            }
        }

        ///<summary>Returns a list of busy times for this room</summary>
        ///<param name="events">List of events to cache</param>
        public void setCache(List<SimpleEvent> events)
        {
            CachedEvents = events;
            CacheLoaded = true;
        }

        ///<summary>Returns this rooms calendar</summary>
        ///<returns>Returns calendar</returns>
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
