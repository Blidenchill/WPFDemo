using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpCompanyTagsListModel
    {
        public List<MoreWelfareCompany> moreWelfareCompany { get; set; }
        public List<EnviromentCompany> enviromentCompany { get; set; }
        public List<SalaryWelfareCompany> salaryWelfareCompany { get; set; }
    }
    public class MoreWelfareCompany
    {
        public string updateTime { get; set; }
        public string group { get; set; }
        public string color { get; set; }
        public string text { get; set; }
        public int tagID { get; set; }
        public int ownerID { get; set; }
        public int type { get; set; }
        public string createTime { get; set; }
    }

    public class EnviromentCompany
    {
        public string updateTime { get; set; }
        public string group { get; set; }
        public string color { get; set; }
        public string text { get; set; }
        public int tagID { get; set; }
        public int ownerID { get; set; }
        public int type { get; set; }
        public string createTime { get; set; }
    }

    public class SalaryWelfareCompany
    {
        public string updateTime { get; set; }
        public string group { get; set; }
        public string color { get; set; }
        public string text { get; set; }
        public int tagID { get; set; }
        public int ownerID { get; set; }
        public int type { get; set; }
        public string createTime { get; set; }
    }

   
}
