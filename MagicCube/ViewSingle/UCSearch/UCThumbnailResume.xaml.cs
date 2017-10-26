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
using MagicCube.HttpModel;
using MagicCube.Common;

using MagicCube.Model;
using MagicCube.View.Message;
using System.Collections.ObjectModel;
using MagicCube.TemplateUC;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCThumbnailResume.xaml 的交互逻辑
    /// </summary>
    public partial class UCThumbnailResume : UserControl
    {
        //public Action<SearchTalentResumModel> ResumeMoreShow;
        public delegate Task ResumeAction(SearchTalentResumModel model);
        /// <summary>
        /// 查看简历
        /// </summary>
        public ResumeAction ResumeMoreShow;
        /// <summary>
        /// 立即沟通
        /// </summary>
        public ResumeAction ResumOfferConnectToSingleAction;
        /// <summary>
        /// 职位邀请
        /// </summary>
        public ResumeAction ResumSendOfferAciton;

        public Action<bool> IsCheckedChangedAction;
        public UCThumbnailResume()
        {
            InitializeComponent();
            //this.lstThumbnailResume.ItemsSource = this.SearchTalentResumModelList;
            //this.popTagsList.ItemsSource = MagicGlobal.TagsManagerList;

        }


        public List<SearchTalentResumModel> SearchTalentResumModelList
        {
            get { return (List<SearchTalentResumModel>)GetValue(SearchTalentResumModelListProperty); }
            set { SetValue(SearchTalentResumModelListProperty, value); }

        }

        // Using a DependencyProperty as the backing store for SearchTalentResumModelList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTalentResumModelListProperty =
            DependencyProperty.Register("SearchTalentResumModelList", typeof(List<SearchTalentResumModel>), typeof(UCThumbnailResume));

        private async void ResumeMoreShow_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            SearchTalentResumModel resultItem = btn.DataContext as SearchTalentResumModel;
            if (ResumeMoreShow != null)
                await ResumeMoreShow(resultItem);
        }

        private void ConnectViewShow_Click(object sender, RoutedEventArgs e)
        {
           
            Button btn = sender as Button;
            SearchTalentResumModel resultItem = btn.DataContext as SearchTalentResumModel;
            if (ResumSendOfferAciton != null)
                ResumSendOfferAciton(resultItem);
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (IsCheckedChangedAction != null)
            {
                IsCheckedChangedAction((bool)chk.IsChecked);
            }
        }

        private void lstThumbnailResume_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image img = e.OriginalSource as System.Windows.Controls.Image;
            if (img != null)
                return;
            e.Handled = true;
            Point aP = e.GetPosition(this.lstThumbnailResume);
            IInputElement obj = this.lstThumbnailResume.InputHitTest(aP);
            System.Windows.DependencyObject target = obj as System.Windows.DependencyObject;


            while (target != null)
            {
                if(target is Button)
                {
                    e.Handled = true;
                }
                if (target is Grid)
                {
                    if ((target as Grid).Name == "bdRow")
                    {
                        SearchTalentResumModel tempModel = (target as Grid).DataContext as SearchTalentResumModel;
                        ResumeMoreShow(tempModel);

                        return;
                    }
                }
                target = VisualTreeHelper.GetParent(target);

            }
        }
        private void lstThumbnailResum_RightClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point aP = e.GetPosition(this.lstThumbnailResume);
            IInputElement obj = this.lstThumbnailResume.InputHitTest(aP);
            System.Windows.DependencyObject target = obj as System.Windows.DependencyObject;


            while (target != null)
            {
                if (target is Grid)
                {
                    if ((target as Grid).Name == "bdRow")
                    {
                        //ResumeMoreShow((target as Border).DataContext as SearchTalentResumModel);
                        this.currentTagsEditItem = (target as Grid).DataContext as SearchTalentResumModel;
                        return;
                    }
                }
                target = VisualTreeHelper.GetParent(target);

            }
        }


        private async void DatagridRight_MouseClick(object sender, RoutedEventArgs e)
        {
            if (this.currentTagsEditItem != null)
            {
                if (ResumeMoreShow != null)
                    await ResumeMoreShow(currentTagsEditItem);
            }
        }
        /// <summary>
        /// 右键免费沟通按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectRightMenu_MouseClick(object sender, RoutedEventArgs e)
        {
            if (ResumOfferConnectToSingleAction != null)
                ResumOfferConnectToSingleAction(currentTagsEditItem);
            //先判断是否有职位发布
            //string mResult = DAL.HttpHelper.Instance.HttpGet(ConfUtil.ServerGetJobPublish);
            //if (mResult.StartsWith("连接失败"))
            //{
            //    WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
            //    pWinMessage.Owner = Window.GetWindow(this);
            //    pWinMessage.ShowDialog();
            //    this.Cursor = Cursors.Arrow;
            //    return;
            //}
            //ObservableCollection<JobPublish> tempJobPublish = DAL.JsonHelper.ToObject<ObservableCollection<JobPublish>>(mResult);
            //if (tempJobPublish == null)
            //{
            //    WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
            //    pWinMessage.Owner = Window.GetWindow(this);
            //    pWinMessage.ShowDialog();
            //    this.Cursor = Cursors.Arrow;
            //    return;
            //}
            //if (tempJobPublish.Count == 0)
            //{
            //    this.Cursor = Cursors.Arrow;
            //    WinPublishJobMessage msg = new WinPublishJobMessage();
            //    msg.Owner = Window.GetWindow(this);
            //    msg.atConfirm += AtConfirmCallback;
            //    msg.ShowDialog();

            //    return;
            //}


            //if (this.currentTagsEditItem != null)
            //{
            //    //打开职位列表选择对话框
            //    GlobalEvent.GlobalEventHandler(this.currentTagsEditItem, new GlobalEventArgs(GlobalFlag.JobPublishList, tempJobPublish));
            //}






        }


        private void ConnectToSingle_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentTagsEditItem != null)
            {
                if (ResumSendOfferAciton != null)
                {
                    //ResumOfferConnectToSingleAction(currentTagsEditItem);
                    ResumSendOfferAciton(currentTagsEditItem);
                }
            }
        }


        #region "标签管理"



        private void chkRedFlag_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    this.Cursor = Cursors.Wait;
            //    CheckBox chk = sender as CheckBox;
            //    SearchTalentResumModel sch = chk.DataContext as SearchTalentResumModel;
            //    if (sch == null)
            //    {
            //        this.Cursor = Cursors.Arrow;
            //        return;
            //    }
            //    sch.flag = (sch.RedFlag).ToString();
            //    HttpResumeGetTags httpResumeTag = new HttpResumeGetTags() { resumeId = sch.resumeId, flag = sch.RedFlag.ToString(), tags = new Dictionary<string, string>() };
            //    if (sch.tagVs == null)
            //        sch.tagVs = new List<TagV>();
            //    foreach (var item in sch.tagVs)
            //    {
            //        httpResumeTag.tags.Add(item.id.ToString(), item.name);
            //    }
            //    string httpSend = MagicCube.DAL.JsonHelper.ToJsonString(httpResumeTag);
            //    string result = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerResumeRemarkTags, httpSend);
            //    if (!result.Contains("200"))
            //    {
            //        WinErroTip message = new WinErroTip("网络异常，请重试");
            //        message.Owner = Window.GetWindow(this);
            //        message.ShowDialog();
            //        sch.RedFlag = !sch.RedFlag;
            //        sch.flag = (!sch.RedFlag).ToString();
            //    }
            //    Console.WriteLine("打红标结果:" + result);
            //    this.Cursor = Cursors.Arrow;
            //}
            //catch
            //{
            //    this.Cursor = Cursors.Arrow;
            //}

        }

        private void chkRightMenuRedFlag_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    this.Cursor = Cursors.Wait;

            //    CheckBox chk = sender as CheckBox;
            //    SearchTalentResumModel sch = currentTagsEditItem;
            //    sch.RedFlag = (bool)chk.IsChecked;
            //    sch.flag = sch.RedFlag.ToString();
            //    HttpResumeGetTags httpResumeTag = new HttpResumeGetTags() { resumeId = sch.resumeId, flag = sch.RedFlag.ToString(), tags = new Dictionary<string, string>() };
            //    if (sch.tagVs == null)
            //        sch.tagVs = new List<TagV>();
            //    foreach (var item in sch.tagVs)
            //    {
            //        httpResumeTag.tags.Add(item.id.ToString(), item.name);
            //    }
            //    string httpSend = MagicCube.DAL.JsonHelper.ToJsonString(httpResumeTag);
            //    string result = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerResumeRemarkTags, httpSend);
            //    if (!result.Contains("200"))
            //    {
            //        WinErroTip message = new WinErroTip("网络异常，请重试");
            //        message.Owner = Window.GetWindow(this);
            //        message.ShowDialog();
            //        sch.RedFlag = !sch.RedFlag;
            //    }

            //    Console.WriteLine("打红标结果:" + result);
            //    // string sendStr = Util.JsonUtil.ToJsonString(sendHttp);
            //    //string result =  DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerResumeRemarkTags, sendStr);
            //    //Console.WriteLine(result);
            //    this.Cursor = Cursors.Arrow;
            //}
            //catch
            //{
            //    this.Cursor = Cursors.Arrow;
            //}
        }

        private SearchTalentResumModel currentTagsEditItem;
        private List<TagsMenuModel> menuList;

        private void btnTagsEdit_Click(object sender, RoutedEventArgs e)
        {
            PopMenu.PlacementTarget = (sender as Button);
            currentTagsEditItem = (sender as Button).DataContext as SearchTalentResumModel;
            menuList = new List<TagsMenuModel>();
            foreach (var item in MagicGlobal.TagsManagerList)
            {
                bool isChecked = false;
                if (currentTagsEditItem.tagVs != null)
                {
                    foreach (var temp in currentTagsEditItem.tagVs)
                    {
                        if (temp == null)
                            continue;
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
            PopMenu.IsOpen = true;
        }

        private void chkTagRemark_Click(object sender, RoutedEventArgs e)
        {
            //this.Cursor = Cursors.Wait;
            //if (currentTagsEditItem == null)
            //    return;
            //CheckBox chk = sender as CheckBox;
            //if (chk == null)
            //    return;
            //TagsMenuModel tagsModel = chk.DataContext as TagsMenuModel;
            //if (tagsModel != null)
            //{
            //    string sendHttp = string.Empty;
            //    HttpResumeGetTags resumeGetTags = new HttpResumeGetTags() { uniqueKey = currentTagsEditItem.uniqueKey, tags = new Dictionary<string, string>() };
            //    foreach (var item in menuList)
            //    {
            //        if (item.IsChecked)
            //        {
            //            resumeGetTags.tags.Add(item.Id.ToString(), item.TagContent);
            //        }
            //    }
            //    if (currentTagsEditItem.RedFlag)
            //    {
            //        resumeGetTags.flag = "True";
            //    }
            //    else
            //    {
            //        resumeGetTags.flag = "False";
            //    }
            //    //if(resumeGetTags.resumeId == 0)
            //    //{
            //    //    resumeGetTags.uniqueKey = currentTagsEditItem.uniqueKey;
            //    //    sendHttp = Util.JsonUtil.ToJsonString(resumeGetTags);
            //    //    sendHttp = sendHttp.Replace("\"resumeId\": 0,", "");
            //    //}

            //    sendHttp = MagicCube.DAL.JsonHelper.ToJsonString(resumeGetTags);
            //    sendHttp = sendHttp.Replace("\"resumeId\": 0,", "");
            //    string result = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerResumeRemarkTags, sendHttp);
            //    if (result.Contains("200"))
            //    {
            //        currentTagsEditItem.TagsList.Clear();
            //        if (currentTagsEditItem.tagVs == null)
            //            currentTagsEditItem.tagVs = new List<TagV>();
            //        currentTagsEditItem.tagVs.Clear();
            //        foreach (var item in menuList)
            //        {
            //            if (item.IsChecked)
            //            {
            //                //currentTagsEditItem.TagsList.Add(item.TagColor);
            //                currentTagsEditItem.TagsList.Add(new TagsManagerModel() { TagColor = item.TagColor, TagContent = item.TagContent });
            //                currentTagsEditItem.tagVs.Add(new TagV() { color = item.TagColor, name = item.TagContent, id = item.Id });
            //            }
            //        }
            //    }
            //    Console.WriteLine("打标签结果:" + result);
            //}
            //this.Cursor = Cursors.Arrow;
        }
        #endregion

        private void btnTagManager_Click(object sender, RoutedEventArgs e)
        {
            GlobalEvent.GlobalEventHandler(this, new GlobalEventArgs(GlobalFlag.openTagsManager, null));
            PopMenu.IsOpen = false;
            this.contextMenu.IsOpen = false;
        }

        /// <summary>
        /// 右键菜单打开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            SearchTalentResumModel sch = currentTagsEditItem;
            if (sch == null)
                return;
            currentTagsEditItem = sch;
            menuList = new List<TagsMenuModel>();
            foreach (var item in MagicGlobal.TagsManagerList)
            {
                bool isChecked = false;
                if (sch.tagVs != null)
                {
                    foreach (var temp in sch.tagVs)
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
            //this.popTagsList2.ItemsSource = menuList;
           // this.chkRightMenuRedFlag.IsChecked = sch.RedFlag;
        }

        private void btnTagAdd_Click(object sender, RoutedEventArgs e)
        {
            GlobalEvent.GlobalEventHandler(this, new GlobalEventArgs(GlobalFlag.openTagsAdd, null));
            PopMenu.IsOpen = false;
            this.contextMenu.IsOpen = false;
        }

        private async void Photo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SearchTalentResumModel tempModel = (sender as Border).DataContext as SearchTalentResumModel;
            await ResumeMoreShow(tempModel);
        }

        private async void Name_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            SearchTalentResumModel tempModel = (sender as TextBlock).DataContext as SearchTalentResumModel;
            await ResumeMoreShow(tempModel);
        }
    }
}
