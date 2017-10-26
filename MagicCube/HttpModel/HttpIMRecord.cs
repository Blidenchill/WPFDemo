using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{

    public class HttpIMRecord
    {
        /// <summary>
        /// 2016-08-25T05:47:06Z
        /// </summary>
        public string updatedTime { get; set; }
        /// <summary>
        /// ENTERPRISE_USER
        /// </summary>
        public string sender { get; set; }
        /// <summary>
        /// Deleted
        /// </summary>
        public bool deleted { get; set; }
        /// <summary>
        /// BOTH
        /// </summary>
        public string receiverType { get; set; }
        /// <summary>
        /// ENTERPRISE_USER
        /// </summary>
        public string ownerType { get; set; }
        /// <summary>
        /// 初步审核，您比较适合本职位，请主动联系我们或耐心等待，了解后续安排。
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 2016-08-25T05:47:06Z
        /// </summary>
        public string createdTime { get; set; }
        /// <summary>
        /// Read
        /// </summary>
        public bool read { get; set; }
        /// <summary>
        /// 2016-08-25T05:47:06Z
        /// </summary>
        public string sendTime { get; set; }
        /// <summary>
        /// MOFANG_USER
        /// </summary>
        public string receiver { get; set; }
        /// <summary>
        /// Owner
        /// </summary>
        public int owner { get; set; }
        /// <summary>
        /// ContactRecordId
        /// </summary>
        public int contactRecordId { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
    }

    public class HttpIMRecordResult
    {
        /// <summary>
        /// PageData
        /// </summary>
        public PageData pageData { get; set; }
        /// <summary>
        /// Results
        /// </summary>
        public List<HttpIMRecord> results { get; set; }
    }
}
