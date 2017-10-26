using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{

    public class Company
    {
        public string briefName { get; set; }
        public string updatedTime { get; set; }
        public string name { get; set; }

        public string province { get; set; }
        public string provinceCode { get; set; }
        public string city { get; set; }
        public string cityCode { get; set; }

        public string district { get; set; }
        public string districtCode { get; set; }
        public bool deleted { get; set; }
        public bool emailVerified { get; set; }
        public List<string> industries { get; set; }
        public List<string> industryCodes { get; set; }
        public string nature { get; set; }
        public string natureCode { get; set; }
        public string ownerType { get; set; }
        public string smallLogoUrl { get; set; }
        public string website { get; set; }
        public string createdTime { get; set; }
        public string emailSuffix { get; set; }
        public string location { get; set; }
        public string logoUrl { get; set; }
        public int owner { get; set; }
        public List<string> imageUrls { get; set; }
        public int id { get; set; }
        public List<string> tags { get; set; }
        public string size { get; set; }
        public string sizeCode { get; set; }
        public List<string> mofangTagCodes { get; set; }
        public List<string> userTags { get; set; }
    }

    public class CompanyIntro
    {
        public string financingAmount { get; set; }
        public string updatedTime { get; set; }
        public string publishName { get; set; }
        public string financingState { get; set; }
        public string financingStateCode { get; set; }
        public List<string> originators { get; set; }
        public int companyId { get; set; }
        public bool deleted { get; set; }
        public string companyName { get; set; }
        public string ownerType { get; set; }
        public string createdTime { get; set; }
        public string intro { get; set; }
        public string workingLocation { get; set; }
        public int owner { get; set; }
        public List<string> entrepreneurTeams { get; set; }
        public int id { get; set; }
    }

    public class HttpCompany
    {
        public Company company { get; set; }
        public CompanyIntro companyIntro { get; set; }
    }

    public class HttpworkingLocation
    {
        /// <summary>
        /// 北京
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 北京
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cityCode { get; set; }
        /// <summary>
        /// 北京
        /// </summary>
        public string workingLocation { get; set; }
        /// <summary>
        /// 东城
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string districtCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string provinceCode { get; set; }
    }

}
