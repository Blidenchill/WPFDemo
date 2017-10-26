using MagicCube.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace MagicCube.Model
{
    public class ResumeTableModel
    {
        public ResumeTableModel()
        {
            tags = new ObservableCollection<ResumeTag>(); 
        }  
        public long userID
        {
            get;
            set;
        }
        public int deliveryID { get; set; }
        public string name
        {
            get;
            set;
        }
        public string thumbnailName
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
        public string location
        {
            get;
            set;
        }
        public List<PersonExperienceJob> career
        {
            get;
            set;
        }

        private List<string> _lstPerInfo;
        public List<string> lstPerInfo
        {
            get
            {
                _lstPerInfo = new List<string>();
                if (!string.IsNullOrWhiteSpace(genderDesc))
                {
                    _lstPerInfo.Add(genderDesc);
                }
                if (!string.IsNullOrWhiteSpace(age))
                {
                    _lstPerInfo.Add(age);
                }
                if (!string.IsNullOrWhiteSpace(CityCodeHelper.GetCityName(location)))
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
        public string resumeTime
        {
            get;
            set;
        }
        public string timeType
        {
            get;
            set;
        }
        public long jobID
        {
            get;
            set;
        }
        public string jobName
        {
            get;
            set;
        }
        public string jobType
        {
            get;
            set;
        }
        public string LastWork
        {
            get
            {
                if (career != null)
                {
                    if (career.Count > 0)
                        return (career[career.Count - 1].name + "-" + career[career.Count - 1].companyName).Trim().Trim('—');
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
        public ObservableCollection<ResumeTag> tags { get; set; }

        public string Salary
        {
            get;
            set;
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

        private List<string> _lstExptInfo;
        public List<string> lstExptInfo
        {
            get
            {
                _lstExptInfo = new List<string>();
                if (!string.IsNullOrWhiteSpace(exptArea))
                {
                    _lstExptInfo.Add(exptArea);
                }
                if (!string.IsNullOrWhiteSpace(exptPositionDesc))
                {
                    _lstExptInfo.Add(exptPositionDesc);
                }
                if (!string.IsNullOrWhiteSpace(exptIndustryDesc))
                {
                    _lstExptInfo.Add(exptIndustryDesc);
                }
                if (!string.IsNullOrWhiteSpace(statusDesc))
                {
                    _lstExptInfo.Add(statusDesc);
                }
                if (!string.IsNullOrWhiteSpace(Salary))
                {
                    _lstExptInfo.Add(Salary);
                }

                return _lstExptInfo;
            }
            set
            {
                _lstExptInfo = value;
            }
        }
        public string mobile
        {
            get;
            set;
        }
        public string email
        {
            get;
            set;
        }
    }
    public class ResumeTag
    {
        public string color { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }
    public static partial class SetModel 
    {
        public static ResumeTableModel SetResumeTableModel(MagicCube.ViewModel.HttpCollect pHttpResumeDetial)
        {
            ResumeTableModel pResumeTableModel = new ResumeTableModel();
            pResumeTableModel.userID = pHttpResumeDetial.resume.userID;
            if (pHttpResumeDetial.job != null)
            {
                pResumeTableModel.jobID = pHttpResumeDetial.job.jobID;
                pResumeTableModel.jobName = pHttpResumeDetial.job.jobName;
            }
            pResumeTableModel.deliveryID = pHttpResumeDetial.flow.deliveryID;
            pResumeTableModel.name = pHttpResumeDetial.resume.name;
            pResumeTableModel.name = ResumeName.GetName(pHttpResumeDetial.resume.name); 
            pResumeTableModel.avatar = Downloader.LocalPath(pHttpResumeDetial.resume.avatar, pHttpResumeDetial.resume.name);
            pResumeTableModel.genderDesc = pHttpResumeDetial.resume.genderDesc;
            pResumeTableModel.age = pHttpResumeDetial.resume.age;
            pResumeTableModel.workExp = pHttpResumeDetial.resume.workingExp;
            pResumeTableModel.educationDesc = pHttpResumeDetial.resume.educationDesc;
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.province))
            {
                pResumeTableModel.location = pHttpResumeDetial.resume.province;
            }
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.city))
            {
                pResumeTableModel.location = pHttpResumeDetial.resume.city;
            }
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.district))
            {
                pResumeTableModel.location = pHttpResumeDetial.resume.district;
            }
            if (pHttpResumeDetial.resume.personExperienceJob != null)
            {
                pResumeTableModel.career = DAL.JsonHelper.ToObject<List<PersonExperienceJob>>(pHttpResumeDetial.resume.personExperienceJob);
            }
            pResumeTableModel.resumeTime = pHttpResumeDetial.flow.createTime.ToString("yyyy-MM-dd");

            pResumeTableModel.exptArea = pHttpResumeDetial.resume.careerObjectiveAreaDesc;
            if (!string.IsNullOrWhiteSpace(pResumeTableModel.exptArea))
            {
                pResumeTableModel.exptArea = pResumeTableModel.exptArea.Replace("$$", "、").Replace("中国", ""); ;
            }
            else
            {
                pResumeTableModel.exptArea = "";
            }

            if (pHttpResumeDetial.resume.minSalary > 0 || pHttpResumeDetial.resume.maxSalary > 0)
            {
                pResumeTableModel.Salary = pHttpResumeDetial.resume.minSalary.ToString() + "K - " + pHttpResumeDetial.resume.maxSalary.ToString() + "K";
            }
            else
            {
                pResumeTableModel.Salary = "面议";
            }


            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.exptPositionDesc))
            {
                pResumeTableModel.exptPositionDesc = pHttpResumeDetial.resume.exptPositionDesc.Replace("$$", "、");
            }
            else
            {
                pResumeTableModel.exptPositionDesc = "";
            }

            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.statusDesc))
            {
                pResumeTableModel.statusDesc = pHttpResumeDetial.resume.statusDesc;
            }
            else
            {
                pResumeTableModel.statusDesc = "";
            }



            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.exptIndustryDesc))
            {
                pResumeTableModel.exptIndustryDesc = pHttpResumeDetial.resume.exptIndustryDesc.Replace("$$", "、");
            }
            else
            {
                pResumeTableModel.exptIndustryDesc = "";
            }

            if (pHttpResumeDetial.flow.userSignList!=null)
            {
                if (pHttpResumeDetial.flow.userSignList.Count > 0) 
                {
                    foreach(MagicCube.ViewModel.HttpResumTags iHttpResumTags in pHttpResumeDetial.flow.userSignList)
                    {
                        pResumeTableModel.tags.Add(new ResumeTag() { id = iHttpResumTags.userSignID, name = iHttpResumTags.userSignName, color = iHttpResumTags.userSignColor });
                    }
                }
            }
            return pResumeTableModel;

        }

        public static ResumeTableModel SetResumeCollectModel(MagicCube.ViewModel.HttpCollect pHttpResumeDetial)
        {
            ResumeTableModel pResumeTableModel = new ResumeTableModel();
            pResumeTableModel.userID = pHttpResumeDetial.resume.userID;
            if(pHttpResumeDetial.flow!=null)
                pResumeTableModel.deliveryID = pHttpResumeDetial.flow.deliveryID;
            pResumeTableModel.name = pHttpResumeDetial.resume.name;
            pResumeTableModel.name = ResumeName.GetName(pHttpResumeDetial.resume.name);
            pResumeTableModel.avatar = Downloader.LocalPath(pHttpResumeDetial.resume.avatar, pHttpResumeDetial.resume.name);
            pResumeTableModel.genderDesc = pHttpResumeDetial.resume.genderDesc;
            pResumeTableModel.age = pHttpResumeDetial.resume.age;
            pResumeTableModel.workExp = pHttpResumeDetial.resume.workingExp;        
            pResumeTableModel.educationDesc = pHttpResumeDetial.resume.educationDesc;
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.province))
            {
                pResumeTableModel.location = pHttpResumeDetial.resume.province;
            }
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.city))
            {
                pResumeTableModel.location = pHttpResumeDetial.resume.city;
            }
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.district))
            {
                pResumeTableModel.location = pHttpResumeDetial.resume.district;
            }
            if (pHttpResumeDetial.resume.personExperienceJob != null)
            {
                pResumeTableModel.career = DAL.JsonHelper.ToObject<List<PersonExperienceJob>>(pHttpResumeDetial.resume.personExperienceJob);
            }
            pResumeTableModel.resumeTime = pHttpResumeDetial.relation.updateTime.ToString("yyyy-MM-dd");

            pResumeTableModel.exptArea = pHttpResumeDetial.resume.careerObjectiveAreaDesc;
            if (!string.IsNullOrWhiteSpace(pResumeTableModel.exptArea))
            {
                pResumeTableModel.exptArea = pResumeTableModel.exptArea.Replace("$$", "、").Replace("中国","");;
            }
            else
            {
                pResumeTableModel.exptArea = "";
            }

            if (pHttpResumeDetial.resume.minSalary > 0 || pHttpResumeDetial.resume.maxSalary > 0)
            {
                pResumeTableModel.Salary = pHttpResumeDetial.resume.minSalary.ToString() + "K - " + pHttpResumeDetial.resume.maxSalary.ToString() + "K";
            }
            else
            {
                pResumeTableModel.Salary = "面议";
            }


            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.exptPositionDesc))
            {
                pResumeTableModel.exptPositionDesc = pHttpResumeDetial.resume.exptPositionDesc.Replace("$$", "、");
            }
            else
            {
                pResumeTableModel.exptPositionDesc = "";
            }

            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.statusDesc))
            {
                pResumeTableModel.statusDesc = pHttpResumeDetial.resume.statusDesc;
            }
            else
            {
                pResumeTableModel.statusDesc = "";
            }



            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.exptIndustryDesc))
            {
                pResumeTableModel.exptIndustryDesc = pHttpResumeDetial.resume.exptIndustryDesc.Replace("$$", "、");
            }
            else
            {
                pResumeTableModel.exptIndustryDesc = "";
            }
            pResumeTableModel.timeType = "收藏时间";
            pResumeTableModel.jobType = "意向职位";
            pResumeTableModel.jobName = pResumeTableModel.exptPositionDesc;
            return pResumeTableModel;

        }

        public static ResumeTableModel SetResumeBuyModel(MagicCube.ViewModel.HttpCollect pHttpResumeDetial)
        {
            ResumeTableModel pResumeTableModel = new ResumeTableModel();
            pResumeTableModel.userID = pHttpResumeDetial.resume.userID;
            pResumeTableModel.name = pHttpResumeDetial.resume.name;
            pResumeTableModel.name = ResumeName.GetName(pHttpResumeDetial.resume.name);
            pResumeTableModel.avatar = Downloader.LocalPath(pHttpResumeDetial.resume.avatar, pHttpResumeDetial.resume.name);
            pResumeTableModel.genderDesc = pHttpResumeDetial.resume.genderDesc;
            pResumeTableModel.age = pHttpResumeDetial.resume.age;
            pResumeTableModel.workExp = pHttpResumeDetial.resume.workingExp;
            pResumeTableModel.educationDesc = pHttpResumeDetial.resume.educationDesc;
            pResumeTableModel.mobile = pHttpResumeDetial.resume.mobile;
            pResumeTableModel.email = pHttpResumeDetial.resume.email;
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.province))
            {
                pResumeTableModel.location = CityCodeHelper.GetCityName(pHttpResumeDetial.resume.province);
            }
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.city))
            {
                pResumeTableModel.location = CityCodeHelper.GetCityName(pHttpResumeDetial.resume.city);
            }
            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.district))
            {
                pResumeTableModel.location = CityCodeHelper.GetCityName(pHttpResumeDetial.resume.district);
            }
            if (pHttpResumeDetial.resume.personExperienceJob != null)
            {
                pResumeTableModel.career = DAL.JsonHelper.ToObject<List<PersonExperienceJob>>(pHttpResumeDetial.resume.personExperienceJob);
            }
            pResumeTableModel.resumeTime = pHttpResumeDetial.relation.updateTime.ToString("yyyy-MM-dd");

            pResumeTableModel.exptArea = pHttpResumeDetial.resume.careerObjectiveAreaDesc;
            if (!string.IsNullOrWhiteSpace(pResumeTableModel.exptArea))
            {
                pResumeTableModel.exptArea = pResumeTableModel.exptArea.Replace("$$", "、").Replace("中国", ""); ;
            }
            else
            {
                pResumeTableModel.exptArea = "";
            }

            if (pHttpResumeDetial.resume.minSalary > 0 || pHttpResumeDetial.resume.maxSalary > 0)
            {
                pResumeTableModel.Salary = pHttpResumeDetial.resume.minSalary.ToString() + "K - " + pHttpResumeDetial.resume.maxSalary.ToString() + "K";
            }
            else
            {
                pResumeTableModel.Salary = "面议";
            }


            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.exptPositionDesc))
            {
                pResumeTableModel.exptPositionDesc = pHttpResumeDetial.resume.exptPositionDesc.Replace("$$", "、");
            }
            else
            {
                pResumeTableModel.exptPositionDesc = "";
            }

            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.statusDesc))
            {
                pResumeTableModel.statusDesc = pHttpResumeDetial.resume.statusDesc;
            }
            else
            {
                pResumeTableModel.statusDesc = "";
            }



            if (!string.IsNullOrWhiteSpace(pHttpResumeDetial.resume.exptIndustryDesc))
            {
                pResumeTableModel.exptIndustryDesc = pHttpResumeDetial.resume.exptIndustryDesc.Replace("$$", "、");
            }
            else
            {
                pResumeTableModel.exptIndustryDesc = "";
            }
            pResumeTableModel.timeType = "更新时间";
            return pResumeTableModel;

        }
    }
}
