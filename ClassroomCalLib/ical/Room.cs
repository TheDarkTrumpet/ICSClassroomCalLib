using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDay.iCal;

namespace ClassroomCalLib.ical
{
    /**<summary>
     * Class that describes a common room.  Contains the RoomNumber and
     * helper methods that can be used to identify the BusyTimes
     */ 
    class Room
    {
        public Room() { }

        public Room(String RoomNumber, Uri URIToLoad)
        {
            this.RoomNumber = RoomNumber;
            this.LoadFromURI(URIToLoad);
        }

        public Room(String RoomNumber)
        {
            this.RoomNumber = RoomNumber;
        }

        // Attributes
        public String RoomNumber { get; set; }
        public Uri URI { get; set; }

        public IEnumerable<IFreeBusyEntry> BusyTimes(int minutesFuture)
        {
            return BusyTimes(DateTime.Now.AddSeconds(minutesFuture));
        }

        public IEnumerable<IFreeBusyEntry> BusyTimes(DateTime DateToGo, DateTime InitialTime=null)
        {
            if (iCal != null)
            {
                if (InitialTime == null)
                {
                    InitialTime = DateTime.Now;
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

        // Utilities
        private IICalendarCollection iCalc;
        private IICalendar iCal;

        /** <summary>
         * Given a URI/URL, we call iCalendar's LoadFromURI, select the first element in the return, then return true.
         * </summary>
         */
        public bool LoadFromURI(Uri myURI)
        {
            iCalc = iCalendar.LoadFromUri(myURI);
            iCal = iCalc.FirstOrDefault();
            this.URI = myURI;
            return true;
        }

        public bool LoadFromFile(string filepath)
        {
            iCalc = iCalendar.LoadFromFile(filepath);
            iCal = iCalc.FirstOrDefault();
            return true;
        }
    }
}
