using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpMBTotalData
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
        /// UserID
        /// </summary>
        public int userID { get; set; }
        /// <summary>
        /// State
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 2016-11-04 13:13:30
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// AvailableAmount
        /// </summary>
        public double availableAmount { get; set; }
        /// <summary>
        /// LockedAmount
        /// </summary>
        public double lockedAmount { get; set; }
        public double totalAmount
        { 
            get
            {
                return availableAmount + lockedAmount;
            }
            set
            {
                value = 0;
            }
        }

        /// <summary>
        /// UserServiceID
        /// </summary>
        public int userServiceID { get; set; }
        /// <summary>
        /// BeginTime
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        /// 2016-11-04 12:59:09
        /// </summary>
        public string createTime { get; set; }
    }

    public class HttpMBTotalRoot
    {
        /// <summary>
        /// 正常调用
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 20161106103916313375_0
        /// </summary>
        public string traceID { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public HttpMBTotalData data { get; set; }
    }

}
