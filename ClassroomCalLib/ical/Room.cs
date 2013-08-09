using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDay.iCal;

namespace ClassroomCalLib.ical
{
    class Room
    {
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

        public IEnumerable<IFreeBusyEntry> BusyTimes(DateTime DateToGo)
        {
            if (iCal != null)
            {
                IFreeBusy ifb = iCal.GetFreeBusy(
                    new iCalDateTime(DateTime.Now, "US-Central"),
                    new iCalDateTime(DateToGo, "US-Central"));
                return ifb.Entries.Select(x => x);
            }
            else
            {
                return null;
            }
        }  

        // Utilities
        private IICalendarCollection iCalc;
        private IICalendar iCal;

        public bool LoadFromURI(Uri myURI)
        {
            iCalc = iCalendar.LoadFromUri(myURI);
            iCal = iCalc.FirstOrDefault();
            this.URI = myURI;
            return true;
        }
    }
}
