using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Threading.Tasks;
using MagicCube.ViewModel;
using System.Reflection;
using MagicCube.Common;
using MagicCube.Model;

namespace MagicCube.ViewSingle
{
    public partial class UCIM : UserControl
    {
        public IMControlModel curIMConotrol = new IMControlModel();
        public IMControlModel curRecentChatModel = new IMControlModel();
        private IMControlModel curGost = new IMControlModel();
        public ObservableCollection<IMControlModel> m_RecentChatList;
        private int RecentCount = 20;
        public long curRecentJobID = -1;
        public Action actionCloseTick;
        #region "公共方法"
        public async Task<bool> OpenIMControl(string userId,string jobID = null)
        {
            busyCtrl.IsBusy = true;
            if (!string.IsNullOrWhiteSpace(jobID))
                await IMHelper.GetIMRecord(Convert.ToInt64(userId), Convert.ToInt64(jobID)); 
            await IniRecentChatList();
            busyCtrl.IsBusy = false;
            curIMConotrol = curRecentChatModel =null;
            foreach(IMControlModel iIMControlModel in m_RecentChatList)
            {
                if (iIMControlModel.UserID.ToString() == userId)
                {
                    lstRecent.SelectedItem = iIMControlModel;
                    curRecentChatModel = iIMControlModel;
                    curIMConotrol = iIMControlModel;
                    break;
                }
            }
            if (curIMConotrol == null)
            {
                BaseHttpModel<HttpRecentList> model = await IMHelper.GetSesionInfo(Convert.ToInt64(userId), jobID);
                if (model != null)
                {
                    if (model.code == 200)
                    {
                        if (model.data.userID != 0 && model.data.jobID != 0)
                        {
                            IMControlModel pIMControlModel = new IMControlModel
                            {
                                UserID = model.data.userID,
                                UnReadCount = model.data.unReadCount,
                                LastTalkTime = model.data.updateDate,
                                LastTalkMsg = model.data.latestMsgDesc,
                                SessionID = model.data.sessionID,

                            };
                            if (pIMControlModel.UnReadCount > 0)
                                pIMControlModel.HaveNewMsg = true;
                            else
                                pIMControlModel.HaveNewMsg = false;
                            if (model.data.user != null)
                            {
                                pIMControlModel.name = model.data.user.name;
                                pIMControlModel.avatarUrl = model.data.user.avatar;

                            }
                            if (model.data.job != null)
                            {
                                pIMControlModel.job = model.data.job.jobName;
                                pIMControlModel.jobId = model.data.job.jobID;
                            }
                            else
                            {
                                pIMControlModel.job = model.data.jobName;
                                pIMControlModel.jobId = model.data.jobID;
                            }
                            pIMControlModel.avatarUrl = Downloader.LocalPath(pIMControlModel.avatarUrl, pIMControlModel.name);
                            curGost = curIMConotrol = curRecentChatModel = pIMControlModel;
                            curGost.gost = Visibility.Collapsed;
                            m_RecentChatList.Add(curGost);
                            lstRecent.SelectedItem = curGost;
                            //初始化小秘书显示
                            if (pIMControlModel.prior == "99")
                            {
                                Grid.SetRow(this.SvConversation, 1);
                                Grid.SetRowSpan(this.SvConversation, 3);
                                btnResumOpen.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                Grid.SetRow(this.SvConversation, 2);
                                Grid.SetRowSpan(this.SvConversation, 1);
                                btnResumOpen.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
            }
            if (curIMConotrol!=null)
            {
                curIMConotrol.ResumeDetail = await openResume(curIMConotrol);
                btnCollection.DataContext = this.curIMConotrol.ResumeDetail;
                if (curIMConotrol.ResumeDetail != null)
                {
                    curIMConotrol.name = curIMConotrol.ResumeDetail.name;
                    GetDeliveryID();
                }
                await SetIMRecord(curIMConotrol);
                RBRecnt.IsChecked = true;
                //初始化小秘书显示
                if (curIMConotrol.prior == "99")
                {
                    Grid.SetRow(this.SvConversation, 1);
                    Grid.SetRowSpan(this.SvConversation, 3);
                    btnResumOpen.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Grid.SetRow(this.SvConversation, 2);
                    Grid.SetRowSpan(this.SvConversation, 1);
                    btnResumOpen.Visibility = Visibility.Visible;
                }
                return true;
            }
            else
            {
                return false;
            }
                
        }

        public async Task IniRecentChatList()
        {
            

            RBRecnt.IsChecked = true;
            lstRecent.SelectedItem = curRecentChatModel = curIMConotrol = null;
            curGost = null;
            RecentCount = 20;
            m_RecentChatList = new ObservableCollection<IMControlModel>();
            await SetRecentChatList();
            if (RBRecnt.IsChecked == true)
            {
                if (m_RecentChatList.Count > 0)
                {
                    if (m_RecentChatList.Count > 1)
                    {
                        if(curRecentJobID>-1)
                        {
                            lstRecent.SelectedIndex = 0;
                        }
                        else
                        {
                            lstRecent.SelectedIndex = 1;
                        }
                        
                    }
                    else
                    {
                        lstRecent.SelectedIndex = 0;
                    }
                    IMControlModel pIMControlModel = lstRecent.SelectedItem as IMControlModel;
                    //初始化小秘书显示
                    if (pIMControlModel.prior == "99")
                    {
                        Grid.SetRow(this.SvConversation, 1);
                        Grid.SetRowSpan(this.SvConversation, 3);
                        btnResumOpen.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        Grid.SetRow(this.SvConversation, 2);
                        Grid.SetRowSpan(this.SvConversation, 1);
                        btnResumOpen.Visibility = Visibility.Visible;
                    }

                    if (pIMControlModel == curIMConotrol)
                        return;
                    SVUCResume.ScrollToTop();

                    if (pIMControlModel != null)
                    {
                        curGost = null;

                        lstRecent.SelectedItem = pIMControlModel;
                        this.curIMConotrol = pIMControlModel;
                        this.curIMConotrol.IsOpenResum = false;
                        curRecentChatModel = pIMControlModel;
                        curRecentChatModel.ResumeDetail = await openResume(pIMControlModel);
                        btnCollection.DataContext = this.curRecentChatModel.ResumeDetail;
                        await SetIMRecord(curIMConotrol);
                        GetDeliveryID();

                    }
                }
            }
        }
        public async Task SetRecentChatList(bool isRefresh = false)
        {
            HttpMessageSessionListParams pParams = new HttpMessageSessionListParams();
            pParams.userID = MagicGlobal.UserInfo.Id;
            pParams.userIdentity = 2;
            pParams.start = 0;
            pParams.size = RecentCount;

            if (!isRefresh)
            {
                tabHintRecent.Visibility = Visibility.Collapsed;
                RecentResult.Visibility = Visibility.Collapsed;
                RecentLoading.Visibility = Visibility.Visible;
                svRecent.Visibility = Visibility.Collapsed;
            }

            if (curRecentJobID>-1)
            {
                pParams.jobID = curRecentJobID.ToString();
            }
            string jsonStr = DAL.JsonHelper.ToJsonString(pParams);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrMessageSessionList, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<List<HttpRecentList>> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<List<HttpRecentList>>>(jsonResult);
            RecentLoading.Visibility = Visibility.Collapsed;
            svRecent.Visibility = Visibility.Visible;
            if (model != null)
            {
                if (model.code == 200)
                {
                    m_RecentChatList.Clear();
                    foreach (HttpRecentList iHttpRecentList in model.data)
                    {
                        IMControlModel pIMControlModel = new IMControlModel
                        {
                            UserID = iHttpRecentList.userID,
                            UnReadCount = iHttpRecentList.unReadCount,
                            LastTalkTime = iHttpRecentList.updateDate,
                            LastTalkMsg = iHttpRecentList.latestMsgDesc,
                            SessionID = iHttpRecentList.sessionID,
                            prior = iHttpRecentList.prior,

                          };
                        if (pIMControlModel.UnReadCount > 0)
                            pIMControlModel.HaveNewMsg = true;
                        else
                            pIMControlModel.HaveNewMsg = false;
                        if (iHttpRecentList.user != null)
                        {
                            //pIMControlModel.name = iHttpRecentList.user.name;
                            pIMControlModel.avatarUrl = iHttpRecentList.user.avatar;
                            pIMControlModel.name = ResumeName.GetName(iHttpRecentList.user.name, pIMControlModel.UserID.ToString()); 

                        }
                        if (iHttpRecentList.job != null)
                        {
                            pIMControlModel.job = iHttpRecentList.job.jobName;
                            pIMControlModel.jobId = iHttpRecentList.job.jobID;
                        }
                        pIMControlModel.avatarUrl = Downloader.LocalPath(pIMControlModel.avatarUrl, pIMControlModel.name);
                        m_RecentChatList.Add(pIMControlModel);
                    }


                    lstRecent.ItemsSource = m_RecentChatList;
                    if (m_RecentChatList.Count < RecentCount)
                    {
                        mbRecentMore.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        mbRecentMore.Visibility = Visibility.Visible;
                    }
                    if (!isRefresh)
                    {
                        if (m_RecentChatList.Count == 0)
                        {
                            if (curRecentJobID > -1)
                            {
                                RecentResult.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                tabHintRecent.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
                else
                {
                    mbRecentMore.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                mbRecentMore.Visibility = Visibility.Collapsed;
            }
        }
        public async Task RefreshRecentChatList()
        {
            await SetRecentChatList(true);
            if (curGost !=null)
            {
                if (m_RecentChatList.FirstOrDefault(x => x.UserID == curGost.UserID)==null)
                {
                    m_RecentChatList.Add(curGost);
                }
            }
            if (curRecentChatModel != null)
            {
                ObservableCollection<MagicCube.ViewModel.HttpMessage> pTalks = curRecentChatModel.TalkMsgs;
                if (m_RecentChatList != null)
                {
                    foreach (IMControlModel iIMControlModel in m_RecentChatList)
                    {
                        if (iIMControlModel.SessionID == curRecentChatModel.SessionID)
                        {  
                            iIMControlModel.SendMessageCache = curRecentChatModel.SendMessageCache;
                            iIMControlModel.IsOpenResum = curRecentChatModel.IsOpenResum;
                            iIMControlModel.ResumeDetail = curRecentChatModel.ResumeDetail;
                            iIMControlModel.TalkMsgs = pTalks;
                            lstRecent.SelectedItem = curIMConotrol = curRecentChatModel = iIMControlModel;

                        }
                    }
                }
            }
        }
        #endregion

        #region "事件"
        private void bgd_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private async void bgd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (actionCloseTick != null)
                actionCloseTick();
            //埋点
            Common.TrackHelper2.TrackOperation("5.2.1.5.1", "clk");
            Common.TrackHelper2.TrackOperation("5.2.1.6.1", "pv");

            e.Handled = true;

            IMControlModel pIMControlModel = (sender as Grid).DataContext as IMControlModel;
            if (pIMControlModel == curIMConotrol)
                return;
            SVUCResume.ScrollToTop();

            if (pIMControlModel != null)
            {
                curGost = null;
                
                lstRecent.SelectedItem = pIMControlModel;
                this.curIMConotrol = pIMControlModel;
                this.curIMConotrol.IsOpenResum = false;
                curRecentChatModel = pIMControlModel;

                if(pIMControlModel.prior == "99")
                {
                    Grid.SetRow(this.SvConversation, 1);
                    Grid.SetRowSpan(this.SvConversation, 3);
                    //SpConversation.Margin = new Thickness(0, 0, 0, 64);
                    this.SvConversation.Padding = new Thickness(0, 0, 0, 50);
                    btnResumOpen.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Grid.SetRow(this.SvConversation, 2);
                    Grid.SetRowSpan(this.SvConversation, 1);
                    btnResumOpen.Visibility = Visibility.Visible;
                    SpConversation.Margin = new Thickness(0, 0, 0, 0);
                    this.SvConversation.Padding = new Thickness(0, 0, 0, 0);
                }

                curRecentChatModel.ResumeDetail = await openResume(pIMControlModel);
                btnCollection.DataContext = this.curRecentChatModel.ResumeDetail;
                await SetIMRecord(curIMConotrol);
                GetDeliveryID();
               
            }

        }
        private async void gdLstRecent_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool visible = (bool)e.NewValue;
            if (visible)
            {
                this.gdMessageAndResume.SetBinding(Grid.DataContextProperty, new Binding("SelectedItem") { Source = this.lstRecent });

                if (lstRecent.SelectedItem != null)
                {
                    curIMConotrol = lstRecent.SelectedItem as IMControlModel;
                    await  SetIMRecord(curIMConotrol);
                }
            }
        }
        private async void RecentRemove_MenuClick(object sender, RoutedEventArgs e)
        {
            IMControlModel pIMControlModel = (sender as MenuItem).DataContext as IMControlModel;
            if (pIMControlModel == null)
                return;
            m_RecentChatList.Remove(pIMControlModel);
            if(m_RecentChatList.Count == 0)
            {
                if (curRecentJobID > -1)
                {
                    RecentResult.Visibility = Visibility.Visible;
                }
                else
                {
                    tabHintRecent.Visibility = Visibility.Visible;
                }
            }
            HttpMessageSessionDeleteParams pParams = new HttpMessageSessionDeleteParams();
            pParams.userID = MagicGlobal.UserInfo.Id;
            pParams.userIdentity = 2;
            pParams.sessionID = pIMControlModel.SessionID;
            pParams.clientVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            pParams.ip = IMHelper.GetIP();
            pParams.platform = DAL.ConfUtil.platform;
            string jsonStr = DAL.JsonHelper.ToJsonString(pParams);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrMessageSessionDelete, MagicGlobal.UserInfo.Version, jsonStr));
            MagicCube.ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel>(jsonResult);
            if(model!=null)
            {
                if (model.code == 200)
                {
                    
                    return;
                }
            }
        }

        private async void RecentMore_Click(object sender, RoutedEventArgs e)
        {
            RecentCount += 20;
            await SetRecentChatList();
            foreach (IMControlModel iIMControlModel in m_RecentChatList)
            {
                if (iIMControlModel.SessionID == curRecentChatModel.SessionID)
                {
                    lstRecent.SelectedItem = curIMConotrol = curRecentChatModel = iIMControlModel;
                }
            }
        }
        #endregion
        private void GetDeliveryID()
        {
            Action method = delegate
            {
                string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<MagicCube.ViewModel.HttpResumeTable>();
                List<string> pkey = new List<string> { "hrUserID", "jhUserID" };
                List<string> pvalue = new List<string> { MagicGlobal.UserInfo.Id.ToString(), curIMConotrol.UserID.ToString() };

                string std = DAL.JsonHelper.JsonParamsToString(pkey, pvalue);
                string jsonResult = DAL.HttpHelper.Instance.HttpGet(
                    string.Format(DAL.ConfUtil.AddrGetByDeliveryID, MagicGlobal.UserInfo.Version, std));
                ViewModel.BaseHttpModel<HttpFlow> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<HttpFlow>>(jsonResult);
                if (model != null)
                {
                    if (model.code == 200)
                    {
                        if (model.data.deliveryID > 0)
                        {
                            this.curIMConotrol.deliveryID = model.data.deliveryID;
                            if (this.curIMConotrol.ResumeDetail != null)
                            {
                                this.curIMConotrol.ResumeDetail.deliveryID = this.curIMConotrol.deliveryID;
                                if (!model.data.createTime.ToString("yyyy-MM-dd").Contains("0001"))
                                    this.curIMConotrol.ResumeDetail.resumeTime = model.data.createTime.ToString("yyyy-MM-dd");
                                this.curIMConotrol.ResumeDetail.timeType = "应聘/邀请";
                                string stdSign = DAL.JsonHelper.JsonParamsToString(new string[] { "deliveryID" }, new string[] { this.curIMConotrol.deliveryID.ToString() });
                                string jsonResultSign = DAL.HttpHelper.Instance.HttpGet(
                                    string.Format(DAL.ConfUtil.AddrGetUserSignList, MagicGlobal.UserInfo.Version, stdSign));
                                ViewModel.BaseHttpModel<HttpFlow> modelSign = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<HttpFlow>>(jsonResultSign);
                                if (modelSign != null)
                                {
                                    if (modelSign.code == 200)
                                    {
                                        if (modelSign.data.userSignList != null)
                                        {
                                            if (modelSign.data.userSignList.Count > 0)
                                            {
                                                foreach (ViewModel.HttpResumTags iHttpResumTags in modelSign.data.userSignList)
                                                {
                                                    this.curIMConotrol.ResumeDetail.tags.Add(new ResumeTag() { id = iHttpResumTags.userSignID, name = iHttpResumTags.userSignName, color = iHttpResumTags.userSignColor });
                                                }
                                            }
                                        }
                                    }
                                }
                                this.curIMConotrol.ResumeDetail.canTag = Visibility.Visible;
                            }
                        }
                    }
                }

            };
            method.BeginInvoke(null, null);
           
          
        }
        private async void cbJobRecent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbJobRecent.SelectedItem == null)
                return;
            if ((cbJobRecent.SelectedItem as HttpJobList).jobID == curRecentJobID)
                return;
            curRecentJobID = (cbJobRecent.SelectedItem as HttpJobList).jobID;
            await IniRecentChatList();

            
        }
    }
}
