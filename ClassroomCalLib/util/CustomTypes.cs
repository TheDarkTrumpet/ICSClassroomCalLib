using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomCalLib.util
{
    public class ICSUri
    {
        private Uri Path { get; set; }

        public IcsUri(string val)
        {
            Path = new Uri(val);
        }
    }

    public class FilePath
    {
        private String Path { get; set; }
        public FilePath() { }
        public FilePath(string val)
        {
            Path = val;
        }
    }
    public class LinqPath : FilePath { }

    public class ICSPath : FilePath { }
}
