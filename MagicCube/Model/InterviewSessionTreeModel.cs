using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace MagicCube.Model
{
    public class InterviewSessionTreeModel : NotifyBaseModel
    {
        public string jobName { get; set; }
        public double cost { get; set; }
        public string time { get; set; }
        public int reservedNum { get; set; }
        public int visitedNum { get; set; }
        public int totalCount { get; set; }
        public string sessionRecordID { get; set; }

        private ObservableCollection<InterviewDetailModel> _items = new ObservableCollection<InterviewDetailModel>();
        public ObservableCollection<InterviewDetailModel> items
        {
            get { return _items; }
            set
            {
                _items = value;
                NotifyPropertyChange(() => items);
            }
        }

        private Visibility _moreVisibility;
        public Visibility moreVisibility
        {
            get { return _moreVisibility; }
            set
            {
                _moreVisibility = value;
                NotifyPropertyChange(() => moreVisibility);
            }
        }

        private Visibility _NoResuleVisibility;
        public Visibility NoResuleVisibility
        {
            get { return _NoResuleVisibility; }
            set
            {
                _NoResuleVisibility = value;
                NotifyPropertyChange(() => NoResuleVisibility);
            }
        }
    }

    public class InterviewSessionListModel : NotifyBaseModel
    {
        public string jobName { get; set; }
        public string time { get; set; }
        public int reservedNum { get; set; }
        public int visitedNum { get; set; }
        public string sessionRecordID { get; set; }
        public string jobID { get; set; }
        public double surplus { get; set; }
        public ViewSingle.jobType isOpen { get; set; }

        private string _timeSurplus = string.Empty;
        public string timeSurplus
        {
            get
            {
                if (surplus > 0)
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
                else
                {
                    return "0小时0分";
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
                if (isOpen == ViewSingle.jobType.open)
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
                if (isOpen == ViewSingle.jobType.close)
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
        public double jobMinSalary { get; set; }
    }

    public class InterviewJobTreeModel : NotifyBaseModel
    {
        public string jobName { get; set; }
        public string jobID { get; set; }
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
        public int totalCount { get; set; }
        public ObservableCollection<InterviewSessionListModel> _items = new ObservableCollection<InterviewSessionListModel>();
        public ObservableCollection<InterviewSessionListModel> items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                NotifyPropertyChange(() => items);
            }
        }
    }
}
