using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpCompanyTagsCodes
    {
        public List<__invalid_type__工作氛围环境> invirment { get; set; }
        public List<__invalid_type__薪酬福利> input { get; set; }
        public List<__invalid_type__更多福利> moreOver { get; set; }
    }

    public class __invalid_type__工作氛围环境
    {
        public int id { get; set; }
        public bool deleted { get; set; }
        public string parentCode { get; set; }
        public string parentLevel { get; set; }
        public string code { get; set; }
        public string level { get; set; }
        public string name { get; set; }
    }

    public class __invalid_type__薪酬福利
    {
        public int id { get; set; }
        public bool deleted { get; set; }
        public string parentCode { get; set; }
        public string parentLevel { get; set; }
        public string code { get; set; }
        public string level { get; set; }
        public string name { get; set; }
    }

    public class __invalid_type__更多福利
    {
        public int id { get; set; }
        public bool deleted { get; set; }
        public string parentCode { get; set; }
        public string parentLevel { get; set; }
        public string code { get; set; }
        public string level { get; set; }
        public string name { get; set; }
    }



}
