using MagicCube.Common;
using MagicCube.HttpModel;
using MagicCube.Model;
using MagicCube.TemplateUC;
using MagicCube.View.Message;
using MagicCube.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class UCResumeBuy : UserControl
    {
        int totalData = 0;
        int curPage = 1;
        int totalPage = 0;
        string searchname = string.Empty;
        public Action UpdataCount;
        public delegate Task delegateChat(string id);
        public delegateChat actionChat;
        private ResumeTableModel curResumeTableModel;
        private ResumeDetailModel curResumeDetailModel;
        public UCResumeBuy()
        {
            InitializeComponent();
            ucUCPageTurn.PageChange = PageChange;
        }

        public async Task IniTable()
        {
            if (MagicGlobal.UserInfo.Id % 2 == 0)
            {
                rdoThumbnail.IsChecked = true;
                rdoList.IsChecked = false;
            }
            else
            {
                rdoThumbnail.IsChecked = false;
                rdoList.IsChecked = true;
            }
            gdResumeView.Visibility = Visibility.Collapsed;
            curPage = 1;
            await UpdateTable();
        }

        public async Task UpdateTable()
        {
            busyCtrl.IsBusy = true;
            GdNoResult.Visibility = Visibility.Collapsed;
            tbResume.ItemsSource = null;
            icResume.ItemsSource = null;
            searchname = txtPartialValue.textBox.Text;
            string propertys = ModelTools.SetHttpPropertys<MagicCube.ViewModel.HttpResumeTable>();

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "startRow", "pageSize", "desc", "properties", "relationSign" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), ((curPage - 1) * DAL.ConfUtil.PageContent).ToString(), DAL.ConfUtil.PageContent.ToString(), "fullName", propertys, "viewContactOrigin" });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrCollectResumeList, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpCollectData> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<HttpCollectData>>(jsonResult);
            busyCtrl.IsBusy = false;
            if (model != null)
            {
                if (model.code == 200)
                {
                    List<ResumeTableModel> lstResume = new List<ResumeTableModel>();
                    foreach (HttpCollect iHttpCollect in model.data.result)
                    {
                        if(iHttpCollect.resume!=null)
                            lstResume.Add(SetModel.SetResumeBuyModel(iHttpCollect));
                    }
                    tbResume.ItemsSource = lstResume;
                    icResume.ItemsSource = lstResume;
                    totalData = model.data.total;

                    totalPage = totalData % DAL.ConfUtil.PageContent > 0 ? (totalData / DAL.ConfUtil.PageContent) + 1 : totalData / DAL.ConfUtil.PageContent;
                    if (totalData == 0)
                        curPage = 0;
                    if (totalPage == 0)
                        totalPage = 0;
                    ucUCPageTurn.setPageState(curPage, totalPage, totalData);
                    if (totalData == 0)
                    {
                        GdNoResult.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SvConversation.ScrollToTop();
                        if (tbResume.Items.Count > 0)
                            tbResume.ScrollIntoView(tbResume.Items[0]);
                    }
                }
            }
        }
        private async Task PageChange(int pcurPage)
        {
            curPage = pcurPage;
            await UpdateTable();
        }
        private async Task SearchName()
        {
            btnSearch.Focus();

            ucUCPageTurn.tbChangePage.Text = string.Empty;
            searchname = txtPartialValue.textBox.Text;
            curPage = 1;
            await UpdateTable();
        }


        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (tbResume.SelectedItem as HttpResumeInterviewer).IsCheck = !(tbResume.SelectedItem as HttpResumeInterviewer).IsCheck;
        }

        private async void Photo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await OpenResume((sender as Border).DataContext as ResumeTableModel);
        }
        private async void Name_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await OpenResume((sender as TextBlock).DataContext as ResumeTableModel);
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

        #region 查看简历操作
        private void BtnResumeViewClose_Click(object sender, RoutedEventArgs e)
        {
            gdResumeView.Visibility = Visibility.Collapsed;
        }

        private async Task OpenResume(ResumeTableModel pResumeTableModel)
        {
            ucResumeDetial.iniResumeComment();
            SVUCResume.ScrollToTop();
            curResumeTableModel = pResumeTableModel;
            busyCtrl.IsBusy = true;
            BaseHttpModel<HttpResumeDetial> model = await IMHelper.GetDetialReusme(pResumeTableModel.userID.ToString());
            busyCtrl.IsBusy = false;
            if (model == null)
            {
                WinErroTip pWinErroTip = new WinErroTip(DAL.ConfUtil.netError);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return;
            }
            if (model.code!=200)
            {
                WinErroTip pWinErroTip = new WinErroTip(model.msg);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return;
            }
            curResumeDetailModel = SetModel.SetResumeDetailModel(model.data);
            ucResumeDetial.DataContext = curResumeDetailModel;
            btnCollection.DataContext = curResumeDetailModel;
            gdResumeView.Visibility = Visibility.Visible;
        }

        #endregion

        private async void btnCollection_Click(object sender, RoutedEventArgs e)
        {
            if (curResumeDetailModel.collectResume)
            {
                BaseHttpModel pBaseHttpModel = await IMHelper.CancelCollectResume(curResumeDetailModel.userID);
                if (pBaseHttpModel != null)
                {
                    if (pBaseHttpModel.code == 200)
                    {
                        curResumeDetailModel.collectResume = false;
                        await UpdateTable();
                        DisappearShow pDisappearShow = new DisappearShow("已取消收藏");
                        pDisappearShow.Owner = Window.GetWindow(this);
                        pDisappearShow.ShowDialog();
                    }
                }


            }
            else
            {
                BaseHttpModel<HttpCollectionCount> pBaseHttpModel = await IMHelper.CollectResume(curResumeDetailModel.userID);
                if (pBaseHttpModel != null)
                {
                    if (pBaseHttpModel.code == 200)
                    {
                        curResumeDetailModel.collectResume = true;
                        await UpdateTable();
                        if(pBaseHttpModel.data.total == 0)
                        {
                            WinMessageLink link = new WinMessageLink("您收藏的简历可以到「简历管理」-「收藏夹」中查看", "我知道了");
                            link.Owner = Window.GetWindow(this);
                            link.ShowDialog();
                        }
                        else
                        {
                            DisappearShow pDisappearShow = new DisappearShow("已收藏");
                            pDisappearShow.Owner = Window.GetWindow(this);
                            pDisappearShow.ShowDialog();
                        }
                       
                    }
                }
            }
            
        }


        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            this.busyCtrl.IsBusy = true;
            BaseHttpModel<HttpRecentList> model = await IMHelper.GetSesionInfo(curResumeDetailModel.userID);
            this.busyCtrl.IsBusy = false;
            if (model == null)
            {
                return;
            }
            if (model.code == 200)
            {
                if (model.data.jobID > 0)
                {
                    GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk, new UserJobModel() { userId = curResumeDetailModel.userID.ToString(), jobId = model.data.jobID.ToString() }));
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.data.latestMsgDesc))
                    {
                        GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk, new UserJobModel() { jobId = "0", userId = this.curResumeDetailModel.userID.ToString() }));
                        return;
                    }
                }
            }      
        }

        private async void Resume_Click(object sender, RoutedEventArgs e)
        {
            ResumeTableModel pResume = (sender as Button).DataContext as ResumeTableModel;
            await OpenResume(pResume);
        }

        private async void Chat_Click(object sender, RoutedEventArgs e)
        {
            ResumeTableModel pResume = (sender as Button).DataContext as ResumeTableModel;
            this.busyCtrl.IsBusy = true;
            BaseHttpModel<HttpRecentList> model = await IMHelper.GetSesionInfo(pResume.userID);
            this.busyCtrl.IsBusy = false;
            if (model == null)
            {
                return;
            }
            if (model.code == 200)
            {
                if (model.data.jobID > 0)
                {
                    GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk, new UserJobModel() { userId = pResume.userID.ToString(), jobId = model.data.jobID.ToString() }));
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.data.latestMsgDesc))
                    {
                        GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk, new UserJobModel() { jobId = "0", userId = pResume.userID.ToString() }));
                        return;
                    }
                }
            }
        }

        private void EditMenu_Click(object sender, RoutedEventArgs e)
        {
            curResumeTableModel = (sender as Button).DataContext as ResumeTableModel;
            PopMenu.PlacementTarget = (sender as Button);
            PopMenu.IsOpen = true;
        }

        private async void Chat_MenuClick(object sender, RoutedEventArgs e)
        {
            PopMenu.IsOpen = false;
            this.busyCtrl.IsBusy = true;
            BaseHttpModel<HttpRecentList> model = await IMHelper.GetSesionInfo(curResumeTableModel.userID);
            this.busyCtrl.IsBusy = false;
            if (model == null)
            {
                return;
            }
            if (model.code == 200)
            {
                if (model.data.jobID > 0)
                {
                    GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk, new UserJobModel() { userId = curResumeTableModel.userID.ToString(), jobId = model.data.jobID.ToString() }));
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.data.latestMsgDesc))
                    {
                        GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk, new UserJobModel() { jobId = "0", userId = curResumeTableModel.userID.ToString() }));
                        return;
                    }
                }
            }
        }
        private async void Resume_MenuClick(object sender, RoutedEventArgs e)
        {
            PopMenu.IsOpen = false;
            await OpenResume(curResumeTableModel);
        }

        private async void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            if (curResumeDetailModel != null)
            {
                string jobName = string.Empty;
                if (curResumeDetailModel.personExperienceJob != null)
                {
                    if (curResumeDetailModel.personExperienceJob.Count > 0)
                    {
                        jobName = curResumeDetailModel.personExperienceJob[0].name;
                    }
                }
                curResumeDetailModel.downloadType = "talentSearch";
                await DownloadResume.DownLoadResume(curResumeDetailModel, Window.GetWindow(this));
            }
        }

        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (curResumeDetailModel != null)
            {
                WinSendEmail pWinSendEmail = new WinSendEmail(Window.GetWindow(this),curResumeDetailModel.userID, curResumeDetailModel.name);
                //pWinSendEmail.Owner = Window.GetWindow(this);
                pWinSendEmail.ShowDialog();
            }
        }

        private async void tbResume_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point aP = e.GetPosition(this.tbResume);
            IInputElement obj = this.tbResume.InputHitTest(aP);
            System.Windows.DependencyObject target = obj as System.Windows.DependencyObject;


            while (target != null)
            {
                if (target is DataGridRow)
                {
                    ResumeTableModel pResume = tbResume.SelectedItem as ResumeTableModel;
                    await OpenResume(pResume);
                    return;
                }
                if (target is Button)
                {
                    if ((target as Button).Name == "btnTagsEdit")
                        return;
                }
                if (target is Grid)
                {
                    if ((target as Grid).Name == "gdIgnoreDoubleClick")
                        return;
                }
                target = VisualTreeHelper.GetParent(target);

            }
        }


        private async void icResume_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point aP = e.GetPosition(this.icResume);
            IInputElement obj = this.icResume.InputHitTest(aP);
            System.Windows.DependencyObject target = obj as System.Windows.DependencyObject;


            while (target != null)
            {
                if (target is Grid)
                {
                    if ((target as Grid).Name == "bdRow")
                    {
                        await OpenResume((target as Grid).DataContext as ResumeTableModel);
                        return;
                    }

                }
                if (target is CheckBox)
                    return;
                target = VisualTreeHelper.GetParent(target);

            }
        }

        private void PrintResume_Click(object sender, RoutedEventArgs e)
        {
            ResumeDetialPrint pResumeDetial = new ResumeDetialPrint();
            pResumeDetial.DataContext = curResumeDetailModel;
            WinResumePrint pWinResumePrint = new WinResumePrint(Window.GetWindow(this));
            if (pWinResumePrint.ShowDialog() == true)
            {
                if (pWinResumePrint.cbCheck.IsChecked == true)
                {
                    pResumeDetial.tbSalary.Visibility = Visibility.Collapsed;
                }
                PrintHelper.print(pResumeDetial);
            }

        }

        private void CommentResume_Click(object sender, RoutedEventArgs e)
        {
            SVUCResume.ScrollToBottom();
        }
    }

}
