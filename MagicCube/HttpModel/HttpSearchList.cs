using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MagicCube.Common;
using MagicCube.Model;

namespace MagicCube.HttpModel
{
    public class HttpSearchList :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string m_partialValue;
        public string partialValue
        {
            get { return m_partialValue; }
            set 
            { 
                m_partialValue = value; 
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("partialValue"));
                }
            }
        }
        private string m_minAge;
        public string minAge
        {
            get { return m_minAge; }
            set
            {
                m_minAge = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("minAge"));
                }
            }
        }
        private string m_maxAge;
        public string maxAge
        {
            get { return m_maxAge; }
            set
            {
                m_maxAge = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("maxAge"));
                }
            }
        }
        private string m_minDegree;
        public string minDegree
        {
            get { return m_minDegree; }
            set
            {
                m_minDegree = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("minDegree"));
                }
            }
        }
        private string m_maxDegree;
        public string maxDegree
        {
            get { return m_maxDegree; }
            set
            {
                m_maxDegree = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("maxDegree"));
                }
            }
        }
        private string m_workingExp;
        public string workingExp
        {
            get { return m_workingExp; }
            set
            {
                m_workingExp = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("workingExp"));
                }
            }
        }
        private string m_timeInterval;
        public string timeInterval
        {
            get { return m_timeInterval; }
            set
            {
                m_timeInterval = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("timeInterval"));
                }
            }
        }

        private string m_targetSalary;
        public string targetSalary
        {
            get { return m_targetSalary; }
            set
            {
                m_targetSalary = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("targetSalary"));
                }
            }
        }
        private string m_location;
        public string location
        {
            get { return m_location; }
            set
            {
                m_location = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("location"));
                }
            }
        }
        private string m_targetWorkLocation;
        public string targetWorkLocation
        {
            get { return m_targetWorkLocation; }
            set
            {
                m_targetWorkLocation = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("targetWorkLocation"));
                }
            }
        }
    }

   


    public class Education
    {
        public string status { get; set; }
        public string major { get; set; }
        public string end { get; set; }
        public string degree { get; set; }
        public string schoolName { get; set; }
        public string isTongzhao { get; set; }
        public string start { get; set; }
    }

    public class Career
    {
        public string start { get; set; }
        public string jobName { get; set; }
        public string end { get; set; }
        public string jobDescription { get; set; }
        public string companyName { get; set; }
        private string _timeInterval;
        public string timeInterval
        {
            get
            {
                if (start == null)
                    return string.Empty;
                if (end == null)
                    return string.Empty;
                int yearStart = 0;
                int monthStart = 0;
                int yearEnd = 0;
                int monthEnd = 0;
                string[] startStr = start.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string[] endStr = end.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string str = string.Empty;
                if(startStr.Length == 2 && endStr.Length == 2)
                {
                    int.TryParse(startStr[0], out yearStart);
                    int.TryParse(startStr[1], out monthStart);
                    int.TryParse(endStr[0], out yearEnd);
                    int.TryParse(endStr[1], out monthEnd);
                }
                try
                {
                    DateTime dtStart = DateTime.Parse(start);
                    DateTime dtEnd = new DateTime();
                    if(end == "至今")
                    {
                        dtEnd = DateTime.Now;
                    }
                    
                    else
                    {
                        dtEnd = DateTime.Parse(end);
                    }
                    TimeSpan span = dtEnd - dtStart;
                    str = "(" + DateTimeOperation.DateDiff(dtStart, dtEnd) + ")";
                    str = str.Replace("(无)", "");
                }
                catch
                {

                }
                
             
                return str;
            }
            set
            {
                _timeInterval = value;
            }
        }

        private List<string> _lstJob;
        public List<string> lstJob
        {
            get
            {
                _lstJob = new List<string>();
                if (!string.IsNullOrWhiteSpace(companyName))
                {
                    _lstJob.Add(companyName);
                }
                if (!string.IsNullOrWhiteSpace(jobName))
                {
                    _lstJob.Add(jobName);
                }
                if (!string.IsNullOrWhiteSpace(start) || !string.IsNullOrWhiteSpace(end))
                {
                    _lstJob.Add(start + "-" + end + timeInterval);
                }
                return _lstJob;
            }
            set
            {
                _lstJob = value;
            }
        }
    }

    public class Project
    {
        public string start { get; set; }
        public string end { get; set; }
        public string projectDescription { get; set; }
        public string responsibility { get; set; }
        public string projectName { get; set; }
    }

    public class HttpSearchingResult : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public string website { get; set; }
        public string maritalStatus { get; set; }
        public List<string> targetJobTypes { get; set; }
        public string targetSalary { get; set; }
        public int userId { get; set; }
        public string hukou { get; set; }
        public string resumeName { get; set; }
        public object activeTime { get; set; }
        public List<string> targetPosition { get; set; }
        public List<Education> education { get; set; }
        public double integrity { get; set; }
        public List<Career> career { get; set; }
        public int salarySortWeight { get; set; }
        public string openId { get; set; }
        public string uniqueKey { get; set; }
        public string workingExp { get; set; }
        public string targetJobType { get; set; }
        public string avatarUrl { get; set; }
        public string externalRefId { get; set; }
        public string source { get; set; }
        public int resumeId { get; set; }
        public string targetWorkLocation { get; set; }
        public string location { get; set; }
        public string skills { get; set; }
        public string email { get; set; }
        public string province { get; set; }
        public string degree { get; set; }
        public string city { get; set; }
        public bool deleted { get; set; }
        public string externalType { get; set; }
        public bool searchEnabled { get; set; }
        public bool jobPushEnabled { get; set; }
        public List<Project> projects { get; set; }
        public string qq { get; set; }
        public string name { get; set; }
        public List<string> sources { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string dob { get; set; }
        public string gender { get; set; }
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
        public bool isRead { get; set; }
        public string jobId { get; set; }
        public string mobile { get; set; }
        public string selfIntroduction { get; set; }
        public string positionType { get; set; }
        public string otherInformation { get; set; }

        //简历所略图绑定特殊需要的属性
        public string _selfInformation;
        public string selfInformation
        {
            get
            {
                _selfInformation = string.Empty;
                if(education != null)
                {
                    if(education.Count != 0)
                    {
                        _selfInformation = location + "  ·  " + gender + "  ·  " + age + "岁  ·  " + education[0].degree + " - " + education[0].major + "-" + education[0].schoolName ;
                    }
                    else
                    {
                        _selfInformation = location + "  ·  " + gender + "  ·  " + age;
                    }
                }
               
                return _selfInformation;
            }
            set
            {
                _selfInformation = value;
            }
        }
        private bool _IsChoosed;
        public bool IsChoosed 
        {
            get { return _IsChoosed; }
            set
            {
                _IsChoosed = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("IsChoosed"));
                }
            }
        }
        private bool _ShowEdit;

        public bool ShowEdit
        {
            get { return _ShowEdit; }
            set
            {
                _ShowEdit = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ShowEdit"));
                }
            }
        }
           
        //简历列表图绑定特殊需要的属性
        private string _educationStr;
        public string educationStr
        {
            get
            {
                if(education != null)
                {
                    if(education.Count != 0)
                    {
                        _educationStr = education[0].degree + "/" + education[0].schoolName + "/" + education[0].major;
                    }
                }
                return _educationStr;
            }
            set
            {
                _educationStr = value;
            }
        }

        private string _careerStr;
        public string careerStr
        {
            get
            {
                if(career != null)
                {
                    if(career.Count != 0)
                    {
                        _careerStr = career[0].jobName + "-" + career[0].companyName;
                    }
                }
                return _careerStr;
            }
            set
            {
                _careerStr = value;
            }
        }


        private bool redFlag;
        public bool RedFlag
        {
            get
            {
                return redFlag;
            }
            set
            {
                redFlag = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("RedFlag"));
            }
        }

        private ObservableCollection<TagsManagerModel> tagsList = new ObservableCollection<TagsManagerModel>();
        public ObservableCollection<TagsManagerModel> TagsList
        {
            get { return tagsList; }
            set { tagsList = value; }
        }



        public List<TagV> tagVs { get; set; }
        public string flag { get; set; }
    }

    //public class SearchingResultBind
    //{
    //    public string name { get; set; }
    //    public string targetWorkLocation { get; set; }
    //    public string targetPosition { get; set; }
    //    public string education { get; set; }
    //    public string workingExp { get; set; }
    //    public string lastWork { get; set; }
    //    public string uniqueKey { get; set; }

    //    public bool Ischeck { get; set; }
    //}

    public class PageInfo
    {
        public int start { get; set; }
        public int dataTotal { get; set; }
        public int pageCount { get; set; }
    }

    public class SearchingTotalResult
    {
        public List<HttpSearchingResult> results { get; set; }
        public PageInfo page { get; set; }
    }

    /// <summary>
    /// 人才查询窗口Cmb控件下拉菜单选项
    /// </summary>
    public class SearchingCmbListItems
    {
        public List<string> timeInterval { get; set; }
        public List<string> targetSalary { get; set; }
        public List<string> targetWorkLocation { get; set; }
        public List<string> workingExp { get; set; }
        public List<string> degree { get; set; }
    }

    public class SaveSearchCondition : INotifyPropertyChanged
    {
        //public ObservableCollection<string> localConditionList = new ObservableCollection<string>();
        //public List<string> httpConditionList = new List<string>();
        //public int count = 0;
        private string localCondition = string.Empty;
        private string httpCondition = string.Empty;

        private string localHead = string.Empty;
        public string LocalCondition
        {
            get { return localCondition; }
            set
            { 
                localCondition = value;
                NotifyPropertyChanged("localCondition"); 
            }
        }
        public string LocalHead
        {
            get { return localHead; }
            set
            {
                localHead = value;
                NotifyPropertyChanged("localHead");
            }
        }
        public string HttpCondition
        {
            get { return httpCondition; }
            set
            {
                httpCondition = value;
                NotifyPropertyChanged("httpCondition");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string GetKeyword()
        {
            string temp = string.Empty;
            string[] listStr = httpCondition.Split(new char[] { '&'});
            foreach(string item in listStr)
            {
                string[] itemList = item.Split(new char[] { '=' });
                if (itemList.Length != 2)
                    break;
                if (itemList[0] == "partialValue")
                    return itemList[1];
            }
            return temp;
        }

        public string ConditionToJson()
        {
            List<string> paramNames = new List<string>();
            List<string> paramValues = new List<string>();
            string[] listStr = httpCondition.Split(new char[] { '&' });
            foreach(string item in listStr)
            {
                string[] itemList = item.Split(new char[] { '=' });
                if (itemList.Length != 2)
                    break;
                paramNames.Add(itemList[0]);
                paramValues.Add(itemList[1]);
            }
            return DAL.JsonHelper.JsonParamsToString(paramNames.ToArray(), paramValues.ToArray());
        }

    }

    public class JobPublish
    {
        public int id { get; set; }
        public string jobName { get; set; }
        public string minSalary { get; set; }
        public string maxSalary { get; set; }
    }

    public class ConstactRecordId
    {
        public string content { get; set; }
        public string contactRecordId { get; set; }
        public string imName { get; set; }
        public string imPassword { get; set; }
    }

}
