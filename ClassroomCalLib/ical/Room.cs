using System;
using System.Collections.Generic;
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

        // Utilities
        private IICalendarCollection iCalc;
        private IICalendar iCal;

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
