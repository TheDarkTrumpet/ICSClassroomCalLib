using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDay.iCal;

namespace ClassroomCalLib.ical
{
    public interface IRoomInfo
    {
        IEnumerable<IFreeBusyEntry> BusyScheduleFor(string RoomNumber, DateTime StartDate, DateTime EndDate);
        IEnumerable<IFreeBusyEntry> FreeScheduleFor(string RoomNumber, DateTime StartDate, DateTime EndDate);
    }
}
