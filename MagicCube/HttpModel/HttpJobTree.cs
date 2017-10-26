using MagicCube.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpJobTreeItem
    {
        public string _age;
        public string age
        {
            get
            {
                return TimeHelper.getAge(dob);
            }
            set
            {
                _age = value;
            }
        }
        public string workingExp { get; set; }
        public string name { get; set; }
        public string degree { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string educationStr { get; set; }
        public string categoryCodesStr { get; set; }
        public string targetJobTypesStr { get; set; }
        public string jobName { get; set; }
        public string careerStr { get; set; }
        public int recordId { get; set; }
        public string resumeId { get; set; }
        public string targetWorkLocation { get; set; }
        public string location { get; set; }
        public string mobile { get; set; }
        public string targetSalary { get; set; }
        public string createdTime { get; set; }
        public int jobId { get; set; }
        public string targetJobType { get; set; }
        public string avatarUrl { get; set; }
        public string targetPositionStr { get; set; }
        public string flag { get; set; }
        public List<TagV> tagVs { get; set; }
    }
    public class HttpJobTree
    {
        public string jobName { get; set; }
        public int untreated { get; set; }
        public int jobId { get; set; }
        public List<HttpJobTreeItem> resume { get; set; }
    }
}
