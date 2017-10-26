using MagicCube.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace MagicCube.Model
{
    public class ResumeDetailModel : NotifyBaseModel
    {
        public ResumeDetailModel()
        {
            tags = new ObservableCollection<ResumeTag>();
        }
    
        public long userID
        {
            get;
            set;
        }
        public int deliveryID
        {
            get;
            set;
        }
        public string resumeSourceDesc
        {
            get;
            set;
        }
        public int accountStatus
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
        public string avatar
        {
            get;
            set;
        }
        public string genderDesc
        {
            get;
            set;
        }
        public string age
        {
            get;
            set;
        }
        public string educationDesc
        {
            get;
            set;
        }
        public string workExp
        {
            get;
            set;
        }
        public string email
        {
            get;
            set;
        }
        private string _mobile;
        public string mobile
        {
            get
            {
                return _mobile;
            }
            set
            {
                _mobile = value;
                NotifyPropertyChange(() => mobile);
            }
        }
        public string location
        {
            get;
            set;
        }

        public string Salary
        {
            get;
            set;
        }
        public double _priceSalary;
        public double priceSalary
        {
            get
            {
                return _priceSalary;
            }
            set
            {
                _priceSalary = value;
                NotifyPropertyChange(() => priceSalary);
            }
        }
        public double _costSalary;
        public double costSalary
        {
            get
            {
                return _costSalary;
            }
            set
            {
                _costSalary = value;
                NotifyPropertyChange(() => costSalary);
            }
        }
        public string _loadSalary;
        public string loadSalary
        {
            get
            {
                return _loadSalary;
            }
            set
            {
                _loadSalary = value;
                NotifyPropertyChange(() => loadSalary);
            }
        }
        public string exptArea
        {
            get;
            set;
        }
        public string exptPositionDesc
        {
            get;
            set;
        }

        public string exptIndustryDesc
        {
            get;
            set;
        }
        public string statusDesc
        {
            get;
            set;
        }
        public List<PersonExperienceJob> personExperienceJob
        {
            get;
            set;
        }

        public List<PersonExperienceProject> personExperienceProject
        {
            get;
            set;
        }
        public List<PersonExperienceEducation> personExperienceEducation
        {
            get;
            set;
        }
        public string advantage
        {
            get;
            set;
        }

        
        public string selfEval
        {
            get;
            set;
        }
        public string language
        {
            get;
            set;
        }
        public Visibility selfEvallanguageVis
        {
            get;
            set;

        }
        public Visibility personExperienceJobVis
        {
            get;
            set;
        }
        public Visibility personExperienceProjectVis
        {
            get;
            set;
        }
        public Visibility personExperienceEducationVis
        {
            get;
            set;
        }

        private Visibility _priceVis = Visibility.Visible;
        public Visibility priceVis
        {
            get
            {
                return _priceVis;
            }
            set
            {
                _priceVis = value;
                NotifyPropertyChange(() => priceVis);
            }
        }
        private List<string> _lstPerInfo;
        public List<string> lstPerInfo
        {
            get
            {
                _lstPerInfo = new List<string>();
                if (!string.IsNullOrWhiteSpace(location))
                {
                    _lstPerInfo.Add(CityCodeHelper.GetCityName(location));
                }
                if (!string.IsNullOrWhiteSpace(educationDesc))
                {
                    _lstPerInfo.Add(educationDesc);
                }
                if (!string.IsNullOrWhiteSpace(workExp))
                {
                    _lstPerInfo.Add(workExp);
                }

                return _lstPerInfo;
            }
            set
            {
                _lstPerInfo = value;
            }
        }

        private string _resumeTime;
        public string resumeTime
        {
            get
            {
                return _resumeTime;
            }
            set
            {
                _resumeTime = value;
                NotifyPropertyChange(() => resumeTime);
            }
        }
        private string _timeType = "更新";
        public string timeType
        {
            get
            {
                return _timeType;
            }
            set
            {
                _timeType = value;
                NotifyPropertyChange(() => timeType);
            }
        }

        private string _jobName;
        public string jobName
        {
            get
            {
                return _jobName;
            }
            set
            {
                _jobName = value;
                NotifyPropertyChange(() => jobName);
            }
        }
        private string _jobType;
        public string jobType
        {
            get
            {
                return _jobType;
            }
            set
            {
                _jobType = value;
                NotifyPropertyChange(() => jobType);
            }
        }

        private bool _viewContact;
        public bool viewContact
        {
            get
            {
                return _viewContact;
            }
            set
            {
                _viewContact = value;
                NotifyPropertyChange(() => viewContact);
            }
        }
        private bool _collectResume;
        public bool collectResume
        {
            get
            {
                return _collectResume;
            }
            set
            {
                _collectResume = value;
                NotifyPropertyChange(() => collectResume);
            }
        }
        public ObservableCollection<ResumeTag> tags { get; set; }

        private Visibility _canTag =  Visibility.Collapsed;
        public Visibility canTag
        {
            get
            {
                return _canTag;
            }
            set
            {
                _canTag = value;
            }
        }

        private string _downloadType;
        public string downloadType
        {
            get
            {
                return _downloadType;
            }
            set
            {
                _downloadType = value;
                NotifyPropertyChange(() => downloadType);
            }
        }

        public ObservableCollection<ResumeComments> resumeComments { get; set; }
    }
    public class PersonExperienceJob
    {
        //        type	职能类别	string	是	三级职能-
        //name	职位名称	string	否	
        //desc	职位描述	string	否	
        //salary	薪资	int	是	
        //companyName	公司名称	string	否	
        //industry	公司行业	int	是	
        //workStartTime	在职起始时间	int	否	年月
        //workEndTime	在职结束时间	int	否	年月
        //department	所属部门	string	否	
        //scale	公司规模	int	是	
        //charact	公司性质	int	是	
        //workDesc	工作内容	string	否

        public string companyName
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
        public string workStartTime
        {
            get;
            set;
        }
        public string _workEndTime;
        public string workEndTime
        {
            get
            {
                if (_workEndTime.Contains("9999"))
                    _workEndTime = "至今";
                return _workEndTime;
            }
            set
            {
                _workEndTime = value;
            }
        }
        public string workTime
        {
            get
            {
                string temp = string.Empty;

                temp = workStartTime.Replace("-", ".") + " — " + workEndTime.Replace("-", ".");
                temp = temp.Trim().Trim('-');
                return temp;
            }
            set
            {
              
            }
        }
        public string workTimeSmall
        {
            get
            {
                string temp = string.Empty;

                temp = workStartTime.Replace("-", ".") + "-" + workEndTime.Replace("-", ".");
                temp = temp.Trim().Trim('-');
                return temp;
            }
            set
            {

            }
        }

        public string workDesc
        {
            get;
            set;
        }
        private string _timeSpn;
        public string timeSpn
        {
            get
            {
                if (workStartTime == null)
                    return string.Empty;
                if (workEndTime == null)
                    return string.Empty;
                int yearStart = 0;
                int monthStart = 0;
                int yearEnd = 0;
                int monthEnd = 0;
                string[] startStr = workStartTime.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string[] endStr = workEndTime.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string str = string.Empty;
                if (startStr.Length == 2 && endStr.Length == 2)
                {
                    int.TryParse(startStr[0], out yearStart);
                    int.TryParse(startStr[1], out monthStart);
                    int.TryParse(endStr[0], out yearEnd);
                    int.TryParse(endStr[1], out monthEnd);
                }
                try
                {
                    DateTime dtStart = DateTime.Parse(workStartTime);
                    DateTime dtEnd = new DateTime();
                    if (workEndTime == "至今")
                    {
                        dtEnd = DateTime.Now;
                    }
                    else if (workEndTime.Contains("9999"))
                    {
                        dtEnd = DateTime.Now;
                    }
                    else
                    {
                        dtEnd = DateTime.Parse(workEndTime);
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
                _timeSpn = value;
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
                if (!string.IsNullOrWhiteSpace(name))
                {
                    _lstJob.Add(name);
                }
                if (workTimeSmall != "-")
                {
                    _lstJob.Add(workTimeSmall + timeSpn);
                }
                return _lstJob;
            }
            set
            {
                _lstJob = value;
            }
        }
    }
    public class PersonExperienceProject
    {
        //projectName	项目名称	string	否	
        //projectRole	项目角色	string	否	
        //projectDesc	项目描述	string	否	
        //startTime	开始时间	int	否	年月
        //achieve	业绩	string	否	
        //dutyDesc	职责描述	string	否	
        //tool	使用工具	string	否	
        //endTime	结束时间	int	否	年月
        public string projectName
        {
            get;
            set;
        }
        public string projectRole
        {
            get;
            set;
        }
        public string projectDesc
        {
            get;
            set;
        }
        public string startTime
        {
            get;
            set;
        }
        public string _endTime;
        public string endTime
        {
            get
            {
                if (_endTime.Contains("9999"))
                    _endTime = "至今";
                return _endTime;
            }
            set
            {
                _endTime = value;
            }
        }
        public string projectTime
        {
            get
            {
                string temp = string.Empty;
                temp = startTime.Replace("-", ".") + " — " + endTime.Replace("-", ".");
                temp = temp.Trim().Trim('-');
                return temp;
            }
            set
            {

            }
        }
    }

    public class PersonExperienceEducation
    {
        //school	学校	string	否	
        //major	专业	string	否	
        //education	学历	int	是	
        //startTime	开始时间	int	否	年月
        //endTime	结束时间	int	否	年月
        //unifyEnroll	是否统招	int	是	跟学历联动
        //schoolExp	在校经历	string	否
        public string school
        {
            get;
            set;
        }
        public string major
        {
            get;
            set;
        }
        public int education
        {
            get;
            set;
        }
        public string educationDesc
        {
            get
            {
               return MinDegreeHelper.GetName(education.ToString());
            }
            set
            {
                value = null;
            }
        }

        public string startTime
        {
            get;
            set;
        }
        public string _endTime;
        public string endTime
        {
            get
            {
                if (_endTime.Contains("9999"))
                    _endTime = "至今";
                return _endTime;
            }
            set
            {
                _endTime = value;
            }
        }
        public string educationTime
        {
            get
            {
                string temp = string.Empty;
                temp = startTime.Replace("-", ".") + " — " + endTime.Replace("-", ".");
                temp = temp.Trim().Trim('-');
                return temp;
            }
            set
            {

            }
        }
    }
    public class ResumeComments
    {
        public string comment { get; set; }
        public Visibility canDelete { get; set; }
        public string hrName { get; set; }
        public int commentID { get; set; }
        public string createTime { get; set; }
    }
    public static partial class SetModel 
    {
        public static ResumeDetailModel SetResumeDetailModel(MagicCube.ViewModel.HttpResumeDetial pHttpResumeDetial)
        {
            ResumeDetailModel pResumeDetailModel = new ResumeDetailModel();
            pResumeDetailModel.userID = pHttpResumeDetial.userID;
            pResumeDetailModel.name = pHttpResumeDetial.name;
            pResumeDetailModel.viewContact = pHttpResumeDetial.isViewRelation;
            pResumeDetailModel.collectResume = pHttpResumeDetial.collectResume;
            pResumeDetailModel.name = ResumeName.GetName(pHttpResumeDetial.name, pResumeDetailModel.userID.ToString());                          
            pResumeDetailModel.accountStatus = pHttpResumeDetial.accountStatus;
            if (pHttpResumeDetial.gender == 0)
                pResumeDetailModel.avatar = Downloader.LocalPath(pHttpResumeDetial.avatar, pHttpResumeDetial.name, true);
            else
                pResumeDetailModel.avatar = Downloader.LocalPath(pHttpResumeDetial.avatar, pHttpResumeDetial.name);
            pResumeDetailModel.genderDesc = pHttpResumeDetial.genderDesc;
            pResumeDetailModel.age = pHttpResumeDetial.age;
            pResumeDetailModel.workExp = pHttpResumeDetial.workingExp;
            pResumeDetailModel.email = pHttpResumeDetial.email;
            pResumeDetailModel.mobile = pHttpResumeDetial.mobile;
            pResumeDetailModel.educationDesc = pHttpResumeDetial.educationDesc;
            if (string.IsNullOrWhiteSpace(pHttpResumeDetial.resumeSourceDesc))
            {
                pResumeDetailModel.resumeSourceDesc = string.Empty;
            }
            else
            {
                pResumeDetailModel.resumeSourceDesc = pHttpResumeDetial.resumeSourceDesc == "魔方" ? "魔方" : "其他招聘网站";
            }
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.province))
            {
                pResumeDetailModel.location = pHttpResumeDetial.province;
            }
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.city))
            {
                pResumeDetailModel.location = pHttpResumeDetial.city;
            }
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.district))
            {
                pResumeDetailModel.location = pHttpResumeDetial.district;
            }

            pResumeDetailModel.exptArea = pHttpResumeDetial.careerObjectiveAreaDesc;
            if (!string.IsNullOrWhiteSpace(pResumeDetailModel.exptArea))
            {
                pResumeDetailModel.exptArea = pResumeDetailModel.exptArea.Replace("$$", "、").Replace("中国","");
            }
            else
            {
                pResumeDetailModel.exptArea = "未填写";
            }

            if (pHttpResumeDetial.minSalary > 0 || pHttpResumeDetial.maxSalary > 0)
            {
                pResumeDetailModel.Salary = pHttpResumeDetial.minSalary.ToString() + "K - " + pHttpResumeDetial.maxSalary.ToString() + "K";
            }
            else
            {
                pResumeDetailModel.Salary = "面议";
            }
            pResumeDetailModel.priceSalary = pHttpResumeDetial.minSalary*10;
            if (pResumeDetailModel.priceSalary<20)
            {
                pResumeDetailModel.priceSalary = 20;
            }
            if (pResumeDetailModel.priceSalary > 250)
            {
                pResumeDetailModel.priceSalary = 250;
            }
            pResumeDetailModel.costSalary = pResumeDetailModel.priceSalary*0.2;
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.exptPositionDesc))
            {
                pResumeDetailModel.exptPositionDesc = pHttpResumeDetial.exptPositionDesc.Replace("$$", "、");
            }
            else
            {
                pResumeDetailModel.exptPositionDesc = "未填写";
            }

            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.statusDesc))
            {
                pResumeDetailModel.statusDesc = pHttpResumeDetial.statusDesc;
            }
            else
            {
                pResumeDetailModel.statusDesc = "未填写";
            }



            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.exptIndustryDesc))
            {
                pResumeDetailModel.exptIndustryDesc = pHttpResumeDetial.exptIndustryDesc.Replace("$$", "、");
            }
            else
            {
                pResumeDetailModel.exptIndustryDesc = "未填写";
            }



            if (pHttpResumeDetial.personExperienceJob != null)
            {
                pResumeDetailModel.personExperienceJob = DAL.JsonHelper.ToObject<List<PersonExperienceJob>>(pHttpResumeDetial.personExperienceJob);
                if (pResumeDetailModel.personExperienceJob.Count > 0)
                {
                    pResumeDetailModel.personExperienceJobVis = Visibility.Visible;
                }
                else
                {
                    pResumeDetailModel.personExperienceJobVis = Visibility.Collapsed;

                }
            }
            else
            {
                pResumeDetailModel.personExperienceJobVis = Visibility.Collapsed;
            }
            if (pHttpResumeDetial.personExperienceProject != null)
            {
                pResumeDetailModel.personExperienceProject = DAL.JsonHelper.ToObject<List<PersonExperienceProject>>(pHttpResumeDetial.personExperienceProject);
                if (pResumeDetailModel.personExperienceProject.Count > 0)
                {
                    pResumeDetailModel.personExperienceProjectVis = Visibility.Visible;
                }
                else
                {
                    pResumeDetailModel.personExperienceProjectVis = Visibility.Collapsed;

                }
            }
            else
            {
                pResumeDetailModel.personExperienceProjectVis = Visibility.Collapsed;
            }
            if (pHttpResumeDetial.personExperienceEducation != null)
            {
                pResumeDetailModel.personExperienceEducation = DAL.JsonHelper.ToObject<List<PersonExperienceEducation>>(pHttpResumeDetial.personExperienceEducation);
                if (pResumeDetailModel.personExperienceEducation.Count > 0)
                {
                    pResumeDetailModel.personExperienceEducationVis = Visibility.Visible;
                }
                else
                {
                    pResumeDetailModel.personExperienceEducationVis = Visibility.Collapsed;

                }
            }
            else
            {
                pResumeDetailModel.personExperienceEducationVis = Visibility.Collapsed;
            }
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.advantage))
                pResumeDetailModel.advantage = pHttpResumeDetial.advantage.Replace("$$", "、");
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.customSelfLabel))
                pResumeDetailModel.advantage += "、"+ pHttpResumeDetial.customSelfLabel.Replace("$$", "、");
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.language))
                pResumeDetailModel.advantage += "、" + pHttpResumeDetial.language.Replace("$$", "、");
            if (!string.IsNullOrWhiteSpace(pResumeDetailModel.advantage))
                pResumeDetailModel.advantage = pResumeDetailModel.advantage.Trim('、');
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.selfEval))
                pResumeDetailModel.selfEval = pHttpResumeDetial.selfEval.Replace("$$", "  ");
            if (string.IsNullOrWhiteSpace(pResumeDetailModel.selfEval) && string.IsNullOrWhiteSpace(pResumeDetailModel.language))
            {
                pResumeDetailModel.selfEvallanguageVis = Visibility.Collapsed;
            }
            else
            {
                pResumeDetailModel.selfEvallanguageVis = Visibility.Visible;
            }
            pResumeDetailModel.resumeTime = pHttpResumeDetial.resumeUpdateTime.ToString("yyyy-MM-dd");
            if (pResumeDetailModel.resumeTime.Contains("0001"))
            {
                pResumeDetailModel.resumeTime = string.Empty;
                pResumeDetailModel.timeType = string.Empty;
            }
            pResumeDetailModel.resumeComments = new ObservableCollection<ResumeComments>();
            if (pHttpResumeDetial.resumeComments.Count>0)
            {
                foreach(ViewModel.HttpresumeComments iHttpresumeComments in pHttpResumeDetial.resumeComments)
                {
                    ResumeComments pResumeComments = new ResumeComments();
                    pResumeComments.canDelete = iHttpresumeComments.canDelete == true? Visibility.Visible: Visibility.Hidden;
                    pResumeComments.comment = iHttpresumeComments.comment;
                    pResumeComments.commentID = iHttpresumeComments.commentID;
                    pResumeComments.createTime = iHttpresumeComments.createTime.ToString("yyyy-MM-dd");
                    pResumeComments.hrName = iHttpresumeComments.hrName;
                    if (string.IsNullOrEmpty(pResumeComments.hrName))
                        pResumeComments.hrName = iHttpresumeComments.ownerName;
                    pResumeDetailModel.resumeComments.Add(pResumeComments);
                }
            }

            return pResumeDetailModel;

        }
    }
}
