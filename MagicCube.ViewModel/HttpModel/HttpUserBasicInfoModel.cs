using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    /// <summary>
    /// 个人注册基本信息（个人信息，公司信息，认证信息）
    /// </summary>
    public class HttpUserBasicInfoModel
    {
        //注册基本信息是否完善
        public bool isUserInfoComplete { get; set; }
        /// <summary>
        /// 个人头像
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// 个人姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 个人邮箱
        /// </summary>
        public string hrEmail { get; set; }

        public string position { get; set; }
        public string companyID { get; set; }
        public string companyName { get; set; }
        public string companyLogo { get; set; }
        public string companyShortName { get; set; }
        public string companyIndustry { get; set; }
        public string companyCharact { get; set; }
        public string companyScale { get; set; }
        public string companyCity { get; set; }
        public string companyAddress { get; set; }
        public string companyIntro { get; set; }
        public string companyBenefit { get; set; }
        public int isHRAuth { get; set; }
        public int isSimilarAuth { get; set; }
    }
}
