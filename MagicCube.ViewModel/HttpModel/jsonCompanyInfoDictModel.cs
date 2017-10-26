using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class jsonCompanyInfoDictModel
    {
        public DataCompanyInfoDic DataDic { get; set; }
    }

    public class CompanyInfoDictItem
    {
        public string simpleName { get; set; }
        public string fullName { get; set; }
        public string value { get; set; }
        public string defaultName { get; set; }
    }

    public class CompanyInfoDictElement
    {
        public List<CompanyInfoDictItem> mappingItem { get; set; }
        public string name { get; set; }
    }

    public class DataCompanyInfoDic
    {
        public List<CompanyInfoDictElement> element { get; set; }
    }
}
