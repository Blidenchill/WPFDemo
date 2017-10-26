using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpLimitData
    {
        /// <summary>
        /// 2016-11-06 14:07:58
        /// </summary>
        public DateTime createTime { get; set; }
        /// <summary>
        /// 限制IM沟通
        /// </summary>
        public string functionName { get; set; }
        /// <summary>
        //F_LimitCommunicate  限制IM沟通
        //F_LimitManageJob  限制使用职位管理
        //F_LimitManageResume  限制使用简历管理
        //F_LimitSearch  限制使用人才库
        //F_LimitUpdateInfo  限制修改资料
        //F_UseManageInterview  使用面试管理 
        /// </summary>
        public string functionSign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// S_SpecialPayUser
        /// </summary>
        public string serviceSign { get; set; }
        /// <summary>
        /// State
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 2016-11-06 14:08:02
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// UserFunctionID
        /// </summary>
        public int userFunctionID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public int userID { get; set; }
    }

    public class HttpLimitRoot
    {
        /// <summary>
        /// Code
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// Data
        /// </summary> 
        public List<HttpLimitData> data { get; set; }
        /// <summary>
        /// 正常调用
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string traceID { get; set; }
    }
}
