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

    public class ICSPath {
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
        public DateTime EventStart { get; set; }
        public DateTime EventStop { get; set; }
        public string Status { get; set; }
    }
}
