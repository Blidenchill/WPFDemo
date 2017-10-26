using MagicCube.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpZhilianCodeVal2
        {
            public string code { get; set; }
            public string name { get; set; }
        }

        public class HttpZhilianCodeVal
        {
            public string name { get; set; }
            public List<HttpZhilianCodeVal2> val { get; set; }
        }

        public class HttpZhilianCodeJobTypes : NotifyBaseModel
        {
            public string name { get; set; }
            public List<HttpZhilianCodeVal> val { get; set; }
            private bool _IsCheck;
            public bool IsCheck
            {
                get { return _IsCheck; }
                set
                {
                    _IsCheck = value;
                    NotifyPropertyChange(() => IsCheck);
                }
            }
        }

        public class HttpZhilianCode
        {
            public List<HttpZhilianCodeJobTypes> jobTypes { get; set; }
        }
}
