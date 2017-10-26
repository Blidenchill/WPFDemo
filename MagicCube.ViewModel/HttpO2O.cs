using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpSessionListFlow
    {

        public int visitTotal { get; set; }

        public int appointmentTotal { get; set; }

        public int question { get; set; }

        public int jobID { get; set; }

        public int pass { get; set; }

        public int novisited { get; set; }

        public int visited { get; set; }

        public int toBeConfirmed { get; set; }

        public string sessionRecordID { get; set; }
    }

    public class HttpSessionList
    {

        public string sessionRecordID { get; set; }

        public DateTime sessionEndTime { get; set; }

        public DateTime sessionBeginTime { get; set; }

        public HttpSessionListFlow flow { get; set; }

        public string jobName { get; set; }

        public string jobSessionID { get; set; }

        public string jobID { get; set; }

        public double jobMinSalary { get; set; }

        public string jobMaxSalary { get; set; }

        public double customPrice { get; set; }

    }

    public class HttpJobSessionItem
    {

        public DateTime updateTime { get; set; }

        public DateTime sessionEndTime { get; set; }

        public DateTime sessionBeginTime { get; set; }

        public string sessionOperator { get; set; }

        public HttpSessionListFlow flowSession { get; set; }

        public string jobSessionID { get; set; }

        public string sessionState { get; set; }

        public string sessionRecordID { get; set; }

        public DateTime createTime { get; set; }
    }

    public class HttpJobSessionFlow
    {

        public int visitTotal { get; set; }

        public int appointmentTotal { get; set; }

        public int question { get; set; }

        public int jobID { get; set; }

        public int pass { get; set; }

        public int novisited { get; set; }

        public int visited { get; set; }

        public int toBeConfirmed { get; set; }

        public string sessionRecordID { get; set; }
    }

    public class HttpJobSessionTreeItem
    {

        public List<HttpJobSessionItem> jobSessions { get; set; }

        public HttpJobSessionFlow flowJob { get; set; }

        public string jobName { get; set; }

        public string jobID { get; set; }

        public double jobMinSalary { get; set; }
        public double customPrice { get; set; }
        public string jobMaxSalary { get; set; }
        public int totalCount { get; set; }
    }


    public class HttpO2OInterviewUser
    {

        public int minSalary { get; set; }

        public string provinceDesc { get; set; }

        public string educationDesc { get; set; }
        public string workingExp { get; set; }

        public string careerObjectiveAreaDesc { get; set; }

        public int maxSalary { get; set; }

        public string education { get; set; }

        public string city { get; set; }

        public string district { get; set; }

        public string joinWorkDate { get; set; }

        public string userID { get; set; }

        public string exptPositionDesc { get; set; }

        public string birthYear { get; set; }

        public string genderDesc { get; set; }

        public string exptIndustry { get; set; }

        public string province { get; set; }

        public string exptIndustryDesc { get; set; }

        public string name { get; set; }

        public string mobile { get; set; }

        public string gender { get; set; }

        public string birthdate { get; set; }

        public string avatar { get; set; }

        public string careerObjectiveArea { get; set; }

        public string exptPosition { get; set; }
    }
    // "sessionRecordID", "jobID", "states"
    public class HttpO2Ostates
    {
        public string sessionRecordID { get; set; }

        public string jobID { get; set; }

        public string orderByCreateTimeDesc { get; set; }
        public List<int> states { get; set; }

        public string pageSize { get; set; }
    }
    public class HttpO2OInterview
    {
        public string remark { get; set; }

        public DateTime interviewTime { get; set; }

        public int userState { get; set; }

        public int mB { get; set; }

        public int userID { get; set; }

        public int userFlowID { get; set; }

        public string sessionRecordID { get; set; }

        public DateTime updateTime { get; set; }

        public int jobID { get; set; }

        public string source { get; set; }
        public bool hasQuestion { get; set; }
        public string state { get; set; }
        public int totalSore { get; set; }
        public int score { get; set; }

        public int companyState { get; set; }

        public HttpO2OInterviewUser user { get; set; }

        public int projectOwnerID { get; set; }

        public int dispatchOwnerID { get; set; }

        public string flowSign { get; set; }

        public string createTime { get; set; }
        public int mB2 { get; set; }


        public HttpO2OInterviewExtraFields extraFields { get; set; }
    }
    public class HttpO2OInterviewExtraFields
    {
        public DateTime BConfirmTime { get; set; }
    }
    public class HttpSessionInterviewStatistics
    {
        public int visitTotal { get; set; }
        public int appointmentTotal { get; set; }
        public int question { get; set; }
        public long jobID { get; set; }
        public int pass { get; set; }
        public int novisited { get; set; }
        public int visited { get; set; }
        public int toBeConfirmed { get; set; }
        public string sessionRecordID { get; set; }
    }

    public class HttpSessionInterview
    {

        public string sessionRecordID { get; set; }
        public DateTime sessionEndTime { get; set; }
        public DateTime sessionBeginTime { get; set; }
        public List<HttpO2OInterview> flows { get; set; }
        public string jobName { get; set; }
        public int totalCount { get; set; }
        public HttpSessionInterviewStatistics statistics { get; set; }
        public string jobSessionID { get; set; }
        public string jobID { get; set; }
        public double jobMinSalary { get; set; }
        public double jobMaxSalary { get; set; }
        public double customPrice { get; set; }
    }
    public class HttpInterviewNum
    {
        public int underwaySessionNum { get; set; }
        public int expiredSessionNum { get; set; }
    }

    public class HttpUnconfimCount
    {
        public int count { get; set; }
    }

    public class HttpQuestionItemList
    {
        /// <summary>
        /// Score
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// 初中
        /// </summary>
        public string option { get; set; }
        /// <summary>
        /// OptionOrder
        /// </summary>
        public int optionOrder { get; set; }
        /// <summary>
        /// Selected
        /// </summary>
        public bool selected { get; set; }
    }

    public class HttpQuestionList
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// SINGLE
        /// </summary>
        public string questionType { get; set; }
        /// <summary>
        /// MustRight
        /// </summary>
        public bool mustRight { get; set; }
        /// <summary>
        /// Closed
        /// </summary>
        public bool closed { get; set; }
        /// <summary>
        /// 你的学历是？
        /// </summary>
        public string question { get; set; }
        /// <summary>
        /// QuestionItemList
        /// </summary>
        public List<HttpQuestionItemList> questionItemList { get; set; }

        public string essay { get; set; }
        /// <summary>
        /// QuestionOrder
        /// </summary>
        public int questionOrder { get; set; }
        /// <summary>
        /// SurveyId
        /// </summary>
        public string surveyId { get; set; }
    }

    public class HttpQuesionFeedBack
    {
        /// <summary>
        /// MinScore
        /// </summary>
        public string minScore { get; set; }
        /// <summary>
        /// MaxScore
        /// </summary>
        public int maxScore { get; set; }
        /// <summary>
        /// 哎呀,你好像不是特别适合这个职位啊,建议你看看其他职位吧~
        /// </summary>
        public string feedBack { get; set; }
    }

    public class HttpQuesion
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// SYSTEM
        /// </summary>
        public string surveyType { get; set; }
        /// <summary>
        /// JobId
        /// </summary>
        public int jobId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 系统问卷
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// QuestionList
        /// </summary>
        public List<HttpQuestionList> questionList { get; set; }
        /// <summary>
        /// RawScore
        /// </summary>
        public int rawScore { get; set; }
        /// <summary>
        /// ActualScore
        /// </summary>
        public int actualScore { get; set; }
        /// <summary>
        /// Passed
        /// </summary>
        public bool passed { get; set; }
        /// <summary>
        /// FeedBack
        /// </summary>
        public HttpQuesionFeedBack feedBack { get; set; }
        /// <summary>
        /// 2017-01-06 07:16:59
        /// </summary>
        public DateTime createdTime { get; set; }
    }
}
