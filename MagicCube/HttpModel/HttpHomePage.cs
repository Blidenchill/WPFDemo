using MagicCube.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace MagicCube.HttpModel
{
    public class HttpToday : NotifyBaseModel
    {
        /// <summary>
        /// Items
        /// </summary>
        public ObservableCollection<HttpSeasonItems> items { get; set; }
        /// <summary>
        /// Total
        /// </summary>
        public int total { get; set; }
    }





    public class HttpSeasonItems : NotifyBaseModel
    {
        public string interviewDate { get; set; }
        public string name { get; set; }
        public string updatedTime { get; set; }
        public string startTime { get; set; }
        public bool deleted { get; set; }
        public int onsiteJobId { get; set; }
        public string ownerType { get; set; }
        public string createdTime { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public int owner { get; set; }
        public string endTime { get; set; }
        public int id { get; set; }
        public int onsiteCustomerId { get; set; }
        public int reservedNum { get; set; }
        public int visitedNum { get; set; }
        public double surplus { get; set; }
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

        /// <summary>
        /// 距离面试开始剩余时间
        /// </summary>
        private string _timeSurplus = string.Empty;
        public string timeSurplus
        {
            get
            {
                if (surplus > 24 * 60 * 60)
                {
                    int ihour = (int)surplus / 3600;
                    int iday = (int)ihour / 24;
                    int ihourpart = ihour - iday * 24;
                    _timeSurplus = iday.ToString() + "天" + ihourpart.ToString() + "小时";
                    return iday.ToString() + "天" + ihourpart.ToString() + "小时";
                }
                else
                {
                    int ihour = (int)surplus / 60;
                    int iday = (int)ihour / 60;
                    int ihourpart = ihour - iday * 60;
                    _timeSurplus = iday.ToString() + "小时" + ihourpart.ToString() + "分";
                    return iday.ToString() + "小时" + ihourpart.ToString() + "分";
                }
            }
            set
            {
                value = timeSurplus;
            }
        }

        private Visibility _openSurplus;
        public Visibility openSurplus
        {
            get
            {
                if (surplus > 0)
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
                _openSurplus = value;
            }
        }

        private Visibility _closeSurplus;
        public Visibility closeSurplus
        {
            get
            {
                if (surplus < 0)
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
                _closeSurplus = value;
            }
        }
    }

    public class OnsiteJobTimeslot : NotifyBaseModel
    {
        /// <summary>
        /// Items
        /// </summary>
        public ObservableCollection<HttpSeasonItems> items { get; set; }
        /// <summary>
        /// Total
        /// </summary>
        public int total { get; set; }
    }

    public class HttpOtherdayItems : NotifyBaseModel
    {

        public string minAge { get; set; }

        public string accountMobile { get; set; }

        public string minSalary { get; set; }

        public string ownerType { get; set; }

        public string maxAge { get; set; }

        public string officeLocation { get; set; }

        public int owner { get; set; }

        public string maxSalary { get; set; }

        public string lng { get; set; }

        public int id { get; set; }

        public string category { get; set; }

        public string businessCode { get; set; }

        public string business { get; set; }

        public string district { get; set; }

        public string introduction { get; set; }

        public string projectId { get; set; }

        public int degreeCode { get; set; }

        public string provinceCode { get; set; }

        public string location { get; set; }

        public string busLine { get; set; }

        public string type { get; set; }

        public string price { get; set; }

        public string province { get; set; }

        public string districtCode { get; set; }

        public string cityCode { get; set; }
        public string advantage { get; set; }
        public string city { get; set; }
        public bool deleted { get; set; }
        public string updatedBy { get; set; }
        public string onstieCustomerName { get; set; }
        public string interviewLocation { get; set; }
        public int needCount { get; set; }
        public string categoryCode { get; set; }
        public string createdBy { get; set; }
        public int onstieCustomerId { get; set; }
        public string lat { get; set; }
        public string locationCode { get; set; }
        public string updatedTime { get; set; }
        public int offerType { get; set; }
        public string name { get; set; }
        public string level { get; set; }
        public string mobile { get; set; }
        public string positionWeight { get; set; }
        public string createdTime { get; set; }
        private OnsiteJobTimeslot _onsiteJobTimeslot;
        public OnsiteJobTimeslot onsiteJobTimeslot
        {
            get
            {
                return _onsiteJobTimeslot;
            }
            set
            {
                _onsiteJobTimeslot = value;
                NotifyPropertyChange(()=>onsiteJobTimeslot);
            }
        }
        public string totalPrice { get; set; }
        public int reservedNum { get; set; }
        public int visitedNum { get; set; }
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
        public HttpJobResumeNum jobResumeNum { get; set; }
    }

    public class HttpOtherday
    {
        /// <summary>
        /// Items
        /// </summary>
        public ObservableCollection<HttpOtherdayItems> items { get; set; }
        /// <summary>
        /// Total
        /// </summary>
        public int total { get; set; }
    }

    public class HttpJobResumeNum
    {
        /// <summary>
        /// ReservedJobResumeNum
        /// </summary>
        public int reservedJobResumeNum { get; set; }
        /// <summary>
        /// VisitedJobResumeNum
        /// </summary>
        public int visitedJobResumeNum { get; set; }
    }
}
