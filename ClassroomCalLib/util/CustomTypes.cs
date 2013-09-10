using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DDay.Collections;
using DDay.iCal;

namespace ClassroomCalLib.util
{
    public class ICSUri
    {
        private Uri icsPath { get; set; }

        public ICSUri(string val)
        {
            icsPath = new Uri(val);
        }

        public Uri toURI()
        {
            return icsPath;
        }
    }

    public class ICSPath
    {
        public String Path { get; set; }

        public ICSPath(string icspath)
        {
            Path = icspath;
        }

        public string ToString()
        {
            return Path;
        }
    }

    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }


    public class SimpleEvent
    {
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventStop { get; set; }
        public string Status { get; set; }
    }

    public class FriendlyEvent
    {
        public string Status { get; set; }
        public double SecondsInStatus { get; set; }
        private readonly string _deltaToFriendlyText;

        public FriendlyEvent(string statusText, DateTime startDateTime, DateTime stopDateTime)
        {
            Status = statusText;
            _deltaToFriendlyText = DeltaToFriendly(statusText, startDateTime, stopDateTime);
        }

        public FriendlyEvent(string statusText)
        {
            Status = statusText;
            _deltaToFriendlyText = statusText;
        }

        public override string ToString()
        {
            return _deltaToFriendlyText;
        }

        private string DeltaToFriendly(string status, DateTime startDateTime, DateTime stopDateTime)
        {
            SecondsInStatus = stopDateTime.Subtract(startDateTime).TotalSeconds;

            StringBuilder sb = new StringBuilder();
            sb.Append(status);
            sb.Append(" for ");
            sb.Append(exactCheck(SecondsInStatus));
            //sb.Append(exactCheck(new TimeSpan(0,0,SecondsInStatus)));
            sb.Append(hoursMinutes(SecondsInStatus));
            //sb.Append(hoursMinutes(new TimeSpan(0,0,int(SecondsInStatus))));
            return sb.ToString();
        }

        // Private functions to help with 
        private string hoursMinutes(double seconds)
        {
            
            if (seconds < 60)
            {
                return seconds.ToString() + " seconds";
            } else if (60 <= seconds && seconds < 60*60)
            {
                double minutes = seconds/60;
                return Math.Round(minutes).ToString() + " minutes";
            } else if (60*60 <= seconds && seconds < 60*60*12)
            {
                double hours = seconds/(60*60);
                double minutes = (seconds - (Math.Floor(hours) * 60 * 60))/60;
                return Math.Floor(hours).ToString() + " hours, and " + Math.Round(minutes,0).ToString() + " minutes";
            }
            else
            {
                double days = seconds/(60*60*24);
                double hours = (seconds - (Math.Floor(days)*60*60*24))/(60*60);
                return Math.Floor(days).ToString() + " days, and " + Math.Round(hours,0).ToString() + " hours";
            }
        }

        private string exactCheck(double seconds)
        {
            if (seconds < 60 || (seconds%60 == 0))  //This doesn't take in account even-ness in hours.
            {
                return "";
            }
            else
            {
                return "about ";
            }
        }
    }
}