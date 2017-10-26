using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpMCoinDetail
    {
        public int code { get; set; }
        public DataLst data { get; set; }
        public string msg { get; set; }
    }

    public class DataLst
    {
        public int total { get; set; }
        public List<HttpMcoinDetailItem> result { get; set; }
    }

    public class HttpMcoinDetailItem
    {
        /// <summary>
        /// S_Coin
        /// </summary>
        public string serviceSign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public string triggerName { get; set; }
        /// <summary>
        /// ConditionEndTime
        /// </summary>
        public string conditionEndTime { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public int userID { get; set; }
        /// <summary>
        /// ConditionBeginTime
        /// </summary>
        public string conditionBeginTime { get; set; }
        /// <summary>
        /// poundage_interview_onsite
        /// </summary>
        public string triggerCode { get; set; }
        /// <summary>
        /// 确认[18501]到访
        /// </summary>
        public string triggerParams { get; set; }
        /// <summary>
        /// ServiceAction
        /// </summary>
        public int serviceAction { get; set; }
        /// <summary>
        /// ServiceQuantity
        /// </summary>
        public double serviceQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int userServiceLogID { get; set; }
        /// <summary>
        /// 2016-12-13 18:35:20
        /// </summary>
        public DateTime createTime { get; set; }
    }

}
