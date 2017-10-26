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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MagicCube.Model;
using MagicCube.HttpModel;
using MagicCube.View.Message;
using System.Collections.ObjectModel;
using System.IO;
using MagicCube.Common;

using MagicCube.TemplateUC;
using MagicCube.BindingConvert;
using MagicCube.ViewModel;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCCompanyInfo.xaml 的交互逻辑
    /// </summary>
    public partial class UCCompanyInfo : UserControl
    {
        public UCCompanyInfo()
        {
            InitializeComponent();
            ucProvinceCity.OKOrCancelActionDelegate += ProvinceCityCallback;
        }
        #region "变量"
        string picturePath = string.Empty;
        InfomationModel infomationModel = new InfomationModel();
        private MagicCube.ViewModel.HttpCompanyInfoModel httpCompany;     
        List<HttpCompanyCodes> NatureList = new List<HttpCompanyCodes>();
        List<HttpCompanyCodes> SizeList = new List<HttpCompanyCodes>();
        List<HttpCompanyCodes> StageList = new List<HttpCompanyCodes>();
        List<CityCodeItem> hotCityList = new List<CityCodeItem>();
        List<HttpProvinceCityCodes> provinceList = new List<HttpProvinceCityCodes>();
        List<HttpCityCodes> cityList = new List<HttpCityCodes>();
        ObservableCollection<HttpIndustresItem> industriesList = new ObservableCollection<HttpIndustresItem>();
        List<HttpIndustresItem> selectIndustry = new List<HttpIndustresItem>();

        ObservableCollection<tagsCompany> tag1List = new ObservableCollection<tagsCompany>();
        ObservableCollection<tagsCompany> tag2List = new ObservableCollection<tagsCompany>();
        ObservableCollection<tagsCompany> tag3List = new ObservableCollection<tagsCompany>();
        private bool IsInitialCompany = false;
        #endregion

        #region "事件"
        private void LeftCompanyImageChoose_Click(object sender, RoutedEventArgs e)
        {
            this.listImageList.SelectedIndex--;
            ucCompanyImageBntChoose.Tag = ComanyImageRightBtnVisble();
        }
        private void RightCompanyImageChoose_Click(object sender, RoutedEventArgs e)
        {
            this.listImageList.SelectedIndex++;
            ucCompanyImageBntChoose.Tag = ComanyImageRightBtnVisble();
        }
        private void listImageList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucCompanyImageBntChoose.Tag = ComanyImageRightBtnVisble();
        }
        private void btnAddLOGOPicture_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.7.4.13.1", "clk");

            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "(*.jpg,*.png)|*.jpg;*.png";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ucCompanyLogoUpload.GetImagePath(openFile.FileName);
                this.ucCompanyLogoUpload.Visibility = Visibility.Visible;
                this.gdEditCompany.Visibility = Visibility.Collapsed;
            }
        }
        private async void UploadCompanyImageShow_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.7.4.10.1", "clk");

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
                            infomationModel.ImageUrls.Add( model.data.url);
                            infomationModel.ImgUrlCount = infomationModel.ImageUrls.Count;
                            this.listImageList.SelectedIndex = infomationModel.ImageUrls.Count - 1;
                            ucCompanyImageBntChoose.Tag = ComanyImageRightBtnVisble();
                            if (infomationModel.ImageUrls.Count >= 5)
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
                   



                //Action method = delegate
                //{
                //    string strTemp = HttpHelper.HttpUploadFile(string.Format(ConfUtil.ServerUploadFile, "test1"), picturePath, null);
                //    if (strTemp != null)
                //    {
                //        HttpReturnUploadPictureUrl urlJson = DAL.JsonHelper.ToObject<HttpReturnUploadPictureUrl>(strTemp);
                //        if (urlJson != null)
                //        {
                //            Action method2 = new Action(() =>
                //            {
                //                infomationModel.ImageUrls.Add(urlJson.url);
                //                infomationModel.ImgUrlCount = infomationModel.ImageUrls.Count;
                //                this.listImageList.SelectedIndex = infomationModel.ImageUrls.Count - 1;
                //                ucCompanyImageBntChoose.Tag = ComanyImageRightBtnVisble();

                //                if (infomationModel.ImageUrls.Count >= 5)
                //                {
                //                    btnAddCompanyPictureShow.Visibility = System.Windows.Visibility.Collapsed;
                //                }
                //            });
                //            this.Dispatcher.BeginInvoke(method2, System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                //        }
                //        Action method3 = delegate
                //        {
                //            this.busyCtrl.IsBusy = false;
                //        };
                //        this.Dispatcher.BeginInvoke(method3, System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                //    }
                //};
                //method.BeginInvoke(null, null);
            }

        }

        private void TagsCheck_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.4.6.1", "clk");

            System.Windows.Controls.CheckBox chk = sender as System.Windows.Controls.CheckBox;
            tagsCompany tag = chk.DataContext as tagsCompany;
            if (infomationModel.Tags.Count >= 8)
            {
                chk.IsChecked = false;
                WinConfirmTip win = new WinConfirmTip("诱惑标签最多只能选择8个");
                win.Owner = Window.GetWindow(this);
                win.ShowDialog();
                return;
            }
            foreach (var item in infomationModel.Tags)
            {
                if (item == tag.Tag)
                    return;
            }
            infomationModel.Tags.Add(tag.Tag);
            if (infomationModel.Tags.Count != 0)
                this.infomationModel.TagsValidationBool = false;
        }

        private void DeleteTags_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            string temp = btn.DataContext as string;
            infomationModel.Tags.Remove(temp);
            UpdateTagsListShow(temp, false);
        }

        private void DeleteCompanyImageShow_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            string str = btn.DataContext as string;
            if (!string.IsNullOrEmpty(str))
            {
                this.infomationModel.ImageUrls.Remove(str);
                infomationModel.ImgUrlCount = infomationModel.ImageUrls.Count;
                if (this.infomationModel.ImageUrls.Count < 5)
                {
                    btnAddCompanyPictureShow.Visibility = Visibility.Visible;
                }
                this.listImageList.SelectedIndex = 0;

            }
            ucCompanyImageBntChoose.Tag = ComanyImageRightBtnVisble();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.7.4.11.1", "clk");

            bool flag = this.CompanyValidateTrigger();
            //判断city是否有
            //if (this.infomationModel.FullCity == null)
            //{
            //    this.rctCityValidate.Stroke = new SolidColorBrush(Colors.Red);
            //    this.tbCityValidate.Visibility = Visibility.Visible;
            //    DisappearShow show = new DisappearShow("您有不正确填写项", 1);
            //    show.Owner = Window.GetWindow(this);
            //    show.ShowDialog();
            //    return;
            //}
            if (!flag)
            {
                DisappearShow show = new DisappearShow("您有不正确填写项", 1);
                show.Owner = Window.GetWindow(this);
                show.ShowDialog();
                return;
            }
            this.busyCtrl.IsBusy = true;
            this.infomationModel.Intro = this.ucHtmlEditor.getText();
            UpdateHttpCompany();
            //pCompany.company.imageUrls = null;


            //保存公司信息

            string jsonStr = DAL.JsonHelper.ToJsonString(this.httpCompany);
            string jsonResult = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrSaveCompany, MagicGlobal.UserInfo.Version), jsonStr);
            //string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrSaveCompany, jsonStr));
            MagicCube.ViewModel.BaseHttpModel<HttpCompanyInfoModel> modelResult = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<HttpCompanyInfoModel>>(jsonResult);
            this.busyCtrl.IsBusy = false;
            if (modelResult == null)
            {
                MagicCube.TemplateUC.WinMessageLink link = new WinMessageLink("网络异常，请重试", "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                return;
            }
            else
            {
                if (modelResult.code == 200)
                {
                    DisappearShow show = new DisappearShow("保存成功", 1);
                    Window win = Window.GetWindow(this);
                    show.Owner = win;
                    show.ShowDialog();
                    this.gdEditCompany.Visibility = Visibility.Collapsed;
                    this.gdCompany.Visibility = Visibility.Visible;
                }
            }
        }

        private async void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.7.4.12.1", "clk");

            this.gdEditCompany.Visibility = Visibility.Collapsed;
            this.gdCompany.Visibility = Visibility.Visible;
            await LoadingConpanyInfo();


        }

        private async void BtnModify_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.3.13.1","clk");

            Common.TrackHelper2.TrackOperation("5.7.4.1.1", "pv");


            this.Cursor = Cursors.Wait;
            this.busyCtrl.IsBusy = true;

            await InitailEditCompanyInfo();
            this.gdEditCompany.Visibility = Visibility.Visible;
            this.gdCompany.Visibility = Visibility.Collapsed;
            this.scrollView.ScrollToTop();
            this.Cursor = Cursors.Arrow;
            this.busyCtrl.IsBusy = false;
        }

        private void BtnAddSelfTag_Click(object sender, RoutedEventArgs e)
        {
            if (infomationModel.Tags.Count >= 8)
            {
                WinConfirmTip win = new WinConfirmTip("诱惑标签最多只能选择8个");
                win.Owner = Window.GetWindow(this);
                win.ShowDialog();
                return;
            }
            string temp = this.txtAddTag.Text;
            temp = temp.Trim();
            if (string.IsNullOrWhiteSpace(temp))
                return;
            temp = temp.Replace(" ", "");
            this.infomationModel.Tags.Add(temp);
            this.txtAddTag.Clear();
            if (infomationModel.Tags.Count != 0)
                this.infomationModel.TagsValidationBool = false;
            this.chkAddTags.IsChecked = false;
        }

        private void cmbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cmbCity_MouseEnter(object sender, MouseEventArgs e)
        {
            //this.rctCityValidate.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
            this.tbCityValidate.Visibility = Visibility.Collapsed;
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

        private void IndustrySelectOK_Click(object sender, RoutedEventArgs e)
        {
            string temp = string.Empty;
            foreach (var item in this.selectIndustry)
            {
                temp += item.name + "、";
            }
            //this.chkIndustryShow.Content = temp.TrimEnd(new char[] { '、' });
            infomationModel.Industries = temp.TrimEnd(new char[] { '、' });
            this.chkIndustryShow.IsChecked = false;
        }

        private void IndustrySelectCancel_Click(object sender, RoutedEventArgs e)
        {
            this.chkIndustryShow.IsChecked = false;
        }

        private void IndustryBorder_IsVisibaleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                //string industriesResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Industries.json");
                //industriesList = DAL.JsonHelper.ToObject<ObservableCollection<HttpIndustresItem>>(industriesResult);
                this.selectIndustry.Clear();
                string[] industryTemp = infomationModel.Industries.Split(new char[] { '、' });
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
        #endregion

        #region "对内功能函数"
        private Visibility ComanyImageRightBtnVisble()
        {
            if (this.listImageList.Items != null)
            {
                int index = this.listImageList.Items.Count - 1;
                if (this.listImageList.SelectedIndex < index)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;


        }
        private void InitialComanyInfo()
        {
            if (this.httpCompany == null)
                return;

            if (! Convert.ToBoolean(this.httpCompany.updateCompanyAuth))
            {
                this.btnModify.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.btnModify.Visibility = Visibility.Visible;
            }

            infomationModel.CompanyName = httpCompany.companyName;
            infomationModel.BriefName = httpCompany.companyShortName;
            if(!string.IsNullOrEmpty(httpCompany.companyIndustry))
            {
                this.infomationModel.Industries = string.Empty;
                string[] tempList = httpCompany.companyIndustry.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var item in tempList)
                {
                    foreach(var jsonIndu in industriesList)
                    {
                        if(jsonIndu.code == item)
                        {
                            infomationModel.Industries += jsonIndu.name + "、";
                        }
                    }
                }
                infomationModel.Industries = infomationModel.Industries.TrimEnd(new char[] { '、' });
            }

            foreach(var jsonitem in NatureList)
            {
                if(jsonitem.code == httpCompany.companyCharact)
                {
                    infomationModel.Nature = jsonitem;
                }
            }
            foreach(var jsonItem in SizeList)
            {
                if(jsonItem.code == httpCompany.companyScale)
                {
                    infomationModel.Size = jsonItem;
                }
            }
            foreach(var jsonStage in StageList)
            {
                if(jsonStage.code == httpCompany.companyStage)
                {
                    infomationModel.FinancingState = jsonStage;
                }
            }

            infomationModel.SmallCompanyLogoUrl = httpCompany.companyLogo;
            infomationModel.Location = httpCompany.companyAddress;
            infomationModel.WebSite = httpCompany.companyWebsite;
            infomationModel.CompanyLatLng = httpCompany.companyLocation;

            //加载省市区
            if(!string.IsNullOrEmpty(httpCompany.companyDistrict))
            {
                if(httpCompany.companyDistrict.Length == 8)
                {
                    foreach (var item in provinceList)
                    {
                        if (item.code == httpCompany.companyDistrict.Substring(0, 6))
                        {
                            infomationModel.Province = item;
                            foreach (var item2 in item.city)
                            {
                                if (item2.code == httpCompany.companyDistrict)
                                {
                                    infomationModel.City = item2;
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
                            infomationModel.Province = item;
                            foreach (var item2 in item.city)
                            {
                                if (item2.code == httpCompany.companyDistrict)
                                {
                                    infomationModel.City = item2;
                                }
                            }
                        }
                    }
                }
              
            }
            else if(!string.IsNullOrEmpty(httpCompany.companyCity))
            {
                foreach(var item in provinceList)
                {
                    if (item.code == httpCompany.companyCity.Substring(0, 4))
                    {
                        infomationModel.Province = item;
                        foreach(var item2 in item.city)
                        {
                            if(item2.code == httpCompany.companyCity)
                            {
                                infomationModel.City = item2;
                            }
                        }
                    }
                }
            }
            else if(!string.IsNullOrEmpty(httpCompany.companyProvince))
            {
                foreach(var item in provinceList)
                {
                    if(item.code == httpCompany.companyProvince)
                    {
                        this.infomationModel.Province = item;
                        this.infomationModel.City = item.city[0];
                    }
                }
            }
            if (this.infomationModel.Province != null)
                this.infomationModel.FullCity = this.infomationModel.Province.name;
            if (this.infomationModel.City != null)
                this.infomationModel.FullCity += "-" + this.infomationModel.City.name;
           
            //地图位置加载
      
            string city = string.Empty;
            if(infomationModel.Province != null)
            {
                if (!string.IsNullOrWhiteSpace(infomationModel.Province.name))
                {
                    city = infomationModel.Province.name;
                }
            }
            if(infomationModel.City != null)
            {
                if (!string.IsNullOrWhiteSpace(infomationModel.City.name))
                {
                    city = infomationModel.City.name;
                }
            }
            if (!string.IsNullOrWhiteSpace(this.infomationModel.CompanyLatLng))
            {
                string[] tempList = this.infomationModel.CompanyLatLng.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempList.Length == 2)
                {
                    //double lng = Convert.ToDouble(tempList[0]);
                    //double lat = Convert.ToDouble(tempList[1]);
                    double lng;
                    double lat;
                    double.TryParse(tempList[0], out lng);
                    double.TryParse(tempList[1], out lat);

                    this.ucGmap.InitialUCGamp(this.infomationModel.Location, city,lat, lng);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(this.infomationModel.Location))
                    {
                         this.ucGmap.InitialUCGamp(this.infomationModel.Location, city);
                    }
                    else
                    {
                        this.ucGmap.InitialUCGamp(city, city);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.infomationModel.Location))
                {
                    this.ucGmap.InitialUCGamp(this.infomationModel.Location, city);
                }
                else
                {
                    this.ucGmap.InitialUCGamp(city, city);
                }
            }


            //标签初始化
            infomationModel.Tags.Clear();
            if (httpCompany.companyBenefit != null)
            {
                string[] tempList = httpCompany.companyBenefit.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in tempList)
                {
                    infomationModel.Tags.Add(item);
                }

            }
            infomationModel.ImageUrls.Clear();
            infomationModel.ImgUrlCount = 0;
            if (httpCompany.companyImageUrls != null)
            {
                string[] tempList = httpCompany.companyImageUrls.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in tempList)
                {
                    string temp = item.Trim();
                    if (temp.StartsWith("["))
                        continue;
                    if ((!string.IsNullOrWhiteSpace(temp)) && temp != "null")
                        infomationModel.ImageUrls.Add(temp.ToString());
                }

                if (this.infomationModel.ImageUrls.Count != 0)
                {
                    this.listImageList.SelectedIndex = 0;
                }
                if (infomationModel.ImageUrls.Count >= 5)
                {
                    btnAddCompanyPictureShow.Visibility = Visibility.Collapsed;
                }
                infomationModel.ImgUrlCount = infomationModel.ImageUrls.Count;
                this.listImageList.ItemsSource = infomationModel.ImageUrls;
            }
            ucCompanyImageBntChoose.Tag = ComanyImageRightBtnVisble();

            infomationModel.Intro = httpCompany.companyIntro;
            if (this.ucHtmlEditor != null)
                this.ucHtmlEditor.setText(infomationModel.Intro);


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
            if(model == null)
            {
                return;
            }
            else
            {
                if(model.code == 200)
                {
                    foreach(var item in model.data.salaryWelfareCompany)
                    {
                        this.tag1List.Add(new tagsCompany() { Tag = item.text, IsChecked = false });
                    }
                    foreach(var item in model.data.enviromentCompany)
                    {
                        this.tag2List.Add(new tagsCompany() { Tag = item.text, IsChecked = false });
                    }
                    foreach(var item in model.data.moreWelfareCompany)
                    {
                        this.tag3List.Add(new tagsCompany() { Tag = item.text, IsChecked = false});
                    }
                }
            }
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
        private bool CompanyValidateTrigger()
        {
            string temp = "zzzzzzzzzz";
            string content;
            int tempInt = 0;
            this.infomationModel.IsCompanyValidation = true;


            content = infomationModel.BriefName;
            infomationModel.BriefName = temp;
            infomationModel.BriefName = content;

          


            content = infomationModel.Location;
            infomationModel.Location = temp;
            infomationModel.Location = content;

            string temp2 = "http://wwww.123.cn";
            content = infomationModel.WebSite;
            infomationModel.WebSite = temp2;
            infomationModel.WebSite = content;


            //tempInt = this.cmbIndustries.SelectedIndex;
            //this.cmbIndustries.SelectedIndex = 0;
            //this.cmbIndustries.SelectedIndex = tempInt;


            tempInt = this.cmbNature.SelectedIndex;
            this.cmbNature.SelectedIndex = 1;
            this.cmbNature.SelectedIndex = tempInt;

            tempInt = this.cmbSize.SelectedIndex;
            this.cmbSize.SelectedIndex = 1;
            this.cmbSize.SelectedIndex = tempInt;
            //tempInt = this.cmbCity.SelectedIndex;
            //this.cmbCity.SelectedIndex = tempInt;



            if (this.infomationModel.Tags == null)
            {
                this.infomationModel.TagsValidationBool = true;
                this.infomationModel.IsCompanyValidation = false;
            }
            else
            {
                if (this.infomationModel.Tags.Count == 0)
                {
                    this.infomationModel.TagsValidationBool = true;
                    this.infomationModel.IsCompanyValidation = false;
                }
                else
                {
                    this.infomationModel.TagsValidationBool = false;
                }
            }

            string info = ucHtmlEditor.getStringText();
            if (string.IsNullOrWhiteSpace(info))
            {
                bdHtmlEditor.BorderBrush = new SolidColorBrush(Colors.Red);
                tbHtmlEditorValidate.Visibility = Visibility.Visible;
                tbHtmlEditorValidate.Text = "请填写公司介绍";
                this.infomationModel.IsCompanyValidation = false;
            }
            else if (info.Length > 1000)
            {
                bdHtmlEditor.BorderBrush = new SolidColorBrush(Colors.Red);
                tbHtmlEditorValidate.Visibility = Visibility.Visible;
                tbHtmlEditorValidate.Text = "公司介绍不能多于1000字";
                this.infomationModel.IsCompanyValidation = false;
            }
            else if (info.Length < 20)
            {
                bdHtmlEditor.BorderBrush = new SolidColorBrush(Colors.Red);
                tbHtmlEditorValidate.Visibility = Visibility.Visible;
                tbHtmlEditorValidate.Text = "公司介绍不能少于20字";
                this.infomationModel.IsCompanyValidation = false;
            }
            else
            {
                this.bdHtmlEditor.BorderBrush = new SolidColorBrush(Colors.Transparent);
                this.tbHtmlEditorValidate.Visibility = Visibility.Collapsed;
            }

            //content = this.Size;
            //this.Size = temp;
            //this.Size = content;

            //content = this.City;
            //this.City = temp;
            //this.City = content;

            //content = this.FinancingState;
            //this.FinancingState = temp;
            //this.FinancingState = content;
            return this.infomationModel.IsCompanyValidation;
        }
        private void UpdateHttpCompany()
        {
            if (this.infomationModel.Province != null)
                this.infomationModel.FullCity = this.infomationModel.Province.name;
            if (this.infomationModel.City != null)
                this.infomationModel.FullCity += "-" + this.infomationModel.City.name;
            this.httpCompany.companyName = this.infomationModel.CompanyName;
            this.httpCompany.companyWebsite = this.infomationModel.WebSite;
            string industryTemp = string.Empty;
            foreach (var item in this.selectIndustry)
            {
                industryTemp += item.code + "$$";
            }
            this.httpCompany.companyIndustry = industryTemp.TrimEnd(new char[] { '$' });

            this.httpCompany.companyScale = this.infomationModel.Size.code;
            string tagsTemp = string.Empty;
            foreach(var item in infomationModel.Tags)
            {
                tagsTemp += item + "$$";
            }
            this.httpCompany.companyBenefit = tagsTemp.TrimEnd(new char[] { '$' });

            this.httpCompany.companyAddress = this.infomationModel.Location;
            this.httpCompany.companyShortName = this.infomationModel.BriefName;

            this.httpCompany.companyCharact = this.infomationModel.Nature.code;
            this.httpCompany.companyLogo = this.infomationModel.SmallCompanyLogoUrl;
            this.httpCompany.companyStage = this.infomationModel.FinancingState.code;
            this.httpCompany.companyIntro = this.infomationModel.Intro;
            string imageTemp = string.Empty;
            foreach(var item in this.infomationModel.ImageUrls)
            {
                imageTemp += item + "$$";
            }
            this.httpCompany.companyImageUrls = imageTemp.TrimEnd(new char[] { '$' });

            //热门城市
            if(this.infomationModel.Province.code.Length == 6)
            {
                this.httpCompany.companyProvince = this.infomationModel.Province.code.Substring(0, 4);
                this.httpCompany.companyCity = this.infomationModel.Province.code;
                this.httpCompany.companyDistrict = this.infomationModel.City.code;
            }
            else
            {
                //直辖市
                if(this.infomationModel.Province.code == "8611" || this.infomationModel.Province.code == "8612" || this.infomationModel.Province.code == "8631" || this.infomationModel.Province.code == "8650")
                {
                    this.httpCompany.companyProvince = this.infomationModel.Province.code.Substring(0,4);
                    this.httpCompany.companyCity = this.infomationModel.Province.code;
                    this.httpCompany.companyDistrict = this.infomationModel.City.code;
                }
                else
                {
                    this.httpCompany.companyProvince = this.infomationModel.Province.code;
                    this.httpCompany.companyDistrict = string.Empty;
                    //除去香港，澳门，台湾
                    if(this.infomationModel.City.code == "8671" || this.infomationModel.City.code == "8681" || this.infomationModel.City.code == "8682")
                    {
                        this.httpCompany.companyCity = string.Empty;
                    }
                    else
                    {
                        this.httpCompany.companyCity = this.infomationModel.City.code;

                    }
                }
                
              
            }
            Point pt = this.ucGmap.GetMarkerLatLng();
            this.httpCompany.companyLocation = pt.Y.ToString() + "$$" + pt.X.ToString();



        }
        #endregion

        #region "对外接口"
        /// <summary>
        /// 初始化公司信息
        /// </summary>
        /// <returns></returns>
        public async Task InitailEditCompanyInfo()
        {
    

            this.gdCompany.Visibility = Visibility.Visible;
            this.gdEditCompany.Visibility = Visibility.Collapsed;
            this.ucCompanyLogoUpload.Visibility = Visibility.Collapsed;

            if (IsInitialCompany)
            {
                await this.LoadingConpanyInfo();
                return;
            }

            IsInitialCompany = true;
            //数值化列表
            string industriesResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "companyInfoDict.json");
            jsonCompanyInfoDictModel jsonCompanyModel = DAL.JsonHelper.ToObject<jsonCompanyInfoDictModel>(industriesResult);
            if(jsonCompanyModel != null)
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
                    else if(item.name == "scale")
                    {
                        foreach(var scaleItem in item.mappingItem)
                        {
                            this.SizeList.Add(new HttpCompanyCodes() { name = scaleItem.fullName, code = scaleItem.value });
                        }
                    }
                    else if(item.name == "stage")
                    {
                        foreach(var stageItem in item.mappingItem)
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

            foreach(var item in jsonCityModel.CityCodes.CityCode)
            {
                if(item.value.Length == 4)
                {
                    ProvinceJsonList.Add(item);
                }
                if(item.value.Length == 6)
                {
                    CityJsonList.Add(item);
                }
                //杭州
                if(item.value.StartsWith("863301"))
                {
                    if(item.value.Length == 6)
                    {
                        ProvinceJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
                //武汉
                if(item.value.StartsWith("864201"))
                {
                    if(item.value.Length == 6)
                    {
                        ProvinceJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
                //成都
                if(item.value.StartsWith("865101"))
                {
                    if(item.value.Length == 6)
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

           foreach(var item in ProvinceJsonList)
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
                else if(item.value == "8682")
                {
                    proCode.city.Add(new HttpCityCodes() { name = item.simpleName, code = item.value });
                }
                //台湾
                else if(item.value == "8671")
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
            //加载公司信息
            string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpCompanyInfoModel>();
            string jsonStr = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "companyID", "properties" },
                new string[] { MagicGlobal.UserInfo.Id.ToString(), MagicGlobal.UserInfo.CompanyId.ToString(), propertys });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetCompanyInfo, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<HttpCompanyInfoModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpCompanyInfoModel>>(resultStr);
            if (model == null)
            {
                return;
            }
            if (model.code == 200)
            {
                this.httpCompany = model.data;
                this.InitialComanyInfo();

                await this.InitialTagsList();
                foreach (var item in infomationModel.Tags)
                {
                    UpdateTagsListShow(item, true);
                }
                this.itemsTag1.ItemsSource = this.tag1List;
                this.itemsTag2.ItemsSource = this.tag2List;
                this.itemsTag3.ItemsSource = this.tag3List;
                this.gdMain.DataContext = infomationModel;

                //测试新空间行业下拉框
                this.selectIndustry.Clear();
                string[] industryTemp = infomationModel.Industries.Split(new char[] { '、' });
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
                ucHintComboBox.ItemsSource = StageList;
                ucHtmlEditor.actionFocus += HtmlEditorFocusedCallback;

                ucCompanyLogoUpload.OKAction += CompanyLogoUploadOKCallback;
                ucCompanyLogoUpload.CancelAction += CompanyLogoUploadCancelCallback;
            }

        }
        #endregion

        #region "回调函数"
        private void HtmlEditorFocusedCallback(bool flag)
        {
            if (flag)
            {
                this.bdHtmlEditor.BorderBrush = new SolidColorBrush(Colors.Transparent);
                this.tbHtmlEditorValidate.Visibility = Visibility.Collapsed;
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
            if(model == null)
            {
                MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                winLink.Owner = Window.GetWindow(this);
                winLink.ShowDialog();
                return;
            }
            else
            {
                if(model.code == 200)
                {
                    infomationModel.SmallCompanyLogoUrl = model.data.url;
                }
                else
                {
                    MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    winLink.Owner = Window.GetWindow(this);
                    winLink.ShowDialog();
                    return;
                }
            }

            //Action method = delegate
            //{
            //    //string strTemp = HttpHelper.HttpUploadFile(string.Format(ConfUtil.ServerUploadFile, "test1"), picturePath, null);
            //    string strTemp = HttpHelper.HttpUploadStream(string.Format(ConfUtil.ServerUploadFile, "test1"), stream, null, Encoding.UTF8);
            //    if (strTemp != null)
            //    {
            //        HttpReturnUploadPictureUrl urlJson = DAL.JsonHelper.ToObject<HttpReturnUploadPictureUrl>(strTemp);
            //        if (urlJson != null)
            //        {
            //            infomationModel.SmallCompanyLogoUrl = urlJson.url;
            //        }
            //        else
            //        {
            //            this.Dispatcher.BeginInvoke(new Action(() =>
            //            {
            //                MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink("哎呀，网络好像不太稳定。上传失败，请您重新上传.", "我知道了");
            //                winLink.Owner = Window.GetWindow(this);
            //                winLink.ShowDialog();
            //                return;
            //            }));
                       
            //        }
            //    }
            //    Action method3 = delegate
            //    {
            //        this.busyCtrl.IsBusy = false;
            //        //DisappearShow show = new DisappearShow("上传失败", 1);
            //        //show.Owner = Window.GetWindow(this);
            //        //show.Show();
            //    };
            //    this.Dispatcher.BeginInvoke(method3, System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            //};
           

            //method.BeginInvoke(null, null);

        }
        private void CompanyLogoUploadCancelCallback()
        {
            this.ucCompanyLogoUpload.Visibility = Visibility.Collapsed;
            this.gdEditCompany.Visibility = Visibility.Visible;
        }

        private async Task ProvinceCityCallback(bool flag)
        {
            this.chkProvineCity.IsChecked = false;
            if(flag)
            {
                if (ucProvinceCity.OutSelectCityList.Count > 0)
                {
                    this.infomationModel.City = ucProvinceCity.OutSelectCityList[0];
                    this.infomationModel.Province = ucProvinceCity.selectedProvince;
                    this.infomationModel.FullCity = this.infomationModel.Province.name + "-" + this.infomationModel.City.name;
                }
                   
            }
            await this.ucGmap.SetAddress(this.infomationModel.City.name+this.infomationModel.Location, this.infomationModel.City.name);
        }



        #endregion

        //private async void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    string temp = this.infomationModel.City.name;
        //    //HttpCityCodes codes = this.cmbCity.SelectedItem as HttpCityCodes;
        //    //await this.ucGmap.SetAddress(codes.name, codes.name);
        //}

        private void ucGmap_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

            if(e.Delta > 0)
            {
                this.ucGmap.GmapZoom(1);
            }
            else
            {
                this.ucGmap.GmapZoom(-1);
            }
            e.Handled = true;
        }

        //公司地址填写更改
        private async void txtAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(this.infomationModel.City == null)
            {
                //MessageBox.Show("请先选择城市!");
                return;
            }
            await ucGmap.SetAddress(this.txtCompanyAddress.Text, this.infomationModel.City.name);
        }

        private void chkProvineCity_Click(object sender, RoutedEventArgs e)
        {
            this.ucProvinceCity.SetSelectedList(this.infomationModel.City);
            this.ucProvinceCity.selectedProvince = this.infomationModel.Province;
        }


        private void TrackOperation_Event(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            switch(fe.Name)
            {
                case "txtBriefName":
                    Common.TrackHelper2.TrackOperation("5.7.4.14.1", "clk");
                    break;
                case "chkIndustryShow":
                    Common.TrackHelper2.TrackOperation("5.7.4.15.1", "clk");
                    break;
                case "cmbNature":
                    Common.TrackHelper2.TrackOperation("5.7.4.16.1","clk");
                    break;
                case "cmbSize":
                    Common.TrackHelper2.TrackOperation("5.7.4.17.1", "clk");
                    break;
                case "chkProvineCity":
                    Common.TrackHelper2.TrackOperation("5.7.4.2.1", "clk");
                    break;
                case "txtCompanyAddress":
                    Common.TrackHelper2.TrackOperation("5.7.4.4.1", "clk");
                    break;
                case "chkAddTags":
                    Common.TrackHelper2.TrackOperation("5.7.4.5.1", "clk");
                    break;
                case "ucHtmlEditor":
                    Common.TrackHelper2.TrackOperation("5.7.4.7.1", "clk");
                    break;
                case "ucHintComboBox":
                    Common.TrackHelper2.TrackOperation("5.7.4.8.1","clk");
                    break;
                case "txtWebSite":
                    Common.TrackHelper2.TrackOperation("5.7.4.9.1", "clk");
                    break;
            }
        }
    }
}
