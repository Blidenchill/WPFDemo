using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using MagicCube.Common;
using MagicCube.HttpModel;

using System.Windows;

namespace MagicCube.Model
{
    public class InfomationModel : NotifyBaseModel, IDataErrorInfo
    {
        #region "个人信息"

        private string realName = string.Empty;
        public string RealName
        {
            get { return realName; }
            set
            {
                this.realName = value;
                NotifyPropertyChange(() => RealName);
            }



        }

        private string userPosition = string.Empty;
        public string UserPosition
        {
            get { return userPosition; }
            set
            {
                this.userPosition = value;
                NotifyPropertyChange(() => UserPosition);
            }
        }
        private string avatarUrl = string.Empty;
        public string AvatarUrl
        {
            get { return avatarUrl; }
            set
            {
                this.avatarUrl = value;
                NotifyPropertyChange(() => AvatarUrl);
            }
        }
        private string officePhone = string.Empty;
        public string OfficePhone
        {
            get { return officePhone; }
            set
            {
                this.officePhone = value;
                NotifyPropertyChange(() => OfficePhone);
            }
        }

        private string telephone = string.Empty;
        public string Telephone
        {
            get { return telephone; }
            set
            {
                telephone = value;
                NotifyPropertyChange(() => Telephone);
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

        private string extensionNumber = string.Empty;
        public string ExtensionNumber
        {
            get { return extensionNumber; }
            set
            {
                extensionNumber = value;
                NotifyPropertyChange(() => ExtensionNumber);
            }
        }
        private string areaCode = string.Empty;
        public string AreaCode
        {
            get { return areaCode; }
            set
            {
                areaCode = value;
                NotifyPropertyChange(() => AreaCode);
            }
        }

        private string fullOfficePhone = string.Empty;
        public string FullOfficePhone
        {
            get
            {
                fullOfficePhone = string.Empty;
                if (areaCode != null && areaCode != string.Empty)
                {
                    fullOfficePhone += areaCode;
                }
                if (officePhone != null && officePhone != string.Empty)
                {
                    fullOfficePhone += "-" + officePhone;
                }
                if (extensionNumber != null && extensionNumber != string.Empty)
                {
                    fullOfficePhone += "-" + extensionNumber;
                }
                if (string.IsNullOrEmpty(fullOfficePhone))
                    fullOfficePhone = "未填写";
                return fullOfficePhone;
            }
            set
            {
                fullOfficePhone = value;
                NotifyPropertyChange(() => FullOfficePhone);
            }
        }

        #endregion


        #region "公司信息"

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
        private List<HttpIndustresCodes> industryList = new List<HttpIndustresCodes>();
        public List<HttpIndustresCodes> IndustryList
        {
            get { return industryList; }
            set
            {
                industryList = value;
                NotifyPropertyChange(() => IndustryList);
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

        #endregion

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

        private bool isPersonValidation = true;
        public bool IsPersoonValidation
        {
            get
            {
                return isPersonValidation;
            }
            set
            {
                isPersonValidation = value;
                NotifyPropertyChange(() => IsPersoonValidation);
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

        public string CompanyLatLng { get; set; }


        string _err;
        public string Error
        {
            get { return _err; }
            set
            {
                _err = value;
                NotifyPropertyChange(() => Error);
            }
        }

       
        
        public string this[string columnName]
        {
            get
            {
                string err = "";
                switch (columnName)
                {
                    case "BriefName":
                        if (this.briefName != null)
                        {
                            this.briefName = this.briefName.Trim();
                            if(this.BriefName.Length > 20)
                            {
                                err = "公司简称不能超过20字";
                                isCompanyValidation = false;
                                break;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(this.BriefName))
                        {
                            err = "请填写公司简称";
                            isCompanyValidation = false;
                            break;
                        }
                        if(!CommonValidationMethod.CompanyShortNameValidate(this.BriefName))
                        {
                            err = "请勿输入特殊字符";
                            isCompanyValidation = false;
                            break;
                        }
                        break;
                    case "Location":

                        if (string.IsNullOrWhiteSpace(this.Location))
                        {
                            err = "请填写公司地址";
                            isCompanyValidation = false;
                            break;
                        }
                        if(this.Location.Length > 50)
                        {
                            err = "公司地址不能超过50个字";
                            isCompanyValidation = false;
                            break;
                        }
                        break;
                        
                    case "Industries":
                        if (string.IsNullOrEmpty(this.Industries))
                        {
                            err = "该项不能为空";
                            isCompanyValidation = false;
                            break;
                        }
             
                        break;
                    case "Nature":
                        if(this.Nature == null)
                        {
                            err = "该项不能为空";
                            isCompanyValidation = false;
                            break;
                        }
                        break;
                    case "Size":
                        if(this.Size == null)
                        {
                            err = "该项不能为空";
                            isCompanyValidation = false;

                        }
                        break;
                    //case "City":
                    //    if(string.IsNullOrEmpty(this.City))
                    //    {
                    //        err = "该项不能为空";
                    //        isCompanyValidation = false;

                    //    }
                    //    break;
                    //case "FinancingState":
                    //    if(string.IsNullOrEmpty(this.City))
                    //    {
                    //        err = "该项不能为空";
                    //        isCompanyValidation = false;

                    //    }
                    //    break;
                    case "Tags":
                        if(Tags == null)
                        {
                            err = "该项不能为空";
                            isCompanyValidation = false;

                        }
                        else
                        {
                            if (this.Tags.Count == 0)
                            {
                                err = "该项不能为空";
                                isCompanyValidation = false;
                            }
                        }
                        break;
                    case "AddTag":
                        if(AddTag != null)
                        {
                            string temp;
                            temp = AddTag.Replace(" ", "");
                            foreach (var item in this.tags)
                            {
                                if (temp == item)
                                {
                                    err = "已存在该标签";
                                    break;
                                }
                            }
                            if(temp.Length > 10)
                            {
                                err = "已超过10个字符";

                            }
                        }
                        break;
                    case "WebSite":
                        if(webSite != null)
                        {
                            //webSite = webSite.Replace('。', '.');
                            if (webSite.Length > 50)
                            {
                                err = "字数不能超过50";
                                isCompanyValidation = false;
                                break;
                            }
                            if(webSite.Length == 0)
                            {
                                break;
                            }
                            if(!CommonValidationMethod.IsUrl(WebSite))
                            {
                                err = "请正确填写您的邮箱";
                                isCompanyValidation = false;
                                break;
                            }
                        }
                        break;
                    case "RealName":
                        if (!string.IsNullOrEmpty(this.RealName))
                        {
                            string temp12 = this.RealName.Replace(" ", "");
                            if (string.IsNullOrEmpty(temp12))
                            {
                                err = "请填写姓名";
                                isPersonValidation = false;
                                break;
                            }
                            if (!CommonValidationMethod.UserNameValidate(temp12))
                            {
                                err = "请勿输入特殊字符";
                                isPersonValidation = false;
                                break;
                            }
                            if (temp12.Length > 10)
                            {
                                if (MagicGlobal.isHRAuth)
                                {
                                    break;
                                }
                                err = "真实姓名不能超过10个字";
                                isPersonValidation = false;
                                break;

                            }
                        }
                        else
                        {
                            err = "请填写姓名";
                            isPersonValidation = false;
                            break;
                        }
                        
                        
                        break;
                    case "UserPosition":
                        if (!string.IsNullOrEmpty(this.UserPosition))
                        {
                            string temp13 = this.UserPosition.Replace(" ", "");
                            if (temp13.Length > 10 || string.IsNullOrEmpty(temp13))
                            {
                                err = "请填写职位";
                                isPersonValidation = false;
                                break;
                            }
                            if (!CommonValidationMethod.UserPositionValidate(temp13))
                            {
                                err = "请勿输入特殊字符";
                                isPersonValidation = false;
                                break;
                            }
                        }
                        else
                        {
                            err = "请填写职位";
                            isPersonValidation = false;
                            break;
                        }
                        
                        break;
                    case "AreaCode":
                        if(!string.IsNullOrEmpty(this.AreaCode))
                        {
                            if(this.AreaCode.Length > 5)
                            {
                                err = "数字个数超过5";
                                isPersonValidation = false;
                                break;
                            }
                            
                        }
                        break;
                    case "OfficePhone":
                        if(!string.IsNullOrEmpty(this.OfficePhone))
                        {
                            if(this.OfficePhone.Length > 8)
                            {
                                err = "数字个数超过8";
                                isPersonValidation = false;
                                break;
                            }
                        }
                        break;
                    case "ExtensionNumber":
                        if(!string.IsNullOrEmpty(this.ExtensionNumber))
                        {
                            if(this.ExtensionNumber.Length > 5)
                            {
                                err = "数字个数超过5";
                                isPersonValidation = false;
                                break;
                            }
                        }
                        break;
                    case "Email":
                        if(this.Email == null)
                        {
                            err = "请填写邮箱";
                            isPersonValidation = false;
                            break;
                        }
                        string temp33 = this.Email.Replace(" ", "");
                        if (string.IsNullOrEmpty(temp33))
                        {
                            err = "请填写邮箱";
                            isPersonValidation = false;
                            break;
                        }
                        else
                        {
                            if (!CommonValidationMethod.IsEmail(this.Email))
                            {
                                err = "请正确填写您的邮箱";
                                isPersonValidation = false;
                                break;
                            }
                        }
                        
                        break;



                }
                if(err != string.Empty)
                    _err = err;
                return err;
            }
        }

        private bool _validStatus;
        public bool validStatus
        {
            get { return _validStatus; }
            set
            {
                _validStatus = value;
                NotifyPropertyChange(() => validStatus);
            }
        }
       
    }

    public class tagsCompany
    {
        public string Tag { get; set; }
        public bool IsChecked { get; set; }
    }
    public class tagsModel: NotifyBaseModel
    {
        public string Tag { get; set; }

        private bool _IsChecked = false;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                NotifyPropertyChange(() => IsChecked);
            }
        }
        public Visibility IsSelf { get; set; }
    }
}
