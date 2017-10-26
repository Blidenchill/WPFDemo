using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicCube.HttpModel;
using System.Collections.ObjectModel;

namespace MagicCube.Model
{
    public class SearchConditioanModel :NotifyBaseModel
    {
        private string keyWords = string.Empty;
        public string KeyWords
        {
            get { return keyWords; }
            set
            {
                this.keyWords = value;
                NotifyPropertyChange(() => KeyWords);
            }
        }

        private string companyName = string.Empty;
        public string CompanyName
        {
            get { return companyName; }
            set
            {
                this.companyName = value;
                NotifyPropertyChange(() => CompanyName);
            }
        }
        private bool onlyLastCompany;
        public bool OnlyLastCompany
        {
            get { return onlyLastCompany; }
            set
            {
                this.onlyLastCompany = value;
                NotifyPropertyChange(() => OnlyLastCompany);
            }
        }

        private List<ValCommon1> jobList = new List<ValCommon1>();
        public List<ValCommon1> JobList
        {
            get { return jobList; }
            set
            {
                this.jobList = value;
                NotifyPropertyChange(() => JobList);
            }
        }

        private List<HttpIndustresItem> industryList = new List<HttpIndustresItem>();
        public List<HttpIndustresItem> IndustryList
        {
            get { return industryList; }
            set
            {
                this.industryList = value;
                NotifyPropertyChange(() => IndustryList);   
            }
        }

        private string minAge;
        public string MinAge
        {
            get { return minAge; }
            set
            {
                this.minAge = value;
                NotifyPropertyChange(() => MinAge);
            }
        }

        private string maxAge;
        public string MaxAge
        {
            get { return maxAge; }
            set
            {
                this.maxAge = value;
                NotifyPropertyChange(() => MaxAge);
            }
        }

        private CodeModel workingExpLow = new CodeModel();
        public CodeModel WorkingExpLow
        {
            get { return workingExpLow; }
            set
            {
                this.workingExpLow = value;
                NotifyPropertyChange(() => workingExpLow);
            }
        }

        private CodeModel workingExpHigh = new CodeModel();
        public CodeModel WorkingExpHigh
        {
            get { return workingExpHigh; }
            set
            {
                this.workingExpHigh = value;
                NotifyPropertyChange(() => WorkingExpHigh);
            }
        }

        private CodeModel educationLow = new CodeModel();
        public CodeModel EducationLow
        {
            get { return educationLow; }
            set
            {
                this.educationLow = value;
                NotifyPropertyChange(() => EducationLow);
            }
        }

        private CodeModel educationHigh = new CodeModel();
        public CodeModel EducationHigh
        {
            get { return educationHigh; }
            set
            {
                this.educationHigh = value;
                NotifyPropertyChange(() => EducationHigh);
            }

        }

        private CodeModel salaryLow = new CodeModel();
        public CodeModel SalaryLow
        {
            get { return salaryLow; }
            set
            {
                this.salaryLow = value;
                NotifyPropertyChange(() => SalaryLow);
            }
        }
        private CodeModel salaryHigh = new CodeModel();
        public  CodeModel SalaryHigh
        {
            get { return salaryHigh; }
            set
            {
                this.salaryHigh = value;
                NotifyPropertyChange(() => salaryHigh);
            }
        }

        //private HttpProvinceCityCodes expectProvince;
        //public HttpProvinceCityCodes ExpectProvince
        //{
        //    get { return expectProvince; }
        //    set
        //    {
        //        expectProvince = value;
        //        NotifyPropertyChange(() => ExpectProvince);
        //    }
        //}
        //private HttpCityCodes expectCity = new HttpCityCodes();
        //public HttpCityCodes ExpectCity
        //{
        //    get { return expectCity; }
        //    set
        //    {
        //        this.expectCity = value;
        //        NotifyPropertyChange(() => ExpectCity);
        //    }
        //}
        private ObservableCollection<HttpCityCodes> expectCity = new ObservableCollection<HttpCityCodes>();
        public ObservableCollection<HttpCityCodes> ExpectCity
        {
            get
            {
                return expectCity;
            }
            set
            {
                this.expectCity = value;
                NotifyPropertyChange(() => ExpectCity);
            }
        }

        private ObservableCollection<HttpCityCodes> locationCity = new ObservableCollection<HttpCityCodes>();
        public ObservableCollection<HttpCityCodes> LocationCity
        {
            get
            {
                return locationCity;
            }
            set
            {
                this.locationCity = value;
                NotifyPropertyChange(() => LocationCity);
            }
        }
        //private HttpProvinceCityCodes locationProvince;
        //public HttpProvinceCityCodes LocationProvince
        //{
        //    get { return locationProvince; }
        //    set
        //    {
        //        locationProvince = value;
        //        NotifyPropertyChange(() => LocationProvince);
        //    }
        //}
        //private HttpCityCodes locationCity = new HttpCityCodes();
        //public HttpCityCodes LocationCity
        //{
        //    get { return locationCity; }
        //    set
        //    {
        //        this.locationCity = value;
        //        NotifyPropertyChange(() => LocationCity);
        //    }
        //}

        private string gender = string.Empty;
        public string Gender
        {
            get { return gender; }
            set
            {
                this.gender = value;
                NotifyPropertyChange(() => Gender);
            }
        }

        private CodeModel updateTime = new CodeModel();
        public CodeModel UpdateTime
        {
            get { return updateTime; }
            set
            {
                updateTime = value;
                NotifyPropertyChange(() => UpdateTime);
            }
        }

        private string major = string.Empty;
        public string Major
        {
            get { return major; }
            set
            {
                this.major = value;
                NotifyPropertyChange(() => Major);
            }
        }

        private string school = string.Empty;
        public string School
        {
            get { return school; }
            set
            {
                this.school = value;
                NotifyPropertyChange(() => School);
            }
        }

        private CodeModel lauguage = new CodeModel();
        public CodeModel Lauguage
        {
            get { return lauguage; }
            set
            {
                this.lauguage = value;
                NotifyPropertyChange(() => Lauguage);
            }
        }

        private CodeModel status = new CodeModel();
        public CodeModel Status
        {
            get { return status; }
            set
            {
                this.status = value;
                NotifyPropertyChange(() => Status);
            }
        }
        private bool group211;
        public bool Group211
        {
            get { return group211; }
            set
            {
                this.group211 = value;
                NotifyPropertyChange(() => Group211);
            }
        }

        private bool group985;
        public bool Gourp985
        {
            get { return group985; }
            set
            {
                this.group985 = value;
                NotifyPropertyChange(() => group985);
            }
        }

        private bool fullTime;
        public bool FullTime
        {
            get { return fullTime; }
            set
            {
                this.fullTime = value;
                NotifyPropertyChange(() => FullTime);
            }
        }

        private bool oversea;
        public bool Oversea
        {
            get { return oversea; }
            set
            {
                this.oversea = value;
                NotifyPropertyChange(() => Oversea);
            }
        }
        
           
        




    }
}
