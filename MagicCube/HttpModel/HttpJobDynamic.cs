using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicCube.Common;

namespace MagicCube.HttpModel
{
    public class HttpJobDynamic
    {
        public int status { get; set; }
        public HttpJobResult result { get; set; }
    }

    public class HttpJobDynamicContactRecordGet
    {
        public int status { get; set; }
        public JobDynamicItem result { get; set; }
    }

    public class HttpContactRecordGet
    {
        /// <summary>
        /// RecordId
        /// </summary>
        public int recordId { get; set; }
    }
    /// <summary>
    /// 职位动态
    /// </summary>
    public class HttpJobResult
    {
        public List<JobDynamicItem> items { get; set; }
        public int total { get; set; }
    }

    public class JobDynamicItem
    {
        public string updatedTime { get; set; }
        public string enterpriseUserId { get; set; }
        public JobDynamicResume resume { get; set; }
        public bool deleted { get; set; }
        public string userId { get; set; }
        public string jobId { get; set; }
        public string source { get; set; }
        public Job job { get; set; }
        public bool interestHasRead { get; set; }
        public bool read { get; set; }
        public JobDynamicUser user { get; set; }
        public string createdTime { get; set; }
        public string type { get; set; }
        public int id { get; set; }
        public string resumeName { get; set; }
    }
    public class JobDynamicResume
    {
        public string resumeId { get; set; }
        public string uniqueKey { get; set; }
    }

    public class Job
    {
        public string name { get; set; }
    }

    public class JobDynamicUser
    {
        private bool StrIsInt(string Str)
        {

            bool flag = true;

            if (Str != "")

            {

                for (int i = 0; i < Str.Length; i++)

                {

                    if (!Char.IsNumber(Str, i))

                    {

                        flag = false;

                        break;

                    }

                }

            }

            else

            {

                flag = false;

            }

            return flag;

        }
        public string workingExp { get; set; }
        public string name
        {
            get;
            set;
        }
        public string degree { get; set; }
        public string avatarUrl { get; set; }
        public string gender { get; set; }
        public string recentJob { get; set; }
        public string recentCompany { get; set; }
    }
}
