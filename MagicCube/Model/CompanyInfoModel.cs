using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Model
{
    public class CompanyInfoModel:NotifyBaseModel
    {
        private string companyName = string.Empty;
        public string CompanyName
        {
            get { return companyName; }
            set
            {
                companyName = value;
                NotifyPropertyChange(() => CompanyName);
            }
        }

        private string briefName = string.Empty;
        public string BriefName
        {
            get { return briefName; }
            set
            {
                briefName = value;
                NotifyPropertyChange(() => BriefName);
            }
        }
        private string industries = string.Empty;
        public string Industries
        {
            get { return industries; }
            set
            {
                industries = value;
                NotifyPropertyChange(() => Industries);
            }
        }
        private string nature = string.Empty;
        public string Nature
        {
            get { return nature; }
            set
            {
                nature = value;
                NotifyPropertyChange(() => Nature);
            }
        }

        private string size = string.Empty;
        public string Size
        {
            get { return size; }
            set
            {
                size = value;
                NotifyPropertyChange(() => Size);
            }
        }

        private string[] tags;
        public string[] Tags
        {
            get { return tags; }
            set
            {
                tags = value;
                NotifyPropertyChange(() => Tags);
            }
        }

        private string intro;
        public string Intro
        {
            get { return intro; }
            set
            {
                intro = value;
                NotifyPropertyChange(() => Intro);
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                NotifyPropertyChange(() => Location);
            }
        }
        private string city;
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                NotifyPropertyChange(() => City);
            }
        }

        private string productUrl;
        public string ProductUrl
        {
            get { return productUrl; }
            set
            {
                productUrl = value;
                NotifyPropertyChange(() => ProductUrl);
            }
        }

        private string financingState;
        public string FinancingState
        {
            get { return financingState; }
            set
            {
                financingState = value;
                NotifyPropertyChange(() => FinancingState);
            }
        }

    }
}
