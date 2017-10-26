
using MagicCube.Common;
using MagicCube.HttpModel;
using MagicCube.Model;
using MagicCube.TemplateUC;

using MagicCube.View.Message;
using MagicCube.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// ResumeGridView.xaml 的交互逻辑
    /// </summary>
    public partial class UCJobClose : UserControl
    {
        string searchname = string.Empty;
        int totalData = 0;
        int curPage = 0;
        int totalPage = 0;
        public delegate Task delegateEdit(long id);
        public delegateEdit actionEdit;
        public delegate Task delegateAddV();
        public delegateAddV actionAddV;
        public Action actionUpdataCount;
        public Action actionPublishJob;
        public Action<string, long> actionOpenResume;
        JobManageModel curJobManageModel = new JobManageModel();
        List<JobManageModel> curJobList = new List<JobManageModel>();
        private Brush normalColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e5e5e5"));
        private Brush errColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f25751"));
        private Brush focusColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00beff"));
        public UCJobClose()
        {
            InitializeComponent();
            ucUCPageTurn.PageChange = PageChange;
        }
        public async Task iniJobClose()
        {
            curPage = 1;
            searchname = string.Empty;
            SearchResult.Visibility = Visibility.Collapsed;
            gdDetialView.Visibility = Visibility.Collapsed;
            ucUCPageTurn.tbChangePage.Text = string.Empty;
            txtPartialValue.Text = string.Empty;
            await UpdateTable();
        }
        public async Task UpdateTable()
        {

            busyCtrl.IsBusy = true;
            GdNoJob.Visibility = Visibility.Collapsed;
            GdNoSearchResult.Visibility = Visibility.Collapsed;
            lstJob.ItemsSource = null;
            string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpJobList>();
            string std = Uri.EscapeDataString( DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "startRow", "pageSize", "properties", "status", "desc", "jobName", "jobType" }, 
                new string[] { MagicGlobal.UserInfo.Id.ToString(), ((curPage - 1) * DAL.ConfUtil.PageContent).ToString(), DAL.ConfUtil.PageContent.ToString(), propertys, "-1", "fullName", searchname,"2" }));
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpJobListData> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(jsonResult);
            busyCtrl.IsBusy = false;
            if (model == null)
            {

                WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                lstJob.ItemsSource = null;
                return;
            }
            if (model.code != 200)
            {

                WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                lstJob.ItemsSource = null;
                return;
            }
            totalData = model.data.singleTolta.count;
            curJobList = new List<JobManageModel>();
            foreach (HttpJobList iHttpJobList in model.data.data)
            {
                curJobList.Add(SetModel.SetJobManageModel(iHttpJobList));
            }

            totalPage = totalData % DAL.ConfUtil.PageContent > 0 ? (totalData / DAL.ConfUtil.PageContent) + 1 : totalData / DAL.ConfUtil.PageContent;
            if (totalData == 0)
                curPage = 0;
            if (totalPage == 0)
                totalPage = 0;
            ucUCPageTurn.setPageState(curPage, totalPage, totalData);
            lstJob.ItemsSource = curJobList;

            if (totalData == 0)
            {
                if (searchname != string.Empty)
                {
                    GdNoSearchResult.Visibility = Visibility.Visible;
                }
                else
                {

                    GdNoJob.Visibility = Visibility.Visible;
                }
            }
            else
            {
                svjob.ScrollToTop();
            }
            actionUpdataCount();
        }
        private async Task PageChange(int pcurPage)
        {
            curPage = pcurPage;
            await UpdateTable();
        }
        private async Task OpenDetial()
        {

            Common.TrackHelper2.TrackOperation("5.3.5.1.1", "pv");
            bdEdit.Visibility = Visibility.Visible;
            gdDetialView.Visibility = Visibility.Visible;
            await ucPreView.iniDetial(curJobManageModel.jobID.ToString());
        }
        private async Task SearchName()
        {
            btnSearch.Focus();

            if (!string.IsNullOrWhiteSpace(txtPartialValue.Text))
            {
                ucUCPageTurn.tbChangePage.Text = string.Empty;
                searchname = txtPartialValue.Text;
                tbSearchResult.Text = txtPartialValue.Text;
                SearchResult.Visibility = Visibility.Visible;
                curPage = 1;

                await UpdateTable();

            }
        }
        private async Task RePublishJob()
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


            string jsonCheck = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "jobNameAbsolute", "jobCity", "location", "firstJobFunc", "secondJobFunc", "thirdJobFunc","jobID" },
               new string[] { MagicGlobal.UserInfo.Id.ToString(), curJobManageModel.jobName, curJobManageModel.jobCity, curJobManageModel.location, curJobManageModel.firstJobFunc, curJobManageModel.secondJobFunc, curJobManageModel.thirdJobFunc, curJobManageModel.jobID.ToString() });

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
            string std = DAL.JsonHelper.JsonParamsToString(new string[] {"userID","jobID","status", "currentStatus" }, new string[] {MagicGlobal.UserInfo.Id.ToString(), curJobManageModel.jobID.ToString(), "1","-1" });
            string jsonResult = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrUpdateJob,MagicGlobal.UserInfo.Version), std);
            BaseHttpModel model = DAL.JsonHelper.ToObject<BaseHttpModel>(jsonResult);
            if (model == null)
            {
                WinErroTip pWinErroTip = new WinErroTip(DAL.ConfUtil.netError);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                busyCtrl.IsBusy = false;
                return;
            }
            if (model.code != 200)
            {
                WinErroTip pWinErroTip = new WinErroTip(model.msg);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                busyCtrl.IsBusy = false;
                return;
            }
            await UpdateTable();
            DisappearShow disappear = new DisappearShow("已重新发布", 1);
            disappear.Owner = Window.GetWindow(this);
            disappear.ShowDialog();
        }
        private async Task EditJob()
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
            await actionEdit(curJobManageModel.jobID);
        }
        private async Task OpenDetial(long id)
        {
            bdEdit.Visibility = Visibility.Visible;
            gdDetialView.Visibility = Visibility.Visible;
            await ucPreView.iniDetial(id.ToString());

        }
        private async void ClearTextBlock_Click(object sender, RoutedEventArgs e)
        {
            SearchResult.Visibility = Visibility.Collapsed;
            txtPartialValue.Text = string.Empty;
            searchname = string.Empty;
            curPage = 1;
            await UpdateTable();
        }
        private async void SearchTextBlock_Click(object sender, RoutedEventArgs e)
        {
            await SearchName();
        }
        private async void txtPartialValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await SearchName();
            }
        }
        private void txtPartialValue_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SearchResult.Visibility = Visibility.Collapsed;
        }
        private void txtPartialValue_LostFocus(object sender, RoutedEventArgs e)
        {
            bdInput.BorderBrush = normalColor;
            Common.TrackHelper2.TrackOperation("5.3.4.3.1", "clk");
            if (searchname != string.Empty)
            {
                SearchResult.Visibility = Visibility.Visible;
            }
        }
        private void PublishJob_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //actionOpenPublish();
        }
        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {

        }
        private  void lstJob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point aP = e.GetPosition(this.lstJob);
            IInputElement obj = this.lstJob.InputHitTest(aP);
            System.Windows.DependencyObject target = obj as System.Windows.DependencyObject;


            while (target != null)
            {
                if (target is Grid)
                {
                    if ((target as Grid).Name == "bdRow")
                    {
                        curJobManageModel = (target as Grid).DataContext as JobManageModel;
 
                        return;
                    }
                }
                target = VisualTreeHelper.GetParent(target);
            }
        }
        private void lstJob_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point aP = e.GetPosition(this.lstJob);
            IInputElement obj = this.lstJob.InputHitTest(aP);
            System.Windows.DependencyObject target = obj as System.Windows.DependencyObject;


            while (target != null)
            {
                if (target is Grid)
                {
                    if ((target as Grid).Name == "bdRow")
                    {
                        curJobManageModel = (target as Grid).DataContext as JobManageModel;
                        return;
                    }
                }
                target = VisualTreeHelper.GetParent(target);
            }
        }
        private async void menuDetials_MenuClick(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.4.11.1", "clk");
            curJobManageModel = (sender as JobListButton).DataContext as JobManageModel;
            await OpenDetial();
        }
        private async void menuRePublish_MenuClick(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.4.9.1", "clk");
            curJobManageModel = (sender as JobListButton).DataContext as JobManageModel;
            await RePublishJob();
        }
        private async void menuEdit_MenuClick(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.4.10.1", "clk");
            curJobManageModel = (sender as JobListButton).DataContext as JobManageModel;
            await EditJob();
        }
        private async void btnDetialRePublish_Click(object sender, RoutedEventArgs e)
        {

            Common.TrackHelper2.TrackOperation("5.3.5.6.1", "clk");
            await RePublishJob();
            bdEdit.Visibility = Visibility.Collapsed;
        }
        private async void btnDetialEdit_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.5.7.1", "clk");
            await EditJob();
        }
        private void BtnDetialViewClose_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.5.5.1", "clk");
            gdDetialView.Visibility = Visibility.Collapsed;
        }
        private void CountButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag.ToString())
            {
                case "initiative":
                    Common.TrackHelper2.TrackOperation("5.3.4.12.1", "clk");
                    break;
                case "invite":
                    Common.TrackHelper2.TrackOperation("5.3.4.13.1", "clk");
                    break;
                case "pass":
                    Common.TrackHelper2.TrackOperation("5.3.4.6.1", "clk");
                    break;
                case "fail":
                    Common.TrackHelper2.TrackOperation("5.3.4.7.1", "clk");
                    break;
                case "reserve_fail":
                    Common.TrackHelper2.TrackOperation("5.3.4.8.1", "clk");
                    break;

            }
            actionOpenResume((sender as Button).Tag.ToString(), ((sender as Button).DataContext as JobManageModel).jobID);
        }

        private async void Detial_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.4.4.1", "clk");
            curJobManageModel = (sender as Button).DataContext as JobManageModel;
            await OpenDetial();
        }

        private void PublicJob_Click(object sender, RoutedEventArgs e)
        {
            if(actionPublishJob!=null)
            {
                actionPublishJob();
            }
        }

        private void txtPartialValue_GotFocus(object sender, RoutedEventArgs e)
        {
            bdInput.BorderBrush = focusColor;
        }
    }

    
}
