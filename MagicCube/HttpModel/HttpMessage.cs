using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{

    public class ZhilianMessageResult
    {
        public string message { get; set; }
        public string code { get; set; }
        public string detailMessage { get; set; }
    }

    public class ZhilianMessage
    {
        public int status { get; set; }
        public ZhilianMessageResult result { get; set; }
    }
}
