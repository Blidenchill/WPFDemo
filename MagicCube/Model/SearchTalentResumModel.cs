using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MagicCube.Common;
using MagicCube.HttpModel;

namespace MagicCube.Model
{
    public class SearchTalentResumModel : NotifyBaseModel
    {
        public string website { get; set; }
        public string maritalStatus { get; set; }
        public List<string> targetJobTypes { get; set; }
        public string targetSalary { get; set; }
        public int userId { get; set; }
        public string hukou { get; set; }
        public string resumeName { get; set; }
        public string activeTime { get; set; }
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
        private bool _isRead = false;
        public bool isRead
        {
            get { return _isRead; }
            set
            {
                _isRead = value;
                NotifyPropertyChange(() => isRead);
            }
        }

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
                if (education != null)
                {
                    if (education.Count != 0)
                    {
                        _selfInformation = location + "  ·  " + gender + "  ·  " + age + "岁  ·  " + education[0].degree + " - " + education[0].major + "-" + education[0].schoolName;
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
                NotifyPropertyChange(() => IsChoosed);
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

        //简历列表图绑定特殊需要的属性
        private string _educationStr;
        public string educationStr
        {
            get
            {
                if (education != null)
                {
                    if (education.Count != 0)
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
                if (career != null)
                {
                    if (career.Count != 0)
                    {
                        if (string.IsNullOrEmpty(career[0].jobName))
                        {
                            _careerStr = career[0].companyName;

                        }
                        else
                        {
                            _careerStr = career[0].jobName + "-" + career[0].companyName;
                        }
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
                NotifyPropertyChange(() => RedFlag);
            }
        }

        private ObservableCollection<TagsManagerModel> tagsList = new ObservableCollection<TagsManagerModel>();
        public ObservableCollection<TagsManagerModel> TagsList
        {
            get { return tagsList; }
            set { tagsList = value; }
        }

        public List<TagV> tagVs { get; set; }

        private string _flag = string.Empty;
        public string flag
        {
            get { return _flag; }
            set
            {
                _flag = value;
                NotifyPropertyChange(() => flag);
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


    }
    public class UserJobModel : NotifyBaseModel
    {
        public string jobId { get; set; }
        public string userId { get; set; }
    }
}
