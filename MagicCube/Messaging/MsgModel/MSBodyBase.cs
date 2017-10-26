using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Messaging
{
    public class MSBodyBase
    {
        public string type { get; set; }
        public object body { get; set; }
    }
    public class MSBodyBase<T>
    {
        public string type { get; set; }
        public T body { get; set; }
    }
}
