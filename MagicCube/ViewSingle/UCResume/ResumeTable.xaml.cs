using MagicCube.Common;
using MagicCube.HttpModel;
using MagicCube.Model;
using MagicCube.TemplateUC;

using MagicCube.View.Message;
using MagicCube.View.UC;
using MagicCube.ViewModel;
using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// ResumeGridView.xaml 的交互逻辑
    /// </summary>
    public partial class ResumeTable : UserControl
    {
        ResumeTableType resumetype;
        string typeTime = string.Empty;
        string jobType = string.Empty;
        int tagId = -1;
        int curViewContact = -2;
        long curJobId = -1;
        int totalData = 0;
        int curPage = 0;
        int totalPage = 0;
        string searchname = string.Empty;
        public Action actionOpenPublish;
        public Action UpdataCount;
        public Action<bool> actionOpenTagManage;
        public delegate Task delegateSearch();
        public delegateSearch actionSearch;
        public delegate Task delegateChat(string id,string jobID);
        public delegateChat actionChat;
        public delegate Task delegateClearCollection();
        public delegateClearCollection actionClearCollection;
        private ResumeDetailModel curResumeDetailModel = new ResumeDetailModel();
        private ResumeTableModel curResumeTableModel;    
        public ResumeTable()
        {
            InitializeComponent();
            ucUCPageTurn.PageChange = PageChange;
            ucResumeDetial.UpdataFlag = UpdateFlag;
           
        }
        public async Task iniJobList(long id) 
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

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "hrUserID", "type" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), Convert.ToInt32(resumetype).ToString() });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetSearchJob, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<List<HttpJobList>> listModel = DAL.JsonHelper.ToObject<BaseHttpModel<List<HttpJobList>>>(resultStr);
            List<HttpJobList> plstJob = new List<HttpJobList>();
            plstJob.Add(new HttpJobList { jobID = -1, jobName = "全部职位" });
            
            if (listModel !=null)
            {
                if(listModel.code == 200)
                {
                    foreach(HttpJobList iHttpJobList in listModel.data)
                    {
                        plstJob.Add(iHttpJobList);
                    }
                }
            }
            plstJob.Add(new HttpJobList { jobID = 0, jobName = "其他" });
            cbJob.ItemsSource = plstJob;
            cbJob.SelectedItem = (cbJob.ItemsSource as List<HttpJobList>).First(x => x.jobID == id);
            List<string> lstView = new List<string>() { "下载联系方式", "已下载", "未下载" };
            cbViewContect.ItemsSource = lstView;
            cbViewContect.SelectedIndex = 0;
            curViewContact = -1;
        }
        public async Task iniTable(ResumeTableType type, long id = -1)
        {
            resumetype = type;
            GdNoResume.Visibility = Visibility.Collapsed;
            GdNoResult.Visibility = Visibility.Collapsed;
            GdNoJob.Visibility = Visibility.Collapsed;

            tbResume.ItemsSource = null;
            icResume.ItemsSource = null;
            gdResumeView.Visibility = Visibility.Collapsed;
            if (resumetype == ResumeTableType.fail || resumetype == ResumeTableType.pass)
            {
                spJudge.Visibility = Visibility.Collapsed;
                spMenuJudge.Visibility = Visibility.Collapsed;

            }
            else
            {
              
                spJudge.Visibility = Visibility.Visible;
                spMenuJudge.Visibility = Visibility.Visible;

            }

            if (resumetype == ResumeTableType.invite)
            {
                jobType = "邀请职位";
                typeTime = "邀请时间";               
            }
            else
            {
                jobType = "应聘职位";
                typeTime = "应聘时间";                              
            }
            dgt.Header = jobType;
            dgtime.Header = typeTime; 
            TagSelectResult.Child = new TextBlock()
            {
                Text = "按标记筛选",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b4b4b4"))
            };
            tagId = -1;
            curPage = 1;
            curViewContact = -2;
            searchname = string.Empty;
            ucUCPageTurn.tbChangePage.Text = string.Empty;
            txtPartialValue.Text = string.Empty;
            await iniJobList(id);
        }
        public async Task UpdateTable()
        {
            busyCtrl.IsBusy = true;
            GdNoResume.Visibility = Visibility.Collapsed;
            GdNoResult.Visibility = Visibility.Collapsed;
            GdNoJob.Visibility = Visibility.Collapsed;

            tbResume.ItemsSource = null;
            icResume.ItemsSource = null;


            string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<MagicCube.ViewModel.HttpResumeTable>();
            List<string> pkey = new List<string> { "hrUserID", "startRow", "pageSize", "desc", "properties", "type","userID", "companyID" };
            List<string> pvalue = new List<string>  { MagicGlobal.UserInfo.Id.ToString(), ((curPage - 1) * DAL.ConfUtil.PageContent).ToString(), 
                    DAL.ConfUtil.PageContent.ToString(), "fullName", propertys, Convert.ToInt32(resumetype).ToString(),MagicGlobal.UserInfo.Id.ToString(),MagicGlobal.UserInfo.CompanyId.ToString() };

            if (curJobId != -1)
            {
                pkey.Add("jobID");
                pvalue.Add(curJobId.ToString());
            }
            if (tagId != -1)
            {
                pkey.Add("userSignID");
                pvalue.Add(tagId.ToString());
            }
            if(curViewContact >-1)
            {
                pkey.Add("viewContact");
                pvalue.Add(curViewContact.ToString());
            }

            string std = DAL.JsonHelper.JsonParamsToString(pkey, pvalue);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(
                string.Format(DAL.ConfUtil.AddrResumeList, MagicGlobal.UserInfo.Version, std));
            MagicCube.ViewModel.BaseHttpModel<HttpCollectData> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<HttpCollectData>>(jsonResult);
            busyCtrl.IsBusy = false;
            if (model != null)
            {
                if (model.code == 200)
                {
                    List<ResumeTableModel> lstResume = new List<ResumeTableModel>();
                    foreach (HttpCollect iHttpCollect in model.data.result)
                    {
                        if (iHttpCollect.resume != null)
                        {
                            ResumeTableModel pResumeTableModel = SetModel.SetResumeTableModel(iHttpCollect);
                            pResumeTableModel.timeType = typeTime;
                            pResumeTableModel.jobType = jobType;
                            lstResume.Add(pResumeTableModel);
                        }

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
                        if (curJobId != -1 || tagId != -1||curViewContact>-1)
                        {
                            GdNoResult.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            if (cbJob.Items.Count > 1)
                            {
                                GdNoResume.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                GdNoJob.Visibility = Visibility.Visible;
                            }
                        }
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
            if (cbJob.SelectedIndex != 0)
                cbJob.SelectedIndex = 0;
            else
            {
                curPage = 1;
                await UpdateTable();
            }

        }
        private void EditMenu_Click(object sender, RoutedEventArgs e)
        {
            switch (resumetype)
            {
                case ResumeTableType.initiative:
                    TrackHelper2.TrackOperation("5.5.1.12.1", "clk");
                    break;
                case ResumeTableType.invite:
                    TrackHelper2.TrackOperation("5.5.2.10.1", "clk");
                    break;
                case ResumeTableType.pass:
                    TrackHelper2.TrackOperation("5.5.3.10.1", "clk");
                    break;
                case ResumeTableType.fail:
                    TrackHelper2.TrackOperation("5.5.4.10.1", "clk");
                    break;
                case ResumeTableType.autoFilter:
                    TrackHelper2.TrackOperation("5.5.5.10.1", "clk");
                    break;
            }
            curResumeTableModel = (sender as Button).DataContext as ResumeTableModel;
            PopMenu.PlacementTarget = (sender as Button);
            PopMenu.IsOpen = true;
        }
        private async void Resume_MenuClick(object sender, RoutedEventArgs e)
        {

            switch (resumetype)
            {
                case ResumeTableType.initiative:
                    TrackHelper2.TrackOperation("5.5.1.13.1", "clk");
                    break;
                case ResumeTableType.invite:
                    TrackHelper2.TrackOperation("5.5.2.11.1", "clk");
                    break;
                case ResumeTableType.pass:
                    TrackHelper2.TrackOperation("5.5.3.11.1", "clk");
                    break;
                case ResumeTableType.fail:
                    TrackHelper2.TrackOperation("5.5.4.11.1", "clk");
                    break;
                case ResumeTableType.autoFilter:
                    TrackHelper2.TrackOperation("5.5.5.11.1", "clk");
                    break;
            }
            PopMenu.IsOpen = false;
            if (curResumeTableModel != null)
            {
                await OpenResume(curResumeTableModel);
            }


        }
        private async void Chat_MenuClick(object sender, RoutedEventArgs e)
        {
            switch (resumetype)
            {
                case ResumeTableType.initiative:
                    TrackHelper2.TrackOperation("5.5.1.14.1", "clk");
                    break;
                case ResumeTableType.invite:
                    TrackHelper2.TrackOperation("5.5.2.12.1", "clk");
                    break;
                case ResumeTableType.pass:
                    TrackHelper2.TrackOperation("5.5.3.12.1", "clk");
                    break;
                case ResumeTableType.fail:
                    TrackHelper2.TrackOperation("5.5.4.12.1", "clk");
                    break;
                case ResumeTableType.autoFilter:
                    TrackHelper2.TrackOperation("5.5.5.12.1", "clk");
                    break;
            }
            PopMenu.IsOpen = false;
            if (curResumeTableModel != null)
            {
                await actionChat(curResumeTableModel.userID.ToString(), curResumeTableModel.jobID.ToString());
            }
        }
        private async void Evaluate_MenuClick(object sender, RoutedEventArgs e)
        {
            PopMenu.IsOpen = false;
            string strJudge = string.Empty;
            bool feed = true;
            string pResult = string.Empty;
            switch ((sender as MagicCube.TemplateUC.UCMenuItem).Text)
            {
                case "合适":
                    switch (resumetype)
                    {
                        case ResumeTableType.initiative:
                            TrackHelper2.TrackOperation("5.5.1.15.1", "clk");
                            break;
                        case ResumeTableType.invite:
                            TrackHelper2.TrackOperation("5.5.2.13.1", "clk");
                            break;
                        case ResumeTableType.pass:
                            TrackHelper2.TrackOperation("5.5.3.13.1", "clk");
                            break;
                        case ResumeTableType.fail:
                            TrackHelper2.TrackOperation("5.5.4.13.1", "clk");
                            break;
                        case ResumeTableType.autoFilter:
                            TrackHelper2.TrackOperation("5.5.5.13.1", "clk");
                            break;
                    }
                    strJudge = DAL.ConfUtil.FeedbackPass;
                    feed = true;
                    pResult = "已处理为合适";
                    break;
                case "不合适":
                    switch (resumetype)
                    {
                        case ResumeTableType.initiative:
                            TrackHelper2.TrackOperation("5.5.1.15.1", "clk");
                            break;
                        case ResumeTableType.invite:
                            TrackHelper2.TrackOperation("5.5.2.13.1", "clk");
                            break;
                        case ResumeTableType.pass:
                            TrackHelper2.TrackOperation("5.5.3.13.1", "clk");
                            break;
                        case ResumeTableType.fail:
                            TrackHelper2.TrackOperation("5.5.4.13.1", "clk");
                            break;
                        case ResumeTableType.autoFilter:
                            TrackHelper2.TrackOperation("5.5.5.13.1", "clk");
                            break;
                    }
                    strJudge = DAL.ConfUtil.FeedbackFail;
                    feed = false;
                    pResult = "已处理为不合适";
                    break;
            }
            if ((MagicGlobal.UserInfo.IsPassTip && strJudge == DAL.ConfUtil.FeedbackPass) || (MagicGlobal.UserInfo.IsFailTip && strJudge == DAL.ConfUtil.FeedbackFail))
            {
                WinConfirmJudge pWinConfirmJudge = new WinConfirmJudge(strJudge);
                pWinConfirmJudge.Owner = Window.GetWindow(this);
                if (pWinConfirmJudge.ShowDialog() == false)
                    return;
               
            }
            busyCtrl.IsBusy = true;
            string pJudgeResult = await JudgeHelper.ResumeJudge(curResumeTableModel.deliveryID, feed);
            if (pJudgeResult == DAL.ConfUtil.judgeSucess)
            {
                await UpdateTable();
                if (UpdataCount != null)
                    UpdataCount();
                DisappearShow disappear = new DisappearShow(pResult, 1);
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                spJudge.Visibility = Visibility.Collapsed;
            }
            else
            {
                WinErroTip pWinErroTip = new WinErroTip(pJudgeResult);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
            }
            busyCtrl.IsBusy = false;
        }
        private async void Resume_Click(object sender, RoutedEventArgs e)
        {
            PopMenu.IsOpen = false;
            ResumeTableModel pResume = (sender as Button).DataContext as ResumeTableModel;
            await OpenResume(pResume);
            //查看简历发送通知
        }
        private void Tag_MenuClick(object sender, RoutedEventArgs e)
        {
            PopMenuTag.PlacementTarget = (sender as Button);
            PopMenuTag.Placement = PlacementMode.Left;
            curPopResumeTableModel = (sender as Button).DataContext as ResumeTableModel;
            List<TagsMenuModel> menuList = new List<TagsMenuModel>();
            foreach (var item in MagicGlobal.TagsManagerList)
            {
                bool isChecked = false;
                if (curPopResumeTableModel.tags != null)
                {
                    foreach (var temp in curPopResumeTableModel.tags)
                    {
                        if (temp.id == item.Id)
                        {
                            isChecked = true;
                            break;
                        }
                    }
                }
                menuList.Add(new TagsMenuModel() { Id = item.Id, IsChecked = isChecked, TagColor = item.TagColor, TagContent = item.TagContent });
            }
            this.popTagsList.ItemsSource = menuList;
            PopMenuTag.IsOpen = true;
        }
        private async void tbResume_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                    switch (resumetype)
                    {
                        case ResumeTableType.initiative:
                            TrackHelper2.TrackOperation("5.5.1.10.1", "clk");
                            break;
                        case ResumeTableType.invite:
                            TrackHelper2.TrackOperation("5.5.2.8.1", "clk");
                            break;
                        case ResumeTableType.pass:
                            TrackHelper2.TrackOperation("5.5.3.8.1", "clk");
                            break;
                        case ResumeTableType.fail:
                            TrackHelper2.TrackOperation("5.5.4.8.1", "clk");
                            break;
                        case ResumeTableType.autoFilter:
                            TrackHelper2.TrackOperation("5.5.5.8.1", "clk");
                            break;
                    }
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
                if (target is Grid)
                {
                    if ((target as Grid).Name == "gdIgnoreDoubleClick")
                        return;
                }
                target = VisualTreeHelper.GetParent(target);

            }
        }

        private async void cbJob_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbJob.SelectedItem == null)
                return;

            curJobId = (cbJob.SelectedItem as HttpJobList).jobID;
            curPage = 1;
            await UpdateTable();
        }
        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //(tbResume.SelectedItem as ResumeTableModel).IsCheck = !(tbResume.SelectedItem as ResumeTableModel).IsCheck;
        }
        private async void ConnectViewShow_Click(object sender, RoutedEventArgs e)
        {
            ResumeTableModel httpResume = (sender as Button).DataContext as ResumeTableModel;
            await actionChat(httpResume.userID.ToString(), httpResume.jobID.ToString());        
        }
        private async void ConnectViewJudge_Click(object sender, RoutedEventArgs e)
        {
            string strJudge = string.Empty;
            string pResult = string.Empty;
            bool feed = true;
            switch ((sender as ICResumeButton).Tag.ToString())
            {
                case "btnJudgePass":
                    strJudge = DAL.ConfUtil.FeedbackPass;
                    feed = true;
                    pResult = "已处理为合适";
                    break;
                case "btnJudgeFail":
                    strJudge = DAL.ConfUtil.FeedbackFail;
                    feed = false;
                    pResult = "已处理为不合适";
                    break;
            }
            if ((MagicGlobal.UserInfo.IsPassTip && strJudge == DAL.ConfUtil.FeedbackPass) || (MagicGlobal.UserInfo.IsFailTip && strJudge == DAL.ConfUtil.FeedbackFail))
            {
                WinConfirmJudge pWinConfirmJudge = new WinConfirmJudge(strJudge);
                pWinConfirmJudge.Owner = Window.GetWindow(this);
                if (pWinConfirmJudge.ShowDialog() == false)
                    return;
            }
            busyCtrl.IsBusy = true;
            string pJudgeResult = await JudgeHelper.ResumeJudge(curResumeTableModel.deliveryID, feed);
            if (pJudgeResult == DAL.ConfUtil.judgeSucess)
            {
                await UpdateTable();
                DisappearShow disappear = new DisappearShow(pResult, 1);
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                if (UpdataCount != null)
                    UpdataCount();
                spJudge.Visibility = Visibility.Collapsed;
            }
            else
            {
                WinErroTip pWinErroTip = new WinErroTip(pJudgeResult);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
            }
            busyCtrl.IsBusy = false;
        }
        private async void Photo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await OpenResume((sender as System.Windows.Controls.Border).DataContext as ResumeTableModel);
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
        private void PublichJob_Click(object sender, RoutedEventArgs e)
        {
            actionOpenPublish();
        }
        #region 标记

        ResumeTableModel curPopResumeTableModel;
        //列表打开菜单
        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            curResumeTableModel = tbResume.SelectedItem as ResumeTableModel;
            if (curResumeTableModel == null)
                return;
            curPopResumeTableModel = curResumeTableModel;
            List<TagsMenuModel> menuList = new List<TagsMenuModel>();
            foreach (var item in MagicGlobal.TagsManagerList)
            {
                bool isChecked = false;
                if (curResumeTableModel.tags != null)
                {
                    foreach (var temp in curResumeTableModel.tags)
                    {
                        if (temp.id == item.Id)
                        {
                            isChecked = true;
                            break;
                        }
                    }
                }
                menuList.Add(new TagsMenuModel() { Id = item.Id, IsChecked = isChecked, TagColor = item.TagColor, TagContent = item.TagContent });
            }
        }
        private void btnTagsEdit_Click(object sender, RoutedEventArgs e)
        {
            switch (resumetype)
            {
                case ResumeTableType.initiative:
                    TrackHelper2.TrackOperation("5.5.1.11.1", "clk");
                    break;
                case ResumeTableType.invite:
                    TrackHelper2.TrackOperation("5.5.2.9.1", "clk");
                    break;
                case ResumeTableType.pass:
                    TrackHelper2.TrackOperation("5.5.3.9.1", "clk");
                    break;
                case ResumeTableType.fail:
                    TrackHelper2.TrackOperation("5.5.4.9.1", "clk");
                    break;
                case ResumeTableType.autoFilter:
                    TrackHelper2.TrackOperation("5.5.5.9.1", "clk");
                    break;
            }
            PopMenuTag.PlacementTarget = (sender as Button);
            PopMenuTag.Placement = PlacementMode.Bottom;
            curPopResumeTableModel = (sender as Button).DataContext as ResumeTableModel;
            List<TagsMenuModel> menuList = new List<TagsMenuModel>();
            foreach (var item in MagicGlobal.TagsManagerList)
            {
                bool isChecked = false;
                if (curPopResumeTableModel.tags != null)
                {
                    foreach (var temp in curPopResumeTableModel.tags)
                    {
                        if (temp.id == item.Id)
                        {
                            isChecked = true;
                            break;
                        }
                    }
                }
                menuList.Add(new TagsMenuModel() { Id = item.Id, IsChecked = isChecked, TagColor = item.TagColor, TagContent = item.TagContent });
            }
            this.popTagsList.ItemsSource = menuList;
            PopMenuTag.IsOpen = true;
        }
        private async void chkTagRemark_Click(object sender, RoutedEventArgs e)
        {
            TagsMenuModel pTagsMenuModel = (sender as CheckBox).DataContext as TagsMenuModel;
            if ((sender as CheckBox).IsChecked == true)
            {

                curPopResumeTableModel.tags.Add(new ResumeTag() { id = pTagsMenuModel.Id, name = pTagsMenuModel.TagContent, color = pTagsMenuModel.TagColor });
            }
            else
            {
                for (int i = 0; i < curPopResumeTableModel.tags.Count; i++)
                {
                    if (curPopResumeTableModel.tags[i].id == pTagsMenuModel.Id)
                    {
                        curPopResumeTableModel.tags.Remove(curPopResumeTableModel.tags[i]);
                    }
                }

            }
            await ManageTagMark(curPopResumeTableModel, (sender as CheckBox).IsChecked, pTagsMenuModel.Id);
        }
        private async Task UpdateFlag(int id, bool? add)
        {
          await  ManageTagMark(curResumeTableModel, add, id);
        }

        private async Task ManageTagMark(ResumeTableModel pResumeTableModel, bool? isCheck, int tagid)
        {
            int id = pResumeTableModel.deliveryID;
            PopMenuTag.IsOpen = false;
            if (isCheck == true)
            {
                await JudgeHelper.ResumeSign(id, tagid);
            }
            else if (isCheck == false)
            {
                await JudgeHelper.ResumeUnSign(id, tagid);
            }
        }
        

        private void btnTagSelect_Click(object sender, RoutedEventArgs e)
        {
            switch (resumetype)
            {
                case ResumeTableType.initiative:
                    TrackHelper2.TrackOperation("5.5.1.6.1", "clk");
                    break;
                case ResumeTableType.invite:
                    TrackHelper2.TrackOperation("5.5.2.4.1", "clk");
                    break;
                case ResumeTableType.pass:
                    TrackHelper2.TrackOperation("5.5.3.4.1", "clk");
                    break;
                case ResumeTableType.fail:
                    TrackHelper2.TrackOperation("5.5.4.4.1", "clk");
                    break;
                case ResumeTableType.autoFilter:
                    TrackHelper2.TrackOperation("5.5.5.4.1", "clk");
                    break;
            }
            PopMenuTagSelect.PlacementTarget = (sender as Button);
            this.SelectTagsList.ItemsSource = MagicGlobal.TagsManagerList;
            PopMenuTagSelect.IsOpen = true;
        }


        private async void btnTagSelectAll_Click(object sender, RoutedEventArgs e)
        {
            TagSelectResult.Child = new TextBlock()
            {
                Text = "按标记筛选",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b4b4b4"))
            };
            tagId = -1;
            PopMenuTagSelect.IsOpen = false;
            curPage = 1;
            await UpdateTable();
        }

        private async void btnTagMarkSelect_Click(object sender, RoutedEventArgs e)
        {
            TagSelectResult.Child = new MagicCube.View.UC.UCMenuItem() { DataContext = (sender as Button).DataContext as TagsManagerModel };
            tagId = ((sender as Button).DataContext as TagsManagerModel).Id;
            PopMenuTagSelect.IsOpen = false;
            curPage = 1;
            await UpdateTable();
        }

        private async void SearchResume_Click(object sender, RoutedEventArgs e)
        {
          await  actionSearch();
        }

        private void btnTagAdd_Click(object sender, RoutedEventArgs e)
        {
            GlobalEvent.GlobalEventHandler(this, new GlobalEventArgs(GlobalFlag.openTagsAdd, null));
            PopMenuTag.IsOpen = false;
        }

        private void btnTagManager_Click(object sender, RoutedEventArgs e)
        {
            GlobalEvent.GlobalEventHandler(this, new GlobalEventArgs(GlobalFlag.openTagsManager, null));
            PopMenuTag.IsOpen = false;
        }

        private void TagManage_Click(object sender, RoutedEventArgs e)
        {
            actionOpenTagManage(true);
            PopMenuTag.IsOpen = false;
        }
        #endregion

        #region 查看简历操作
        private void BtnResumeViewClose_Click(object sender, RoutedEventArgs e)
        {
            gdResumeView.Visibility = Visibility.Collapsed;
        }

        private async Task OpenResume(ResumeTableModel pResumeTableModel)
        {
            ucResumeDetial.iniResumeComment();
            SVUCResume.ScrollToTop();
            if (resumetype == ResumeTableType.pass || resumetype == ResumeTableType.fail)
            {
                spJudge.Visibility = Visibility.Collapsed;
            }
            else
            {
                spJudge.Visibility = Visibility.Visible;
            }
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
            if (model.code != 200)
            {
                WinErroTip pWinErroTip = new WinErroTip(model.msg);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return;
            }
            curResumeDetailModel = SetModel.SetResumeDetailModel(model.data);
            curResumeDetailModel.timeType = typeTime.Substring(0, 2);
            curResumeDetailModel.resumeTime = pResumeTableModel.resumeTime;
            curResumeDetailModel.tags = curResumeTableModel.tags;
            curResumeDetailModel.canTag = Visibility.Visible;
            curResumeDetailModel.deliveryID = curResumeTableModel.deliveryID;
            curResumeDetailModel.jobName = curResumeTableModel.jobName;
            ucResumeDetial.DataContext = curResumeDetailModel;
            btnCollection.DataContext = curResumeDetailModel;
            
            gdResumeView.Visibility = Visibility.Visible;
        }

        private async void btnChat_Click(object sender, RoutedEventArgs e)
        {
            await actionChat(curResumeTableModel.userID.ToString(), curResumeTableModel.jobID.ToString());
        }

        private async void btnPass_Click(object sender, RoutedEventArgs e)
        {
            string strRusult = string.Empty;
            if (MagicGlobal.UserInfo.IsPassTip)
            {
                WinConfirmJudge pWinConfirmJudge = new WinConfirmJudge("合适");
                pWinConfirmJudge.Owner = Window.GetWindow(this);
                if (pWinConfirmJudge.ShowDialog() == false)
                    return;
            }
            busyCtrl.IsBusy = true;
            string pJudgeResult = await JudgeHelper.ResumeJudge(curResumeTableModel.deliveryID, true);
            if (pJudgeResult == DAL.ConfUtil.judgeSucess)
            {
                await UpdateTable();
                DisappearShow disappear = new DisappearShow("已处理为合适", 1);
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                if (UpdataCount != null)
                    UpdataCount();
                spJudge.Visibility = Visibility.Collapsed;
            }
            else
            {
                WinErroTip pWinErroTip = new WinErroTip(pJudgeResult);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
            }
            busyCtrl.IsBusy = false;
        }

        private async void btnFail_Click(object sender, RoutedEventArgs e)
        {

            string strRusult = string.Empty;
            if (MagicGlobal.UserInfo.IsFailTip)
            {
                WinConfirmJudge pWinConfirmJudge = new WinConfirmJudge("不合适");
                pWinConfirmJudge.Owner = Window.GetWindow(this);
                if (pWinConfirmJudge.ShowDialog() == false)
                    return;
            }
            busyCtrl.IsBusy = true;
            string pJudgeResult = await JudgeHelper.ResumeJudge(curResumeTableModel.deliveryID, false);
            if (pJudgeResult == DAL.ConfUtil.judgeSucess)
            {
                await UpdateTable();
                DisappearShow disappear = new DisappearShow("已处理为不合适", 1);
                disappear.Owner = Window.GetWindow(this);
                disappear.ShowDialog();
                if (UpdataCount != null)
                    UpdataCount();
                spJudge.Visibility = Visibility.Collapsed;
            }
            else
            {
                WinErroTip pWinErroTip = new WinErroTip(pJudgeResult);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
            }
            busyCtrl.IsBusy = false ;
        }

       
        private void PhoneAction()
        {
            //TrackAction.TrackSet("OPEN_PRIVACY", null, "INTERVIEWER_CALL_BY_PHONE");
            //TrackAction.TrackSet("Face_INTERACTION", curResumeDetailModel.contactRecord.uniqueKey);


            //WinConfirmTip pWinPhoneHint = new WinConfirmTip("免费电话为双向呼叫模式，请您在接到呼叫后耐心等待对方应答，是否确认拨打？");
            //pWinPhoneHint.Owner = Window.GetWindow(this);
            //if (pWinPhoneHint.ShowDialog() == true)
            //{
            //    busyCtrl.IsBusy = true;
            //    Action action = new Action(() =>
            //    {
            //        if (!curResumeDetailModel.resume.showContactInformation)
            //        {
            //            if (PayForContact(ConfUtil.ServerPayForPhone))
            //            {
            //                PhoneCall();
            //            }
            //        }
            //        else
            //        {
            //            PhoneCall();
            //        }
            //        this.Dispatcher.BeginInvoke(new Action(() =>
            //        {
            //            busyCtrl.IsBusy = false;
            //        }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            //    });
            //    action.BeginInvoke(null, null);
            //}
        }

        private void SeeCall()
        {
           

            //busyCtrl.IsBusy = true;
            //Action action = new Action(() =>
            //{
            //    PayForContact(ConfUtil.ServerPayForShow);

            //    this.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        busyCtrl.IsBusy = false;
            //    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            //});
            //action.BeginInvoke(null, null);
        }

        private void PhoneCall()
        {

            //string pResult = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerCallFor, JsonUtil.CallFor(curResumeDetailModel.contactRecord.id));
            //if (!pResult.Contains("电话拨打成功"))
            //{
                

            //    this.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        WinErroTip pWinMessage = new WinErroTip("连接失败，电话无法拨出");
            //        pWinMessage.Owner = Window.GetWindow(this);
            //        pWinMessage.ShowDialog();
            //    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            //}
            //else
            //{
                
            //    this.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        WinConfirmTip pWinMessage = new WinConfirmTip("电话已拨打,此拨打为双呼模式，请注意接听来电~");
            //        pWinMessage.Owner = Window.GetWindow(this);
            //        pWinMessage.ShowDialog();
            //    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            //}
        }
        #endregion

        private void cbJob_DropDownClosed(object sender, EventArgs e)
        {
            switch (resumetype)
            {
                case ResumeTableType.initiative:
                    TrackHelper2.TrackOperation("5.5.1.5.1", "clk");
                    break;
                case ResumeTableType.invite:
                    TrackHelper2.TrackOperation("5.5.2.3.1", "clk");
                    break;
                case ResumeTableType.pass:
                    TrackHelper2.TrackOperation("5.5.3.3.1", "clk");
                    break;
                case ResumeTableType.fail:
                    TrackHelper2.TrackOperation("5.5.4.3.1", "clk");
                    break;
                case ResumeTableType.autoFilter:
                    TrackHelper2.TrackOperation("5.5.5.3.1", "clk");
                    break;
            }
        }

        private void rdoList_Click(object sender, RoutedEventArgs e)
        {
            switch (resumetype)
            {
                case ResumeTableType.initiative:
                    TrackHelper2.TrackOperation("5.5.1.8.1", "clk");
                    break;
                case ResumeTableType.invite:
                    TrackHelper2.TrackOperation("5.5.2.6.1", "clk");
                    break;
                case ResumeTableType.pass:
                    TrackHelper2.TrackOperation("5.5.3.6.1", "clk");
                    break;
                case ResumeTableType.fail:
                    TrackHelper2.TrackOperation("5.5.4.6.1", "clk");
                    break;
                case ResumeTableType.autoFilter:
                    TrackHelper2.TrackOperation("5.5.5.6.1", "clk");
                    break;
            }
        }

        private void rdoThumbnail_Click(object sender, RoutedEventArgs e)
        {
            switch (resumetype)
            {
                case ResumeTableType.initiative:
                    TrackHelper2.TrackOperation("5.5.1.9.1", "clk");
                    break;
                case ResumeTableType.invite:
                    TrackHelper2.TrackOperation("5.5.2.7.1", "clk");
                    break;
                case ResumeTableType.pass:
                    TrackHelper2.TrackOperation("5.5.3.7.1", "clk");
                    break;
                case ResumeTableType.fail:
                    TrackHelper2.TrackOperation("5.5.4.7.1", "clk");
                    break;
                case ResumeTableType.autoFilter:
                    TrackHelper2.TrackOperation("5.5.5.7.1", "clk");
                    break;
            }
        }

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
                        if (pBaseHttpModel.data.total == 0)
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
                    else if (pBaseHttpModel.code == -133)
                    {
                        WinClearCollection pWinErroTip = new WinClearCollection();
                        pWinErroTip.Owner = Window.GetWindow(this);
                        if (pWinErroTip.ShowDialog() == true)
                        {
                            if (actionClearCollection != null)
                                await actionClearCollection();
                        }
                    }
                }
            }

        }

        private async void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            if (curResumeDetailModel != null)
            {
                curResumeDetailModel.downloadType = "candidate";
                await   DownloadResume.DownLoadResume(curResumeDetailModel,Window.GetWindow(this),curResumeTableModel.jobName);
            }
        }

        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (curResumeDetailModel != null)
            {
                WinSendEmail pWinSendEmail = new WinSendEmail(Window.GetWindow(this),curResumeDetailModel.userID, curResumeDetailModel.name, curResumeTableModel.jobID, curResumeTableModel.jobName);
                //pWinSendEmail.Owner = Window.GetWindow(this);
                pWinSendEmail.ShowDialog();
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

        private async void cbViewContect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbViewContect.SelectedItem == null)
                return;
            curPage = 1;
          
            if (curViewContact == -2)
            {
                return;
            }
            if (cbViewContect.SelectedValue.ToString() == "下载联系方式")
            {
                curViewContact = -1;
            }            
            else if (cbViewContect.SelectedValue.ToString() == "已下载")
            {
                curViewContact = 1;
            }
            else if (cbViewContect.SelectedValue.ToString() == "未下载")
            {
                curViewContact = 0;
            }
            await UpdateTable();

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
