using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpResumeItemModel
    {
        public string userID { get; set; }
        public string name { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string language { get; set; }
        public string district { get; set; }
        public string mobile { get; set; }
        public string gender { get; set; }
        public string genderDesc { get; set; }
        public string minSalary { get; set; }
        public string birthdate { get; set; }
        public string status { get; set; }
        public string statusDesc { get; set; }
        public string marriage { get; set; }
        public string avatar { get; set; }
        public string maxSalary { get; set; }
        public string education { get; set; }
        public string educationDesc { get; set; }
        public string email { get; set; }
        public string exptPosition { get; set; }
        public string exptPositionDesc { get; set; }
        public string exptIndustry { get; set; }
        public string exptIndustryDesc { get; set; }
        public string personExperienceJob { get; set; }
        public string personExperienceEducation { get; set;  }
        public string workingExp { get; set; }
        public string joinWorkDate { get; set; }
        public string resumeUpdateTime { get; set; }
        public string viewedResume { get; set; }

    }

    public class ExperienceJobModel
    {
        public string name { get; set; }
        public string salary { get; set; }
        public string companyName { get; set; }
        public string workStartTime { get; set; }
        public string workEndTime { get; set; }

        public string workDesc { get; set; }

    }

    public class ExperienceEducation
    {
        public string school { get; set; }
        public string major { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
    }


    public class HttpSearchResumeList
    {
        public int count { get; set; }
        public List<HttpResumeItemModel> resumeList { get; set; }
        public object aggregation { get; set; }
        public int pageCount { get; set; }
        
    }
}
