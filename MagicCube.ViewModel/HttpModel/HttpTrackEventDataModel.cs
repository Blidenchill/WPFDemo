using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpTrackEventDataModel
    {
        public List<string> item { get; set; }
        public string session { get; set; }
        public string algo { get; set; }
        public string page { get; set; }
        public string location { get; set; }
        public string id { get; set; }
        public string userID { get; set; }
        public string jobID { get; set; }
        public string jobType { get; set; }
    }


}
