using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpPublishJob
    {
        public int isHRAuth { get; set; }
        public int isSimilarAuth { get; set; }
        public long userID
        {
            get;
            set;
        }
        public long companyID
        {
            get;
            set;
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
        public string jobName
        {
            get;
            set;
        }
        public string workCharact
        {
            get;
            set;
        }
        public string jobLocation
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
        public string jobDesc
        {
            get;
            set;
        }
        public string jobTemptation
        {
            get;
            set;
        }
        public string jobTemptationCustom
        {
            get;
            set;
        }

        public ObservableCollection<string> jobTemptationList
        {
            get
            {
                ObservableCollection<string> pOC = new ObservableCollection<string>();
                if (!string.IsNullOrWhiteSpace(jobTemptation))
                {
                    string[] pStr = jobTemptation.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string istr in pStr)
                    {
                        pOC.Add(istr);
                    }
                }
                if (!string.IsNullOrWhiteSpace(jobTemptationCustom))
                {
                    string[] pStr = jobTemptationCustom.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string istr in pStr)
                    {
                        pOC.Add(istr);
                    }
                }
                return pOC;
            }
            set
            {
                value = null;
            }
        }
        public string location
        {
            get;
            set;
        }
        public string jobCity
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

        public int status
        {
            get;
            set;
        }
        public int jobType
        {
            get;
            set;
        }
        public int jobSource  
        {
            get;
            set;
        }
         
    }
    public class HttpEditJob : HttpPublishJob
    {
        public long jobID
        {
            get;
            set;
        }
        
        public int currentStatus
        {
            get;
            set;
        }
    }
    public class HttpDetialJob : HttpEditJob
    {
        public DateTime createTime
        {
            get;
            set;
        }
        public DateTime updateTime
        {
            get;
            set;
        }
        public string refreshTime
        {
            get;
            set;
        }      
        
        
    }
    public class HttpSingleTolta
    {
        /// <summary>
        /// 14
        /// </summary>
        public int count { get; set; }
    }
    public class HttpJobListData
    {
        public HttpSingleTolta singleTolta { get; set; }
        public JobTotal total { get; set; }
        public List<HttpJobList> data { get; set; }
    }
    public class JobTotal
    {
        public int onlineJobNum { get; set; }
        public int offlineJobNum { get; set; }
    }
    public class HttpJobList
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
        public DateTime updateTime
        {
            get;
            set;
        }
        public int status
        {
            get;
            set;
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
        public int pass
        {
            get;
            set;
        }
        public int fail
        {
            get;
            set;
        }
        public int autoFilter
        {
            get;
            set;
        }
    }
    public class HttpJobCmb
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
    }

    public class HttpJobTemplate
    {
        public string jobTemplateDetail { get; set; }
        public string jobTemplateName { get; set; }
        public string jobTemplateID { get; set; }
    }

    public class HttpJobTemplateDetial
    {
        public string thirdJobFunc
        {
            get;
            set;
        }
        public string jobName
        {
            get;
            set;
        }
        public string workCharact
        {
            get;
            set;
        }
        public string jobLocation
        {
            get;
            set;
        }
        public string jobMinSalary
        {
            get;
            set;
        }
        public string jobMaxSalary
        {
            get;
            set;
        }
        public string jobDesc
        {
            get;
            set;
        }
        public string jobTemptation
        {
            get;
            set;
        }
        public string jobTemptationCustom
        {
            get;
            set;
        }
        public string location
        {
            get;
            set;
        }
        public string jobCity
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
        public ObservableCollection<string> jobTemptationList
        {
            get
            {
                ObservableCollection<string> pOC = new ObservableCollection<string>();
                if (!string.IsNullOrWhiteSpace(jobTemptation))
                {
                    string[] pStr = jobTemptation.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string istr in pStr)
                    {
                        pOC.Add(istr);
                    }
                }
                if (!string.IsNullOrWhiteSpace(jobTemptationCustom))
                {
                    string[] pStr = jobTemptationCustom.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string istr in pStr)
                    {
                        pOC.Add(istr);
                    }
                }
                return pOC;
            }
            set
            {
                value = null;
            }
        }
    }

}
