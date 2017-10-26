using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpSocketBaseModel<T>
    {
        public string cmd { get; set; }
        public int status { get; set; }
        public string errorMessage { get; set; }

        public T data { get; set; }
        public HttpSocketBaseModel(T model)
        {
            this.data = model;
        }
        public HttpSocketBaseModel()
        {
           
        }
    }

    public class HttpSocketBaseModel
    {
        public string cmd { get; set; }

        public object data { get; set; }
    }
}
