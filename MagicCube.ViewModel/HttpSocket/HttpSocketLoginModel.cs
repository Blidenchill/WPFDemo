using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpSocketLoginModel
    {
        public string deviceID { get; set; }
        public string userIdentity { get; set; }
        public string userID { get; set; }
        public string version { get; set; }
        public string platform { get; set; }
    }
}
