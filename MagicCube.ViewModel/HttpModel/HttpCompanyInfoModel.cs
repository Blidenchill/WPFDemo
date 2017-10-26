using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpCompanyInfoModel
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public string companyID { get; set; }
        /// <summary>
        /// 公司全称
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 公司logo
        /// </summary>
        public string companyLogo { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string companyShortName { get; set; }
        /// <summary>
        /// 所属行业
        /// </summary>
        public string companyIndustry { get; set; }
        /// <summary>
        /// 公司性质
        /// </summary>
        public string companyCharact { get; set; }
        /// <summary>
        /// 发展规模
        /// </summary>
        public string companyScale { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string companyCity { get; set; }
        ///所在省
        public string companyProvince { get; set; }
        /// <summary>
        /// 所在区
        /// </summary>
        public string companyDistrict { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string companyAddress { get; set; }
        /// <summary>
        /// 地址经纬度
        /// </summary>
        public string companyLocation { get; set; }
        /// <summary>
        /// 公司简介
        /// </summary>
        public string companyIntro { get; set; }
        /// <summary>
        /// 公司福利
        /// </summary>
        public string companyBenefit { get; set; }
        /// <summary>
        /// 发展阶段
        /// </summary>
        public string companyStage { get; set; }
        /// <summary>
        /// 公司官网
        /// </summary>
        public string companyWebsite { get; set; }
        /// <summary>
        /// 公司照片
        /// </summary>
        public string companyImageUrls { get; set; }
        /// <summary>
        /// 是否有修改公司信息的权限
        /// </summary>
        public string updateCompanyAuth { get; set; }



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
        /// <summary>
        /// 招聘者的职位
        /// </summary>
        public string position { get; set; }
        /// <summary>
        /// 座机号
        /// </summary>
        public string telNr { get; set; }

        public string userID { get; set; }
    }
}
