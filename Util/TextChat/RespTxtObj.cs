using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Util
{
    public class RespTxtObj
    {
        public string id { get; set; }
        public string target_type { get; set; }
        public string to { get; set; }
        public IList<MsgObj> bodies { get; set; }
        public string from { get; set; }
        public IDictionary<string, string> ext { get; set; } 
    }

    public class MsgObj
    {
        public string type { get; set; }
        public string msg { get; set; }
    }
}
