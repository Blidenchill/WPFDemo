using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpAuthStateModel
    {
        public string updateTime { get; set; }
        public string authPlatform { get; set; }
        public string authClientVersion { get; set; }
        public string auditStatus { get; set; }

        public string auditReason { get; set; }
        public string imageUrl { get; set; }
        public string hrAuthID { get; set; }
        public string userID { get; set; }
        public string createTime { get; set; }
    }
}
