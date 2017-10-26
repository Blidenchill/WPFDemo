using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MagicCube.HttpModel
{
    class HttpInterviewDetail
    {
        public List<HttpInterviewDetailItem> items { get; set; }
        public int total { get; set; }
    }


    public class HttpInterviewDetailItem
    {
        public bool hasQuestion { get; set; }
        public string status { get; set; }
        public int onsiteInterviewId { get; set; }
        public InterviewDetailResume resume { get; set; }
        public string mobile { get; set; }
        public int onsiteJobId { get; set; }
        public int userId { get; set; }
        public int onsiteTimeSlotId { get; set; }
        public int answerScore { get; set; }
        public int allScore { get; set; }
        public int onsiteCustomerId { get; set; }
        public string createTime { get; set; }

        public long presentInterviewTime { get; set; }
        public string enteripseStatus { get; set; }
        public string result { get; set; }
    }


    public class InterviewDetailResume
    {
        public string name { get; set; }
        public string degree { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string targetWorkLocation { get; set; }
        public string targetSalary { get; set; }
        public string province { get; set; }
        public string workingExp { get; set; }
        public List<string> targetPosition { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        //public string targetJobType { get; set; }
        public string avatarUrl { get; set; }
    }




}
