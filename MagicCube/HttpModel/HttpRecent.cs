using MagicCube.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{



    public class RecentResult
    {
        public string updatedTime { get; set; }
        public string initiator { get; set; }
        public object minSalary { get; set; }
        public string imName { get; set; }
        public string enterpriseAccountId { get; set; }
        public string isRecord { get; set; }
        public bool deleted { get; set; }
        public int userId { get; set; }
        public string systemFiltrateStatus { get; set; }
        public string ownerType { get; set; }
        public string userMobile { get; set; }
        public int jobId { get; set; }
        public string createdTime { get; set; }
        public string isEvaluation { get; set; }
        public string mobile { get; set; }
        public string owner { get; set; }
        public bool userDeleted { get; set; }
        public string evaluation { get; set; }
        public int id { get; set; }
        public string jobName { get; set; }
        public string uniqueKey { get; set; }
        public string workingExp { get; set; }
        public string avatarUrl { get; set; }
        public string resumeId { get; set; }
        public string degree { get; set; }


        public string name
        {
            get;
            set;
        }
        public string gender { get; set; }

        public List<NewIm> newIm { get; set; }
    }

    public class NewIm
    {
        public string updatedTime { get; set; }
        public string sender { get; set; }
        public bool deleted { get; set; }
        public string receiverType { get; set; }
        public string ownerType { get; set; }
        public string content { get; set; }
        public string createdTime { get; set; }
        public bool read { get; set; }
        public string sendTime { get; set; }
        public string receiver { get; set; }
        public int owner { get; set; }
        public int contactRecordId { get; set; }
        public int id { get; set; }
    }
    public class HttpRecent
    {
        public PageData pageData { get; set; }
        public string resumeType { get; set; }
        public List<RecentResult> results { get; set; }
        public string jobId { get; set; }
    }

}
