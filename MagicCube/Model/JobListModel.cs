using MagicCube.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Model
{
    public class JobManageModel : NotifyBaseModel
    {
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
        public int jobMinSalary
        {
            get;
            set;
        }
        public int jobMaxSalary
        {
            get;
            set;
        }
        public string jobCity
        {
            get;
            set;
        }
        public string location
        {
            get;
            set;
        }
        public string minDegree
        {
            get;
            set;
        }
        public string minExp
        {
            get;
            set;
        }
        public DateTime createTime
        {
            get;
            set;
        }
        public string refreshTime
        {
            get;
            set;
        }
        public bool _canRefresh;
        public bool canRefresh
        {
            get
            {
                if(string.IsNullOrWhiteSpace(refreshTime))
                {
                    return true;
                }
                else
                {
                    if(Convert.ToDateTime(refreshTime).ToString("d") == DateTime.Now.ToString("d"))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

            }
            set
            {
                _canRefresh = value;
            }
        }
        public int status
        {
            get;
            set;
        }
        public string _jobNeed;
        public string jobNeed
        {
            get
            {
                string temp = minDegree + " | " + minExp + " | " + jobMinSalary + "K - " + jobMaxSalary + "K";
                if (!string.IsNullOrEmpty(jobCity))
                {
                    temp += " | " + CityCodeHelper.GetCityName(jobCity);
                }
                return temp;
            }
            set
            {
                _jobNeed = value;
            }
        }

        public string publishTime
        {
            get
            {
                string temp = createTime.ToString("yyyy-MM-dd");
                
                return temp;
            }
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
        public string firstJobFunc
        {
            get;
            set;
        }
        public string secondJobFunc
        {
            get;
            set;
        }
        public string thirdJobFunc
        {
            get;
            set;
        }
        public DateTime updateTime
        {
            get;
            set;
        }
        public string updateTimeShow
        {
            get
            {
                string temp = updateTime.ToString("yyyy-MM-dd");

                return temp;
            }
        }

        public int initiative
        {
            get;
            set;
        }
        public int invite
        {
            get;
            set;
        }
        public int countPass
        {
            get;
            set;
        }
        public int countFail
        {
            get;
            set;
        }
        public int countReserveFail
        {
            get;
            set;
        }
    }

    public static partial class SetModel
    {
        public static JobManageModel SetJobManageModel(MagicCube.ViewModel.HttpJobList pHttpJobList)
        {
            JobManageModel pJobListModel = new JobManageModel();
            pJobListModel.jobID = Convert.ToInt64(pHttpJobList.jobID);
            pJobListModel.jobName = pHttpJobList.jobName;
                pJobListModel.createTime = pHttpJobList.createTime;
            pJobListModel.minDegree = MinDegreeHelper.GetName(pHttpJobList.minDegree);
            pJobListModel.minExp = MinExpHelper.GetName(pHttpJobList.minExp);

            pJobListModel.jobCity = pHttpJobList.jobCity;
            //pJobListModel.jobCity = CityCodeHelper.GetCityName(pHttpJobList.jobCity);
            pJobListModel.jobMinSalary = pHttpJobList.jobMinSalary;
            pJobListModel.jobMaxSalary = pHttpJobList.jobMaxSalary;
            pJobListModel.status = pHttpJobList.status;
            pJobListModel.refreshTime = pHttpJobList.refreshTime;
            pJobListModel.firstJobFunc = pHttpJobList.firstJobFunc;
            pJobListModel.secondJobFunc = pHttpJobList.secondJobFunc;
            pJobListModel.thirdJobFunc = pHttpJobList.thirdJobFunc;
            pJobListModel.updateTime = pHttpJobList.updateTime;
            pJobListModel.initiative = pHttpJobList.initiative;
            pJobListModel.invite = pHttpJobList.invite;
            pJobListModel.countPass = pHttpJobList.pass;
            pJobListModel.countFail = pHttpJobList.fail;
            pJobListModel.countReserveFail = pHttpJobList.autoFilter;
            return pJobListModel;

        }
    }
}
