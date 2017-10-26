using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpSelfInfoModel
    {
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
