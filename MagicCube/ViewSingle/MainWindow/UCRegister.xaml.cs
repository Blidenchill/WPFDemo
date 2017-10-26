using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MagicCube.ViewModel;
using MagicCube.Model;
using MagicCube.HttpModel;
using System.Collections.ObjectModel;
using MagicCube.Common;
using MagicCube.TemplateUC;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCRegister.xaml 的交互逻辑
    /// </summary>
    public partial class UCRegister : UserControl
    {
        public UCRegister()
        {
            InitializeComponent();
            this.ucImageUpload.OKAction += ImageUploadOKCallback;
            this.ucImageUpload.CancelAction += ImageUploadCancelCallback;
            ucCompanyLogoUpload.OKAction += CompanyLogoUploadOKCallback;
            ucCompanyLogoUpload.CancelAction += CompanyLogoUploadCancelCallback;
            this.gdMain.DataContext = this.RegisterViewModel;
            ucProvinceCity.OKOrCancelActionDelegate += ProvinceCityCallback;
            ucAuthen.UploadAuthOKAction += UploadAuthOKCallback;
            ucAuthen.GotoJobAction += GotoJobCallback;
            ucHtmlEditor.setText("");
            ucHtmlEditor.actionText += HtmlEditorTextChangedCallback;
            ucAuthen.GoToJobIsVisible = Visibility.Visible;
            this.Loaded += UCRegister_Loaded;
        }

        private void UCRegister_Loaded(object sender, RoutedEventArgs e)
        {
            ucHtmlEditor.setText("");
        }

        #region "变量"
        public RegisterModel RegisterViewModel = new RegisterModel();
        private MagicCube.ViewModel.HttpCompanyInfoModel httpCompany;

        List<HttpIndustresItem> selectIndustry = new List<HttpIndustresItem>();
        ObservableCollection<HttpIndustresItem> industriesList = new ObservableCollection<HttpIndustresItem>();

        ObservableCollection<tagsCompany> tag1List = new ObservableCollection<tagsCompany>();
        ObservableCollection<tagsCompany> tag2List = new ObservableCollection<tagsCompany>();
        ObservableCollection<tagsCompany> tag3List = new ObservableCollection<tagsCompany>();
        List<HttpCompanyCodes> NatureList = new List<HttpCompanyCodes>();
        List<HttpCompanyCodes> SizeList = new List<HttpCompanyCodes>();
        List<HttpCompanyCodes> StageList = new List<HttpCompanyCodes>();
        List<HttpProvinceCityCodes> provinceList = new List<HttpProvinceCityCodes>();

        private ObservableCollection<string> autoAssociateList = new ObservableCollection<string>();

        string picturePath = string.Empty;


        public Action CloseRegistAction;
        public Action GotJobPublishAction;

        private string CurrentcompnayID = string.Empty;
        #endregion



        #region "回调函数"
        private async void ImageUploadOKCallback(BitmapSource bs)
        {
            this.busyCtrl.IsBusy = true;
            this.gdEditPerson.Visibility = Visibility.Visible;
            this.ucImageUpload.Visibility = Visibility.Collapsed;
            MemoryStream stream = MagicCube.Common.ImageProcessor.SaveImageToStream(bs);
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "appKey", "fileName" }, new string[] { "head", "head" });

            string resultStr = await DAL.HttpHelper.Instance.HttpUploadFile(
                string.Format(DAL.ConfUtil.AddrUploadFile, MagicGlobal.UserInfo.Version, std), stream, "head");
            BaseHttpModel<HttpUploadFileModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpUploadFileModel>>(resultStr);
            this.busyCtrl.IsBusy = false;
            if (model == null)
            {
                MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                winLink.Owner = Window.GetWindow(this);
                winLink.ShowDialog();
                return;
            }
            else
            {
                if (model.code == 200)
                {
                    RegisterViewModel.AvatarUrl = model.data.url;
                    return;
                }
                else
                {

                    MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    winLink.Owner = Window.GetWindow(this);
                    winLink.ShowDialog();
                    return;
                }

            }
        }
        private void ImageUploadCancelCallback()
        {
            this.gdEditPerson.Visibility = Visibility.Visible;
            this.ucImageUpload.Visibility = Visibility.Collapsed;
        }

        private async Task ProvinceCityCallback(bool flag)
        {
            this.chkProvineCity.IsChecked = false;
            if (flag)
            {
                if (ucProvinceCity.OutSelectCityList.Count > 0)
                {
                    this.RegisterViewModel.City = ucProvinceCity.OutSelectCityList[0];
                    this.RegisterViewModel.Province = ucProvinceCity.selectedProvince;
                    this.RegisterViewModel.FullCity = this.RegisterViewModel.Province.name + "-" + this.RegisterViewModel.City.name;
                }
                  
            }
            await this.ucGmap.SetAddress(this.RegisterViewModel.City.name+ this.RegisterViewModel.Location, this.RegisterViewModel.City.name);
        }

        private void HtmlEditorFocusedCallback(bool flag)
        {
            if (flag)
            {
                this.bdHtmlEditor.BorderBrush = new SolidColorBrush(Colors.Transparent);
                this.tbErrorIntro.Visibility = Visibility.Collapsed;
            }
        }

        private async void CompanyLogoUploadOKCallback(BitmapSource bs)
        {
            this.busyCtrl.IsBusy = true;
            this.ucCompanyLogoUpload.Visibility = Visibility.Collapsed;
            this.gdEditCompany.Visibility = Visibility.Visible;
            MemoryStream stream = ImageProcessor.SaveImageToStream(bs);

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "appKey", "fileName" }, new string[] { "head", "head" });
            string resultStr = await DAL.HttpHelper.Instance.HttpUploadFile(string.Format(DAL.ConfUtil.AddrUploadFile, MagicGlobal.UserInfo.Version, std), stream, "head");
            this.busyCtrl.IsBusy = false;
            BaseHttpModel<HttpUploadFileModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpUploadFileModel>>(resultStr);
            if (model == null)
            {
                MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                winLink.Owner = Window.GetWindow(this);
                winLink.ShowDialog();
                return;
            }
            else
            {
                if (model.code == 200)
                {
                    RegisterViewModel.SmallCompanyLogoUrl = model.data.url;
                }
                else
                {
                    MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    winLink.Owner = Window.GetWindow(this);
                    winLink.ShowDialog();
                    return;
                }
            }

           

        }
        private void CompanyLogoUploadCancelCallback()
        {
            this.ucCompanyLogoUpload.Visibility = Visibility.Collapsed;
            this.gdEditCompany.Visibility = Visibility.Visible;
        }

        private void UploadAuthOKCallback()
        {
            this.gdUploadAuthOK.Visibility = Visibility.Visible;
        }
        private void GotoJobCallback()
        {
            if (this.GotJobPublishAction != null)
                this.GotJobPublishAction();
        }

        private void HtmlEditorTextChangedCallback(bool flag)
        {
            if(flag)
            {
                this.tbWaterStr.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.tbWaterStr.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region "对内函数"

        private void InitialComanyInfo()
        {
            if (this.httpCompany == null)
                return;
            RegisterViewModel.CompanyName = httpCompany.companyName;
            RegisterViewModel.BriefName = httpCompany.companyShortName;
            if (!string.IsNullOrEmpty(httpCompany.companyIndustry))
            {
                this.RegisterViewModel.Industries = string.Empty;
                string[] tempList = httpCompany.companyIndustry.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in tempList)
                {
                    foreach (var jsonIndu in industriesList)
                    {
                        if (jsonIndu.code == item)
                        {
                            RegisterViewModel.Industries += jsonIndu.name + "、";
                        }
                    }
                }
                RegisterViewModel.Industries = RegisterViewModel.Industries.TrimEnd(new char[] { '、' });
            }

            foreach (var jsonitem in NatureList)
            {
                if (jsonitem.code == httpCompany.companyCharact)
                {
                    RegisterViewModel.Nature = jsonitem;
                    RegisterViewModel.NatureStr = jsonitem.name;
                }
            }
            foreach (var jsonItem in SizeList)
            {
                if (jsonItem.code == httpCompany.companyScale)
                {
                    RegisterViewModel.Size = jsonItem;
                    RegisterViewModel.SizeStr = jsonItem.name;
                }
            }
            foreach (var jsonStage in StageList)
            {
                if (jsonStage.code == httpCompany.companyStage)
                {
                    RegisterViewModel.FinancingState = jsonStage;
                    RegisterViewModel.FinacingStateStr = jsonStage.name;
                }
            }

            RegisterViewModel.SmallCompanyLogoUrl = httpCompany.companyLogo;
            RegisterViewModel.Location = httpCompany.companyAddress;
            RegisterViewModel.WebSite = httpCompany.companyWebsite;
            RegisterViewModel.CompanyLatLng = httpCompany.companyLocation;

            //加载省市区
            if (!string.IsNullOrEmpty(httpCompany.companyDistrict))
            {
                if (httpCompany.companyDistrict.Length == 8)
                {
                    foreach (var item in provinceList)
                    {
                        if (item.code == httpCompany.companyDistrict.Substring(0, 6))
                        {
                            RegisterViewModel.Province = item;
                            foreach (var item2 in item.city)
                            {
                                if (item2.code == httpCompany.companyDistrict)
                                {
                                    RegisterViewModel.City = item2;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in provinceList)
                    {
                        if (item.code == httpCompany.companyDistrict.Substring(0, 4))
                        {
                            RegisterViewModel.Province = item;
                            foreach (var item2 in item.city)
                            {
                                if (item2.code == httpCompany.companyDistrict)
                                {
                                    RegisterViewModel.City = item2;
                                }
                            }
                        }
                    }
                }

            }
            else if (!string.IsNullOrEmpty(httpCompany.companyCity))
            {
                foreach (var item in provinceList)
                {
                    if (item.code == httpCompany.companyCity.Substring(0, 4))
                    {
                        RegisterViewModel.Province = item;
                        foreach (var item2 in item.city)
                        {
                            if (item2.code == httpCompany.companyCity)
                            {
                                RegisterViewModel.City = item2;
                            }
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(httpCompany.companyProvince))
            {
                foreach (var item in provinceList)
                {
                    if (item.code == httpCompany.companyProvince)
                    {
                        this.RegisterViewModel.Province = item;
                        this.RegisterViewModel.City = item.city[0];
                    }
                }
            }
            if (this.RegisterViewModel.Province != null)
                this.RegisterViewModel.FullCity = this.RegisterViewModel.Province.name;
            if (this.RegisterViewModel.City != null)
                this.RegisterViewModel.FullCity += "-" + this.RegisterViewModel.City.name;

            //地图位置加载

            string city = string.Empty;
            if (RegisterViewModel.Province != null)
            {
                if (!string.IsNullOrWhiteSpace(RegisterViewModel.Province.name))
                {
                    city = RegisterViewModel.Province.name;
                }
            }
            if (RegisterViewModel.City != null)
            {
                if (!string.IsNullOrWhiteSpace(RegisterViewModel.City.name))
                {
                    city = RegisterViewModel.City.name;
                }
            }
            if (!string.IsNullOrWhiteSpace(this.RegisterViewModel.CompanyLatLng))
            {
                string[] tempList = this.RegisterViewModel.CompanyLatLng.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempList.Length == 2)
                {
                    double lng = Convert.ToDouble(tempList[0]);
                    double lat = Convert.ToDouble(tempList[1]);
                    this.ucGmap.InitialUCGamp(this.RegisterViewModel.Location, city, lat, lng);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(this.RegisterViewModel.Location))
                    {
                        this.ucGmap.InitialUCGamp(this.RegisterViewModel.Location, city);
                    }
                    else
                    {
                        this.ucGmap.InitialUCGamp(city, city);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.RegisterViewModel.Location))
                {
                    this.ucGmap.InitialUCGamp(this.RegisterViewModel.Location, city);
                }
                else
                {
                    this.ucGmap.InitialUCGamp(city, city);
                }
            }

            this.ucUnEditGmap.InitialUCGamp(this.RegisterViewModel.Location, city);
            //标签初始化
            RegisterViewModel.Tags.Clear();
            if (httpCompany.companyBenefit != null)
            {
                string[] tempList = httpCompany.companyBenefit.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in tempList)
                {
                    RegisterViewModel.Tags.Add(item);
                }

            }
            RegisterViewModel.ImageUrls.Clear();
            RegisterViewModel.ImgUrlCount = 0;
            if (httpCompany.companyImageUrls != null)
            {
                string[] tempList = httpCompany.companyImageUrls.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in tempList)
                {
                    string temp = item.Trim();
                    if (temp.StartsWith("["))
                        continue;
                    if ((!string.IsNullOrWhiteSpace(temp)) && temp != "null")
                        RegisterViewModel.ImageUrls.Add(temp.ToString());
                }

                //if (this.RegisterViewModel.ImageUrls.Count != 0)
                //{
                //    this.listImageList.SelectedIndex = 0;
                //}
                if (RegisterViewModel.ImageUrls.Count >= 5)
                {
                    btnAddCompanyPictureShow.Visibility = Visibility.Collapsed;
                }
                RegisterViewModel.ImgUrlCount = RegisterViewModel.ImageUrls.Count;
                //this.listImageList.ItemsSource = RegisterViewModel.ImageUrls;
            }
            //ucCompanyImageBntChoose.Tag = ComanyImageRightBtnVisble();
            if (RegisterViewModel.ImageUrls.Count == 0)
                stkUnEditImage.Visibility = Visibility.Collapsed;
            else
                stkUnEditImage.Visibility = Visibility.Visible;


            RegisterViewModel.Intro = httpCompany.companyIntro;
            if (this.ucHtmlEditor != null)
                this.ucHtmlEditor.setText(RegisterViewModel.Intro);
            if (this.ucUnEdtiIntro != null)
                this.ucUnEdtiIntro.setText(RegisterViewModel.Intro);
            this.gdMain.DataContext = null;
            this.gdMain.DataContext = this.RegisterViewModel;

        }
        private void UpdateTagsListShow(string tag, bool isCheck)
        {
            int index = 0;
            foreach (var item in tag1List)
            {
                if (item.Tag == tag)
                {
                    tag1List.Remove(item);
                    tag1List.Insert(index, new tagsCompany() { Tag = item.Tag, IsChecked = isCheck });
                    return;
                }
                index++;
            }
            index = 0;
            foreach (var item in tag2List)
            {
                if (item.Tag == tag)
                {
                    tag2List.Remove(item);
                    tag2List.Insert(index, new tagsCompany() { Tag = item.Tag, IsChecked = isCheck });
                    return;
                }
                index++;
            }
            index = 0;
            foreach (var item in tag3List)
            {
                if (item.Tag == tag)
                {
                    tag3List.Remove(item);
                    tag3List.Insert(index, new tagsCompany() { Tag = item.Tag, IsChecked = isCheck });
                    return;
                }
                index++;
            }
        }

        public async Task InitailEditCompanyInfo()
        {


            //this.gdCompany.Visibility = Visibility.Visible;
            //this.gdEditCompany.Visibility = Visibility.Collapsed;
            //this.ucCompanyLogoUpload.Visibility = Visibility.Collapsed;

            //if (IsInitialCompany)
            //{
            //    await this.LoadingConpanyInfo();
            //    return;
            //}

            //IsInitialCompany = true;
            //数值化列表
            string industriesResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "companyInfoDict.json");
            jsonCompanyInfoDictModel jsonCompanyModel = DAL.JsonHelper.ToObject<jsonCompanyInfoDictModel>(industriesResult);
            if (jsonCompanyModel != null)
            {
                foreach (var item in jsonCompanyModel.DataDic.element)
                {
                    if (item.name == "industry")
                    {
                        foreach (var industryItem in item.mappingItem)
                        {
                            industriesList.Add(new HttpIndustresItem() { name = industryItem.fullName, code = industryItem.value, isChoose = false });
                        }
                    }
                    else if (item.name == "charact")
                    {
                        foreach (var charactItem in item.mappingItem)
                        {
                            this.NatureList.Add(new HttpCompanyCodes() { name = charactItem.fullName, code = charactItem.value });
                        }
                    }
                    else if (item.name == "scale")
                    {
                        foreach (var scaleItem in item.mappingItem)
                        {
                            this.SizeList.Add(new HttpCompanyCodes() { name = scaleItem.fullName, code = scaleItem.value });
                        }
                    }
                    else if (item.name == "stage")
                    {
                        foreach (var stageItem in item.mappingItem)
                        {
                            this.StageList.Add(new HttpCompanyCodes() { name = stageItem.fullName, code = stageItem.value });
                        }
                    }
                }
            }

            //加载城市二级列表
            string cityCodeStr = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "CityCode.json");
            JsonCityCodeMode jsonCityModel = DAL.JsonHelper.ToObject<JsonCityCodeMode>(cityCodeStr);
            List<CityCodeItem> ProvinceJsonList = new List<CityCodeItem>();
            List<CityCodeItem> CityJsonList = new List<CityCodeItem>();

            foreach (var item in jsonCityModel.CityCodes.CityCode)
            {
                if (item.value.Length == 4)
                {
                    ProvinceJsonList.Add(item);
                }
                if (item.value.Length == 6)
                {
                    CityJsonList.Add(item);
                }
                //杭州
                if (item.value.StartsWith("863301"))
                {
                    if (item.value.Length == 6)
                    {
                        ProvinceJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
                //武汉
                if (item.value.StartsWith("864201"))
                {
                    if (item.value.Length == 6)
                    {
                        ProvinceJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
                //成都
                if (item.value.StartsWith("865101"))
                {
                    if (item.value.Length == 6)
                    {
                        ProvinceJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
                //广州
                if (item.value.StartsWith("864401"))
                {
                    if (item.value.Length == 6)
                    {
                        ProvinceJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
                //深圳
                if (item.value.StartsWith("864403"))
                {
                    if (item.value.Length == 6)
                    {
                        ProvinceJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
            }

            foreach (var item in ProvinceJsonList)
            {

                HttpProvinceCityCodes proCode = new HttpProvinceCityCodes();
                proCode.name = item.simpleName;
                proCode.code = item.value;
                proCode.city = new List<HttpCityCodes>();
                //香港
                if (item.value == "8681")
                {
                    proCode.city.Add(new HttpCityCodes() { name = item.simpleName, code = item.value });
                }
                //澳门
                else if (item.value == "8682")
                {
                    proCode.city.Add(new HttpCityCodes() { name = item.simpleName, code = item.value });
                }
                //台湾
                else if (item.value == "8671")
                {
                    proCode.city.Add(new HttpCityCodes() { name = item.simpleName, code = item.value });
                }
                else
                {
                    foreach (var item2 in CityJsonList)
                    {
                        if (item2.value.StartsWith(item.value) && item2.value != item.value)
                            proCode.city.Add(new HttpCityCodes() { name = item2.simpleName, code = item2.value });
                    }
                }
                this.provinceList.Add(proCode);
            }
            //测试
            //this.ucGmap.InitialUCGamp("北京市海淀区银网中心", "北京");
            await this.LoadingConpanyInfo();
        }

        private async Task LoadingConpanyInfo()
        {
            ////加载公司信息
            //string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpCompanyInfoModel>();
            //string jsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "companyID", "properties" },
            //    new string[] { MagicGlobal.UserInfo.Id.ToString(), MagicGlobal.UserInfo.CompanyId.ToString(), propertys });
            //string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrGetCompanyInfo, MagicGlobal.UserInfo.Version, jsonStr));
            //BaseHttpModel<HttpCompanyInfoModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpCompanyInfoModel>>(resultStr);
            //if (model == null)
            //{
            //    return;
            //}
            //if (model.code == 200)
            //{
            //    this.httpCompany = model.data;
               
            //}

            //this.InitialComanyInfo();
            await this.InitialTagsList();
            foreach (var item in RegisterViewModel.Tags)
            {
                UpdateTagsListShow(item, true);
            }
            this.itemsTag1.ItemsSource = this.tag1List;
            this.itemsTag2.ItemsSource = this.tag2List;
            this.itemsTag3.ItemsSource = this.tag3List;
            //this.gdMain.DataContext = RegisterViewModel;

            //测试新空间行业下拉框
            this.selectIndustry.Clear();
            string[] industryTemp = RegisterViewModel.Industries.Split(new char[] { '、' });
            if (industryTemp.Length != 0)
            {
                for (int i = 0; i < industryTemp.Length; i++)
                {
                    foreach (var item in industriesList)
                    {
                        if (item.name == industryTemp[i])
                        {
                            item.isChoose = true;
                            this.selectIndustry.Add(item);
                            break;
                        }
                    }
                }
            }


            itemsIndustries.ItemsSource = industriesList;


            cmbNature.ItemsSource = NatureList;
            cmbSize.ItemsSource = SizeList;
            //cmbCity.ItemsSource = cityList;

            //cmbProvince.ItemsSource = provinceList;
            cmbFinancingState.ItemsSource = StageList;
            ucHtmlEditor.actionFocus += HtmlEditorFocusedCallback;
            this.ucGmap.InitialUCGamp();



        }

        private async Task InitialTagsList()
        {
            this.tag1List.Clear();
            this.tag2List.Clear();
            this.tag3List.Clear();

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "ownerID", "group" }, new string[] { MagicGlobal.UserInfo.CompanyId.ToString(), "salaryWelfareCompany$$enviromentCompany$$moreWelfareCompany" });
            //string std2 = DAL.JsonHelper.JsonParamsToString(new string[] { "ownerID", "group" }, new string[] { MagicGlobal.UserInfo.CompanyId.ToString(), "enviromentCompany" });
            //string std3 = DAL.JsonHelper.JsonParamsToString(new string[] { "ownerID", "group" }, new string[] { MagicGlobal.UserInfo.CompanyId.ToString(), "moreWelfareCompany" });
            //string std = string.Format("[{0},{1},{2}]", std1, std2, std3);
            string url = string.Format(DAL.ConfUtil.AddrCompanyTagsList, MagicGlobal.UserInfo.Version, std);
            string result1 = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            ViewModel.BaseHttpModel<ViewModel.HttpCompanyTagsListModel> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpCompanyTagsListModel>>(result1);
            if (model == null)
            {
                return;
            }
            else
            {
                if (model.code == 200)
                {
                    foreach (var item in model.data.salaryWelfareCompany)
                    {
                        this.tag1List.Add(new tagsCompany() { Tag = item.text, IsChecked = false });
                    }
                    foreach (var item in model.data.enviromentCompany)
                    {
                        this.tag2List.Add(new tagsCompany() { Tag = item.text, IsChecked = false });
                    }
                    foreach (var item in model.data.moreWelfareCompany)
                    {
                        this.tag3List.Add(new tagsCompany() { Tag = item.text, IsChecked = false });
                    }
                }
            }
        }

        private bool CheckSelfInfo()
        {
            bool flag1 = true;
            bool flag2 = true;
            bool flag3 = true;
            bool flag4 = true;
            bool flag5 = true;
            if (string.IsNullOrEmpty(RegisterViewModel.AvatarUrl))
            {
                this.tbErrorAvatarUrl.Visibility = Visibility.Visible;
                flag1 = false;
            }
            if(string.IsNullOrEmpty(this.txtNameEdit.Text))
            {
                this.rectName.Stroke = new SolidColorBrush(Colors.Red);
                this.tbErrorName.Text = "请填写姓名";
                this.tbErrorName.Visibility = Visibility.Visible;
                flag2 = false;
            }
            else
            {
                if (!CommonValidationMethod.UserNameValidate(this.txtNameEdit.Text))
                {
                    this.rectName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorName.Visibility = Visibility.Visible;
                    this.tbErrorName.Text = "请勿输入数字或特殊字符";
                    flag2 = false;
                }
                if (this.txtNameEdit.Text.Length > 10)
                {
                    this.tbErrorName.Visibility = Visibility.Visible;
                    this.tbErrorName.Text = "真实姓名不能超过10个字";
                    this.rectName.Stroke = new SolidColorBrush(Colors.Red);
                    flag2 = false;
                }
            }
            if(string.IsNullOrEmpty(this.txtPosition.Text))
            {
                this.rectPosition.Stroke = new SolidColorBrush(Colors.Red);
                this.tbErrorPosition.Text = "请填写职位";
                this.tbErrorPosition.Visibility = Visibility.Visible;
                flag3 = false;
            }
            else
            {
                if (!CommonValidationMethod.UserPositionValidate(this.txtPosition.Text))
                {
                    this.rectPosition.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorPosition.Visibility = Visibility.Visible;
                    this.tbErrorPosition.Text = "请勿输入特殊字符";
                    flag3 = false;
                }
            
                if (this.txtPosition.Text.Length > 10)
                {
                    this.tbErrorPosition.Visibility = Visibility.Visible;
                    this.tbErrorPosition.Text = "职位不能超过10个字";
                    this.rectPosition.Stroke = new SolidColorBrush(Colors.Red);
                    flag3 = false;
                }

            }
            if(string.IsNullOrEmpty(this.txtEmail.Text))
            {
                this.rectEmail.Stroke = new SolidColorBrush(Colors.Red);
                this.tbErrorEmail.Visibility = Visibility.Visible;
                this.tbErrorEmail.Text = "请填写邮箱";
                flag4 = false;
            }
            else
            {
                if (!CommonValidationMethod.IsEmail(this.txtEmail.Text))
                {
                    this.rectEmail.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorEmail.Visibility = Visibility.Visible;
                    this.tbErrorEmail.Text = "请正确填写您的邮箱";
                    flag4 = false;
                }
            }
            if(string.IsNullOrEmpty(this.txtCompanyName.Text))
            {
                this.rectCompanyName.Stroke = new SolidColorBrush(Colors.Red);
                this.tbErrorCompanyName.Visibility = Visibility.Visible;
                this.tbErrorCompanyName.Text = "请填写公司全称";
                flag5 = false;
            }
            else
            {
                if(!CommonValidationMethod.CompanyNameValidate(this.txtCompanyName.Text))
                {
                    this.rectCompanyName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorCompanyName.Visibility = Visibility.Visible;
                    this.tbErrorCompanyName.Text = "请勿输入特殊字符";
                    flag5 = false;
                }

                
                if (this.txtCompanyName.Text.Length > 50)
                {
                    this.tbErrorCompanyName.Visibility = Visibility.Visible;
                    this.rectCompanyName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorCompanyName.Text = "公司全称不能超过50个字";
                    flag5 = false;
                }
            }
            if (flag1 && flag2 && flag3 && flag4 && flag5)
            {
                return true;
            }
            else
                return false;
        }

        private bool CheckCompanyInfo()
        {
            bool flag1 = true;
            bool flag2 = true;
            bool flag3 = true;
            bool flag4 = true;
            bool flag5 = true;
            bool flag6 = true;
            bool flag7 = true;
            bool flag8 = true;
            bool flag9 = true;
            bool flag10 = true;
            if (string.IsNullOrEmpty(RegisterViewModel.SmallCompanyLogoUrl))
            {
                this.tbErrorCompanyLogoUrl.Visibility = Visibility.Visible;
                flag1 = false;
            }
            if(string.IsNullOrEmpty(RegisterViewModel.BriefName))
            {
                this.tbErrorBriefName.Visibility = Visibility.Visible;
                this.rectBriefName.Stroke = new SolidColorBrush(Colors.Red);
                this.tbErrorBriefName.Text = "请填写公司简称";
                flag2 = false;
            }
            else
            {
                if (!CommonValidationMethod.CompanyNameValidate(RegisterViewModel.BriefName))
                {
                    this.tbErrorBriefName.Visibility = Visibility.Visible;
                    this.rectBriefName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorBriefName.Text = "请勿输入特殊字符";
                    flag2 = false;
                }
                if (RegisterViewModel.BriefName.Length > 20)
                {
                    this.tbErrorBriefName.Visibility = Visibility.Visible;
                    this.rectBriefName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorBriefName.Text = "公司简称不能超过20个字";
                    flag2 = false;
                }
            }
            if(string.IsNullOrEmpty(RegisterViewModel.Industries))
            {
                this.tbErrorIndustry.Visibility = Visibility.Visible;
                this.rectIndustry.Stroke = new SolidColorBrush(Colors.Red);
                flag3 = false;
            }
            if(string.IsNullOrEmpty(RegisterViewModel.Nature.code))
            {
                this.tbErrorNature.Visibility = Visibility.Visible;
                this.rectNatrue.Stroke = new SolidColorBrush(Colors.Red);
                flag4 = false;
            }
            if(string.IsNullOrEmpty(RegisterViewModel.Size.code))
            {
                this.tbErrorSize.Visibility = Visibility.Visible;
                this.rectSize.Stroke = new SolidColorBrush(Colors.Red);
                flag5 = false;
            }
            if(string.IsNullOrEmpty(RegisterViewModel.FullCity))
            {
                this.tbErrorCity.Visibility = Visibility.Visible;
                this.rectCity.Stroke = new SolidColorBrush(Colors.Red);
                flag6 = false;
            }
            if(string.IsNullOrEmpty(RegisterViewModel.Location))
            {
                this.tbErrorLocation.Visibility = Visibility.Visible;
                this.rectLocation.Stroke = new SolidColorBrush(Colors.Red);
                flag7 = false;
            }
            if(RegisterViewModel.Tags.Count == 0)
            {
                this.tbErrorTags.Visibility = Visibility.Visible;
                flag8 = false;
            }

            string info = ucHtmlEditor.getStringText();
            if (string.IsNullOrWhiteSpace(info))
            {
                bdHtmlEditor.BorderBrush = new SolidColorBrush(Colors.Red);
                tbErrorIntro.Visibility = Visibility.Visible;
                tbErrorIntro.Text = "请填写公司介绍";
                flag9 = false;
            }
            else if (info.Length > 1000)
            {
                bdHtmlEditor.BorderBrush = new SolidColorBrush(Colors.Red);
                tbErrorIntro.Visibility = Visibility.Visible;
                tbErrorIntro.Text = "公司介绍不能多于1000字";
                flag9 = false;
            }
            else if (info.Length < 20)
            {
                bdHtmlEditor.BorderBrush = new SolidColorBrush(Colors.Red);
                tbErrorIntro.Visibility = Visibility.Visible;
                tbErrorIntro.Text = "公司介绍不能少于20字";
                flag9 = false;
            }

            if (!string.IsNullOrEmpty(RegisterViewModel.WebSite))
            {
                if (!CommonValidationMethod.IsWebSite(RegisterViewModel.WebSite))
                {
                    flag10 = false;
                }
            }
            if (flag1 && flag2 && flag3 && flag4 && flag5 && flag6 && flag7 && flag8 && flag9 && flag10)
            {
                return true;
            }
            else
                return false;
        }
        #endregion



        private void btnAddHeadPicture_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.6.2.1", "clk");
            tbErrorAvatarUrl.Visibility = Visibility.Collapsed;
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "(*.jpg,*.png)|*.jpg;*.png";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ucImageUpload.GetImagePath(openFile.FileName);
                this.ucImageUpload.Visibility = Visibility.Visible;
            }
        }

        private void btnAddCompanyLogo_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.7.2.1", "clk");
            this.tbErrorCompanyLogoUrl.Visibility = Visibility.Collapsed;
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "(*.jpg,*.png)|*.jpg;*.png";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ucCompanyLogoUpload.GetImagePath(openFile.FileName);
                this.ucCompanyLogoUpload.Visibility = Visibility.Visible;
            }
        }

        private   void btnSelfInfoNext_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.6.7.1", "clk");
            TrackHelper2.TrackOperation("5.1.7.1.1", "pv");
            if (!CheckSelfInfo())
            {
                MagicCube.View.Message.DisappearShow show = new MagicCube.View.Message.DisappearShow("您有不正确填写项", 1);
                show.Owner = Window.GetWindow(this);
                show.ShowDialog();
                return;
            }
            this.gdConfirmCompanyName.Visibility = Visibility.Visible;
        }

     

        private void chkProvineCity_Click(object sender, RoutedEventArgs e)
        {
            this.ucProvinceCity.SetSelectedList(this.RegisterViewModel.City);
        }


        private void IndustryBorder_IsVisibaleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                //string industriesResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Industries.json");
                //industriesList = DAL.JsonHelper.ToObject<ObservableCollection<HttpIndustresItem>>(industriesResult);
                this.selectIndustry.Clear();
                string[] industryTemp = RegisterViewModel.Industries.Split(new char[] { '、' });
                if (industryTemp.Length != 0)
                {
                    for (int i = 0; i < industryTemp.Length; i++)
                    {
                        foreach (var item in industriesList)
                        {
                            if (item.name == industryTemp[i])
                            {
                                item.isChoose = true;
                                this.selectIndustry.Add(item);
                                break;
                            }
                        }
                    }
                }
                this.itemsIndustries.ItemsSource = null;
                this.itemsIndustries.ItemsSource = this.industriesList;
            }
        }

        private void DeleteTags_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            string temp = btn.DataContext as string;
            RegisterViewModel.Tags.Remove(temp);
            UpdateTagsListShow(temp, false);
        }

        private void DeleteCompanyImageShow_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.7.19.1", "clk");
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            string str = btn.DataContext as string;
            if (!string.IsNullOrEmpty(str))
            {
                this.RegisterViewModel.ImageUrls.Remove(str);
                RegisterViewModel.ImgUrlCount = RegisterViewModel.ImageUrls.Count;
                if (this.RegisterViewModel.ImageUrls.Count < 5)
                {
                    btnAddCompanyPictureShow.Visibility = Visibility.Visible;
                }
            }
        }

        private async void UploadCompanyImageShow_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.7.18.1", "clk");
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "(*.jpg,*.png)|*.jpg;*.png";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                picturePath = openFile.FileName;
                string tempPath = DAL.ConfUtil.LocalHomePath + "/" + openFile.SafeFileName;
                bool compressFlag = ImageCompress.CompressTo1M(picturePath, tempPath);
                if (compressFlag)
                {
                    picturePath = tempPath;
                }
                busyCtrl.IsBusy = true;



                using (FileStream fs = new FileStream(picturePath, FileMode.Open, FileAccess.Read))
                {
                    string std = DAL.JsonHelper.JsonParamsToString(new string[] { "appKey", "fileName" }, new string[] { "head", "head" });
                    string resultStr = await DAL.HttpHelper.Instance.HttpUploadFile(string.Format(DAL.ConfUtil.AddrUploadFile, MagicGlobal.UserInfo.Version, std), fs, "head");
                    busyCtrl.IsBusy = false;
                    BaseHttpModel<HttpUploadFileModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpUploadFileModel>>(resultStr);
                    if (model == null)
                    {
                        MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                        winLink.Owner = Window.GetWindow(this);
                        winLink.ShowDialog();
                        return;
                    }
                    else
                    {
                        if (model.code == 200)
                        {
                            RegisterViewModel.ImageUrls.Add(model.data.url);
                            RegisterViewModel.ImgUrlCount = RegisterViewModel.ImageUrls.Count;
                            //this.listImageList.SelectedIndex = infomationModel.ImageUrls.Count - 1;
                            //ucCompanyImageBntChoose.Tag = ComanyImageRightBtnVisble();
                            if (RegisterViewModel.ImageUrls.Count >= 5)
                            {
                                btnAddCompanyPictureShow.Visibility = Visibility.Collapsed;
                            }
                            return;
                        }
                        else
                        {

                            MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                            winLink.Owner = Window.GetWindow(this);
                            winLink.ShowDialog();
                            return;
                        }

                    }
                }
            }
        }

        private async void txtAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.RegisterViewModel.City == null)
            {
                //MessageBox.Show("请先选择城市!");
                return;
            }
            await ucGmap.SetAddress(this.txtCompanyAddress.Text, this.RegisterViewModel.City.name);
        }

        private void TagsCheck_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.7.14.1", "clk");
            this.tbErrorTags.Visibility = Visibility.Collapsed;
            System.Windows.Controls.CheckBox chk = sender as System.Windows.Controls.CheckBox;
            tagsCompany tag = chk.DataContext as tagsCompany;
            if (RegisterViewModel.Tags.Count >= 8)
            {
                chk.IsChecked = false;
                TemplateUC.WinMessageLink win = new TemplateUC.WinMessageLink("诱惑标签最多只能选择8个", "我知道了");
                win.Owner = Window.GetWindow(this);
                win.ShowDialog();
                return;
            }
            foreach (var item in RegisterViewModel.Tags)
            {
                if (item == tag.Tag)
                    return;
            }
            RegisterViewModel.Tags.Add(tag.Tag);
            if (RegisterViewModel.Tags.Count != 0)
                this.RegisterViewModel.TagsValidationBool = false;
        }

        private void chkIndustry_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if ((bool)chk.IsChecked)
            {
                if (this.selectIndustry.Count == 3)
                {
                    foreach (var item in industriesList)
                    {
                        if (item.name == this.selectIndustry[0].name)
                        {
                            item.isChoose = false;
                            this.itemsIndustries.ItemsSource = null;
                            this.itemsIndustries.ItemsSource = this.industriesList;
                            break;
                        }
                    }
                    foreach (var item in industriesList)
                    {
                        if (item.name == (chk.Content as TextBlock).Text)
                        {
                            this.selectIndustry.RemoveAt(0);
                            this.selectIndustry.Add(item);
                        }
                    }

                }
                else
                {
                    this.selectIndustry.Add(chk.DataContext as HttpIndustresItem);
                }
            }
            else
            {
                foreach (var item in this.selectIndustry)
                {
                    if (item.name == (chk.Content as TextBlock).Text)
                    {
                        this.selectIndustry.Remove(item);
                        break;
                    }
                }
            }

        }

        private void rbAuthen_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.8.1.1", "pv");
        }

        private void rbCompanyInfo_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.7.1.1", "pv");
        }

        private void rbSelfInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IndustrySelectOK_Click(object sender, RoutedEventArgs e)
        {
            string temp = string.Empty;
            foreach (var item in this.selectIndustry)
            {
                temp += item.name + "、";
            }
            //this.chkIndustryShow.Content = temp.TrimEnd(new char[] { '、' });
            RegisterViewModel.Industries = temp.TrimEnd(new char[] { '、' });
            this.chkIndustryShow.IsChecked = false;
        }

        private void IndustrySelectCancel_Click(object sender, RoutedEventArgs e)
        {
            this.chkIndustryShow.IsChecked = false;
            this.selectIndustry.Clear();
            foreach (var item in industriesList)
            {
                item.isChoose = false;
            }

            string[] industryTemp = RegisterViewModel.Industries.Split(new char[] { '、' });
            if (industryTemp.Length != 0)
            {
                for (int i = 0; i < industryTemp.Length; i++)
                {
                    foreach (var item in industriesList)
                    {
                        if (item.name == industryTemp[i])
                        {
                            item.isChoose = true;
                            this.selectIndustry.Add(item);
                            break;
                        }
                    }
                }
            }
        }

        private void ucGmap_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                this.ucGmap.GmapZoom(1);
            }
            else
            {
                this.ucGmap.GmapZoom(-1);
            }
            e.Handled = true;
        }

        private void BtnAddSelfTag_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.7.10.1", "clk");
            if (RegisterViewModel.Tags.Count >= 8)
            {
                TemplateUC.WinMessageLink win = new TemplateUC.WinMessageLink("诱惑标签最多只能选择8个","我知道了");
                win.Owner = Window.GetWindow(this);
                win.ShowDialog();
                return;
            }
            string temp = this.txtAddTag.Text;
            temp = temp.Trim();
            if (string.IsNullOrWhiteSpace(temp))
                return;
            temp = temp.Replace(" ", "");
            this.RegisterViewModel.Tags.Add(temp);
            this.txtAddTag.Clear();
            if (RegisterViewModel.Tags.Count != 0)
                this.RegisterViewModel.TagsValidationBool = false;
            this.chkAddTags.IsChecked = false;
        }

        private async void BtnCompanyEditNext_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.7.20.1", "clk");
            this.RegisterViewModel.Intro = this.ucHtmlEditor.getText();
            Point pt = this.ucGmap.GetMarkerLatLng();
            this.RegisterViewModel.CompanyLatLng = pt.Y.ToString() + "$$" + pt.X.ToString();
            bool flag = CheckCompanyInfo();
            if (!flag)
            {
                MagicCube.View.Message.DisappearShow show = new MagicCube.View.Message.DisappearShow("您有不正确填写项", 1);
                show.Owner = Window.GetWindow(this);
                show.ShowDialog();
                return;
            }


            if (!await SaveRegisterInfo())
            {
                return;
            }
            this.ucAuthen.SetPanel(9);
            TrackHelper2.TrackOperation("5.1.8.1.1", "pv");
            ucAuthen.CompanyName = RegisterViewModel.CompanyName;
            this.scrollView.Visibility = Visibility.Collapsed;
            this.rbAuthen.IsChecked = true;

        }

        private async void txtCompanyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            await TaskEx.Delay(200);
            if (!txt.Focusable)
                return;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                this.popupAuto.IsOpen = false;
                return;
            }
            

            else
            {
                //判断
                if(!CommonValidationMethod.CompanyNameValidate(txt.Text))
                {
                    this.tbErrorCompanyName.Visibility = Visibility.Visible;
                    this.rectCompanyName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorCompanyName.Text = "请勿输入特殊字符";
                }
                else
                {
                    this.tbErrorCompanyName.Visibility = Visibility.Collapsed;
                    this.rectCompanyName.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                }
                if(txt.Text.Length > 50)
                {
                    this.tbErrorCompanyName.Visibility = Visibility.Visible;
                    this.rectCompanyName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorCompanyName.Text = "公司全称不能超过50个字";
                }


                string std1 = ViewModel.ModelTools.SetHttpPropertys<ViewModel.HttpCompanyNameModel>();
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "companyName", "startRow", "pageSize", "properties" }, new string[] { txt.Text,"0","20", std1 });
                string resultStd = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetCompanyAutoAssociate, MagicGlobal.UserInfo.Version, std));
                ViewModel.BaseHttpModel<List<ViewModel.HttpCompanyNameModel>> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpCompanyNameModel>>>(resultStd);
                if (model == null)
                {
                    this.popupAuto.IsOpen = false;
                    return;

                }
                else
                {
                    if (model.code == 200)
                    {
                        this.autoAssociateList.Clear();
                        foreach (var item in model.data)
                        {
                            this.autoAssociateList.Add(item.companyName);
                        }
                        if (this.autoAssociateList.Count > 0)
                        {
                            this.popupAutoList.ItemsSource = this.autoAssociateList;
                            this.popupAuto.Tag = "companyName";
                            this.popupAuto.PlacementTarget = this.gdCompnayName;
                            //this.popupAuto.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                            this.popupAuto.IsOpen = true;
                        }
                        else
                        {
                            this.popupAuto.IsOpen = false;
                        }
                        return;
                    }
                }
            }
            this.popupAuto.IsOpen = false;
        }

        private void popupAutoList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch (this.popupAuto.Tag.ToString())
                {
                    case "companyName":
                        this.txtCompanyName.Focusable = false;
                        this.txtCompanyName.Text = this.popupAutoList.SelectedValue.ToString();
                        this.popupAuto.IsOpen = false;
                        break;

                }

            }
        }

        private void tb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.popupAuto.IsOpen = false;
            TextBlock tb = sender as TextBlock;
            if (tb == null)
                return;
            switch (this.popupAuto.Tag.ToString())
            {
               
                case "companyName":
                    if (this.txtCompanyName == null)
                        return;
                    this.txtCompanyName.Text = tb.DataContext as string;
                    this.txtCompanyName.Focusable = false;
                    this.rectCompanyName.Fill = new SolidColorBrush(Color.FromRgb(0xf5, 0xf5, 0xf5));
                    this.txtCompanyName.Width = 324;
                    this.txtCompanyName.Margin = new Thickness(0, 0, 0, 0);
                    this.txtCompanyName.Padding = new Thickness(10, 0, 0, 0);
                    this.txtCompanyName.Foreground = new SolidColorBrush(Color.FromRgb(0x99, 0x99, 0x99));
                    break; 
            }
        }

        private void btnClearCompanyName_Click(object sender, RoutedEventArgs e)
        {
            this.txtCompanyName.Clear();
            this.txtCompanyName.Focusable = true;
            this.rectCompanyName.Fill = new SolidColorBrush(Colors.Transparent);
            this.txtCompanyName.Margin = new Thickness(10, 0, 0, 0);
            this.txtCompanyName.Width = 314;
            this.txtCompanyName.Padding = new Thickness(0, 0, 0, 0);

            this.txtCompanyName.Foreground = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33));
        }

        private void txtCompanyName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.tbErrorCompanyName.Visibility != Visibility.Visible)
                this.rectCompanyName.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
            if (string.IsNullOrWhiteSpace(this.txtCompanyName.Text))
            {
                return;
            }
            this.txtCompanyName.Focusable = false;
            this.rectCompanyName.Fill = new SolidColorBrush(Color.FromRgb(0xf5, 0xf5, 0xf5));
            this.txtCompanyName.Width = 324;
            this.txtCompanyName.Margin = new Thickness(0, 0, 0, 0);
            this.txtCompanyName.Padding = new Thickness(10, 0, 0, 0);
            this.txtCompanyName.Foreground = new SolidColorBrush(Color.FromRgb(0x99, 0x99, 0x99));
     


        }

        private void GotFocus_Event(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            switch(fe.Name)
            {
                case "txtNameEdit":
                    this.rectName.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    this.tbErrorName.Visibility = Visibility.Collapsed;
                    TrackHelper2.TrackOperation("5.1.6.3.1", "clk");
                    break;
                case "txtPosition":
                    this.rectPosition.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    this.tbErrorPosition.Visibility = Visibility.Collapsed;
                    TrackHelper2.TrackOperation("5.1.6.4.1", "clk");
                    break;
                case "txtEmail":
                    this.rectEmail.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    this.tbErrorEmail.Visibility = Visibility.Collapsed;
                    TrackHelper2.TrackOperation("5.1.6.5.1", "clk");

                    break;
                case "txtCompanyName":
                    this.rectCompanyName.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    this.tbErrorCompanyName.Visibility = Visibility.Collapsed;
                    TrackHelper2.TrackOperation("5.1.6.6.1", "clk");
                    break;

                case "txtBriefName":
                    this.rectBriefName.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    this.tbErrorBriefName.Visibility = Visibility.Collapsed;
                    TrackHelper2.TrackOperation("5.1.7.3.1", "clk");
                    break;
                case "chkIndustryShow":
                    this.rectIndustry.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                    this.tbErrorIndustry.Visibility = Visibility.Collapsed;
                    TrackHelper2.TrackOperation("5.1.7.4.1", "clk");
                    break;
                case "cmbNature":
                    this.rectNatrue.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                    this.tbErrorNature.Visibility = Visibility.Collapsed;
          
                    break;
                case "cmbSize":
                    this.rectSize.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                    this.tbErrorSize.Visibility = Visibility.Collapsed;
                   
                    break;
                case "chkProvineCity":
                    this.rectCity.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                    this.tbErrorCity.Visibility = Visibility.Collapsed;
                    TrackHelper2.TrackOperation("5.1.7.7.1", "clk");
                    break;
                case "txtCompanyAddress":
                    this.rectLocation.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    this.tbErrorLocation.Visibility = Visibility.Collapsed;
                    TrackHelper2.TrackOperation("5.1.7.8.1", "clk");
                    break;
                case "chkAddTags":
                    this.tbErrorTags.Visibility = Visibility.Collapsed;
                    TrackHelper2.TrackOperation("5.1.7.9.1", "clk");
                    break;
                case "ucHtmlEditor":
                    bdHtmlEditor.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                    tbErrorIntro.Visibility = Visibility.Collapsed;
                    this.tbErrorIntro.Visibility = Visibility.Collapsed;
                    TrackHelper2.TrackOperation("5.1.7.15.1", "clk");
                    break;
                case "cmbFinancingState":
                   
                    break;
                case "txtWebSite":
                    TrackHelper2.TrackOperation("5.1.7.17.1", "clk");
                    this.rectWebSite.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    this.tbErrorWebSite.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private async void btnCompanyNameConfirm_Click(object sender, RoutedEventArgs e)
        {
            MagicGlobal.UserInfo.avatarUrl = RegisterViewModel.AvatarUrl;
            MagicGlobal.UserInfo.RealName = RegisterViewModel.UserName;
            MagicGlobal.UserInfo.UserPosition = RegisterViewModel.Position;
            MagicGlobal.UserInfo.Email = RegisterViewModel.Email;
            MagicGlobal.UserInfo.CompanyName = RegisterViewModel.CompanyName;
            this.busyCtrl.IsBusy = true;
            await InitailEditCompanyInfo();
            await CompanyNameExist();
            this.busyCtrl.IsBusy = false;
            this.rbCompanyInfo.IsChecked = true;
            
            this.gdConfirmCompanyName.Visibility = Visibility.Collapsed;
        }

        private void btnCompanyNameCancel_Click(object sender, RoutedEventArgs e)
        {
            this.gdConfirmCompanyName.Visibility = Visibility.Collapsed;
        }

        private async Task CompanyNameExist()
        {
            if(RegisterViewModel.ImageUrls.Count == 0)
            {
                stkUnEditImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                stkCompanyEdit.Visibility = Visibility.Visible;
            }
            string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpCompanyInfoModel>();
            string jsonStr = DAL.JsonHelper.JsonParamsToString(new string[] {"companyName", "properties" },
                new string[] { RegisterViewModel.CompanyName, propertys });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrCompanyNameExist, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<HttpCompanyInfoModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpCompanyInfoModel>>(resultStr);
            if (model == null)
            {
                return;
            }
            if (model.code == 200)
            {
                this.CurrentcompnayID = model.data.companyID;
            }
            if (string.IsNullOrEmpty(this.CurrentcompnayID))
            {
                this.scrollView.Visibility = Visibility.Visible;
                this.scrollViewUnEdit.Visibility = Visibility.Collapsed;
                return;
            }
               
            //获取已有公司的信息
            ////加载公司信息
            string jsonStr2 = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "companyID", "properties" },
                new string[] { MagicGlobal.UserInfo.Id.ToString(), this.CurrentcompnayID, propertys });
            string resultStr2 = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetCompanyInfo, MagicGlobal.UserInfo.Version, jsonStr2));
            BaseHttpModel<HttpCompanyInfoModel> model2 = DAL.JsonHelper.ToObject<BaseHttpModel<HttpCompanyInfoModel>>(resultStr2);
            if (model2 == null)
            {
                return;
            }
            if (model2.code == 200)
            {
                this.httpCompany = model2.data;
                this.CurrentcompnayID = model2.data.companyID;
                this.InitialComanyInfo();
                //this.stkCompanyEdit.IsEnabled = Convert.ToBoolean(model2.data.updateCompanyAuth);
                if(Convert.ToBoolean(model2.data.updateCompanyAuth))
                {
                    this.scrollViewUnEdit.Visibility = Visibility.Collapsed;
                    this.scrollView.Visibility = Visibility.Visible;
                }
                else
                {
                    this.scrollView.Visibility = Visibility.Collapsed;
                    this.scrollViewUnEdit.Visibility = Visibility.Visible;
                }
                
            }
        }


        private async Task<bool>  SaveRegisterInfo()
        {
            UpdateHttpCompany();
            httpCompany.name = this.RegisterViewModel.UserName;
            httpCompany.avatar = this.RegisterViewModel.AvatarUrl;
            httpCompany.hrEmail = this.RegisterViewModel.Email;
            httpCompany.position = this.RegisterViewModel.Position;
            httpCompany.userID = MagicGlobal.UserInfo.Id.ToString();
            if (!string.IsNullOrEmpty(this.CurrentcompnayID))
                httpCompany.companyID = this.CurrentcompnayID;


            string jsonStr = DAL.JsonHelper.ToJsonString(httpCompany);
            this.busyCtrl.IsBusy = true;
            string jsonResult = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrSaveUserInfo, MagicGlobal.UserInfo.Version), jsonStr);
            this.busyCtrl.IsBusy = false;
            MagicCube.ViewModel.BaseHttpModel modelResult = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel>(jsonResult);
            if (modelResult == null)
                return false;
            else
            {
                if (modelResult.code == 200)
                {
                    //MagicGlobal.UserInfo.CompanyId = Convert.ToInt32(httpCompany.companyID);
                    //获取companyID
                    string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpCompanyInfoModel>();
                    string jsonStr2 = DAL.JsonHelper.JsonParamsToString(new string[] { "companyName", "properties" },
                        new string[] { RegisterViewModel.CompanyName, propertys });
                    string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrCompanyNameExist, MagicGlobal.UserInfo.Version, jsonStr2));
                    BaseHttpModel<HttpCompanyInfoModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpCompanyInfoModel>>(resultStr);
                    if (model != null)
                    {
                        if (model.code == 200)
                        {
                            MagicGlobal.UserInfo.CompanyId= Convert.ToInt32(model.data.companyID);
                        }
                    }
                    MagicGlobal.UserInfo.CompanyName = httpCompany.companyName;
                    return true;
                }
                    
                else
                {
                    MagicCube.TemplateUC.WinMessageLink link = new WinMessageLink(modelResult.msg, "我知道了");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                    return false;
                }
            }
        }

        private void UpdateHttpCompany()
        {
            this.httpCompany = new HttpCompanyInfoModel();
            if (this.RegisterViewModel.Province != null)
                this.RegisterViewModel.FullCity = this.RegisterViewModel.Province.name;
            if (this.RegisterViewModel.City != null)
                this.RegisterViewModel.FullCity += "-" + this.RegisterViewModel.City.name;
            this.httpCompany.companyName = this.RegisterViewModel.CompanyName;
            this.httpCompany.companyWebsite = this.RegisterViewModel.WebSite;
            string industryTemp = string.Empty;
            foreach (var item in this.selectIndustry)
            {
                industryTemp += item.code + "$$";
            }
            this.httpCompany.companyIndustry = industryTemp.TrimEnd(new char[] { '$' });

            this.httpCompany.companyScale = this.RegisterViewModel.Size.code;
            string tagsTemp = string.Empty;
            foreach (var item in RegisterViewModel.Tags)
            {
                tagsTemp += item + "$$";
            }
            this.httpCompany.companyBenefit = tagsTemp.TrimEnd(new char[] { '$' });

            this.httpCompany.companyAddress = this.RegisterViewModel.Location;
            this.httpCompany.companyShortName = this.RegisterViewModel.BriefName;

            this.httpCompany.companyCharact = this.RegisterViewModel.Nature.code;
            this.httpCompany.companyLogo = this.RegisterViewModel.SmallCompanyLogoUrl;
            this.httpCompany.companyStage = this.RegisterViewModel.FinancingState.code;
            this.httpCompany.companyIntro = this.RegisterViewModel.Intro;
            string imageTemp = string.Empty;
            foreach (var item in this.RegisterViewModel.ImageUrls)
            {
                imageTemp += item + "$$";
            }
            this.httpCompany.companyImageUrls = imageTemp.TrimEnd(new char[] { '$' });

            //热门城市
            if (this.RegisterViewModel.Province.code.Length == 6)
            {
                this.httpCompany.companyProvince = this.RegisterViewModel.Province.code.Substring(0, 4);
                this.httpCompany.companyCity = this.RegisterViewModel.Province.code;
                if (this.RegisterViewModel.City != null)
                    this.httpCompany.companyDistrict = this.RegisterViewModel.City.code;
            }
            else
            {
                //直辖市
                if (this.RegisterViewModel.Province.code == "8611" || this.RegisterViewModel.Province.code == "8612" || this.RegisterViewModel.Province.code == "8631" || this.RegisterViewModel.Province.code == "8650")
                {
                    this.httpCompany.companyProvince = this.RegisterViewModel.Province.code.Substring(0, 4);
                    this.httpCompany.companyCity = this.RegisterViewModel.Province.code;
                    if (this.RegisterViewModel.City != null)
                        this.httpCompany.companyDistrict = this.RegisterViewModel.City.code;
                }
                else
                {
                    this.httpCompany.companyProvince = this.RegisterViewModel.Province.code;
                    this.httpCompany.companyDistrict = string.Empty;
                    //除去香港，澳门，台湾
                    if (this.RegisterViewModel.City.code == "8671" || this.RegisterViewModel.City.code == "8681" || this.RegisterViewModel.City.code == "8682")
                    {
                        this.httpCompany.companyCity = string.Empty;
                    }
                    else
                    {
                        if (this.RegisterViewModel.City != null)
                            this.httpCompany.companyCity = this.RegisterViewModel.City.code;

                    }
                }

            }
            Point pt = this.ucGmap.GetMarkerLatLng();
            this.httpCompany.companyLocation = pt.Y.ToString() + "$$" + pt.X.ToString();



        }

        private void btnCloseAuthenOKPanel_Click(object sender, RoutedEventArgs e)
        {
            this.gdUploadAuthOK.Visibility = Visibility.Collapsed;
            if (this.CloseRegistAction != null)
                this.CloseRegistAction();
        }

        private void txtBriefName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(this.txtBriefName.Text))
            {
                if(!CommonValidationMethod.CompanyNameValidate(this.txtBriefName.Text))
                {
                    this.tbErrorBriefName.Visibility = Visibility.Visible;
                    this.rectBriefName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorBriefName.Text = "请勿输入特殊字符";
                }
                else
                {
                    this.tbErrorBriefName.Visibility = Visibility.Collapsed;
                    this.rectBriefName.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    this.tbErrorBriefName.Text = "请填写公司简称";
                }

                if(this.txtBriefName.Text.Length > 20)
                {
                    this.tbErrorBriefName.Visibility = Visibility.Visible;
                    this.rectBriefName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorBriefName.Text = "公司简称不能超过20个字";
                }
            }
            else
            {
                this.tbErrorBriefName.Visibility = Visibility.Collapsed;
                this.rectBriefName.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                this.tbErrorBriefName.Text = "请填写公司简称";
            }


        }

        private void txtNameEdit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtNameEdit.Text))
            {
                if (!CommonValidationMethod.UserNameValidate(this.txtNameEdit.Text))
                {
                    this.tbErrorName.Visibility = Visibility.Visible;
                    this.tbErrorName.Text = "请勿输入数字或特殊字符";
                    this.rectName.Stroke = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    this.tbErrorName.Visibility = Visibility.Collapsed;
                    this.rectName.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                }

                if(this.txtNameEdit.Text.Length > 10)
                {
                    this.tbErrorName.Visibility = Visibility.Visible;
                    this.tbErrorName.Text = "真实姓名不能超过10个字";
                    this.rectName.Stroke = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                this.tbErrorName.Visibility = Visibility.Collapsed;
                this.rectName.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
            }
        }

        private void txtPosition_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtPosition.Text))
            {
                if (!CommonValidationMethod.UserPositionValidate(this.txtPosition.Text))
                {
                    this.tbErrorPosition.Visibility = Visibility.Visible;
                    this.tbErrorPosition.Text = "请勿输入特殊字符";
                    this.rectPosition.Stroke = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    this.tbErrorPosition.Visibility = Visibility.Collapsed;
                    this.rectPosition.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                }

                if(this.txtPosition.Text.Length > 10)
                {
                    this.tbErrorPosition.Visibility = Visibility.Visible;
                    this.tbErrorPosition.Text = "职位不能超过10个字";
                    this.rectPosition.Stroke = new SolidColorBrush(Colors.Red);
                }
            }
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtEmail.Text))
            {
                if (!CommonValidationMethod.IsEmail(this.txtEmail.Text))
                {
                    this.tbErrorEmail.Visibility = Visibility.Visible;
                    this.tbErrorEmail.Text = "请正确填写您的邮箱";
                    this.rectEmail.Stroke = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    this.tbErrorEmail.Visibility = Visibility.Collapsed;
                    this.rectEmail.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                }
            }
            else
            {
                this.tbErrorEmail.Visibility = Visibility.Collapsed;
                this.rectEmail.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
            }

        }

        private void txtWebSite_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void txtWebSite_LostFocus(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(this.txtWebSite.Text))
            {
                this.tbErrorWebSite.Visibility = Visibility.Collapsed;
                this.rectWebSite.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                return;
            }
            if (!CommonValidationMethod.IsWebSite(this.txtWebSite.Text))
            {
                this.tbErrorWebSite.Visibility = Visibility.Visible;
                this.rectWebSite.Stroke = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.tbErrorWebSite.Visibility = Visibility.Collapsed;
                this.rectWebSite.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
            }
        }

        
      
        private void tabTag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            switch(fe.Name)
            {
                case "tabTag1":
                    TrackHelper2.TrackOperation("5.1.7.11.1", "clk");
                    break;
                case "tabTag2":
                    TrackHelper2.TrackOperation("5.1.7.12.1", "clk");
                    break;
                case "tabTag3":
                    TrackHelper2.TrackOperation("5.1.7.13.1", "clk");
                    break;
            }
        }

        private void txtNameEdit_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtNameEdit.Text))
            {
                if (!CommonValidationMethod.UserNameValidate(this.txtNameEdit.Text))
                {
                    this.tbErrorName.Visibility = Visibility.Visible;
                    this.tbErrorName.Text = "请勿输入数字或特殊字符";
                    this.rectName.Stroke = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    this.tbErrorName.Visibility = Visibility.Collapsed;
                    this.rectName.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                }

                if (this.txtNameEdit.Text.Length > 10)
                {
                    this.tbErrorName.Visibility = Visibility.Visible;
                    this.tbErrorName.Text = "真实姓名不能超过10个字";
                    this.rectName.Stroke = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                this.tbErrorName.Visibility = Visibility.Collapsed;
                this.rectName.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
            }

        }

        private void txtPosition_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtPosition.Text))
            {
                if (!CommonValidationMethod.UserPositionValidate(this.txtPosition.Text))
                {
                    this.tbErrorPosition.Visibility = Visibility.Visible;
                    this.tbErrorPosition.Text = "请勿输入特殊字符";
                    this.rectPosition.Stroke = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    this.tbErrorPosition.Visibility = Visibility.Collapsed;
                    this.rectPosition.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                }

                if (this.txtPosition.Text.Length > 10)
                {
                    this.tbErrorPosition.Visibility = Visibility.Visible;
                    this.tbErrorPosition.Text = "职位不能超过10个字";
                    this.rectPosition.Stroke = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                this.tbErrorPosition.Visibility = Visibility.Collapsed;
                this.rectPosition.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
            }
        }

        private void txtBriefName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtBriefName.Text))
            {
                if (!CommonValidationMethod.CompanyNameValidate(this.txtBriefName.Text))
                {
                    this.tbErrorBriefName.Visibility = Visibility.Visible;
                    this.rectBriefName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorBriefName.Text = "请勿输入特殊字符";
                }
                else
                {
                    this.tbErrorBriefName.Visibility = Visibility.Collapsed;
                    this.rectBriefName.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                    this.tbErrorBriefName.Text = "请填写公司简称";
                }

                if (this.txtBriefName.Text.Length > 20)
                {
                    this.tbErrorBriefName.Visibility = Visibility.Visible;
                    this.rectBriefName.Stroke = new SolidColorBrush(Colors.Red);
                    this.tbErrorBriefName.Text = "公司简称不能超过20个字";
                }
            }
            else
            {
                this.tbErrorBriefName.Visibility = Visibility.Collapsed;
                this.rectBriefName.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                this.tbErrorBriefName.Text = "请填写公司简称";
            }

        }

        private void txtCompanyAddress_LostFocus(object sender, RoutedEventArgs e)
        {
            this.rectLocation.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
        }

        private void cmbNature_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.7.5.1", "clk");
        }

        private void cmbSize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.7.6.1", "clk");
        }

        private void cmbFinancingState_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.7.16.1", "clk");
        }
    }
}
