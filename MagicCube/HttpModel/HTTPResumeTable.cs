using MagicCube.Common;
using MagicCube.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpResumeTable
    {
        public HttpPageData pageDate
        {
            get;
            set;
        }


        public List<HttpResumeInterviewer> results
        {
            get;
            set;
        }
    }

    public class HttpResumeInterviewer : NotifyBaseModel
    {

        public int recordId
        {
            get;
            set;
        }
        public string workingExp
        {
            get;
            set;
        }
        public string gender
        {
            get;
            set;
        }
        private string _name;
        public string name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                {               
                    return "未填写";
                }
                else
                {
                    return _name;
                }
            }
            set
            {
                _name = value;
            }
        }
        public string degree
        {
            get;
            set;
        }

        public List<Career> career
        {
            get;
            set;
        }

        private string _educationStr;
        public string educationStr
        {
            get
            {
                if (_educationStr != null)
                {
                    List<HttpEducation> pTable = DAL.JsonHelper.ToObject<List<HttpEducation>>(_educationStr.Replace("\n", ""));
                    return pTable[0].degree + " - " + pTable[0].major + "-" + pTable[0].schoolName;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _educationStr = value;
            }
        }
        public string jobId
        {
            get;
            set;
        }

        public int resumeId
        {
            get;
            set;
        }

        public int id
        {
            get;
            set;
        }
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

        public string location
        {
            get;
            set;
        }

        public string dob
        {
            get;
            set;
        }

        public string jobName
        {
            get;
            set;
        }



        private bool _IsCheck;

        public bool IsCheck
        {
            get { return _IsCheck; }
            set
            {
                _IsCheck = value;
                NotifyPropertyChange(() => IsCheck);
            }
        }
        private bool _ShowEdit;

        public bool ShowEdit
        {
            get { return _ShowEdit; }
            set
            {
                _ShowEdit = value;
                NotifyPropertyChange(() => ShowEdit);
            }
        }

        private bool _judgeEnable;

        public bool judgeEnable
        {
            get { return _judgeEnable; }
            set
            {
                _judgeEnable = value;
                NotifyPropertyChange(() => judgeEnable);
            }
        }
      
        public string LastWork
        {
            get
            {
                if (career != null)
                {
                    if (career.Count > 0)
                        return career[career.Count - 1].jobName + "-" + career[career.Count - 1].companyName;
                    else
                        return "暂无工作经历";
                }
                else
                    return "暂无工作经历";
            }
            set
            {
                LastWork = value;
            }
        }
        private string _selfInformation;
        public string selfInformation
        {
            get
            {
                _selfInformation = string.Empty;

                _selfInformation = location + "  ·  " + gender + "  ·  " + age + "岁  ·  " + educationStr;

           

                return _selfInformation;
            }
            set
            {
                _selfInformation = value;
            }
        }

        public string targetSalary { get; set; }
        public string targetWorkLocation { get; set; }

        private string _targetPositionStr;
        public string targetPositionStr
        {
            get
            {
                if (_targetPositionStr != null)
                {
                    List<string> pTable = DAL.JsonHelper.ToObject<List<string>>(_targetPositionStr.Replace("\n", "").Replace("\t", ""));
                    if (pTable != null)
                    {
                        string t = pTable[0];
                        for (int i = 1; i < pTable.Count; i++)
                        {
                            t += "/" + pTable[i];
                        }
                        return t;
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _targetPositionStr = value;
            }
        }

        private string _avatarUrl;
        public string avatarUrl
        {
            get
            {
                _avatarUrl = Downloader.LocalPath(_avatarUrl, this.name);       
                return _avatarUrl;
            }
            set
            {
                _avatarUrl = value;
            }
        }

        public string _createdTime;
        public string createdTime
        {
            get
            {
                return (Convert.ToDateTime(_createdTime.Replace("Z", "").Replace("T", " ")).AddHours(8)).ToString();
            }
            set
            {
                _createdTime = value;
            }
        }


        private string _flag;

        public string flag
        {
            get { return _flag; }
            set
            {
                _flag = value;
                NotifyPropertyChange(() => flag);
            }
        }
        public ObservableCollection<TagV> tagVs { get; set; }
        public string typeStandard { get; set; }

        public string typeTime{ get; set; }
    }

    public class HttpPageData
    {
        public int count
        {
            get;
            set;
        }

        public int start
        {
            get;
            set;
        }

        public int dataTotal
        {
            get;
            set;
        }
    }
   
}
