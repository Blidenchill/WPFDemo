using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class ThirdSiteResult 
    {
        public string message { get; set; }
        public string code { get; set; }
        public string key { get; set; }
    }

    public class HttpThirdSiteLogin
    {
        public int status { get; set; }
        public ThirdSiteResult result { get; set; }
    }
}
