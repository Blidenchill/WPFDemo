using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicCube.Common;

namespace MagicCube.HttpModel
{
    public class PageData
    {
        public int count { get; set; }
        public int start { get; set; }
        public int total { get; set; }
    }

    public class HttpRecommend
    {
        public List<Career> career { get; set; }
        public string major { get; set; }
        public string maritalStatus { get; set; }
        public List<string> targetJobTypes { get; set; }
        public string schoolName { get; set; }
        public string targetSalary { get; set; }
        public bool imConduct { get; set; }
        public int userId { get; set; }
        public bool showEnabled { get; set; }
        public int jobId { get; set; }
        public string hukou { get; set; }
        public string resumeName { get; set; }
        public List<string> targetPosition { get; set; }
        public int owner { get; set; }
        public List<Education> education { get; set; }
        public double integrity { get; set; }
        public int id { get; set; }
        public string jobName { get; set; }
        public string uniqueKey { get; set; }
        public string workingExp { get; set; }
        public string targetJobType { get; set; }
        public int enterpriseAccountId { get; set; }
        public string avatarUrl { get; set; }
        public string externalRefId { get; set; }
        public int degreeCode { get; set; }
        public int genderCode { get; set; }
        public List<NewIm> newIm { get; set; }
        public string source { get; set; }
        public string targetWorkLocation { get; set; }
        public bool read { get; set; }
        public string location { get; set; }
        public string skills { get; set; }
        public string email { get; set; }
        public string previousJobName { get; set; }
        public int workingExpCode { get; set; }
        public string degree { get; set; }
        public bool deleted { get; set; }
        public string ownerType { get; set; }
        public int minTargetSalary { get; set; }
        public int maxTargetSalary { get; set; }
        public bool searchEnabled { get; set; }
        public List<string> categoryCodes { get; set; }
        public string currentStatus { get; set; }
        public object recommendedTime { get; set; }
        public List<Project> projects { get; set; }
        public string qq { get; set; }
        public string updatedTime { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string gender { get; set; }
        public bool isRead { get; set; }
        public List<string> semanticCodes { get; set; }
        public string dob { get; set; }
        public string createdTime { get; set; }
        public string otherInformation { get; set; }
        public string previousCompanyName { get; set; }
        public string website { get; set; }
        public long? activeTime { get; set; }
        public int? invitedCount { get; set; }
        public string openId { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string externalType { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public bool? jobPushEnabled { get; set; }
        public int? salarySortWeight { get; set; }
        public List<string> sources { get; set; }
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
        public string selfIntroduction { get; set; }
        public string positionType { get; set; }
        public string resumeId { get; set; }
        public bool recommendHasRead { get; set; }
    }

    public class HttpRecommendResults
    {
        public PageData pageData { get; set; }
        public List<HttpRecommend> results { get; set; }
        public string jobId { get; set; }
    }
}
