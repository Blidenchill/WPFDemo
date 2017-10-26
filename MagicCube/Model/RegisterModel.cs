using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicCube.HttpModel;
using System.Collections.ObjectModel;
using MagicCube.Common;

namespace MagicCube.Model
{
    public class RegisterModel:NotifyBaseModel
    {
        private string userName = string.Empty;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                NotifyPropertyChange(() => UserName);
            }
        }

        private string avatarUrl = string.Empty;
        public string AvatarUrl
        {
            get { return avatarUrl; }
            set
            {
                avatarUrl = value;
                NotifyPropertyChange(() => AvatarUrl);
            }
        }

        private string position = string.Empty;
        public string Position
        {
            get { return position; }
            set
            {
                position = value;
                NotifyPropertyChange(() => Position);
            }
        }

        private string email = string.Empty;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                NotifyPropertyChange(() => Email);
            }
        }

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

        private HttpCityCodes city;
        public HttpCityCodes City
        {
            get { return city; }
            set
            {
                city = value;
                NotifyPropertyChange(() => City);
            }
        }


        private string fullCity = string.Empty;
        public string FullCity
        {
            get
            {
                return fullCity;
            }
            set
            {
                fullCity = value;
                NotifyPropertyChange(() => FullCity);
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


        private HttpProvinceCityCodes province;
        public HttpProvinceCityCodes Province
        {
            get { return province; }
            set
            {
                province = value;
                NotifyPropertyChange(() => Province);
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

        private ObservableCollection<string> tags = new ObservableCollection<string>();

        public ObservableCollection<string> Tags
        {
            get
            {
                return tags;
            }
            set
            {
                tags = value;
                NotifyPropertyChange(() => Tags);
            }
        }

        private ObservableCollection<string> imageUrls = new ObservableCollection<string>();
        public ObservableCollection<string> ImageUrls
        {
            get { return imageUrls; }
            set
            {
                imageUrls = value;
                NotifyPropertyChange(() => ImageUrls);
            }
        }

        private int imgUrlCount;
        public int ImgUrlCount
        {
            get { return imgUrlCount; }
            set
            {
                imgUrlCount = value;
                NotifyPropertyChange(() => ImgUrlCount);
            }
        }

        private bool tagsValidationBool = false;
        public bool TagsValidationBool
        {
            get
            {


                return tagsValidationBool;
            }
            set
            {
                tagsValidationBool = value;
                NotifyPropertyChange(() => TagsValidationBool);
            }
        }

        private string smallCompanyLogoUrl;
        public string SmallCompanyLogoUrl
        {
            get { return smallCompanyLogoUrl; }
            set
            {
                smallCompanyLogoUrl = value;
                NotifyPropertyChange(() => SmallCompanyLogoUrl);
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

        private HttpCompanyCodes nature = new HttpCompanyCodes();
        public HttpCompanyCodes Nature
        {
            get { return nature; }
            set
            {
                nature = value;
                NotifyPropertyChange(() => Nature);
            }
        }

        public string NatureStr
        {
            get;set;
        }

        private HttpCompanyCodes size = new HttpCompanyCodes();
        public HttpCompanyCodes Size
        {
            get { return size; }
            set
            {
                size = value;
                NotifyPropertyChange(() => Size);
            }
        }

        public string SizeStr
        {
            get;set;
        }

        private HttpCompanyCodes financingState = new HttpCompanyCodes();
        public HttpCompanyCodes FinancingState
        {
            get { return financingState; }
            set
            {
                financingState = value;
                NotifyPropertyChange(() => FinancingState);
            }
        }

        public string FinacingStateStr
        {
            get;set;
        }


        private string webSite;
        public string WebSite
        {
            get { return webSite; }
            set
            {
                webSite = value;
                NotifyPropertyChange(() => WebSite);
            }
        }

        public string CompanyLatLng { get; set; }

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

        private string authenPictureUrl = string.Empty;
        public string AuthenPictureUrl
        {
            get { return authenPictureUrl; }
            set
            {
                authenPictureUrl = value;
                NotifyPropertyChange(() => AuthenPictureUrl);
            }
        }


        private bool isCompanyValidation = true;
        public bool IsCompanyValidation
        {
            get
            {
                return isCompanyValidation;
            }
            set
            {
                isCompanyValidation = value;
                NotifyPropertyChange(() => IsCompanyValidation);
            }
        }
        private string addTag;
        public string AddTag
        {
            get
            {
                return addTag;
            }
            set
            {
                addTag = value;
                NotifyPropertyChange(() => AddTag);
            }
        }





    }


}
