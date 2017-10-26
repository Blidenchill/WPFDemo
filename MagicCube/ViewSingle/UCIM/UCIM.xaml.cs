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
using MagicCube.Model;
using MagicCube.Common;
using System.Collections.ObjectModel;
using MagicCube.TemplateUC;
using System.Threading.Tasks;
using MagicCube.ViewModel;
using MagicCube.View.Message;
using System.IO;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCIM2.xaml 的交互逻辑
    /// </summary>
    public partial class UCIM : UserControl
    {
        #region "变量"

        public Action actionOpenPublish;
        public delegate Task delegateSearch();
        public delegateSearch actionSearch;
        public delegate Task delegateClearCollection();
        public delegateClearCollection actionClearCollection;
        private enum TabNameEnum
        {
            JobDynamic,
            JobRecoomend,
            RecentList,
        }
        #endregion

        #region "常量"
        private const int MESSAGE_TYPE_SYS = 0;
        private const int MESSAGE_TYPE_SEND = 1;
        private const int MESSAGE_TYPE_RECV = 2;
        private const int MESSAGE_TYPE_TIME = 3;
        private const int MESSAGE_JOBOFFER = 4;
        private const int MESSAGE_PHONESUCESS = 5;
        private const int MESSAGE_PHONEFAIL = 6;
        private const int MESSAGE_BUTTONLINK = 7;

        public static int ADDTALK_SEND = 0;
        public static int ADDTALK_DYNAMIC = 1;
        public static int ADDTALK_TALENT = 2;
        public static int AddTALK_DYNAMIC_UNREAD = 3;
        #endregion
        public UCIM()
        {
            InitializeComponent();
            this.Loaded += UCIM_Loaded;
            this.ucResumeDetail.ucb.Click += TrackOperation_Event;
            this.ucResumeDetail.btnFreePhone.Click += TrackOperation_Event;
            this.ucResumeDetail.btnVideo.Click += TrackOperation_Event;
       
        }

        public async Task UCIMInitial()
        {
            await InitialJobList();       
            await IniIniJobDynamic();
            await IniJobDynamicList();
            await IniRecommendList();
            //await SetViewJobRed();
            await InitialQuipReply();
    


        }

        

        void UCIM_Loaded(object sender, RoutedEventArgs e)
        {
            this.lstRecent.ItemsSource = m_RecentChatList;
            this.lstRecommend.ItemsSource = jobRecommendModelList;
            this.lstJobDynamic.ItemsSource = jobDynamicModelList;
            this.lstQuipReply.ItemsSource = MagicGlobal.ContentCommonReplyList;
        }


        private void BtnResumeViewClose_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)this.RBRecommend.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.6.7.1", "clk");
               
            }
            else if ((bool)this.RBInterest.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.4.3.1", "clk");
                
            }
            else if ((bool)this.RBRecnt.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.2.3.1", "clk");
            }
            this.curIMConotrol.IsOpenResum = false;
        }

        private async void btnChat_Click(object sender, RoutedEventArgs e)
        {
            IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
            //埋点
            if ((bool)this.RBRecommend.IsChecked)
            {
                ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                eventDataModel.session = this.jobRecommendSession;
                eventDataModel.algo = this.algorithmID;
                eventDataModel.item = new List<string>();
                eventDataModel.item.Add(temp.UserID.ToString());
                for (int i = 0; i < jobRecommendModelList.Count; i++)
                {
                    if (jobRecommendModelList[i].UserID == temp.UserID)
                    {
                        eventDataModel.location = i.ToString();
                        break;
                    }
                }
                eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                Common.TrackHelper2.TrackOperation("5.2.6.13.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
            }
            else if ((bool)this.RBInterest.IsChecked)
            {
                ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                //eventDataModel.session = Guid.NewGuid().ToString(); ;
                eventDataModel.session = this.jobDynamicSession;
                eventDataModel.algo = "0";
                eventDataModel.item = new List<string>();
                eventDataModel.item.Add(temp.UserID.ToString());
                for (int i = 0; i < jobDynamicModelList.Count; i++)
                {
                    if (jobDynamicModelList[i].UserID == temp.UserID)
                    {
                        eventDataModel.location = i.ToString();
                        break;
                    }
                }
                eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                Common.TrackHelper2.TrackOperation("5.2.4.8.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
            }
            else if ((bool)this.RBRecnt.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.2.8.1", "clk");
            }


            if(RBRecnt.IsChecked == false)
                await SetIMRecord(temp);
            this.curIMConotrol.IsOpenResum = false;       
        }
        private void PublishLink_Click(object sender, RoutedEventArgs e)
        {
            actionOpenPublish();
        }

        private async void SearchLink_Click(object sender, RoutedEventArgs e)
        {
            await  actionSearch();
        }

        private async void Phone_Click(object sender, RoutedEventArgs e)
        {
            IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
            //埋点
            if ((bool)this.RBRecommend.IsChecked)
            {
                ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                eventDataModel.session = this.jobRecommendSession;
                eventDataModel.algo = this.algorithmID;
                eventDataModel.item = new List<string>();
                eventDataModel.item.Add(temp.UserID.ToString());
                for (int i = 0; i < jobRecommendModelList.Count; i++)
                {
                    if (jobRecommendModelList[i].UserID == temp.UserID)
                    {
                        eventDataModel.location = i.ToString();
                        break;
                    }
                }
                eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                Common.TrackHelper2.TrackOperation("5.2.5.7.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
            }
            else if ((bool)this.RBInterest.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.3.6.1", "clk");
            }
            else if ((bool)this.RBRecnt.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.1.8.1", "clk");
            }


            WinPhoneCall pWinPhoneHint = new WinPhoneCall(Window.GetWindow(this));
            if (pWinPhoneHint.ShowDialog() == true)
            {
                ViewModel.BaseHttpModel<HttpPhoneRoot> pBaseHttpModel = await IMHelper.FreePhone(curIMConotrol.UserID);
                if (pBaseHttpModel != null)
                {
                    if (pBaseHttpModel.code != 200)
                    {
                        if (pBaseHttpModel.code == -129)
                        {
                            WinMessageLink pWinErroTip = new WinMessageLink("很抱歉，对方可能不方便接听电话\r\n请您在每日（9:00-21:00）联系TA", "知道了");
                            pWinErroTip.Owner = Window.GetWindow(this);
                            pWinErroTip.ShowDialog();
                        }
                        else if(pBaseHttpModel.code == -503)
                        {
                            WinMessageLink pWinErroTip = new WinMessageLink("今天已经给TA打过5次电话了，已达上限", "知道了");
                            pWinErroTip.Owner = Window.GetWindow(this);
                            pWinErroTip.ShowDialog();
                        }
                        else  if(pBaseHttpModel.code == -132)
                        {
                            WinPayMB pWinPayMB = new WinPayMB();
                            pWinPayMB.Owner = Window.GetWindow(this);
                            pWinPayMB.ShowDialog();
                        }
                    }
                    else
                    {
                        curIMConotrol.ResumeDetail.mobile = pBaseHttpModel.data.contact.mobile;
                        curIMConotrol.ResumeDetail.viewContact = true;
                    }

                }
                
            }
        }

        private void gdMessageAndResume_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                IMControlModel oldModel = e.OldValue as IMControlModel;
                if (oldModel != null)
                    oldModel.SendMessageCache = this.TbConvrInput.Text;

                IMControlModel newModel = e.NewValue as IMControlModel;
                if (newModel != null)
                {
                    this.TbConvrInput.Text = newModel.SendMessageCache;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        private void lstQuipReply_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //埋点
            if ((bool)this.RBRecommend.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.5.9.1", "clk");

            }
            else if ((bool)this.RBInterest.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.3.8.1", "clk");

            }
            else if ((bool)this.RBRecnt.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.1.11.1", "clk");
            }

            ListBox lst = sender as ListBox;
            if (lst.SelectedItem == null)
                return;
            ContentCommonReply content = lst.SelectedItem as ContentCommonReply;
            if (content == null)
                return;
            string body = content.context;
            TbConvrInput.Text = body;
            TbConvrInput.SelectionStart = TbConvrInput.Text.Length;
            TbConvrInput.Focus();
            //BtnSend_Click(BtnSend, null);

            this.PopupQuipReplySetting.IsOpen = false;
        }

        private void Video_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            if ((bool)this.RBRecommend.IsChecked)
            {
                IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
                ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                eventDataModel.session = this.jobRecommendSession;
                eventDataModel.algo = this.algorithmID;
                eventDataModel.item = new List<string>();
                eventDataModel.item.Add(temp.UserID.ToString());
                for (int i = 0; i < jobRecommendModelList.Count; i++)
                {
                    if (jobRecommendModelList[i].UserID == temp.UserID)
                    {
                        eventDataModel.location = i.ToString();
                        break;
                    }
                }
                eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                Common.TrackHelper2.TrackOperation("5.2.5.8.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
            }
            else if ((bool)this.RBInterest.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.3.7.1", "clk");
            }
            else if ((bool)this.RBRecnt.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.1.9.1", "clk");
            }

            WinMessageLink pWinConfirmTip = new WinMessageLink("此功能即将上线，敬请期待！","我知道了");
            pWinConfirmTip.Owner = Window.GetWindow(this);
            pWinConfirmTip.ShowDialog();
        }

        private void chkResumeOpen_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            if((bool)this.RBRecommend.IsChecked)
            {
                IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
                ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                eventDataModel.session = this.jobRecommendSession;
                eventDataModel.algo = this.algorithmID;
                eventDataModel.item = new List<string>();
                eventDataModel.item.Add(temp.UserID.ToString());
                for (int i = 0; i < jobRecommendModelList.Count; i++)
                {
                    if (jobRecommendModelList[i].UserID == temp.UserID)
                    {
                        eventDataModel.location = i.ToString();
                        break;
                    }
                }
                eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                Common.TrackHelper2.TrackOperation("5.2.5.6.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                Common.TrackHelper2.TrackOperation("5.2.6.1.1", "pv");

            }
            else if((bool)this.RBInterest.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.3.2.1", "clk");
                Common.TrackHelper2.TrackOperation("5.2.4.1.1", "pv");
            }
            else if ((bool)this.RBRecnt.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.1.7.1", "clk");
                Common.TrackHelper2.TrackOperation("5.2.2.1.1", "pv");
            }


            SVUCResume.ScrollToTop();
            curIMConotrol.IsOpenResum = true;
        }


        private async Task<ResumeDetailModel> openResume(IMControlModel pControl)
        {
            if (pControl == null)
                return null;
            busyCtrl.IsBusy = true;
            BaseHttpModel<HttpResumeDetial> model = await IMHelper.GetDetialReusme(pControl.UserID.ToString());
            busyCtrl.IsBusy = false;
            if (model == null)
            {
                WinErroTip pWinErroTip = new WinErroTip(DAL.ConfUtil.netError);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return null;
            }
            if (model.code != 200)
            {
                WinErroTip pWinErroTip = new WinErroTip(model.msg);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return null;;
            }
            ResumeDetailModel pResumeDetailModel = SetModel.SetResumeDetailModel(model.data);
            if (pResumeDetailModel.accountStatus == -3 || pResumeDetailModel.accountStatus == -1 || pResumeDetailModel.accountStatus == -4)
            {
                bdFreeze.Visibility = Visibility.Visible;
            }
            else
            {
                bdFreeze.Visibility = Visibility.Collapsed;
            }
            pResumeDetailModel.jobName = curIMConotrol.job;
            if ((bool)this.RBRecommend.IsChecked)
            {
                //埋点
                ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                eventDataModel.session = this.jobRecommendSession;
                eventDataModel.algo = this.algorithmID;
                eventDataModel.item = new List<string>();
                eventDataModel.item.Add(pControl.UserID.ToString());
                for (int i = 0; i < jobRecommendModelList.Count; i++)
                {
                    if (jobRecommendModelList[i].UserID == pControl.UserID)
                    {
                        eventDataModel.location = i.ToString();
                        break;
                    }
                }
                eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                Common.TrackHelper2.TrackOperation("5.2.6.2.1", "lv", DAL.JsonHelper.ToJsonString(eventDataModel));
            }
            else if((bool)this.RBInterest.IsChecked)
            {
                //埋点
                ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                //eventDataModel.session = Guid.NewGuid().ToString();
                eventDataModel.session = this.jobDynamicSession;
                eventDataModel.algo = "0";
                eventDataModel.item = new List<string>();
                eventDataModel.item.Add(pControl.UserID.ToString());
                for (int i = 0; i < jobDynamicModelList.Count; i++)
                {
                    if (jobDynamicModelList[i].UserID == pControl.UserID)
                    {
                        eventDataModel.location = i.ToString();
                        break;
                    }
                }
                eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                Common.TrackHelper2.TrackOperation("5.2.4.11.1", "lv", DAL.JsonHelper.ToJsonString(eventDataModel));
            }

            ucResumeDetail.iniResumeComment();
            return pResumeDetailModel;
        }


        /// <summary>
        /// 埋点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackOperation_Event(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            switch(fe.Name)
            {
                case "RBRecnt":
                    Common.TrackHelper2.TrackOperation("5.2.1.3.1", "clk");
                    Common.TrackHelper2.TrackOperation("5.2.1.4.1", "pv");
                    cbJobRecent.SelectedIndex = 0;
                    IMControlModel tempList = lstRecent.SelectedItem as IMControlModel;
                    //if(lstRecent.SelectedIndex == 0)
                    //{
                    //    Grid.SetRow(this.SvConversation, 1);
                    //    Grid.SetRowSpan(this.SvConversation, 3);
                    //    this.SvConversation.Padding = new Thickness(0, 0, 0, 50);
                    //    btnResumOpen.Visibility = Visibility.Collapsed;
                    //}
                    if(tempList.prior == "99")
                    {
                        Grid.SetRow(this.SvConversation, 1);
                        Grid.SetRowSpan(this.SvConversation, 3);
                        this.SvConversation.Padding = new Thickness(0, 0, 0, 50);
                        btnResumOpen.Visibility = Visibility.Collapsed;
                    }
                    break;
                case "RBInterest":
                    Common.TrackHelper2.TrackOperation("5.2.3.1.1", "clk");
                    Common.TrackHelper2.TrackOperation("5.2.3.2.1","pv");
                    cbJobDynamic.SelectedIndex = 0;
                    Grid.SetRow(this.SvConversation, 2);
                    Grid.SetRowSpan(this.SvConversation, 1);
                    btnResumOpen.Visibility = Visibility.Visible;
                    SpConversation.Margin = new Thickness(0, 0, 0, 0);
                    this.SvConversation.Padding = new Thickness(0, 0, 0, 0);
                    InterestNew.Visibility = Visibility.Collapsed;
                    break;
                case "RBRecommend":
                    Common.TrackHelper2.TrackOperation("5.2.5.1.1", "clk");
                    Common.TrackHelper2.TrackOperation("5.2.5.2.1", "pv");
                    cbJobRecommend.SelectedIndex = 0;
                    Grid.SetRow(this.SvConversation, 2);
                    Grid.SetRowSpan(this.SvConversation, 1);
                    btnResumOpen.Visibility = Visibility.Visible;
                    SpConversation.Margin = new Thickness(0, 0, 0, 0);
                    this.SvConversation.Padding = new Thickness(0, 0, 0, 0);
                    break;
                    //查看联系方式
                case "ucb":
                    //埋点
                    if ((bool)this.RBRecommend.IsChecked)
                    {
                        IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
                        ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                        eventDataModel.session = Guid.NewGuid().ToString(); ;
                        eventDataModel.algo = this.algorithmID;
                        eventDataModel.item = new List<string>();
                        eventDataModel.item.Add(temp.UserID.ToString());
                        for (int i = 0; i < jobRecommendModelList.Count; i++)
                        {
                            if (jobRecommendModelList[i].UserID == temp.UserID)
                            {
                                eventDataModel.location = i.ToString();
                                break;
                            }
                        }
                        eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                        Common.TrackHelper2.TrackOperation("5.2.6.10.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                    }
                    else if ((bool)this.RBInterest.IsChecked)
                    {
                        IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
                        ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                        //eventDataModel.session = Guid.NewGuid().ToString(); ;
                        eventDataModel.session = this.jobDynamicSession;
                        eventDataModel.algo = "0";
                        eventDataModel.item = new List<string>();
                        eventDataModel.item.Add(temp.UserID.ToString());
                        for (int i = 0; i < jobDynamicModelList.Count; i++)
                        {
                            if (jobDynamicModelList[i].UserID == temp.UserID)
                            {
                                eventDataModel.location = i.ToString();
                                break;
                            }
                        }
                        eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                        Common.TrackHelper2.TrackOperation("5.2.4.5.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                    }
                    else if ((bool)this.RBRecnt.IsChecked)
                    {
                        Common.TrackHelper2.TrackOperation("5.2.2.5.1", "clk");
                    }

                    break;
                case "btnFreePhone":
                    if ((bool)this.RBRecommend.IsChecked)
                    {
                        IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
                        ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                        eventDataModel.session = Guid.NewGuid().ToString(); ;
                        eventDataModel.algo = this.algorithmID;
                        eventDataModel.item = new List<string>();
                        eventDataModel.item.Add(temp.UserID.ToString());
                        for (int i = 0; i < jobRecommendModelList.Count; i++)
                        {
                            if (jobRecommendModelList[i].UserID == temp.UserID)
                            {
                                eventDataModel.location = i.ToString();
                                break;
                            }
                        }
                        eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                        Common.TrackHelper2.TrackOperation("5.2.6.11.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                    }
                    else if ((bool)this.RBInterest.IsChecked)
                    {
                        IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
                        ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                        //eventDataModel.session = Guid.NewGuid().ToString(); ;
                        eventDataModel.session = this.jobDynamicSession;
                        eventDataModel.algo = "0";
                        eventDataModel.item = new List<string>();
                        eventDataModel.item.Add(temp.UserID.ToString());
                        for (int i = 0; i < jobDynamicModelList.Count; i++)
                        {
                            if (jobDynamicModelList[i].UserID == temp.UserID)
                            {
                                eventDataModel.location = i.ToString();
                                break;
                            }
                        }
                        eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                        Common.TrackHelper2.TrackOperation("5.2.4.6.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                    }
                    else if ((bool)this.RBRecnt.IsChecked)
                    {
                        Common.TrackHelper2.TrackOperation("5.2.2.6.1", "clk");
                    }
                    break;
                case "btnVideo":
                    if ((bool)this.RBRecommend.IsChecked)
                    {
                        IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
                        ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                        eventDataModel.session = Guid.NewGuid().ToString(); ;
                        eventDataModel.algo = this.algorithmID;
                        eventDataModel.item = new List<string>();
                        eventDataModel.item.Add(temp.UserID.ToString());
                        for (int i = 0; i < jobRecommendModelList.Count; i++)
                        {
                            if (jobRecommendModelList[i].UserID == temp.UserID)
                            {
                                eventDataModel.location = i.ToString();
                                break;
                            }
                        }
                        eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                        Common.TrackHelper2.TrackOperation("5.2.6.12.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                    }
                    else if ((bool)this.RBInterest.IsChecked)
                    {
                        IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
                        ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                        //eventDataModel.session = Guid.NewGuid().ToString(); ;
                        eventDataModel.session = this.jobDynamicSession;
                        eventDataModel.algo = "0";
                        eventDataModel.item = new List<string>();
                        eventDataModel.item.Add(temp.UserID.ToString());
                        for (int i = 0; i < jobDynamicModelList.Count; i++)
                        {
                            if (jobDynamicModelList[i].UserID == temp.UserID)
                            {
                                eventDataModel.location = i.ToString();
                                break;
                            }
                        }
                        eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                        Common.TrackHelper2.TrackOperation("5.2.4.7.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                    }
                    else if ((bool)this.RBRecnt.IsChecked)
                    {
                        Common.TrackHelper2.TrackOperation("5.2.2.7.1", "clk");
                    }
                    break;
            }
        }

        private void RBRecnt_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private async void btnCollection_Click(object sender, RoutedEventArgs e)
        {
            if (curIMConotrol.ResumeDetail!=null)
            {
                if ((bool)this.RBRecommend.IsChecked)
                {
                    ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                    eventDataModel.session = Guid.NewGuid().ToString(); ;
                    eventDataModel.algo = this.algorithmID;
                    eventDataModel.item = new List<string>();
                    eventDataModel.item.Add(curIMConotrol.UserID.ToString());
                    for (int i = 0; i < jobRecommendModelList.Count; i++)
                    {
                        if (jobRecommendModelList[i].UserID == curIMConotrol.UserID)
                        {
                            eventDataModel.location = i.ToString();
                            break;
                        }
                    }
                    eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                    Common.TrackHelper2.TrackOperation("5.2.6.18.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                }
                else if ((bool)this.RBInterest.IsChecked)
                {
                    ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                    //eventDataModel.session = Guid.NewGuid().ToString(); ;
                    eventDataModel.session = this.jobDynamicSession;
                    eventDataModel.algo = "0";
                    eventDataModel.item = new List<string>();
                    eventDataModel.item.Add(curIMConotrol.UserID.ToString());
                    for (int i = 0; i < jobDynamicModelList.Count; i++)
                    {
                        if (jobDynamicModelList[i].UserID == curIMConotrol.UserID)
                        {
                            eventDataModel.location = i.ToString();
                            break;
                        }
                    }
                    eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                    Common.TrackHelper2.TrackOperation("5.2.4.14.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                }
                else if ((bool)this.RBRecnt.IsChecked)
                {
                    Common.TrackHelper2.TrackOperation("5.2.2.13.1", "clk");
                }

                if (curIMConotrol.ResumeDetail.collectResume)
                {
                    BaseHttpModel pBaseHttpModel = await IMHelper.CancelCollectResume(curIMConotrol.ResumeDetail.userID);
                    if (pBaseHttpModel != null)
                    {
                        if (pBaseHttpModel.code == 200)
                        {
                            curIMConotrol.ResumeDetail.collectResume = false;
                            DisappearShow pDisappearShow = new DisappearShow("已取消收藏");
                            pDisappearShow.Owner = Window.GetWindow(this);
                            pDisappearShow.ShowDialog();
                        }
                    }
                }
                else
                {
                    BaseHttpModel<HttpCollectionCount> pBaseHttpModel =  await IMHelper.CollectResume(curIMConotrol.ResumeDetail.userID);
                    if(pBaseHttpModel!=null)
                    {
                        if (pBaseHttpModel.code == 200)
                        {
                            curIMConotrol.ResumeDetail.collectResume = true;
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
        }

        private async void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            if (curIMConotrol.ResumeDetail != null)
            {

                if ((bool)this.RBRecommend.IsChecked)
                {
                    ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                    eventDataModel.session = Guid.NewGuid().ToString(); ;
                    eventDataModel.algo = this.algorithmID;
                    eventDataModel.item = new List<string>();
                    eventDataModel.item.Add(curIMConotrol.UserID.ToString());
                    for (int i = 0; i < jobRecommendModelList.Count; i++)
                    {
                        if (jobRecommendModelList[i].UserID == curIMConotrol.UserID)
                        {
                            eventDataModel.location = i.ToString();
                            break;
                        }
                    }
                    eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                    Common.TrackHelper2.TrackOperation("5.2.6.16.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                }
                else if ((bool)this.RBInterest.IsChecked)
                {
                    ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                    //eventDataModel.session = Guid.NewGuid().ToString(); ;
                    eventDataModel.session = this.jobDynamicSession;
                    eventDataModel.algo = "0";
                    eventDataModel.item = new List<string>();
                    eventDataModel.item.Add(curIMConotrol.UserID.ToString());
                    for (int i = 0; i < jobDynamicModelList.Count; i++)
                    {
                        if (jobDynamicModelList[i].UserID == curIMConotrol.UserID)
                        {
                            eventDataModel.location = i.ToString();
                            break;
                        }
                    }
                    eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                    Common.TrackHelper2.TrackOperation("5.2.4.12.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                }
                else if ((bool)this.RBRecnt.IsChecked)
                {
                    Common.TrackHelper2.TrackOperation("5.2.2.11.1", "clk");
                }

                string jobName = curIMConotrol.job;
                if (string.IsNullOrWhiteSpace(jobName))
                {
                    if (curIMConotrol.ResumeDetail.personExperienceJob != null)
                    {
                        if (curIMConotrol.ResumeDetail.personExperienceJob.Count > 0)
                        {
                            jobName = curIMConotrol.ResumeDetail.personExperienceJob[0].name;
                        }
                    }
                }

                if(RBRecnt.IsChecked == true)
                {
                    curIMConotrol.ResumeDetail.downloadType = "candidate";
                }
                else if(RBInterest.IsChecked == true)
                {
                    curIMConotrol.ResumeDetail.downloadType = "sb";
                }
                else if(RBRecommend.IsChecked == true)
                {
                    curIMConotrol.ResumeDetail.downloadType = "recommend";
                }
                await DownloadResume.DownLoadResume(curIMConotrol.ResumeDetail, Window.GetWindow(this), curIMConotrol.job);
            }
        }

        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (curIMConotrol.ResumeDetail != null)
            {
                if ((bool)this.RBRecommend.IsChecked)
                {
                    ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                    eventDataModel.session = Guid.NewGuid().ToString(); ;
                    eventDataModel.algo = this.algorithmID;
                    eventDataModel.item = new List<string>();
                    eventDataModel.item.Add(curIMConotrol.UserID.ToString());
                    for (int i = 0; i < jobRecommendModelList.Count; i++)
                    {
                        if (jobRecommendModelList[i].UserID == curIMConotrol.UserID)
                        {
                            eventDataModel.location = i.ToString();
                            break;
                        }
                    }
                    eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                    Common.TrackHelper2.TrackOperation("5.2.6.19.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                }
                else if ((bool)this.RBInterest.IsChecked)
                {
                    ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                    //eventDataModel.session = Guid.NewGuid().ToString(); ;
                    eventDataModel.session = this.jobDynamicSession;
                    eventDataModel.algo = "0";
                    eventDataModel.item = new List<string>();
                    eventDataModel.item.Add(curIMConotrol.UserID.ToString());
                    for (int i = 0; i < jobDynamicModelList.Count; i++)
                    {
                        if (jobDynamicModelList[i].UserID == curIMConotrol.UserID)
                        {
                            eventDataModel.location = i.ToString();
                            break;
                        }
                    }
                    eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();

                    Common.TrackHelper2.TrackOperation("5.2.4.15.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
                }
                else if ((bool)this.RBRecnt.IsChecked)
                {
                    Common.TrackHelper2.TrackOperation("5.2.2.14.1", "clk");
                }

                WinSendEmail pWinSendEmail = new WinSendEmail(Window.GetWindow(this),curIMConotrol.ResumeDetail.userID, curIMConotrol.ResumeDetail.name, curIMConotrol.jobId, curIMConotrol.job);
                //pWinSendEmail.Owner = ;
                pWinSendEmail.ShowDialog();
            }
        }
        private void PrintResume_Click(object sender, RoutedEventArgs e)
        {
            ResumeDetialPrint pResumeDetial = new ResumeDetialPrint();
            pResumeDetial.DataContext = curIMConotrol.ResumeDetail;
            WinResumePrint pWinResumePrint = new WinResumePrint(Window.GetWindow(this));
            if (pWinResumePrint.ShowDialog()==true)
            {
                if(pWinResumePrint.cbCheck.IsChecked == true)
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

        private async Task InitialJobList()
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "jobType" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "2" });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<HttpJobListData> listModel = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(resultStr);
            List<HttpJobList> plstJob = new List<HttpJobList>();
            plstJob.Add(new HttpJobList { jobID = -1, jobName = "全部职位" });
            List<HttpJobList> plstJobRecent = new List<HttpJobList>();
            plstJobRecent.Add(new HttpJobList { jobID = -1, jobName = "全部职位" });
            if (listModel != null)
            {
                if (listModel.code == 200)
                {
                    foreach (HttpJobList iHttpJobList in listModel.data.data)
                    {
                        if(iHttpJobList.jobName.Length>16)
                        {
                            iHttpJobList.jobName = iHttpJobList.jobName.Substring(0, 14) + "...";
                        }
                        plstJob.Add(iHttpJobList);
                        plstJobRecent.Add(iHttpJobList);
                    }
                }
            }
            plstJobRecent.Add(new HttpJobList { jobID = 0, jobName = "其他" });
            cbJobRecent.ItemsSource = plstJobRecent;
            cbJobRecent.SelectedIndex = 0;
            cbJobDynamic.ItemsSource = plstJob;
            cbJobDynamic.SelectedIndex = 0;
            cbJobRecommend.ItemsSource = plstJob;
            cbJobRecommend.SelectedIndex = 0;
        }
    }
}
