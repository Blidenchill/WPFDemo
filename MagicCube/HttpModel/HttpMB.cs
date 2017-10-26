using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpIntegralResult
    {
        public string integral { get; set; }
        public string MBBalance { get; set; }
    }

    public class HttpIntegral
    {
        public HttpIntegralResult result { get; set; }
    }
}
