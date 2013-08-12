using System;
using System.Collections.Generic;
using System.Linq;
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

    [Serializable]
    public class CachedEvents
    {
        public DateTime LastCached { get; set; }
        public  IFreeBusy CachedBusy { get; set; }
    }
}
