using MagicCube.Common;
using MagicCube.HttpModel;
using MagicCube.Model;
using MagicCube.TemplateUC;

using MagicCube.View.Message;
using MagicCube.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MagicCube.ViewSingle
{
   
    public partial class UCIM : UserControl
    {
        public delegate Task delegateAddV();
        public delegateAddV actionAddV;
        public Action actionHRInfo;
  
        public async Task SetIMRecord(IMControlModel pControl)
        {
            SpConversation.Children.Clear();
            
            BaseHttpModel<HttpMessageRoot> model = await IMHelper.GetIMRecord(pControl.UserID, pControl.jobId, pControl.SessionID);
            SpConversation.Children.Clear();
            if (model != null)
            {
                if(model.code == -135)
                {
                    WinValidateMessage pWinValidateMessage = new WinValidateMessage("很抱歉，认证后才能与更多候选人主动发起沟通");
                    pWinValidateMessage.Owner = Window.GetWindow(this);
                    if(pWinValidateMessage.ShowDialog()==true)
                    {
                        if (actionAddV != null)
                        {
                            await actionAddV();
                        }
                    }
                }
                if (model.code == 200)
                {
                    if (pControl.SessionID == null)
                        pControl.SessionID = model.data.sessionID;
                    pControl.TalkMsgs = new ObservableCollection<HttpMessage>();
                    foreach (HttpMessage iTalkMsg in model.data.msgList)
                    {
                        if (pControl.TalkMsgs.FirstOrDefault(x => x.msgID == iTalkMsg.msgID) == null)
                        {
                            if (curIMConotrol.SessionID == iTalkMsg.sessionID || curIMConotrol.SessionID==null)
                            {
                                //小秘书消息处理
                                if(iTalkMsg.type == "17")
                                {
                                    HttpSecretaryLandingPageModel lpModel = DAL.JsonHelper.ToObject<HttpSecretaryLandingPageModel>(iTalkMsg.landingPage);
                                    if (lpModel != null)
                                    {
                                        //按钮提示跳转信息
                                        pControl.TalkMsgs.Add(iTalkMsg);
                                        ShowTime(iTalkMsg, pControl);
                                        ShowConvrMessSecretary(iTalkMsg.content, lpModel.buttonName, lpModel.buttonID);
                                    }
                                    else
                                    {
                                        //文本信息
                                        if(!string.IsNullOrEmpty(iTalkMsg.content))
                                        {
                                            pControl.TalkMsgs.Add(iTalkMsg);
                                            ShowTime(iTalkMsg, pControl);
                                            ShowTextSecretary(iTalkMsg.content);
                                        }
                                    }
                                }


                                if (!string.IsNullOrEmpty(iTalkMsg.content))
                                {
                                    if (iTalkMsg.type == "1" || iTalkMsg.type == "14" || iTalkMsg.type == "15" || iTalkMsg.type == "16")
                                    {
                                        pControl.TalkMsgs.Add(iTalkMsg);
                                        ShowTime(iTalkMsg, pControl);
                                        int itype = 0;
                                        if (iTalkMsg.fromSide == "mySide")
                                        {
                                            if (iTalkMsg.type == "14")
                                            {
                                                itype = MESSAGE_TYPE_SYS;
                                            }
                                            else
                                            {
                                                itype = MESSAGE_TYPE_SEND;
                                            }

                                        }

                                        else if (iTalkMsg.fromSide == "systemSide")
                                            itype = MESSAGE_TYPE_SYS;
                                        else
                                            itype = MESSAGE_TYPE_RECV;
                                        ShowConvrMess(iTalkMsg.content, itype);

                                    }
                                    if (iTalkMsg.type == "10")
                                    {
                                        pControl.TalkMsgs.Add(iTalkMsg);
                                        ShowTime(iTalkMsg, pControl);
                                        int itype = 0;
                                        HttpPhoneMessage modelPhone = DAL.JsonHelper.ToObject<HttpPhoneMessage>(iTalkMsg.content);
                                        if(modelPhone.type == "success")
                                        {
                                            itype = MESSAGE_PHONESUCESS;
                                        }
                                        else if(modelPhone.type == "failure")
                                        {
                                            itype = MESSAGE_PHONEFAIL;
                                        }
                                        ShowConvrMess(modelPhone.msg, itype);

                                    }
                                }
                            }
                        }
                        else
                        {
                            string a = "";
                            string b = a;
                        }
                    }
                    HttpMessageUpdataToReadParams lstParams = new HttpMessageUpdataToReadParams
                    {

                        sessionID = pControl.SessionID,
                        userID = MagicGlobal.UserInfo.Id,
                        userIdentity = 2,
                        clientVersion = Assembly.GetEntryAssembly().GetName().Version.ToString(),
                    };
                    string jsonStr2 = DAL.JsonHelper.ToJsonString(lstParams);
                    string result2 = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrUpdateToRead, MagicGlobal.UserInfo.Version, jsonStr2));
                    MagicCube.ViewModel.BaseHttpModel model2 = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel>(result2);
                }
            }
        }

        public async Task RefreshIMRecord(IMControlModel pControl)
        {
           
            if (gdMessageAndResume.Visibility == Visibility.Visible)
            {
                if (gdResumeView.Visibility == Visibility.Collapsed)
                {
                    HttpMessageListParams pParams = new HttpMessageListParams();
                    pParams.userID = MagicGlobal.UserInfo.Id;
                    pParams.userIdentity = 2;
                    pParams.sessionID = pControl.SessionID;
                    pParams.start = null;
                    pParams.size = 50;
                    pParams.jobID = pControl.jobId;
                    pParams.anotherUserID = pControl.UserID;
                    pParams.anotherUserIdentity = 1;
                    string jsonStr = DAL.JsonHelper.ToJsonString(pParams);
                    string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrMessageList, MagicGlobal.UserInfo.Version, jsonStr));
                    MagicCube.ViewModel.BaseHttpModel<HttpMessageRoot> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<HttpMessageRoot>>(jsonResult);
                    if (model != null)
                    {
                        if (model.code == 200)
                        {
                            foreach (HttpMessage iHttpMessage in model.data.msgList)
                            {
                                if (pControl.TalkMsgs.FirstOrDefault(x => x.msgID == iHttpMessage.msgID) == null)
                                {
                                    if (pControl.SessionID == iHttpMessage.sessionID || pControl.SessionID == null)
                                    {
                                        pControl.TalkMsgs.Add(iHttpMessage);
                                        //小秘书消息处理
                                        if (iHttpMessage.type == "17")
                                        {
                                            HttpSecretaryLandingPageModel lpModel = DAL.JsonHelper.ToObject<HttpSecretaryLandingPageModel>(iHttpMessage.landingPage);
                                            if (lpModel != null)
                                            {
                                                //按钮提示跳转信息
                                                ShowTime(iHttpMessage, pControl);
                                                ShowConvrMessSecretary(iHttpMessage.content, lpModel.buttonName, lpModel.buttonID);
                                            }
                                            else
                                            {
                                                //文本信息
                                                if (!string.IsNullOrEmpty(iHttpMessage.content))
                                                {
                                                    ShowTime(iHttpMessage, pControl);
                                                    ShowTextSecretary(iHttpMessage.content);
                                                }
                                            }
                                        }


                                        if (iHttpMessage.type == "1" || iHttpMessage.type == "14" || iHttpMessage.type == "15" || iHttpMessage.type == "16")
                                        {
                                            ShowTime(iHttpMessage, pControl);
                                            int itype = 0;
                                            if (iHttpMessage.fromSide == "mySide")
                                            {
                                                if (iHttpMessage.type == "14")
                                                {
                                                    itype = MESSAGE_TYPE_SYS;
                                                }
                                                else
                                                {
                                                    itype = MESSAGE_TYPE_SEND;
                                                }

                                            }
                                            else if (iHttpMessage.fromSide == "systemSide")
                                                itype = MESSAGE_TYPE_SYS;
                                            else
                                                itype = MESSAGE_TYPE_RECV;
                                            ShowConvrMess(iHttpMessage.content, itype);
                                        }
                                        if (iHttpMessage.type == "10")
                                        {
                                            pControl.TalkMsgs.Add(iHttpMessage);
                                            ShowTime(iHttpMessage, pControl);
                                            int itype = 0;
                                            HttpPhoneMessage modelPhone = DAL.JsonHelper.ToObject<HttpPhoneMessage>(iHttpMessage.content);
                                            if (modelPhone.type == "success")
                                            {
                                                itype = MESSAGE_PHONESUCESS;
                                            }
                                            else if (modelPhone.type == "failure")
                                            {
                                                itype = MESSAGE_PHONEFAIL;
                                            }
                                            ShowConvrMess(modelPhone.msg, itype);

                                        }
                                    }
                                }
                            }


                            HttpMessageUpdataToReadParams lstParams = new HttpMessageUpdataToReadParams
                            {

                                sessionID = pControl.SessionID,
                                userID = MagicGlobal.UserInfo.Id,
                                userIdentity = 2,
                                clientVersion = Assembly.GetEntryAssembly().GetName().Version.ToString(),
                            };
                            string jsonStr2 = DAL.JsonHelper.ToJsonString(lstParams);
                            string result2 = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrUpdateToRead, MagicGlobal.UserInfo.Version, jsonStr2));
                            BaseHttpModel model2 = DAL.JsonHelper.ToObject<BaseHttpModel>(result2);
                        }
                    }
                }
            }
        }
        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            if((bool)this.RBRecommend.IsChecked)
            {
                IMControlModel temp = gdMessageAndResume.DataContext as IMControlModel;
                ViewModel.HttpTrackEventDataModel eventDataModel = new HttpTrackEventDataModel();
                //eventDataModel.session = Guid.NewGuid().ToString(); ;
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
                Common.TrackHelper2.TrackOperation("5.2.5.14.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));
            }
            else if((bool)this.RBInterest.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.3.13.1", "clk");
            }
            else if((bool)this.RBRecnt.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.1.15.1", "clk");
            }




            if (string.IsNullOrWhiteSpace(TbConvrInput.Text))
            {
                return;
            }
            string msg = TbConvrInput.Text;
            TbConvrInput.Text = string.Empty;
            await sendMessage(msg);

            if(RBRecnt.IsChecked == true)
            {
                msgNotice(msg);
            }
        }
        public async Task sendMessage(string message)
        {

            ShowTime(DateTime.Now);
            HttpMessage pHttpMessage = new HttpMessage { content = message, createTime = DateTime.Now, fromUserID = MagicGlobal.UserInfo.Id, toUserID = curIMConotrol.UserID, status = 1, sessionID = curIMConotrol.SessionID };
            curIMConotrol.TalkMsgs.Add(pHttpMessage);
            Panel messageContainer = this.ShowConvrMess(message, 1);
            string strMsg = message;
            string failMessage = string.Empty;
            BaseHttpModel<HttpMessage> model = await IMHelper.SendMessage(curIMConotrol.UserID, message);
            if (model == null)
            {
                failMessage = "失败";
            }
            if (model != null)
            {
                if (model.code != 200)
                {
                    failMessage = "失败";
                }
            }
            if (failMessage != string.Empty)
            {
                if (messageContainer != null)
                {
                    TextBlock t = new TextBlock();
                    t.Text = failMessage;
                    t.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    t.Width = 30;
                    t.Height = 20;
                    t.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    t.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    t.Margin = new Thickness(0, 5, 5, 0);
                    messageContainer.Children.Add(t);

                }
            }
            else
            {
                pHttpMessage.msgID = model.data.msgID;
            }
        }
        public void ShowTime(HttpMessage pTalkMsg, IMControlModel pIMControlModel)
        {
            int index = -1;
            HttpMessage pHttpMessage = pIMControlModel.TalkMsgs.FirstOrDefault(x => x.msgID == pTalkMsg.msgID);
            if (pHttpMessage != null)
                index = pIMControlModel.TalkMsgs.IndexOf(pHttpMessage);
            if (index > 0)
            {
                if (pIMControlModel.TalkMsgs[index - 1].createTime.AddMinutes(DAL.ConfUtil.INSTANT_SHOW_TIME).CompareTo(pTalkMsg.createTime) < 0)
                {
                    ShowConvrMess(TimeHelper.CurrentChatTime(pTalkMsg.createTime), MESSAGE_TYPE_TIME);
                }
            }
            else
            {
                ShowConvrMess(TimeHelper.CurrentChatTime(pTalkMsg.createTime), MESSAGE_TYPE_TIME);
            }
        }
        public void ShowTime(DateTime pDateTime)
        {
            if (curIMConotrol.TalkMsgs.Count > 0)
            {
                if (curIMConotrol.TalkMsgs[curIMConotrol.TalkMsgs.Count - 1].createTime.AddMinutes(DAL.ConfUtil.INSTANT_SHOW_TIME).CompareTo(DateTime.Now) < 0)
                {
                    ShowConvrMess(TimeHelper.CurrentChatTime(pDateTime), MESSAGE_TYPE_TIME);
                }
            }
            else
            {
                ShowConvrMess(TimeHelper.CurrentChatTime(pDateTime), MESSAGE_TYPE_TIME);
            }
        }
        public Panel ShowConvrMess(string message, int messageType)
        {
            if (string.IsNullOrEmpty(message))
            {
                return null;
            }

            //系统图标
            Image messageImage = new Image();
            Panel messageContainer = null;

            switch (messageType)
            {
                case MESSAGE_TYPE_SYS:
                    messageContainer = new Grid();
                    SystemTemp pSystemTemp = new SystemTemp();
                    pSystemTemp.msg.Text = message;
                    messageContainer.Children.Insert(0,pSystemTemp);
                    messageContainer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    break;
                case MESSAGE_TYPE_SEND:
                    messageContainer = new StackPanel();
                    SendTemp pSendTemp = new SendTemp();
                    pSendTemp.bdhand.MouseLeftButtonUp+=HRhand_MouseLeftButtonUp;
                    pSendTemp.IBPath.ImageSource = new BitmapImage(new Uri(Downloader.LocalPathCompany(MagicGlobal.UserInfo.avatarUrl)));
                    pSendTemp.msg.Text = message;
                    messageContainer.Children.Add(pSendTemp);
                    break;
                case MESSAGE_TYPE_RECV:
                    messageContainer = new Grid();
                    RecvTemp pRecvTemp = new RecvTemp();
                    pRecvTemp.bdhand.MouseLeftButtonUp+=Chand_MouseLeftButtonUp;
                    pRecvTemp.UrlPic = curIMConotrol.avatarUrl;
                    pRecvTemp.msg.Text = message;
                    messageContainer.Children.Add(pRecvTemp);
                    break;
                case MESSAGE_TYPE_TIME:
                    messageContainer = new Grid();
                    TimeTemp pTimeTemp = new TimeTemp();
                    pTimeTemp.msg.Text = message;
                    messageContainer.Children.Add(pTimeTemp);
                    messageContainer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    break;
                case MESSAGE_PHONESUCESS:
                    messageContainer = new Grid();
                    PhoneSucessTemp pPhoneSucessTemp = new PhoneSucessTemp();
                    pPhoneSucessTemp.msg.Text = message;
                    messageContainer.Children.Add(pPhoneSucessTemp);
                    messageContainer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    break;
                case MESSAGE_PHONEFAIL:
                    messageContainer = new Grid();
                    PhoneFailTemp pPhoneFailTemp = new PhoneFailTemp();
                    pPhoneFailTemp.msg.Text = message;
                    messageContainer.Children.Add(pPhoneFailTemp);
                    messageContainer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    break;
                //case MESSAGE_JOBOFFER:
                //    messageContainer = new Grid();
                //    SendJobOffer pSendJobOffer = new SendJobOffer();
                //    pSendJobOffer.IBPath.ImageSource = new BitmapImage(new Uri(Downloader.LocalPathCompany(MagicGlobal.UserInfo.avatarUrl)));
                //    pSendJobOffer.txtSendMessage.Text = message;
                //    pSendJobOffer.tbJobName.Text = curIMConotrol.job;
                //    pSendJobOffer.tbSalary.Text = curIMConotrol.minSalary + "K" + "~" + curIMConotrol.maxSalary + "K";
                //    //pSendJobOffer.DetailShowAction += JobDetailShowCallback;
                //    messageContainer.Children.Add(pSendJobOffer);
                //    break;


                default:
                    return messageContainer;
            }

            //控制最大显示条目数
            if (SpConversation.Children.Count > DAL.ConfUtil.MAX_MESSAGE_NUM)
            {
                SpConversation.Children.RemoveAt(0);
            }
            messageContainer.Margin = new Thickness(0, 5, 0, 5);
            SpConversation.Children.Add(messageContainer);
            SvConversation.ScrollToEnd();
            return messageContainer;
        }

        public Panel ShowConvrMessSecretary(string msg, string buttonName, string buttonID)
        {
            //系统图标
            Image messageImage = new Image();
            Panel messageContainer = null;

            messageContainer = new Grid();
            UCSecretButtonIM imButtonTemp = new UCSecretButtonIM(msg, buttonName, curIMConotrol.avatarUrl, buttonID);
            messageContainer.Children.Add(imButtonTemp);
            //控制最大显示条目数
            if (SpConversation.Children.Count > DAL.ConfUtil.MAX_MESSAGE_NUM)
            {
                SpConversation.Children.RemoveAt(0);
            }
            messageContainer.Margin = new Thickness(0, 5, 0, 5);
            SpConversation.Children.Add(messageContainer);
            SvConversation.ScrollToEnd();
            return messageContainer;
        }

        public Panel ShowTextSecretary(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return null;
            }

            //系统图标
            Image messageImage = new Image();
            Panel messageContainer = null;
            messageContainer = new Grid();
            RecvTemp pRecvTemp = new RecvTemp();
            pRecvTemp.UrlPic = curIMConotrol.avatarUrl;
            pRecvTemp.msg.Text = message;
            pRecvTemp.bdhand.Cursor = Cursors.Arrow;
            messageContainer.Children.Add(pRecvTemp);

            //控制最大显示条目数
            if (SpConversation.Children.Count > DAL.ConfUtil.MAX_MESSAGE_NUM)
            {
                SpConversation.Children.RemoveAt(0);
            }
            messageContainer.Margin = new Thickness(0, 5, 0, 5);
            SpConversation.Children.Add(messageContainer);
            SvConversation.ScrollToEnd();
            return messageContainer;

        }

        private void Chand_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            curIMConotrol.IsOpenResum = true;
        }

        private  void HRhand_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(actionHRInfo!=null)
            {
                actionHRInfo();
            }
        }
        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            string strResult = string.Empty;
            WinConfirmJudge pWinConfirmJudge = new WinConfirmJudge("合适");
            pWinConfirmJudge.Owner = Window.GetWindow(this);
            if (pWinConfirmJudge.ShowDialog() == false)
                return;
            //if (await JudgeHelper.JudgeInterviewer(curIMConotrol.RecordId, ConfUtil.FeedbackPass) == MagicCube.DAL.ConfUtil.judgeSucess)
            //{
            //    curIMConotrol.evaluation = "合适";
            //    DisappearShow disappear = new DisappearShow("已处理为合适", 1);
            //    disappear.Owner = Window.GetWindow(this);
            //    disappear.ShowDialog();
            //}
            //else
            //{
            //    WinErroTip pWinErroTip = new WinErroTip(strResult);
            //    pWinErroTip.Owner = Window.GetWindow(this);
            //    pWinErroTip.ShowDialog();
            //}
        }
        private void btnFail_Click(object sender, RoutedEventArgs e)
        {
            string strResult = string.Empty;
            WinConfirmJudge pWinConfirmJudge = new WinConfirmJudge("不合适");
            pWinConfirmJudge.Owner = Window.GetWindow(this);
            if (pWinConfirmJudge.ShowDialog() == false)
                return;
            //if (await JudgeHelper.JudgeInterviewer(curIMConotrol.RecordId, ConfUtil.FeedbackFail) == MagicCube.DAL.ConfUtil.judgeSucess)
            //{
            //    curIMConotrol.evaluation = "不合适";
            //    DisappearShow disappear = new DisappearShow("已处理为不合适", 1);
            //    disappear.Owner = Window.GetWindow(this);
            //    disappear.ShowDialog();
            //}
            //else
            //{
            //    WinErroTip pWinErroTip = new WinErroTip(strResult);
            //    pWinErroTip.Owner = Window.GetWindow(this);
            //    pWinErroTip.ShowDialog();
            //}
        }
        public void msgNotice(string message)
        {
            curIMConotrol.LastTalkMsg = message;
            curIMConotrol.LastTalkTime = DateTime.Now;
            if(m_RecentChatList.IndexOf(curIMConotrol)!=0)
            {
                 if(m_RecentChatList.Count > 0)
                {
                    if(m_RecentChatList[0].prior != "99")
                    {
                        m_RecentChatList.Remove(curIMConotrol);
                        m_RecentChatList.Insert(0, curIMConotrol);
                        lstRecent.SelectedItem = curIMConotrol;
                    }
                }
                //m_RecentChatList.Remove(curIMConotrol);
                //m_RecentChatList.Insert(0, curIMConotrol);
                //lstRecent.SelectedItem = curIMConotrol;
            }
        }
    }
}
