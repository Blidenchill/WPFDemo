using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class BaseHttpModel<T>
    {
        public int code { get; set; }
        public string msg { get; set; }
        public string traceID { get; set; }

        public T data { get; set; }

        public BaseHttpModel(T data)
        {
            this.data = data;
        }
        public BaseExtraData extraData { get; set; }
    }
    public class BaseExtraData
    {
        public DateTime serverTime { get; set; }
        public long previousTime { get; set; }
    }
    public class BaseHttpModel
    {
        public int code { get; set; }
        public string msg { get; set; }
        public string traceID { get; set; }

        public object data { get; set; }

        public BaseHttpModel(object data)
        {
            this.data = data;
        }
        public BaseExtraData extraData { get; set; }
    }
}
