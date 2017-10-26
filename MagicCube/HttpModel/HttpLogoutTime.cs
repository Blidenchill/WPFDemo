using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{

        public class HttpLogoutTime
        {
            public int fromUser { get; set; }
            public string fromUserType { get; set; }
            public string messageType { get; set; }
            public string responseDate { get; set; }
            public string source { get; set; }
            public string nonce { get; set; }
            
            public int toUser { get; set; }
            public string toUserType { get; set; }
        }

     public class HttpServerTime
     {
         public string status { get; set; }
         public string result { get; set; }
     }
    
}
