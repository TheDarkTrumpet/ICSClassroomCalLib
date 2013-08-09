using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDay.iCal;

namespace ClassroomCalLib.ical
{
    internal class RoomInfoWeb : IRoomInfo
    {
        private IEnumerable<Room> myRooms = new Room[](
            new Room
                {
                    RoomNumber = "RES-PHAR-129",
                    URI =
                        new Uri(
                "http://email.uiowa.edu/owa/calendar/1e6dbdb8e4814563ab994449345e95d4@iowa.uiowa.edu/5a824eb9eed54decbcc295e54102622c11906123505361280052/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-226",
                    URI =
                        new Uri(
                "http://email.uiowa.edu/owa/calendar/9357b43b5d12499eb6401e018008d778@iowa.uiowa.edu/10f19757292f4032940035bd6bf2c9278625850918483163074/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-S538",
                    URI =
                        new Uri(
                "http://email.uiowa.edu/owa/calendar/6360779ea39448228938d8af2ea1fdb0@iowa.uiowa.edu/4faef19495c34caebd3d1150ef4f0e8412999831582725087249/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-S543",
                    URI =
                        new Uri(
                "http://email.uiowa.edu/owa/calendar/a240763f0c3e42f5bdf7abbfed04bcf9@iowa.uiowa.edu/8229b275c732441998e33566a87d99e18120430139117729276/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-S545",
                    URI =
                        new Uri(
                "http://email.uiowa.edu/owa/calendar/40b5b7aa4292485288df207b10b67df1@iowa.uiowa.edu/dc27ae284f1c4776808b9e43b03faaae4944822449095139734/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-S552",
                    URI =
                        new Uri(
                "http://email.uiowa.edu/owa/calendar/1e2f1b0998fe4005bdd71fabed0d10d9@iowa.uiowa.edu/42deb627faf9417ba6345c3d723aad678591608703955087174/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-SOPH-100A",
                    URI =
                        new Uri(
                "http://email.uiowa.edu/owa/calendar/5dbd85075a2c4f1282f26f83fe4a5441@iowa.uiowa.edu/c79da2e03cf64036aa7489a9672a9cd15569082608891722194/calendar.ics")
                },
            new Room
                {
                    RoomNumber = "RES-PHAR-XOPH-100B",
                    URI =
                        new Uri(
                "http://email.uiowa.edu/owa/calendar/dfca5bf180484ed589e55cc3a8e6f239@iowa.uiowa.edu/5fa0fb650082495e9e6add737cb304fc1655299616341240835/calendar.ics")
                }
            );


        public IEnumerable<IFreeBusyEntry> BusyScheduleFor(string RoomNumber, DateTime StartDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFreeBusyEntry> FreeScheduleFor(string RoomNumber, DateTime StartDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }
    }
}
}
