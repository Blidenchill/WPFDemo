using MagicCube.Common;
using MagicCube.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace MagicCube.HttpModel
{

    public class HttpUnconfirmResume : NotifyBaseModel
    {
        public string province { get; set; }
        public string city { get; set; }
        public string workingExp { get; set; }
        public string name { get; set; }
        public string degree { get; set; }
        public string dob { get; set; }
        public string avatarUrl { get; set; }
        public string gender { get; set; }
        public string targetWorkLocation { get; set; }
        public string district { get; set; }
        public string targetSalary { get; set; }
        public List<string> targetPosition { get; set; }
    }

    public class HttpUnconfirmItem : NotifyBaseModel
    {
        public bool hasQuestion { get; set; }
        public string status { get; set; }
        public string enteripseStatus { get; set; }   
        public string result { get; set; }
        public int onsiteInterviewId { get; set; }
        public string degree { get; set; }
        public HttpUnconfirmResume resume { get; set; }
        public string mobile { get; set; }
        public string gender { get; set; }
        public int onsiteJobId { get; set; }
        public int userId { get; set; }
        private string _name;
        public string name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                    return "姓名未填写";
                else
                    return _name;
            }
            set
            {
                _name = value;

            }
        }
        public int allScore { get; set; }
        public string targetJobTypes { get; set; }
        public int onsiteTimeSlotId { get; set; }
        public int answerScore { get; set; }
        public string createTime { get; set; }
        public int onsiteCustomerId { get; set; }
        public string _age;
        public string age
        {
            get
            {
                if (resume == null)
                    return string.Empty;
                else
                {
                    if (string.IsNullOrWhiteSpace(resume.dob))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return TimeHelper.getAge(resume.dob) + "岁";
                    }
                   
                }

                
            }
            set
            {
                _age = value;

            }
        }

        private int _match;
        public int match
        {
            get
            {
                if (allScore == 0)
                    return 100;
                return (int)((answerScore * 100) / allScore);
            }
            set
            {
                _match = value;

            }
        }
        private string _avatarUrl;
        public string avatarUrl
        {
            get
            {
                if (resume == null)
                    return Downloader.LocalPath("", _name);
                else
                    return Downloader.LocalPath(resume.avatarUrl, _name);
            }
            set
            {
                _avatarUrl = value;

            }
        }

        public HttpInterviewResume interviewResume
        {
            get;
            set;
        }
        public HttpQuestionRoot interviewQuestion
        {
            get;
            set;
        }

        private Visibility _openStatus;
        public Visibility openStatus
        {
            get
            {
                if (string.IsNullOrEmpty(enteripseStatus))
                {
                    return Visibility.Visible;
                }
                else
                {
                    if (enteripseStatus == "WAITING_REVIEW")
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        return Visibility.Visible;
                    }
                    
                }
            }
            set
            {
                _openStatus = value ;
            }
        }

        private Visibility _closeStatus;
        public Visibility closeStatus
        {
            get
            {
                if (string.IsNullOrEmpty(enteripseStatus))
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    if (enteripseStatus == "WAITING_REVIEW")
                    {
                        return Visibility.Visible;
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
                if (!string.IsNullOrWhiteSpace(degree))
                {
                    _lstPerInfo.Add(degree);
                }
                if (resume != null)
                {
                    if (!string.IsNullOrWhiteSpace(resume.workingExp))
                    {
                        _lstPerInfo.Add(resume.workingExp);
                    }
                    if (!string.IsNullOrWhiteSpace(resume.province))
                    {
                        _lstPerInfo.Add(resume.province);
                    }
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
                if (resume != null)
                {
                    if (!string.IsNullOrWhiteSpace(resume.targetWorkLocation))
                    {
                        _lstJobInfo.Add(resume.targetWorkLocation);
                    }
                }
                if (!string.IsNullOrWhiteSpace(targetPosition))
                {
                    _lstJobInfo.Add(targetPosition);
                }
                if (resume != null)
                {
                    if (!string.IsNullOrWhiteSpace(resume.targetSalary))
                    {
                        _lstJobInfo.Add(resume.targetSalary);
                    }
                }
                return _lstJobInfo;
            }
            set
            {
                _lstJobInfo = value;
            }
        }
         private string _targetPosition;
        public string targetPosition
        {
            get
            {
               
                _targetPosition = string.Empty;
                if (resume != null)
                {
                    if (resume.targetPosition != null)
                    {
                        foreach (string it in resume.targetPosition)
                        {
                            _targetPosition += it + "、";
                        }
                    }
                    _targetPosition = _targetPosition.TrimEnd(new char[] { '、' });
                }
                return _targetPosition;
            }
            set
            {
                _targetPosition = value;
            }
        }
        
                    
    }

    public class HttpUnconfirmList : NotifyBaseModel
    {
        public ObservableCollection<HttpUnconfirmItem> items { get; set; }
        public int total { get; set; }
    }

    public class HttpUnconfirm : NotifyBaseModel
    {
        public string status { get; set; }
        public string updatedTime { get; set; }
        public int onsiteCustomerId { get; set; }
        public int reservedNum { get; set; }

        public int visitedNum { get; set; }
        public bool deleted { get; set; }
        public int userId { get; set; }
        public int onsiteJobId { get; set; }
        public string ownerType { get; set; }
        public string onsiteJobName { get; set; }
        public string createdTime { get; set; }

        private HttpUnconfirmList _list;
        public HttpUnconfirmList list
        {
            get
            {
                return _list;
            }
            set
            {
                _list = value;
                NotifyPropertyChange(() => list);
            }
        }


        public int onsiteTimeSlotId { get; set; }
        public int owner { get; set; }
        public int id { get; set; }
        public string startTime { get; set; }
        /// <summary>
        /// 面试开始时间
        /// </summary>
        private string _time;
        public string time
        {
            get
            {
                return Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm");
            }
            set
            {
                _time = value;
            }
        }
        private Visibility _moreVisibility;
        public Visibility moreVisibility
        {
            get
            {
                return _moreVisibility;
            }
            set
            {
                _moreVisibility = value;
                NotifyPropertyChange(() => moreVisibility);
            }
        }
        private Visibility _NoResuleVisibility = Visibility.Collapsed;
        public Visibility NoResuleVisibility
        {
            get
            {
                return _NoResuleVisibility;
            }
            set
            {
                _NoResuleVisibility = value;
                NotifyPropertyChange(() => NoResuleVisibility);
            }
        }
    }

    public class HttpConfirmPrice : NotifyBaseModel
    {
        public string price { get; set; }
    }
}
