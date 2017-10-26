
using MagicCube.Common;
using MagicCube.HttpModel;
using MagicCube.Model;
using MagicCube.TemplateUC;
using MagicCube.View.Message;
using MagicCube.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class UCJobOpen : UserControl
    {
        string searchname = string.Empty;
        int totalData = 0;
        int curPage = 0;
        int totalPage = 0;
        public delegate Task delegateEdit(long id);
        public delegateEdit actionEdit;
        public delegate Task delegateAddV();
        public delegateAddV actionAddV;
        public Action actionOpenPublish;
        public Action actionUpdataCount;
        public Action<string,long> actionOpenResume;
        JobManageModel curJobManageModel = new JobManageModel();
        List<JobManageModel> curJobList = new List<JobManageModel>();
        private Brush normalColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e5e5e5"));
        private Brush errColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f25751"));
        private Brush focusColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00beff"));
        public UCJobOpen()
        {
            InitializeComponent();
            ucUCPageTurn.PageChange = PageChange;
        }
        public async Task iniJobOpen()
        {
            curPage = 1;
            CheckAll.IsChecked = false;
            SearchResult.Visibility = Visibility.Collapsed;
            gdDetialView.Visibility = Visibility.Collapsed;
            searchname = string.Empty;
            ucUCPageTurn.tbChangePage.Text = string.Empty;
            txtPartialValue.Text = string.Empty;
            await UpdateTable();
        }
        public async Task UpdateTable()
        {
            busyCtrl.IsBusy = true;
            GdNoResult.Visibility = Visibility.Collapsed;
            GdNoSearchResult.Visibility = Visibility.Collapsed;
            lstJob.ItemsSource = null;
            string propertys = ModelTools.SetHttpPropertys<HttpJobList>();
            string std = Uri.EscapeDataString(DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "startRow", "pageSize", "properties", "status", "desc", "jobName", "jobType", "orderBy" }, 
                new string[] { MagicGlobal.UserInfo.Id.ToString(), ((curPage - 1) * DAL.ConfUtil.PageContent).ToString(), DAL.ConfUtil.PageContent.ToString(), propertys, "1", "fullName", searchname,"2", "\"createTime\" asc nulls last" }));


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
            

            setJudgeAllEnable();

            if (totalData == 0)
            {
                if (searchname != string.Empty)
                {
                    GdNoSearchResult.Visibility = Visibility.Visible;
                }
                else
                {
                    GdNoResult.Visibility = Visibility.Visible;
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
        private async Task RefreshJob()
        {

            long id = curJobManageModel.jobID;

            MagicGlobal.isJobTableEdit = true;
            busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] {"userID", "jobIDs" }, new string[] {MagicGlobal.UserInfo.Id.ToString(), id.ToString() });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobListRefresh, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel model = DAL.JsonHelper.ToObject<BaseHttpModel>(jsonResult);
            busyCtrl.IsBusy = false;
            if (model == null)
            {
                WinErroTip pWinErroTip = new WinErroTip(DAL.ConfUtil.netError);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return;
            }
            if (model.code != 200)
            {
                WinErroTip pWinErroTip = new WinErroTip(model.msg);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return;
            }

            await UpdateTable();
            DisappearShow disappear = new DisappearShow("刷新成功", 1);
            disappear.Owner = Window.GetWindow(this);
            disappear.ShowDialog();
        }
        private async Task EditJob()
        {
            //发布职位编辑重新发布，不算计数。

            //int code = await UnActivatePublishHelper.UnActivatePublish();
            //if (code == 0)
            //    return;
            //if (code != 200)
            //{
            //    if (code == -127)
            //    {
            //        WinValidateMessage pWinMessage = new WinValidateMessage("您的账号尚未认证，认证后才能发布更多职位哦");
            //        pWinMessage.Owner = Window.GetWindow(this);
            //        if ((bool)pWinMessage.ShowDialog())
            //        {
            //            if (actionAddV != null)
            //            {
            //                await actionAddV();
            //            }
            //        }
            //        return;
            //    }
            //    else if (code == -126)
            //    {
            //        WinMessageLink pWinErroTip = new WinMessageLink("贵公司今天已发布20个职位，请明天再来发布", "我知道了");
            //        pWinErroTip.Owner = Window.GetWindow(this);
            //        pWinErroTip.ShowDialog();
            //        return;
            //    }
            //}
            await actionEdit(curJobManageModel.jobID);
        }
        private async Task OutLineJob()
        {
            MagicGlobal.isJobTableEdit = true;
            MagicGlobal.isJobTableEdit = true;
            busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "jobIDs", "status" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), curJobManageModel.jobID.ToString(), "-1" });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobListOnline, MagicGlobal.UserInfo.Version, std));
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

            DisappearShow disappear = new DisappearShow("职位已下线", 1);
            disappear.Owner = Window.GetWindow(this);
            disappear.ShowDialog();
        }
        private async Task OpenDetial()
        {

            Common.TrackHelper2.TrackOperation("5.3.3.1.1", "pv");
            bdEdit.Visibility = Visibility.Visible;
            gdDetialView.Visibility = Visibility.Visible;
            if (curJobManageModel.canRefresh == true)
                btnDetialRefresh.Visibility = Visibility.Visible;
            else
                btnDetialRefresh.Visibility = Visibility.Collapsed;

            await ucPreView.iniDetial(curJobManageModel.jobID.ToString());

        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.3.2.3.1", "clk");
            foreach (JobManageModel iJobManageModel in curJobList)
            {
                iJobManageModel.IsCheck = (bool)(sender as CheckBox).IsChecked;
            }
            setJudgeAllEnable();
        }
        private void setJudgeAllEnable()
        {
            if (curJobList.Count() < 1)
            {
                CheckAll.IsChecked = false;
                return;
            }

            if (curJobList.Where(x => x.IsCheck == true).Count() == curJobList.Count())
            {
                CheckAll.IsChecked = true;
            }
            else
            {
                CheckAll.IsChecked = false;
            }

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
            Common.TrackHelper2.TrackOperation("5.3.2.6.1", "clk");
            if (searchname != string.Empty)
            {
                SearchResult.Visibility = Visibility.Visible;
            }
        }
        private void PublishJob_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            actionOpenPublish();
        }
        private void lstJob_MouseDown(object sender, MouseButtonEventArgs e)
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

        private async void multiRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (curJobList == null)
            {

                WinErroTip disappear = new WinErroTip("没有可以刷新的职位");
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                return;
            }
            if (curJobList.Count() < 1)
            {

                WinErroTip disappear = new WinErroTip("没有可以刷新的职位");
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                return;
            }
            if (curJobList.Where(x => x.IsCheck == true).Count() < 1)
            {
                WinErroTip disappear = new WinErroTip("请先选择职位");
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                return;
            }
            Common.TrackHelper2.TrackOperation("5.3.2.4.1", "clk");


            string temp = string.Empty;
            foreach (JobManageModel iJobManageModel in curJobList)
            {
                if (iJobManageModel.canRefresh == true && iJobManageModel.IsCheck == true)
                {
                    temp += iJobManageModel.jobID.ToString() + "$$";
                }
            }
            if (temp == string.Empty)
            {
                DisappearShow disappearResult = new DisappearShow("已刷新", 1);
                disappearResult.Owner = Window.GetWindow(this);
                disappearResult.ShowDialog();
                return;

            }
            temp = temp.Substring(0, temp.Length - 2);
            
            busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] {"userID", "jobIDs" },new string[] {MagicGlobal.UserInfo.Id.ToString(), temp });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobListRefresh, MagicGlobal.UserInfo.Version, std));
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
            DisappearShow disappearSucess = new DisappearShow("刷新成功", 1);
            disappearSucess.Owner = Window.GetWindow(this);
            disappearSucess.ShowDialog();

        }
        private async void multiOutline_Click(object sender, RoutedEventArgs e)
        {

            if (curJobList == null)
            {

                WinErroTip disappear = new WinErroTip("没有可以下线的职位");
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                return;
            }
            if (curJobList.Count() < 1)
            {

                WinErroTip disappear = new WinErroTip("没有可以下线的职位");
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                return;
            }
            if (curJobList.Where(x => x.IsCheck == true).Count() < 1)
            {
                WinErroTip disappear = new WinErroTip("请先选择职位");
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                return;
            }
            Common.TrackHelper2.TrackOperation("5.3.2.5.1", "clk");
            string temp = string.Empty;
            foreach (JobManageModel iJobManageModel in curJobList)
            {
                if (iJobManageModel.IsCheck == true)
                {
                    temp += iJobManageModel.jobID.ToString() + "$$";
                }
            }
            temp = temp.Substring(0, temp.Length - 2);
            WinConfirmTip pWinConfirmTip = new WinConfirmTip("您确定要将选中的职位下线么？");
            pWinConfirmTip.Owner = Window.GetWindow(this);
            if (pWinConfirmTip.ShowDialog() == false)
            {
                return;
            }

            busyCtrl.IsBusy = true;
           
            string std = DAL.JsonHelper.JsonParamsToString(new string[] {"userID", "jobIDs", "status" }, new string[] {MagicGlobal.UserInfo.Id.ToString(), temp,"-1" });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobListOnline, MagicGlobal.UserInfo.Version, std));
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
            WinErroTip pWinErroTipResult = new WinErroTip("职位已下线！如需恢复，在已下线职位列表中重新发布即可。");
            pWinErroTipResult.Owner = Window.GetWindow(this);
            pWinErroTipResult.ShowDialog();

        }
        private async void menuEdit_MenuClick(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.2.14.1", "clk");
            curJobManageModel = (sender as JobListButton).DataContext as JobManageModel;
            await EditJob();
        }
        private async void menuDetials_MenuClick(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.2.16.1", "clk");
            curJobManageModel = (sender as JobListButton).DataContext as JobManageModel;
            await OpenDetial();
        }
        private async void menuRefresh_MenuClick(object sender, RoutedEventArgs e)
        {

            Common.TrackHelper2.TrackOperation("5.3.2.13.1", "clk");
            curJobManageModel = (sender as JobListButton).DataContext as JobManageModel;
            await RefreshJob();
        }
        private async void menuOutLine_MenuClick(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.2.15.1", "clk");
            curJobManageModel = (sender as JobListButton).DataContext as JobManageModel;
            WinConfirmTip pWinConfirmTip = new WinConfirmTip("您确定要下线职位【" + curJobManageModel .jobName+ "】吗？");
            pWinConfirmTip.Owner = Window.GetWindow(this);
            if (pWinConfirmTip.ShowDialog() == false)
            {
                return;
            }
            await OutLineJob();
        }      
        private async void btnDetialRefresh_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.3.3.1", "clk");
            btnDetialRefresh.Visibility = Visibility.Collapsed;
            await RefreshJob();
            
        }
        private async void btnDetialOutLine_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.3.5.1", "clk");
            WinConfirmTip pWinConfirmTip = new WinConfirmTip("您确定要下线职位【" + curJobManageModel.jobName + "】吗？");
            pWinConfirmTip.Owner = Window.GetWindow(this);
            if (pWinConfirmTip.ShowDialog() == false)
            {
                return;
            }
            await OutLineJob();
            bdEdit.Visibility = Visibility.Collapsed;
        }
        private async void btnDetialEdit_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.3.4.1", "clk");
           await EditJob();
        }
        private void BtnDetialViewClose_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.3.2.1", "clk");
            gdDetialView.Visibility = Visibility.Collapsed;
        }
        private void CountButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag.ToString())
            {
                case "initiative":
                    Common.TrackHelper2.TrackOperation("5.3.2.17.1", "clk");
                    break;
                case "invite":
                    Common.TrackHelper2.TrackOperation("5.3.2.18.1", "clk");
                    break;
                case "pass":
                    Common.TrackHelper2.TrackOperation("5.3.2.8.1", "clk");
                    break;
                case "fail":
                    Common.TrackHelper2.TrackOperation("5.3.2.9.1", "clk");
                    break;
                case "reserve_fail":
                    Common.TrackHelper2.TrackOperation("5.3.2.10.1", "clk");
                    break;

            }
            actionOpenResume((sender as Button).Tag.ToString(), ((sender as Button).DataContext as JobManageModel).jobID);
            
        }
        private void btnPublishJob_Click(object sender, RoutedEventArgs e)
        {
            actionOpenPublish();
        }
        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            Common.TrackHelper2.TrackOperation("5.3.2.8.1", "clk");
            curJobManageModel.IsCheck = !curJobManageModel.IsCheck;
            setJudgeAllEnable();
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

        private async void Detial_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.2.7.1", "clk");
            curJobManageModel = (sender as Button).DataContext as JobManageModel;
            await OpenDetial();
        }

        private void PublicJob_Click(object sender, RoutedEventArgs e)
        {
            if (actionOpenPublish != null)
            {
                actionOpenPublish();
            }
        }

        private async void menuShare_MenuClick(object sender, RoutedEventArgs e)
        {
            curJobManageModel = (sender as JobListButton).DataContext as JobManageModel;
            this.busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string url = string.Format(DAL.ConfUtil.AddrGetQrCode, MagicGlobal.UserInfo.Version, std);
            string strResult = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            this.busyCtrl.IsBusy = false;
            ViewModel.BaseHttpModel<ViewModel.HttpGetQrCodeModel> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpGetQrCodeModel>>(strResult);
            if(model == null)
            {
                TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                return;
            }
            else
            {
                if(model.code == 200)
                {
                    string urlQrCode = "https://i.mofanghr.com/operation/job_share.html?jobID={0}&enterpriseCode={1}";
                    TemplateUC.WinQrCode winQrCode = new WinQrCode();
                    winQrCode.QrCoding = string.Format(urlQrCode, curJobManageModel.jobID, model.data.md5);
                    winQrCode.Owner = Window.GetWindow(this);
                    //winQrCode.Left = Window.GetWindow(this).Left + (Window.GetWindow(this).ActualWidth) / 2;
                    //winQrCode.Top = Window.GetWindow(this).Top + (Window.GetWindow(this).ActualHeight) / 2 - 150;
                   
                    
                    winQrCode.ShowDialog();
                }
                else
                {
                    TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                }
            }


        }

        private void txtPartialValue_GotFocus(object sender, RoutedEventArgs e)
        {
            bdInput.BorderBrush = focusColor;
        }
    }

    
}
