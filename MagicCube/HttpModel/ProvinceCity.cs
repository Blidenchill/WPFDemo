using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class ProvinceCity
    {
        public class RootObject
        {
            public string province { get; set; }
            public List<string> city { get; set; }
        }
    }
}
