using MagicCube.HttpModel;

using MagicCube.Common;
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
using MagicCube.View.Message;
using System.IO;
using MagicCube.Model;
using System.Text.RegularExpressions;
using MagicCube.TemplateUC;
using MagicCube.ViewModel;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// ResumeView.xaml 的交互逻辑
    /// </summary>
    public partial class UCJobPublish : UserControl
    {

        List<HttpProvinceCityCodes> provinceList = new List<HttpProvinceCityCodes>();
        List<HttpCityCodes> cityList = new List<HttpCityCodes>();
        List<string> systemTemptation = new List<string>();
        MagicCube.ViewModel.HttpEditJob mHttpEditJob;
        MagicCube.ViewModel.HttpPublishJob mHttpPublishJob;
        List<HttpJobTreeThree> m_HttpJobTreeThree;
        List<Val2> m_HttpJobThreeList;
        ObservableCollection<tagsModel> tagList = new ObservableCollection<tagsModel>();
        ObservableCollection<string> tagChoose = new ObservableCollection<string>();
        string publishResult = string.Empty;
        bool isEdit = false;
        bool mFromClose = false;
        public delegate Task delegateSearch();
        public delegateSearch actionSearch;
        public delegate Task delegateAddV();
        public delegateAddV actionAddV;
        public Action actionUpdataCount;
        private Brush normalColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e5e5e5"));
        private Brush errColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f25751"));
        private Brush focusColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00beff"));

        public delegate Task delegateReturn(bool isFromClose);
        public delegateReturn actionReturn;

        ObservableCollection<Val2> tempList = new ObservableCollection<Val2>();
        public UCJobPublish()
        {
            InitializeComponent();
            tbContent.actionText = tbContent_TextChange;
            btnPublishJob.IsEnabled = true;
            ucPreView.spState.Visibility = Visibility.Collapsed;
            tbContent.actionFocus = tbContent_LostFocus;
            ucProvinceCity.OKOrCancelActionDelegate = ProvinceCityCallback;
            this.Loaded += UCJobPublish_Loaded;
        }

        private void UCJobPublish_Loaded(object sender, RoutedEventArgs e)
        {
            popThreeCodeList.ItemsSource = tempList;
        }

        public void iniJobTree()
        {
            if (m_HttpJobTreeThree == null)
            {
                string pResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "jobCatagary.json");
                HttpJobCatagaryCodeRoot pJobCatagaryRoot = DAL.JsonHelper.ToObject<HttpJobCatagaryCodeRoot>(pResult);
                m_HttpJobTreeThree = new List<HttpJobTreeThree>();
                List<Val> secondVal = new List<Val>();
                m_HttpJobThreeList = new List<Val2>();
                foreach (HttpJobCatagaryCodeRECORD iRECORD in pJobCatagaryRoot.RECORDS.RECORD)
                {
                    if (iRECORD.type_level == "1")
                    {
                        m_HttpJobTreeThree.Add(new HttpJobTreeThree() { code = iRECORD.type_code, name = iRECORD.type_name });
                    }
                    if (iRECORD.type_level == "2")
                    {
                        secondVal.Add(new Val() { code = iRECORD.type_code, name = iRECORD.type_name, parentCode = iRECORD.parent_type_code });
                    }
                    if (iRECORD.type_level == "3")
                    {
                        m_HttpJobThreeList.Add(new Val2() { code = iRECORD.type_code, name = iRECORD.type_name, parentCode = iRECORD.parent_type_code });
                    }
                }
                foreach (Val2 iVal2 in m_HttpJobThreeList)
                {
                    foreach (Val iVal in secondVal)
                    {
                        if (iVal2.parentCode == iVal.code)
                        {
                            if (iVal.val == null)
                                iVal.val = new List<Val2>();
                            iVal.val.Add(iVal2);
                            iVal2.parent = iVal;
                        }
                    }
                }
                foreach (Val iVal2 in secondVal)
                {
                    foreach (HttpJobTreeThree iVal in m_HttpJobTreeThree)
                    {
                        if (iVal2.parentCode == iVal.code)
                        {
                            if (iVal.val == null)
                                iVal.val = new List<Val>();
                            iVal.val.Add(iVal2);
                            iVal2.parent = iVal;
                        }
                    }
                }

                icJobTree.ItemsSource = m_HttpJobTreeThree;
                if (m_HttpJobTreeThree.Count > 0)
                {
                    m_HttpJobTreeThree[0].IsCheck = true;
                    icJobContent.ItemsSource = m_HttpJobTreeThree[0].val;
                }
            }
        }

        public void iniWorkingDegree()
        {
            MappingRoot lstEducation = MinDegreeHelper.GetCode();

            MappingRoot lstCharact = workCharactHelper.GetCode();

            MappingRoot lstExp = MinExpHelper.GetCode();

            cmbDegree.ItemsSource = lstEducation.root;
            cmbWorkingYear.ItemsSource = lstExp.root;
            cmbCategory.ItemsSource = lstCharact.root;
        }


        private async void btnClear_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.6.1", "clk");
            WinConfirmTip pWinMessage = new WinConfirmTip("将重置所有内容，是否确认？");
            pWinMessage.Owner = Window.GetWindow(this);
            if ((bool)pWinMessage.ShowDialog())
            {
                await ReClear();
            }
        }

        private Val2 findNamebyCode(string code)
        {
            foreach (HttpJobTreeThree iHttpJobTreeThree in m_HttpJobTreeThree)
            {
                foreach (Val iVal in iHttpJobTreeThree.val)
                {
                    foreach (Val2 iVal2 in iVal.val)
                    {
                        if (code == iVal2.code)
                        {
                            return iVal2;
                        }
                    }
                }
            }
            return null;
        }
        private async Task ReClear()
        {
            if (!isEdit)
            {
                iniPublish();
            }
            else
            {
                long id = mHttpEditJob.jobID;
                await iniEdit(id);
            }
        }
        private void reSetBorder()
        {
            bdJobCategory.BorderBrush = normalColor;
            tbjobName.BorderBrush = normalColor;
            tbLocation.BorderBrush = normalColor;
            tbMinSalary.BorderBrush = normalColor;
            tbMaxSalary.BorderBrush = normalColor;
            bdContent.BorderBrush = normalColor;
            bdCity.BorderBrush = normalColor;

            spJobCategory.Visibility = Visibility.Collapsed;
            spjobName.Visibility = Visibility.Collapsed;
            spLocation.Visibility = Visibility.Collapsed;
            spSalary.Visibility = Visibility.Collapsed;
            spSalary.Visibility = Visibility.Collapsed;
            spContentV.Visibility = Visibility.Collapsed;
            spjobTemptation.Visibility = Visibility.Collapsed;
            spCity.Visibility = Visibility.Collapsed;
        }
        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.5.1", "clk");
            if (!checktbContentTel())
            {
                WinMessageLink winLink = new WinMessageLink("职位描述中请勿包含手机号、微信、QQ、邮箱等联系方式", "我知道了");
                winLink.Owner = Window.GetWindow(this);
                winLink.ShowDialog();
                return;
            }
            if (!checkItem())
            {
                DisappearShow disappear = new DisappearShow("请填写完整后再预览", 1);
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                return;
            }

            if (!isEdit)
            {
                PrePublicJob();
            }
            else
            {
                PreEditJob();
            }

            gdPreView.Visibility = Visibility.Visible;
        }
        private async void ResultReturn_Click(object sender, RoutedEventArgs e)
        {
            int code = await UnActivatePublishHelper.UnActivatePublish();
            if (code == 0)
                return;
            if (code != 200)
            {
                if (code == -127)
                {
                    WinValidateMessage pWinMessage = new WinValidateMessage("您的账号尚未认证，认证后才能发布更多职位哦");
                    pWinMessage.Owner = Window.GetWindow(this);
                    if ((bool)pWinMessage.ShowDialog())
                    {
                        if (actionAddV != null)
                        {
                            await actionAddV();
                        }
                    }
                    return;
                }
                else if (code == -126)
                {
                    WinMessageLink pWinErroTip = new WinMessageLink("贵公司今天已发布20个职位，请明天再来发布", "我知道了");
                    pWinErroTip.Owner = Window.GetWindow(this);
                    pWinErroTip.ShowDialog();
                    return;
                }
            }
            gdPublishResult.Visibility = Visibility.Collapsed;
        }
        private async void SearchResume_Click(object sender, RoutedEventArgs e)
        {
            if (actionSearch != null)
                await actionSearch();
            MagicCube.Messaging.Messenger.Default.Send<Messaging.MSBodyBase, ViewSingle.UCSearchCondition2>(new Messaging.MSBodyBase() { body = null, type = "jobPublish" });
        }
        private void BtnPreViewClose_Click(object sender, RoutedEventArgs e)
        {
            gdPreView.Visibility = Visibility.Collapsed;
        }

        public void iniPublishEditCommon()
        {

            tagList = new ObservableCollection<tagsModel>();
            itemsTag.ItemsSource = tagList;
            itemsTagChoosed.ItemsSource = tagChoose;
            tagChoose.Clear();
            reSetBorder();
            gdEdit.Visibility = Visibility.Visible;
            tbFirstTip.Visibility = Visibility.Collapsed;
            gdJobChoose.Visibility = Visibility.Collapsed;
            gdPublishResult.Visibility = Visibility.Collapsed;
            svEdit.ScrollToTop();
            iniProvinceCity();
            iniWorkingDegree();
            iniJobTree();

            tbJobCategoryInput.Text = string.Empty;
        }
        public void iniPublish(bool isFromClose = false)
        {
            mFromClose = isFromClose;
            iniPublishEditCommon();

            SetSelfTag();
            btnPublishJob.Text = "发布职位";
            btnEditClear.Visibility = Visibility.Visible;
            spTemp.Visibility = Visibility.Visible;
            btnTempJob.Visibility = Visibility.Visible;

            gdTempShow.Visibility = Visibility.Collapsed;
            gdTempInput.Visibility = Visibility.Visible;
            tbTempShow.Text = "";


            spJobNameE.Visibility = Visibility.Collapsed;
            spJobNameP.Visibility = Visibility.Visible;
            spJobCategoryE.Visibility = Visibility.Collapsed;
            spJobCategoryP.Visibility = Visibility.Visible;
            spJobCityE.Visibility = Visibility.Collapsed;
            spJobCityP.Visibility = Visibility.Visible;

            gdJobCategoryShow.Visibility = Visibility.Collapsed;
            gdJobCategoryInput.Visibility = Visibility.Visible;

            tbProvince.DataContext = null;
            tbCity.DataContext = null;
            gdCityShow.Visibility = Visibility.Collapsed;
            gdCityInput.Visibility = Visibility.Visible;

            isEdit = false;
            tbjobName.Text = string.Empty;
            tbContent.setText("");
            tbJobCategory.DataContext = null;
            tbLocation.Text = string.Empty;
            cmbDegree.SelectedIndex = 0;
            cmbWorkingYear.SelectedIndex = 0;
            cmbCategory.SelectedIndex = 0;
            tbMinSalary.Text = string.Empty;
            tbMaxSalary.Text = string.Empty;
            ucGmap.InitialUCGamp("北京", "北京");
        }
        private void PrePublicJob()
        {
            mHttpPublishJob = new ViewModel.HttpPublishJob();
            mHttpPublishJob.userID = MagicGlobal.UserInfo.Id;
            mHttpPublishJob.companyID = MagicGlobal.UserInfo.CompanyId;
            mHttpPublishJob.jobType = 2;
            mHttpPublishJob.jobSource = 1;
            mHttpPublishJob.isHRAuth = MagicGlobal.UserInfo.IsHRAuth;
            mHttpPublishJob.isSimilarAuth = MagicGlobal.UserInfo.IsSimilarAuth;
            mHttpPublishJob.firstJobFunc = (tbJobCategory.DataContext as Val2).parent.parentCode;
            mHttpPublishJob.secondJobFunc = (tbJobCategory.DataContext as Val2).parentCode;
            mHttpPublishJob.thirdJobFunc = (tbJobCategory.DataContext as Val2).code;
            mHttpPublishJob.jobCity = (tbCity.DataContext as HttpCityCodes).code;
            mHttpPublishJob.jobDesc = tbContent.getText();
            mHttpPublishJob.jobName = tbjobName.Text;
            mHttpPublishJob.minDegree = (cmbDegree.SelectedItem as MappingItem).value;
            mHttpPublishJob.workCharact = (cmbCategory.SelectedItem as MappingItem).value;
            mHttpPublishJob.minExp = (cmbWorkingYear.SelectedItem as MappingItem).value;
            mHttpPublishJob.jobTemptation = GetTemptation();
            mHttpPublishJob.jobTemptationCustom = GetTemptationCustom();
            mHttpPublishJob.location = tbLocation.Text;
            mHttpPublishJob.jobMinSalary = Convert.ToInt32(tbMinSalary.Text);
            mHttpPublishJob.jobMaxSalary = Convert.ToInt32(tbMaxSalary.Text);
            mHttpPublishJob.status = 1;
            Point pt = this.ucGmap.GetMarkerLatLng();
            this.mHttpPublishJob.jobLocation = pt.Y.ToString() + "$$" + pt.X.ToString();
            ucPreView.gdDetial.DataContext = mHttpPublishJob;

        }
        private async Task PublicJob()
        {
            int code = await UnActivatePublishHelper.UnActivatePublish();
            if (code == 0)
                return;
            if (code != 200)
            {
                if (code == -127)
                {
                    WinValidateMessage pWinMessage = new WinValidateMessage("您的账号尚未认证，认证后才能发布更多职位哦");
                    pWinMessage.Owner = Window.GetWindow(this);
                    if ((bool)pWinMessage.ShowDialog())
                    {
                        if (actionAddV != null)
                        {
                            await actionAddV();
                        }
                    }
                    return;
                }
                else if (code == -126)
                {
                    WinMessageLink pWinErroTip = new WinMessageLink("贵公司今天已发布20个职位，请明天再来发布", "我知道了");
                    pWinErroTip.Owner = Window.GetWindow(this);
                    pWinErroTip.ShowDialog();
                    return;
                }
            }

            PrePublicJob();


            string jsonCheck = DAL.JsonHelper.JsonParamsToString(new string[] {"userID", "jobNameAbsolute", "jobCity", "location", "firstJobFunc", "secondJobFunc", "thirdJobFunc" },
                new string[] {MagicGlobal.UserInfo.Id.ToString(), mHttpPublishJob.jobName, mHttpPublishJob.jobCity, mHttpPublishJob.location, mHttpPublishJob.firstJobFunc, mHttpPublishJob.secondJobFunc, mHttpPublishJob.thirdJobFunc });

            string jsonResultCheck = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrCreatOpinion, MagicGlobal.UserInfo.Version, jsonCheck));
            BaseHttpModel modelCheck = DAL.JsonHelper.ToObject<BaseHttpModel>(jsonResultCheck);
            if (modelCheck != null)
            {
                if (modelCheck.code == -138)
                {
                    WinJobCheck pWinJobCheck = new WinJobCheck();
                    pWinJobCheck.Owner = Window.GetWindow(this);
                    pWinJobCheck.ShowDialog();
                    return;                                                                                     
                }
            }


            //发职位接口 带完成
            string jsonStr = DAL.JsonHelper.ToJsonString(this.mHttpPublishJob);

            string jsonResult = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrCreateJob, MagicGlobal.UserInfo.Version), jsonStr);
            MagicCube.ViewModel.BaseHttpModel<MagicCube.ViewModel.HttpPublishJob> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<MagicCube.ViewModel.HttpPublishJob>>(jsonResult);
            Console.WriteLine(jsonResult);
            busyCtrl.IsBusy = false;
            if (model != null)
            {
                if (model.code != 200)
                {
                    if (model.code == -127)
                    {
                        WinValidateMessage pWinMessage = new WinValidateMessage("您的账号尚未认证，认证后才能发布更多职位哦");
                        pWinMessage.Owner = Window.GetWindow(this);
                        if ((bool)pWinMessage.ShowDialog())
                        {
                            if (actionAddV != null)
                            {
                                await actionAddV();
                            }
                        }
                    }
                    else if (model.code == -126)
                    {
                        WinErroTip pWinErroTip = new WinErroTip("贵公司今天已发布20个职位，请明天再来发布");
                        pWinErroTip.Owner = Window.GetWindow(this);
                        pWinErroTip.ShowDialog();
                    }
                    else
                    {
                        DisappearShow disappear = new DisappearShow("发布失败", 1);
                        disappear.Owner = Window.GetWindow(this);
                        disappear.ShowDialog();
                    }

                }
                else
                {
                    iniPublish();
                    gdPublishResult.Visibility = Visibility.Visible;
                    if (actionUpdataCount != null)
                        actionUpdataCount();
                    svEdit.ScrollToTop();
                }
            }
            else
            {
                WinErroTip pWinErroTip = new WinErroTip(DAL.ConfUtil.netError);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();

            }
        }
        private async void PulishJob_Click(object sender, RoutedEventArgs e)
        {
            //if (await UnActivatePublishHelper.UnActivatePublish() )
            //    return;
            //发布职位编辑重新发布，不算计数。

           


            Common.TrackHelper2.TrackOperation("5.3.1.22.1", "clk");
            if (!checktbContentTel())
            {
                WinMessageLink winLink = new WinMessageLink("职位描述中请勿包含手机号、微信、QQ、邮箱等联系方式", "我知道了");
                winLink.Owner = Window.GetWindow(this);
                winLink.ShowDialog();
                return;
            }
            if (!checkItem())
            {
                DisappearShow disappear = new DisappearShow("检查填写", 1);
                disappear.Owner = disappear.Owner = Window.GetWindow(this); ;
                disappear.ShowDialog();
                return;
            }
            btnPublishJob.IsEnabled = false;
            if (!isEdit)
            {
                await PublicJob();
            }
            else
            {
                await EditJob();
            }
            MagicGlobal.isJobDetialEdit = true;
            btnPublishJob.IsEnabled = true;

        }
        public async Task iniEdit(long id, bool isFromClose = false)
        {
            mFromClose = isFromClose;
            iniPublishEditCommon();
            btnEditClear.Visibility = Visibility.Collapsed;
            btnPublishJob.Text = "保存修改";
            spTemp.Visibility = Visibility.Collapsed;
            btnTempJob.Visibility = Visibility.Collapsed;

            spJobNameE.Visibility = Visibility.Visible;
            spJobNameP.Visibility = Visibility.Collapsed;
            spJobCategoryE.Visibility = Visibility.Visible;
            spJobCategoryP.Visibility = Visibility.Collapsed;
            spJobCityE.Visibility = Visibility.Visible;
            spJobCityP.Visibility = Visibility.Collapsed;


            await SetSelfEdit();
                                 
            gdJobCategoryShow.Visibility = Visibility.Visible;
            gdJobCategoryInput.Visibility = Visibility.Collapsed;

            gdCityShow.Visibility = Visibility.Visible;
            gdCityInput.Visibility = Visibility.Collapsed;

            isEdit = true;
            string propertys = ModelTools.SetHttpPropertys<HttpEditJob>();
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "jobID", "properties" }, new string[] { id.ToString(), propertys });
            busyCtrl.IsBusy = true;
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobDetail, MagicGlobal.UserInfo.Version, std));

            BaseHttpModel<MagicCube.ViewModel.HttpEditJob> model = DAL.JsonHelper.ToObject<BaseHttpModel<MagicCube.ViewModel.HttpEditJob>>(jsonResult);
            busyCtrl.IsBusy = false;
            if (model.code != 200)
            {
                return;
            }
            mHttpEditJob = model.data;

            tbjobName.Text = mHttpEditJob.jobName;
            tbJobNameE.Text = mHttpEditJob.jobName;
            tbContent.setText(mHttpEditJob.jobDesc);
            if (mHttpEditJob.minDegree == null)
                mHttpEditJob.minDegree = string.Empty;
            if (mHttpEditJob.minExp == null)
                mHttpEditJob.minExp = string.Empty;
            cmbDegree.SelectedItem = (cmbDegree.ItemsSource as List<MappingItem>).FirstOrDefault(x => x.value == mHttpEditJob.minDegree);
            cmbWorkingYear.SelectedItem = (cmbWorkingYear.ItemsSource as List<MappingItem>).FirstOrDefault(x => x.value == mHttpEditJob.minExp);
            cmbCategory.SelectedItem = (cmbCategory.ItemsSource as List<MappingItem>).FirstOrDefault(x => x.value == mHttpEditJob.workCharact);
            if (findNamebyCode(mHttpEditJob.thirdJobFunc) != null)
            {
                tbJobCategory.DataContext = findNamebyCode(mHttpEditJob.thirdJobFunc);
                tbJobCategoryE.DataContext = findNamebyCode(mHttpEditJob.thirdJobFunc);
            }               
            else
            {
                spJobCategoryP.Visibility = Visibility.Visible;
                spJobCategoryE.Visibility = Visibility.Collapsed;
                gdJobCategoryShow.Visibility = Visibility.Visible;
                gdJobCategoryInput.Visibility = Visibility.Collapsed;
                tbJobCategory.DataContext = null;
            }
            tbMinSalary.Text = mHttpEditJob.jobMinSalary.ToString();
            tbMaxSalary.Text = mHttpEditJob.jobMaxSalary.ToString();
            foreach (string istr in mHttpEditJob.jobTemptationList)
            {
                tagChoose.Add(istr);
                tagsModel ptagsModel = tagList.FirstOrDefault(x => x.Tag == istr);
                if (ptagsModel != null)
                    ptagsModel.IsChecked = true;
            }
            tbLocation.Text = mHttpEditJob.location;
            if (!string.IsNullOrWhiteSpace(mHttpEditJob.jobCity))
            {
                HttpProvinceCityCodes province = provinceList.Where(x => (x.city.Where(y => y.code == mHttpEditJob.jobCity).Count() > 0)).FirstOrDefault();
                HttpCityCodes city  = cityList.Where(x => x.code == mHttpEditJob.jobCity).FirstOrDefault();

                tbProvince.DataContext = province;
                tbCity.DataContext = city;
                if (province!=null)
                {
                    tbJobCityE.Text = province.name;
                }
                if(city != null)
                {
                    tbJobCityE.Text +="-"+ city.name;
                }
                   
            }
            if (tbCity.DataContext as HttpCityCodes != null)
            {
                if (!string.IsNullOrWhiteSpace(mHttpEditJob.jobLocation))
                {
                    string[] tempList = this.mHttpEditJob.jobLocation.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempList.Length == 2)
                    {
                        double lat = Convert.ToDouble(tempList[1]);
                        double lng = Convert.ToDouble(tempList[0]);
                        this.ucGmap.InitialUCGamp(mHttpEditJob.location, (tbCity.DataContext as HttpCityCodes).name, lat, lng);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(mHttpEditJob.location))
                        {
                            this.ucGmap.InitialUCGamp(mHttpEditJob.location, (tbCity.DataContext as HttpCityCodes).name);
                        }
                        else
                        {
                            this.ucGmap.InitialUCGamp((tbCity.DataContext as HttpCityCodes).name, (tbCity.DataContext as HttpCityCodes).name);
                        }
                    }
                }
                else
                {


                    if (!string.IsNullOrWhiteSpace(mHttpEditJob.location))
                    {

                        this.ucGmap.InitialUCGamp(mHttpEditJob.location, (tbCity.DataContext as HttpCityCodes).name);

                    }
                    else
                    {
                        this.ucGmap.InitialUCGamp((tbCity.DataContext as HttpCityCodes).name, (tbCity.DataContext as HttpCityCodes).name);
                    }

                }
            }
            else
            {
                this.ucGmap.InitialUCGamp("北京", "北京");
                gdCityShow.Visibility = Visibility.Collapsed;
                gdCityInput.Visibility = Visibility.Visible;
            }
        }
        private void PreEditJob()
        {
            mHttpEditJob.firstJobFunc = (tbJobCategory.DataContext as Val2).parent.parentCode;
            mHttpEditJob.secondJobFunc = (tbJobCategory.DataContext as Val2).parentCode;
            mHttpEditJob.thirdJobFunc = (tbJobCategory.DataContext as Val2).code;
            mHttpEditJob.jobCity = (tbCity.DataContext as HttpCityCodes).code;
            mHttpEditJob.jobDesc = tbContent.getText();
            mHttpEditJob.jobName = tbjobName.Text;
            mHttpEditJob.minDegree = (cmbDegree.SelectedItem as MappingItem).value;
            mHttpEditJob.workCharact = (cmbCategory.SelectedItem as MappingItem).value;
            mHttpEditJob.minExp = (cmbWorkingYear.SelectedItem as MappingItem).value;
            mHttpEditJob.jobTemptation = GetTemptation();
            mHttpEditJob.jobTemptationCustom = GetTemptationCustom();
            mHttpEditJob.location = tbLocation.Text;
            mHttpEditJob.jobMinSalary = Convert.ToInt32(tbMinSalary.Text);
            mHttpEditJob.jobMaxSalary = Convert.ToInt32(tbMaxSalary.Text);
            mHttpEditJob.currentStatus = mHttpEditJob.status;
            mHttpEditJob.status = 1;
            Point pt = this.ucGmap.GetMarkerLatLng();
            this.mHttpEditJob.jobLocation = pt.Y.ToString() + "$$" + pt.X.ToString();
            ucPreView.gdDetial.DataContext = mHttpEditJob;
        }
        private async Task EditJob()
        {
            if (mHttpEditJob.status == -1)
            {
                int code = await UnActivatePublishHelper.UnActivatePublish();
                if (code == 0)
                    return;
                if (code != 200)
                {
                    if (code == -127)
                    {
                        WinValidateMessage pWinMessage = new WinValidateMessage("您的账号尚未认证，认证后才能发布更多职位哦");
                        pWinMessage.Owner = Window.GetWindow(this);
                        if ((bool)pWinMessage.ShowDialog())
                        {
                            if (actionAddV != null)
                            {
                                await actionAddV();
                            }
                        }
                        return;
                    }
                    else if (code == -126)
                    {
                        WinMessageLink pWinErroTip = new WinMessageLink("贵公司今天已发布20个职位，请明天再来发布", "我知道了");
                        pWinErroTip.Owner = Window.GetWindow(this);
                        pWinErroTip.ShowDialog();
                        return;
                    }
                }
            }
            PreEditJob();
            string json = DAL.JsonHelper.ToJsonString(mHttpEditJob);
            string jsonCheck = DAL.JsonHelper.JsonParamsToString(new string[] { "userID","jobID", "jobNameAbsolute", "jobCity", "location", "firstJobFunc", "secondJobFunc", "thirdJobFunc" },
               new string[] { MagicGlobal.UserInfo.Id.ToString(), mHttpEditJob.jobID.ToString(), mHttpEditJob.jobName, mHttpEditJob.jobCity, mHttpEditJob.location, mHttpEditJob.firstJobFunc, mHttpEditJob.secondJobFunc, mHttpEditJob.thirdJobFunc });

            string jsonResultCheck = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrCreatOpinion, MagicGlobal.UserInfo.Version, jsonCheck));
            BaseHttpModel modelCheck = DAL.JsonHelper.ToObject<BaseHttpModel>(jsonResultCheck);
            if (modelCheck != null)
            {
                if (modelCheck.code == -138)
                {
                    WinJobCheck pWinJobCheck = new WinJobCheck();
                    pWinJobCheck.Owner = Window.GetWindow(this);
                    pWinJobCheck.ShowDialog();
                    return;
                }
            }

            busyCtrl.IsBusy = true;

            //编辑职位接口待完善
            string std = DAL.JsonHelper.ToJsonString(this.mHttpEditJob);
            string jsonResult = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrUpdateJob, MagicGlobal.UserInfo.Version), std);
            MagicCube.ViewModel.BaseHttpModel<MagicCube.ViewModel.HttpEditJob> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<MagicCube.ViewModel.HttpEditJob>>(jsonResult);
            Console.WriteLine(jsonResult);
            busyCtrl.IsBusy = false;
            if (model != null)
            {
                if (model.code != 200)
                {

                    DisappearShow disappear = new DisappearShow("修改失败", 1);
                    disappear.Owner = Window.GetWindow(this);
                    disappear.ShowDialog();

                }
                else
                {
                    iniPublish();
                    gdPublishResult.Visibility = Visibility.Visible;
                    if (actionUpdataCount != null)
                        actionUpdataCount();
                    svEdit.ScrollToTop();
                }
            }
            else
            {
                WinErroTip pWinErroTip = new WinErroTip(DAL.ConfUtil.netError);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();

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

        #region 合法检测
        private void tbMaxSalary_LostFocus(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.11.1", "clk");
            if (checktbSalary())
            {
                tbMinSalary.BorderBrush = tbMaxSalary.BorderBrush = normalColor;
                spSalary.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbMinSalary.BorderBrush = tbMaxSalary.BorderBrush = errColor;
                spSalary.Visibility = Visibility.Visible;
            }
        }
        private void tbMinSalary_LostFocus(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.10.1", "clk");
            if (string.IsNullOrEmpty(tbMaxSalary.Text))
                return;
            if (checktbSalary())
            {
                tbMinSalary.BorderBrush = tbMaxSalary.BorderBrush = normalColor;
                spSalary.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbMinSalary.BorderBrush = tbMaxSalary.BorderBrush = errColor;
                spSalary.Visibility = Visibility.Visible;
            }
        }
        private void tbjobName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbjobName.Text))
            {
                tbjobName.BorderBrush = normalColor;
                spjobName.Visibility = Visibility.Collapsed;
            }

        }
        private async void tbLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.20.1", "clk");
            if (!string.IsNullOrWhiteSpace(tbLocation.Text))
            {
                tbLocation.BorderBrush = normalColor;
                spLocation.Visibility = Visibility.Collapsed;
                if (tbCity.DataContext as HttpCityCodes != null)
                {
                    await ucGmap.SetAddress(tbLocation.Text, (tbCity.DataContext as HttpCityCodes).name);
                }

            }
        }
        private void Salary_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (checktbSalary())
            {
                tbMinSalary.BorderBrush = tbMaxSalary.BorderBrush = normalColor;
                spSalary.Visibility = Visibility.Collapsed;
            }
        }
        private void tbContent_TextChange(bool obj)
        {
            if (obj)
            {
                spContent.Visibility = Visibility.Collapsed;
            }
            else
            {
                spContent.Visibility = Visibility.Visible;
            }

            if (checktbContent())
            {
                bdContent.BorderBrush = normalColor;
                spContentV.Visibility = Visibility.Collapsed;
            }

        }
        private bool checktbContent()
        {
            bool isCheck = true;
            if (tbContent.getStringText().Trim() == string.Empty)
            {
                isCheck = false;
            }
            else
            {
                if (tbContent.getLength() > 1000 || tbContent.getLength() < 20)
                {
                    isCheck = false;
                }
            }

            string temp = tbContent.getStringText();       
            return isCheck;
        }
        private bool checktbContentTel()
        {
            bool isCheck = true;
            string temp = tbContent.getStringText();
            if (CommonValidationMethod.IsContainNum(temp))
            {
                isCheck = false;
            }
            if (CommonValidationMethod.IsContainEmail(temp))
            {
                isCheck = false;
            }
            return isCheck;
        }

        private bool checktbjobTemptation()
        {
            bool isCheck = true;
            if (tagChoose.Count == 0)
            {
                isCheck = false;
            }
            return isCheck;
        }

        private bool checktbSalary()
        {
            //首先判断薪酬的输入是否为1-100的正整数，不是的出现提示信息请输入整数月薪，如6；
            //其次进行校验最高月薪的范围是否为最低月薪的x-2x，不是的出现错误提示：最高月薪需大于最低月薪；最高月薪不能高于最低月薪的2倍
            //最后检验最高月薪是否高于100，超过的出现提示：最高月薪不能高于100
            bool isCheck = true;
            if (!IsNumeric(tbMinSalary.Text) || !IsNumeric(tbMaxSalary.Text))
            {
                isCheck = false;
                tbSalatyError.Text = "请输入整数月薪，如6";
                return isCheck;
            }
            int iMinSalary = Convert.ToInt32(tbMinSalary.Text);
            int iMaxSalary = Convert.ToInt32(tbMaxSalary.Text);
            if (iMinSalary == 0 || iMaxSalary == 0)
            {
                isCheck = false;
                tbSalatyError.Text = "请输入整数月薪，如6";
                return isCheck;
            }
            if (iMinSalary > iMaxSalary)
            {
                isCheck = false;
                tbSalatyError.Text = "最高月薪需大于最低月薪";
                return isCheck;
            }
            if (iMaxSalary > iMinSalary * 2)
            {
                isCheck = false;
                tbSalatyError.Text = "最高月薪不能高于最低月薪的2倍";
                return isCheck;
            }
            if (iMaxSalary > 100)
            {
                isCheck = false;
                tbSalatyError.Text = "最高月薪不能高于100k";
                return isCheck;
            }
            return isCheck;
        }
        private bool checkItem()
        {
            bool isCheck = true;
            if (string.IsNullOrWhiteSpace(tbJobCategory.Text))
            {
                bdJobCategory.BorderBrush = errColor;
                spJobCategory.Visibility = Visibility.Visible;
                isCheck = false;
                svEdit.PageUp();
            }
            if (string.IsNullOrWhiteSpace(tbjobName.Text))
            {
                tbjobName.BorderBrush = errColor;
                spjobName.Visibility = Visibility.Visible;
                isCheck = false;
                svEdit.PageUp();
            }
            if (string.IsNullOrWhiteSpace(tbLocation.Text))
            {
                tbLocation.BorderBrush = errColor;
                spLocation.Visibility = Visibility.Visible;
                isCheck = false;
                svEdit.PageUp();
            }
            if (!checktbSalary())
            {
                tbMinSalary.BorderBrush = tbMaxSalary.BorderBrush = errColor;
                spSalary.Visibility = Visibility.Visible;
                isCheck = false;
                svEdit.PageUp();
            }

            if (!checktbContent())
            {
                bdContent.BorderBrush = errColor;
                spContentV.Visibility = Visibility.Visible;
                isCheck = false;
            }
           
            if (!checktbjobTemptation())
            {
                spjobTemptation.Visibility = Visibility.Visible;
                isCheck = false;
            }
            if (tbProvince.DataContext as HttpProvinceCityCodes == null || tbCity.DataContext as HttpCityCodes == null)
            {
                bdCity.BorderBrush = errColor;
                spCity.Visibility = Visibility.Visible;
                isCheck = false;
            }
            return isCheck;
        }
        bool IsNumeric(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            return reg1.IsMatch(str);
        }
        #endregion


        #region 职能选择
        private async void tbJobCategoryInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            await TaskEx.Delay(200);
            if (!txt.Focusable)
                return;

            if (string.IsNullOrWhiteSpace(tbJobCategoryInput.Text))
            {
                PopThreeCode.IsOpen = false;
                tempList.Clear();
            }
            else
            {
                PopThreeCode.IsOpen = true;
                string tempstr = tbJobCategoryInput.Text;
                tempList.Clear();
                foreach (Val2 iVal in m_HttpJobThreeList)
                {
                    if (iVal.name.Contains(tempstr.Trim()))
                    {
                        tempList.Add(iVal);
                    }
                }

                if (popThreeCodeList.Items.Count > 0)
                {
                    popThreeCodeList.Visibility = Visibility.Visible;
                    popTip.Visibility = Visibility.Collapsed;
                }
                else
                {
                    popThreeCodeList.Visibility = Visibility.Collapsed;
                    popTip.Visibility = Visibility.Visible;
                }

            }

        }
        private void gdThreeCode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PopThreeCode.IsOpen = false;
            tbJobCategory.DataContext = ((sender as Grid).DataContext as Val2);
            bdJobCategory.BorderBrush = normalColor;
            gdJobChoose.Visibility = Visibility.Collapsed;
            spJobCategory.Visibility = Visibility.Collapsed;
            gdJobCategoryShow.Visibility = Visibility.Visible;
            gdJobCategoryInput.Visibility = Visibility.Collapsed;
        }
        private void tbJobCategoryDelete_Click(object sender, RoutedEventArgs e)
        {
            tbJobCategory.DataContext = null;
            tbJobCategoryInput.Text = string.Empty;
            gdJobCategoryShow.Visibility = Visibility.Collapsed;
            gdJobCategoryInput.Visibility = Visibility.Visible;
        }
        private void tbJobCategoryInput_LostFocus(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.8.1", "clk");
            if (tbJobCategory.DataContext == null)
                tbJobCategoryInput.Text = string.Empty;

        }
        private void tbJobCategory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            gdJobChoose.Visibility = Visibility.Visible;
        }
        private void btnOpenSel_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.9.1", "clk");
            gdJobChoose.Visibility = Visibility.Visible;
            Val2 pVal2 = tbJobCategory.DataContext as Val2;
            if (pVal2 != null)
            {
                foreach (HttpJobTreeThree iHttpJobTreeThree in m_HttpJobTreeThree)
                {
                    if (iHttpJobTreeThree.code == pVal2.parent.parent.code)
                    {
                        iHttpJobTreeThree.IsCheck = true;
                    }
                    else
                    {
                        iHttpJobTreeThree.IsCheck = false;
                    }
                    foreach (Val iVal in iHttpJobTreeThree.val)
                    {
                        foreach (Val2 iVal2 in iVal.val)
                        {
                            if (iVal2.code == pVal2.code)
                            {
                                iVal2.isChoose = true;
                            }
                            else
                            {
                                iVal2.isChoose = false;
                            }
                        }
                    }
                }
            }
            else
            {

                foreach (HttpJobTreeThree iHttpJobTreeThree in m_HttpJobTreeThree)
                {
                    foreach (Val iVal in iHttpJobTreeThree.val)
                    {
                        foreach (Val2 iVal2 in iVal.val)
                        {
                            iVal2.isChoose = false;
                        }
                    }
                }
            }
        }
        private void RadioButton_MouseEnter(object sender, MouseEventArgs e)
        {

            icJobContent.ItemsSource = ((sender as RadioButton).DataContext as HttpJobTreeThree).val;
            ((sender as RadioButton).DataContext as HttpJobTreeThree).IsCheck = true;
            svJobContent.ScrollToTop();
            foreach (HttpJobTreeThree iHttpJobTreeThree in m_HttpJobTreeThree)
            {
                if (iHttpJobTreeThree != ((sender as RadioButton).DataContext as HttpJobTreeThree))
                {
                    iHttpJobTreeThree.IsCheck = false;
                }
            }
        }
        private void BtnChooseClose_Click(object sender, RoutedEventArgs e)
        {
            gdJobChoose.Visibility = Visibility.Collapsed;
        }
        private void JobCategorySelect_Click(object sender, RoutedEventArgs e)
        {
            tbJobCategory.DataContext = ((sender as CheckBox).DataContext as Val2);
            bdJobCategory.BorderBrush = normalColor;
            gdJobChoose.Visibility = Visibility.Collapsed;
            spJobCategory.Visibility = Visibility.Collapsed;
            gdJobCategoryShow.Visibility = Visibility.Visible;
            gdJobCategoryInput.Visibility = Visibility.Collapsed;

        }
        #endregion


        #region 职位诱惑标签
        private void TagsCheck_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox chk = sender as System.Windows.Controls.CheckBox;
            tagsModel tag = chk.DataContext as tagsModel;
            if (tag.IsSelf == Visibility.Visible)
            {
                TrackHelper2.TrackOperation("5.3.1.16.1", "clk");
            }
            else
            {
                TrackHelper2.TrackOperation("5.3.1.17.1", "clk");
            }
            if (tagChoose.Count >= 5)
            {
                chk.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink("亲，职位诱惑标签最多可选择5个哦", "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                return;
            }
            foreach (var item in tagChoose)
            {
                if (item == tag.Tag)
                    return;
            }
            tagChoose.Add(tag.Tag);
            spjobTemptation.Visibility = Visibility.Collapsed;
        }

        private void DeleteTags_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            string temp = btn.DataContext as string;
            tagChoose.Remove(temp);
            UpdateTagsListShow(temp, false);
        }

        private async void BtnAddSelfTag_Click(object sender, RoutedEventArgs e)
        {
            string temp = this.txtAddTag.Text;
            temp = temp.Trim();
            if (string.IsNullOrWhiteSpace(temp))
                return;
            temp = temp.Replace(" ", "");
            if (tagList.Where(x => x.Tag == temp).Count() > 0)
            {
                WinConfirmTip win = new WinConfirmTip("标签已存在");
                win.Owner = Window.GetWindow(this);
                win.ShowDialog();
                return;
            }
            if (tagChoose.Count >= 5)
            {
                WinConfirmTip win = new WinConfirmTip("诱惑标签最多只能选择5个");
                win.Owner = Window.GetWindow(this);
                win.ShowDialog();
                return;
            }
            if (tagList.Count >= 38)
            {
                WinConfirmTip win = new WinConfirmTip("亲，您已添加30个自定义标签，先去把不常用的删除再添加吧");
                win.Owner = Window.GetWindow(this);
                win.ShowDialog();
                return;
            }
            if (await CreatTag(temp))
            {
                tagChoose.Add(temp);
                this.txtAddTag.Clear();
                this.chkAddTags.IsChecked = false;
                tagList.Add(new tagsModel { IsChecked = true, Tag = temp, IsSelf = Visibility });
            }
        }

        private void UpdateTagsListShow(string tag, bool isCheck)
        {
            foreach (var item in tagList)
            {
                if (item.Tag == tag)
                {
                    item.IsChecked = isCheck;
                    return;
                }
            }
        }

        private async void DeleteSelfTags_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            tagsModel ptagsModel = btn.DataContext as tagsModel;
            if (ptagsModel != null)
            {
                if (await DeleteTag(ptagsModel.Tag))
                {
                    tagList.Remove(ptagsModel);
                }

            }

        }

        private string GetTemptation()
        {
            string pStr = string.Empty;
            foreach (var item in tagChoose)
            {
                if (systemTemptation.Contains(item))
                    pStr += item + "$$";
            }
            pStr = pStr.TrimEnd(new char[] { '$' });
            return pStr;
        }
        private string GetTemptationCustom()
        {
            string pStr = string.Empty;
            foreach (var item in tagChoose)
            {
                if (!systemTemptation.Contains(item))
                    pStr += item + "$$";
            }
            pStr = pStr.TrimEnd(new char[] { '$' });
            return pStr;
        }

        private async Task<bool> CreatTag(string tag)
        {
            List<MagicCube.ViewModel.HttpTag> lst = new List<HttpTag>();
            MagicCube.ViewModel.HttpTag pTag = new ViewModel.HttpTag();
            pTag.text = tag;
            pTag.ownerID = MagicGlobal.UserInfo.Id;
            pTag.group = "userDefinedJob";
            lst.Add(pTag);
            string jsonStr = DAL.JsonHelper.ToJsonString(lst);
            busyCtrl.IsBusy = true;
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrTagCreate, MagicGlobal.UserInfo.Version, jsonStr));
            busyCtrl.IsBusy = false;
            MagicCube.ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel>(jsonResult);
            if (model != null)
            {
                if (model.code == 200)
                {
                    return true;
                }
            }

            return false;
        }
        private async Task<bool> DeleteTag(string tag)
        {
            List<MagicCube.ViewModel.HttpTag> lst = new List<HttpTag>();

            MagicCube.ViewModel.HttpTag pTag = new ViewModel.HttpTag();
            pTag.text = tag;
            pTag.ownerID = MagicGlobal.UserInfo.Id;
            pTag.group = "userDefinedJob";
            pTag.isDel = "1";
            lst.Add(pTag);
            string jsonStr = DAL.JsonHelper.ToJsonString(lst);
            busyCtrl.IsBusy = true;
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrTagCreate, MagicGlobal.UserInfo.Version, jsonStr));
            busyCtrl.IsBusy = false;
            MagicCube.ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel>(jsonResult);
            if (model != null)
            {
                if (model.code == 200)
                {
                    return true;
                }
            }

            return false;
        }
        private void SetSelfTag()
        {
            MagicCube.ViewModel.HttpTag pTag = new ViewModel.HttpTag();
            pTag.ownerID = MagicGlobal.UserInfo.Id;
            pTag.group = "attractionJob$$userDefinedJob";
            string jsonStr = DAL.JsonHelper.ToJsonString(pTag);
            Action action = new Action(() =>
            {
                string jsonResult = DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrTagList, MagicGlobal.UserInfo.Version, jsonStr));
                MagicCube.ViewModel.BaseHttpModel<MagicCube.ViewModel.HttpTagAttractionJob> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<MagicCube.ViewModel.HttpTagAttractionJob>>(jsonResult);
                this.Dispatcher.BeginInvoke(new Action(() =>
                      {
                          if (model != null)
                          {
                              if (model.code == 200)
                              {
                                  tagList.Clear();
                                  systemTemptation.Clear();
                                  foreach (MagicCube.ViewModel.HttpTag itag in model.data.attractionJob)
                                  {
                                      tagList.Add(new tagsModel() { IsChecked = false, IsSelf = Visibility.Collapsed, Tag = itag.text });
                                      systemTemptation.Add(itag.text);
                                  }

                                  if (model.data.userDefinedJob != null)
                                  {
                                      foreach (MagicCube.ViewModel.HttpTag itag in model.data.userDefinedJob)
                                      {
                                          tagList.Add(new tagsModel() { IsChecked = false, IsSelf = Visibility.Visible, Tag = itag.text });

                                      }
                                  }
                              }
                          }
                      }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            });
            action.BeginInvoke(null, null);
        }
        private async Task SetSelfEdit()
        {
            MagicCube.ViewModel.HttpTag pTag = new ViewModel.HttpTag();
            pTag.ownerID = MagicGlobal.UserInfo.Id;
            pTag.group = "attractionJob$$userDefinedJob";
            string jsonStr = DAL.JsonHelper.ToJsonString(pTag);
            busyCtrl.IsBusy = true;
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrTagList, MagicGlobal.UserInfo.Version, jsonStr));
            MagicCube.ViewModel.BaseHttpModel<MagicCube.ViewModel.HttpTagAttractionJob> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<MagicCube.ViewModel.HttpTagAttractionJob>>(jsonResult);
            busyCtrl.IsBusy = false;
            if (model != null)
            {
                if (model.code == 200)
                {
                    tagList.Clear();
                    systemTemptation.Clear();
                    foreach (MagicCube.ViewModel.HttpTag itag in model.data.attractionJob)
                    {
                        tagList.Add(new tagsModel() { IsChecked = false, IsSelf = Visibility.Collapsed, Tag = itag.text });
                        systemTemptation.Add(itag.text);
                    }

                    if (model.data.userDefinedJob != null)
                    {
                        foreach (MagicCube.ViewModel.HttpTag itag in model.data.userDefinedJob)
                        {
                            tagList.Add(new tagsModel() { IsChecked = false, IsSelf = Visibility.Visible, Tag = itag.text });

                        }
                    }
                }
            }

        }
        #endregion

        private void tbJobName_LostFocus(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.7.1", "clk");
        }

        private void cmbCategory_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void cmbDegree_LostFocus(object sender, RoutedEventArgs e)
        {

        }


        private void cmbWorkingYear_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void tbContent_LostFocus(bool obj)
        {
            if (!obj)
            {
                TrackHelper2.TrackOperation("5.3.1.15.1", "clk");
            }
        }

        private void ucGmap_LostFocus(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.21.1", "clk");
        }


        public void iniProvinceCity()
        {

            JsonCityCodeMode jsonCityModel = CityCodeHelper.GetCityCode();
            List<CityCodeItem> ProvinceJsonList = new List<CityCodeItem>();
            List<CityCodeItem> CityJsonList = new List<CityCodeItem>();
            cityList = new List<HttpCityCodes>();
            foreach (var item in jsonCityModel.CityCodes.CityCode)
            {
                if (item.value.Length == 4)
                {
                    ProvinceJsonList.Add(item);
                }
                if (item.value.Length == 6)
                {
                    CityJsonList.Add(item);
                    cityList.Add(new HttpCityCodes() { name = item.simpleName, code = item.value });
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
                        cityList.Add(new HttpCityCodes() { name = item.simpleName, code = item.value });
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
                        cityList.Add(new HttpCityCodes() { name = item.simpleName, code = item.value });
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
                        cityList.Add(new HttpCityCodes() { name = item.simpleName, code = item.value });
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
                        cityList.Add(new HttpCityCodes() { name = item.simpleName, code = item.value });
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
                        cityList.Add(new HttpCityCodes() { name = item.simpleName, code = item.value });
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
        }


        private async Task ProvinceCityCallback(bool obj)
        {
            gdProvinceCity.Visibility = Visibility.Collapsed;
            if (obj)
            {
                if (ucProvinceCity.OutSelectCityList.Count > 0)
                {
                    this.tbCity.DataContext = ucProvinceCity.OutSelectCityList[0];
                    this.tbProvince.DataContext = ucProvinceCity.selectedProvince;                   
                    gdCityShow.Visibility = Visibility.Visible;
                    gdCityInput.Visibility = Visibility.Collapsed;
                    bdCity.BorderBrush = normalColor;
                    spCity.Visibility = Visibility.Collapsed;
                    await this.ucGmap.SetAddress(ucProvinceCity.OutSelectCityList[0].name, ucProvinceCity.OutSelectCityList[0].name);
                }
                else
                {
                    this.tbCity.DataContext = null;
                    this.tbProvince.DataContext = null;
                    gdCityInput.Visibility = Visibility.Visible;
                    gdCityShow.Visibility = Visibility.Collapsed;
                }

            }

        }


        private void City_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gdProvinceCity.Visibility = Visibility.Visible;
            ucProvinceCity.selectedProvince = tbProvince.DataContext as HttpProvinceCityCodes;
            if ((tbProvince.DataContext as HttpProvinceCityCodes) != null)
            {
                Collection<HttpCityCodes> pHttpCityCodes = new Collection<HttpCityCodes>();
                pHttpCityCodes.Add(tbCity.DataContext as HttpCityCodes);
                ucProvinceCity.SetSelectedList(pHttpCityCodes);

            }
            else
            {
                Collection<HttpCityCodes> pHttpCityCodes = new Collection<HttpCityCodes>();
                ucProvinceCity.SetSelectedList(pHttpCityCodes);
            }
        }

        private void cmbCategory_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.12.1", "clk");
        }

        private void cmbDegree_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.13.1", "clk");
        }

        private void cmbWorkingYear_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.1.14.1", "clk");
        }

        #region 职位模版

        private async void TempJob_Click(object sender, RoutedEventArgs e)
        {
            WinJobTemplate pWinJobTemplate = new WinJobTemplate(Window.GetWindow(this));
            string tempName = string.Empty;
            if ((tbJobCategory.DataContext as Val2) != null)
            {
                tempName = pWinJobTemplate.tbTempName.Text = (tbJobCategory.DataContext as Val2).name;
            }
            if (pWinJobTemplate.ShowDialog()==true)
            {
                tempName = pWinJobTemplate.tbTempName.Text;
                string jsonList = DAL.JsonHelper.JsonParamsToString(new string[] { "jobTemplateOwnerID", "properties" },
                        new string[] { MagicGlobal.UserInfo.Id.ToString(), "jobTemplateName$$jobTemplateDetail$$jobTemplateID" });

                string jsonResultList = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrTemplateGetList, MagicGlobal.UserInfo.Version), jsonList);
                BaseHttpModel<ObservableCollection<HttpJobTemplate>> modelList = DAL.JsonHelper.ToObject<BaseHttpModel<ObservableCollection<HttpJobTemplate>>>(jsonResultList);

                for(int i = 0;i< modelList.data.Count;i++)
                {
                    if (i >= 9)
                    {
                        string jsonDelete = DAL.JsonHelper.JsonParamsToString(new string[] { "jobTemplateID" },
                                            new string[] { modelList.data[i].jobTemplateID });
                        await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrTemplateDelete, MagicGlobal.UserInfo.Version), jsonDelete);
                        modelList.data.RemoveAt(i);
                    }
                }
                string newFullPath = tempName;
                if (modelList.data.Where(x=>x.jobTemplateName == tempName).Count()>0)
                {
                    int counter = 1;
                    do
                    {
                        newFullPath = string.Format("{0}({1})", tempName, counter);
               
                        counter++;
                    } while (modelList.data.Where(x => x.jobTemplateName == newFullPath).Count() > 0);
                }

                HttpJobTemplateDetial model = new HttpJobTemplateDetial();

                model.jobName = tbjobName.Text;
                model.minDegree = (cmbDegree.SelectedItem as MappingItem).value;
                model.minExp = (cmbWorkingYear.SelectedItem as MappingItem).value;
                model.workCharact = (cmbCategory.SelectedItem as MappingItem).value;

                if((tbJobCategory.DataContext as Val2 )!=null)
                {
                    model.thirdJobFunc = (tbJobCategory.DataContext as Val2).code;
                }
                else
                {
                    model.thirdJobFunc = "";
                }
                model.jobMinSalary = tbMinSalary.Text;
                model.jobMaxSalary = tbMaxSalary.Text;
                model.jobTemptation = GetTemptation();
                model.jobTemptationCustom = GetTemptationCustom();
                model.location = tbLocation.Text;
                model.jobDesc = tbContent.getText();
                Point pt = this.ucGmap.GetMarkerLatLng();
                model.jobLocation = pt.Y.ToString() + "$$" + pt.X.ToString();
                if ((tbCity.DataContext as HttpCityCodes) != null)
                {
                    model.jobCity = (tbCity.DataContext as HttpCityCodes).code;
                }

                string json = DAL.JsonHelper.JsonParamsToString(new string[] { "jobTemplateOwnerID", "jobTemplateName", "jobTemplateDetail" },
                    new string[] { MagicGlobal.UserInfo.Id.ToString(), newFullPath, DAL.JsonHelper.ToJsonString(model)});

                string jsonResult = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrTemplateInsert, MagicGlobal.UserInfo.Version), json);
                BaseHttpModel model2 = DAL.JsonHelper.ToObject<BaseHttpModel>(jsonResult);
            }
        }

        private async void Temp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PopTemplate.IsOpen = true;
            
            string json = DAL.JsonHelper.JsonParamsToString(new string[] { "jobTemplateOwnerID", "properties" },
               new string[] { MagicGlobal.UserInfo.Id.ToString(), "jobTemplateName$$jobTemplateDetail$$jobTemplateID" });

            string jsonResult = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrTemplateGetList, MagicGlobal.UserInfo.Version), json);
            BaseHttpModel<ObservableCollection<HttpJobTemplate>> model = DAL.JsonHelper.ToObject<BaseHttpModel<ObservableCollection<HttpJobTemplate>>>(jsonResult);
            if (model != null)
            {
                if (model.code == 200)
                {
                    popTemplateList.ItemsSource = model.data;
                    if(popTemplateList.Items.Count>0)
                    {
                        popTemplateList.Visibility = Visibility.Visible;
                        popTemplateTip.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        popTemplateList.Visibility = Visibility.Collapsed;
                        popTemplateTip.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private async void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            if(actionReturn!=null)
            {
                await actionReturn(mFromClose);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            HttpJobTemplate pHttpJobTemplate = (sender as Button).DataContext as HttpJobTemplate;
            ObservableCollection<HttpJobTemplate> ptemp = popTemplateList.ItemsSource as ObservableCollection<HttpJobTemplate>;
            ptemp.Remove(pHttpJobTemplate);

            string json = DAL.JsonHelper.JsonParamsToString(new string[] { "jobTemplateID" },
              new string[] { pHttpJobTemplate.jobTemplateID });

            string jsonResult = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrTemplateDelete, MagicGlobal.UserInfo.Version), json);
            BaseHttpModel model = DAL.JsonHelper.ToObject<BaseHttpModel>(jsonResult);

        }

        private void gdTemplate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HttpJobTemplate pHttpJobTemplate = (sender as Grid).DataContext as HttpJobTemplate;
            HttpJobTemplateDetial model = DAL.JsonHelper.ToObject<HttpJobTemplateDetial>(pHttpJobTemplate.jobTemplateDetail);
            gdTempShow.Visibility = Visibility.Visible;
            gdTempInput.Visibility = Visibility.Collapsed;
            tbTempShow.Text = pHttpJobTemplate.jobTemplateName;
            if (!string.IsNullOrEmpty(model.jobName))
            {
                tbjobName.Text = model.jobName;
            }
            if(!string.IsNullOrWhiteSpace(model.minDegree))
            {
                cmbDegree.SelectedItem = (cmbDegree.ItemsSource as List<MappingItem>).FirstOrDefault(x => x.value == model.minDegree);
            }
            if(!string.IsNullOrWhiteSpace(model.minExp))
            {
                cmbWorkingYear.SelectedItem = (cmbWorkingYear.ItemsSource as List<MappingItem>).FirstOrDefault(x => x.value == model.minExp);
            }
            if(!string.IsNullOrWhiteSpace(model.workCharact))
            {
                cmbCategory.SelectedItem = (cmbCategory.ItemsSource as List<MappingItem>).FirstOrDefault(x => x.value == model.workCharact);
            }
            if(!string.IsNullOrWhiteSpace(model.thirdJobFunc))
            {
                if (findNamebyCode(model.thirdJobFunc) != null)
                {
                    tbJobCategory.DataContext = findNamebyCode(model.thirdJobFunc);
                    bdJobCategory.BorderBrush = normalColor;
                    gdJobChoose.Visibility = Visibility.Collapsed;
                    spJobCategory.Visibility = Visibility.Collapsed;
                    gdJobCategoryShow.Visibility = Visibility.Visible;
                    gdJobCategoryInput.Visibility = Visibility.Collapsed;
                }
            }
            if(!string.IsNullOrWhiteSpace(model.jobMinSalary))
            {
                tbMinSalary.Text = model.jobMinSalary;
            }
            if (!string.IsNullOrWhiteSpace(model.jobMaxSalary))
            {
                tbMaxSalary.Text = model.jobMaxSalary;
            }
            foreach (string istr in model.jobTemptationList)
            {
                tagChoose.Add(istr);
                tagsModel ptagsModel = tagList.FirstOrDefault(x => x.Tag == istr);
                if (ptagsModel != null)
                    ptagsModel.IsChecked = true;
            }
            if (!string.IsNullOrWhiteSpace(model.location))
            {
                tbLocation.Text = model.location;
            }
            if (!string.IsNullOrWhiteSpace(model.jobDesc))
            {
                tbContent.setText(model.jobDesc);
            }
            if (!string.IsNullOrWhiteSpace(model.jobCity))
            {
                HttpProvinceCityCodes province = provinceList.Where(x => (x.city.Where(y => y.code == model.jobCity).Count() > 0)).FirstOrDefault();
                HttpCityCodes city = cityList.Where(x => x.code == model.jobCity).FirstOrDefault();

                tbProvince.DataContext = province;
                tbCity.DataContext = city;
                gdCityShow.Visibility = Visibility.Visible;
                gdCityInput.Visibility = Visibility.Collapsed;

            }
            if (tbCity.DataContext as HttpCityCodes != null)
            {
                if (!string.IsNullOrWhiteSpace(model.jobLocation))
                {
                    string[] tempList = model.jobLocation.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempList.Length == 2)
                    {
                        double lat = Convert.ToDouble(tempList[1]);
                        double lng = Convert.ToDouble(tempList[0]);
                        this.ucGmap.InitialUCGamp(model.location, (tbCity.DataContext as HttpCityCodes).name, lat, lng);
                        //this.ucGmap.InitialUCGamp(model.location, "北京", lat, lng);
                        this.ucGmap.GmapZoom(14);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(model.location))
                        {
                            this.ucGmap.InitialUCGamp(model.location, (tbCity.DataContext as HttpCityCodes).name);
                            this.ucGmap.GmapZoom(14);
                        }
                        else
                        {
                            this.ucGmap.InitialUCGamp((tbCity.DataContext as HttpCityCodes).name, (tbCity.DataContext as HttpCityCodes).name);
                        }
                    }
                }
                else
                {


                    if (!string.IsNullOrWhiteSpace(model.location))
                    {

                        this.ucGmap.InitialUCGamp(model.location, (tbCity.DataContext as HttpCityCodes).name);

                    }
                    else
                    {
                        this.ucGmap.InitialUCGamp((tbCity.DataContext as HttpCityCodes).name, (tbCity.DataContext as HttpCityCodes).name);
                    }

                }
            }
            else
            {
                this.ucGmap.InitialUCGamp("北京", "北京");
                gdCityShow.Visibility = Visibility.Collapsed;
                gdCityInput.Visibility = Visibility.Visible;
            }
            PopTemplate.IsOpen = false;
        }

        #endregion

        private void PopTemplate_Closed(object sender, EventArgs e)
        {
            bdTemplate.BorderBrush = normalColor;
        }

        private void PopTemplate_Opened(object sender, EventArgs e)
        {
            bdTemplate.BorderBrush = focusColor;
        }
    }

}