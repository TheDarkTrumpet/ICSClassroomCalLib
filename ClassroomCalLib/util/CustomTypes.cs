using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomCalLib.util
{
    public class CustomCalLocation
    {
        public Uri toURI()
        {
            throw new Exception("Not Implemented");
        }

        public String Path()
        {
            throw new Exception("Not Implemented");
        }
    }

    public class ICSUri : CustomCalLocation
    {
        private Uri icsPath { get; set; }

        public ICSUri(string val)
        {
            icsPath = new Uri(val);
        }

        public new Uri toURI()
        {
            return icsPath;
        }
    }

    public class FilePath : CustomCalLocation
    {
        private new String Path { get; set; }
        public FilePath() { }
        public FilePath(string val)
        {
            Path = val;
        }
    }
    public class LinqPath : FilePath { }

    public class ICSPath : FilePath { }
}
