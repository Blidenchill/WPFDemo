using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MagicCube.Model
{
    public class InterviewDetailModel : NotifyBaseModel
    {
        private string avatarUrl = string.Empty;
        public string AvatarUrl
        {
            get { return avatarUrl; }
            set
            {
                this.avatarUrl = value;
                this.NotifyPropertyChange(() => AvatarUrl);
            }
        }

        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set
            {
                this.name = value;
                this.NotifyPropertyChange(() => Name);
            }
        }

        private string gender = string.Empty;
        public string Gender
        {
            get { return gender; }
            set
            {
                this.gender = value;
                this.NotifyPropertyChange(() => Gender);
            }
        }

        private string age = string.Empty;
        public string Age
        {
            get { return age; }
            set
            {
                this.age = value;
                this.NotifyPropertyChange(() => Age);
            }
        }

        private string education = string.Empty;

        public string Education
        {
            get { return education; }
            set
            {
                this.education = value;
                this.NotifyPropertyChange(() => Education);
            }
        }

        private string workingExp = string.Empty;
        public string WorkingExp
        {
            get { return workingExp; }
            set
            {
                this.workingExp = value;
                this.NotifyPropertyChange(() => WorkingExp);
            }
        }
        private string location = string.Empty;
        public string Location
        {
            get { return location; }
            set
            {
                this.location = value;
                this.NotifyPropertyChange(() => Location);
            }
        }
        private string expectWorkPlace = string.Empty;
        public string ExpectWorkPlace
        {
            get { return expectWorkPlace; }
            set
            {
                this.expectWorkPlace = value;
                this.NotifyPropertyChange(() => ExpectWorkPlace);
            }
        }
        private string expectJobName = string.Empty;
        public string ExpectJobName
        {
            get { return expectJobName; }
            set
            {
                this.expectJobName = value;
                this.NotifyPropertyChange(() => ExpectJobName);
            }
        }

        private string expectSalary = string.Empty;
        public string ExpectSalary
        {
            get { return expectSalary; }
            set
            {
                this.expectSalary = value;
                this.NotifyPropertyChange(() => ExpectSalary);
            }
        }

        private string matchPercent = string.Empty;
        public string MatchPercent
        {
            get { return matchPercent; }
            set
            {
                this.matchPercent = value;
                this.NotifyPropertyChange(() => MatchPercent);
            }
        }

        private string verifyTime = string.Empty;
        /// <summary>
        /// 面试到访确认时间
        /// </summary>
        public string VerifyTime
        {
            get { return verifyTime; }
            set
            {
                this.verifyTime = value;
                this.NotifyPropertyChange(() => VerifyTime);
            }
        }

        private string updateTime = string.Empty;
        public string UpdateTime
        {
            get { return updateTime; }
            set
            {
                this.updateTime = value;
                this.NotifyPropertyChange(() => UpdateTime);
            }
        }

        private InterviewProcess interviewProcessEnum;
        public InterviewProcess InterviewProcessEnum
        {
            get { return interviewProcessEnum; }
            set
            {
                this.interviewProcessEnum = value;
                this.NotifyPropertyChange(() => InterviewProcessEnum);
            }
        }

        private bool isNewInsert = false;
        public bool IsNewInsert
        {
            get { return isNewInsert; }
            set
            {
                this.isNewInsert = value;
                this.NotifyPropertyChange(() => IsNewInsert);
            }
        }

        private string enteripseStatus = string.Empty;
        /// <summary>
        /// 待审核状态 WAITING_REVIEW
        /// </summary>
        public string EnteripseStatus
        {
            get { return enteripseStatus; }
            set
            {
                this.enteripseStatus = value;
                this.NotifyPropertyChange(() => EnteripseStatus);
            }
        }
        private Visibility _openStatus;
        public Visibility openStatus
        {
            get
            {
                if (this.Status == "100")
                {
                    if (this.companyState != 2)
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            set
            {
                _openStatus = value;
            }
        }

        private Visibility _closeStatus;
        public Visibility closeStatus
        {
            get
            {
                if (this.Status == "120")
                {
                    return Visibility.Visible;
                }
                else
                {
                    if (this.Status == "100")
                    {
                        if (this.companyState == 2)
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }
                }
            }
            set
            {
                _closeStatus = value;
            }
        }

        public InterviewQuestion InterQuestion { get; set; }

        public ResumeDetailModel InterResume { get; set; }

        public string UserId { get; set; }

        public string jobId { get; set; }
        public string sessionRecordID { get; set; }
        public string Mobile { get; set; }
        public int mB { get; set; }
        public string OnsiteInterviewId { get; set; }

        public string Status { get; set; }
        public int companyState { get; set; }
        public string GetResult { get; set; }

        public bool hasQuestion { get; set; }
        private List<string> _lstPerInfo;
        public List<string> lstPerInfo
        {
            get
            {
                _lstPerInfo = new List<string>();
                if (!string.IsNullOrWhiteSpace(gender))
                {
                    _lstPerInfo.Add(gender);
                }
                if (!string.IsNullOrWhiteSpace(age))
                {
                    _lstPerInfo.Add(age);
                }
                if (!string.IsNullOrWhiteSpace(Education))
                {
                    _lstPerInfo.Add(Education);
                }
                if (!string.IsNullOrWhiteSpace(WorkingExp))
                {
                    _lstPerInfo.Add(WorkingExp);
                }
                if (!string.IsNullOrWhiteSpace(Location))
                {
                    _lstPerInfo.Add(Location);
                }
                return _lstPerInfo;
            }
            set
            {
                _lstPerInfo = value;
            }
        }
        private List<string> _lstJobInfo;
        public List<string> lstJobInfo
        {
            get
            {
                _lstJobInfo = new List<string>();
                if (!string.IsNullOrWhiteSpace(ExpectWorkPlace))
                {
                    _lstJobInfo.Add(ExpectWorkPlace);
                }
                if (!string.IsNullOrWhiteSpace(ExpectJobName))
                {
                    _lstJobInfo.Add(ExpectJobName);
                }
                if (!string.IsNullOrWhiteSpace(ExpectSalary))
                {
                    _lstJobInfo.Add(ExpectSalary);
                }
                return _lstJobInfo;
            }
            set
            {
                _lstJobInfo = value;
            }
        }
        private Visibility _quesionStatus;
        public Visibility quesionStatus
        {
            get
            {

                if (hasQuestion)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }


            }
            set
            {
                _quesionStatus = value;
            }
        }
        public double cost { get; set; }

    }
    public class InterviewQuestionItem
    {
        public List<string> answer { get; set; }
        public string question { get; set; }
        public string type { get; set; }
    }

    public class InterviewQuestion
    {
        public List<InterviewQuestionItem> question { get; set; }
    }
    public enum InterviewProcess
    {
        /// <summary>
        /// 预约面试
        /// </summary>
        Interview =10 ,
        /// <summary>
        /// 待确认到访
        /// </summary>
        WaitingOK = 100,
        /// <summary>
        /// 已到访
        /// </summary>
        CheckIn = 110,
    }
}
