using MagicCube.Common;
using MagicCube.HttpModel;
using MagicCube.Index;
using MagicCube.Model;
using MagicCube.TemplateUC;
using MagicCube.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MagicCube.ViewSingle
{
    public partial class MainWindow : Window
    {
        private bool hasNew = false;
        DispatcherTimer timeIM;
        DispatcherTimer timeList;
        DispatcherTimer timeMessage;
        DispatcherTimer timeNewIM;
        DispatcherTimer timePallet;

        private bool RBIMIsChecked = false;
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hwnd);
        public void initTextChat()
        {
            timeIM = new DispatcherTimer();
            timeIM.Interval = new TimeSpan(0, 0, 5);
            timeIM.Tick += new EventHandler(RecvIM_Tick);
            timeIM.Start();
            timeList = new DispatcherTimer();
            timeList.Interval = new TimeSpan(0, 0, 0,4,888);
            timeList.Tick += new EventHandler(RecvList_Tick);
            timeList.Start();
            timeMessage = new DispatcherTimer();
            timeMessage.Interval = new TimeSpan(0, 0,0, 4,999);
            timeMessage.Tick += new EventHandler(RecvMessage_Tick);
            timeMessage.Start();
            timeNewIM = new DispatcherTimer();
            timeNewIM.Interval = new TimeSpan(0, 10, 0);
            timeNewIM.Tick += new EventHandler(NewIM_Tick);
            timeNewIM.Start();
            timePallet = new DispatcherTimer();
            timePallet.Interval = new TimeSpan(0, 0, 5);
            timePallet.Tick += new EventHandler(Pallet_Tick);
            timePallet.Start();

            DAL.SocketEventHandel.PushMessageEventHandle += PushMsgCallback;
        }

        private async Task PushMsgCallback(string data)
        {
            if(this.RBIMIsChecked)
            {
                await ucIM.RefreshIMRecord(ucIM.curIMConotrol);
            }
        }
        private async void Pallet_Tick(object sender, EventArgs e)
        {
            string jsonStr = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrMessageNotice, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<HttpMessageNotice> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMessageNotice>>(jsonResult);
            if (model != null)
            {
                if (model.code == 200)
                {
                    if (model.data.type == "1")
                    {
                        HttpMessageUnReadParams pParams = new HttpMessageUnReadParams();
                        pParams.userID = MagicGlobal.UserInfo.Id;
                        pParams.userIdentity = 2;
                        pParams.sessionID = null;
                        string jsonStr2 = DAL.JsonHelper.ToJsonString(pParams);
                        string jsonResult2 = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrMessageUnReadCount, MagicGlobal.UserInfo.Version, jsonStr2));
                        BaseHttpModel<HttpMessageUnRead> model2 = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<HttpMessageUnRead>>(jsonResult2);
                        if (model2 != null)
                        {
                            if (model2.code == 200)
                            {
                                if (model2.data.unReadCount > 0)
                                {
                                    hasNew = true;
                                    notifyFlack(true);
                                    if (this.IsFocused == false)
                                    {
                                        TaskBarHelper.FlashWindow(this, 5);
                                    }

                                }
                            }
                        }
                        
                    }
                    else if (model.data.type == "2")
                    {
                        HttpMessageNoticeSign modelSign = DAL.JsonHelper.ToObject<HttpMessageNoticeSign>(model.data.content);
                        IntPtr activeForm = GetActiveWindow(); // 先得到当前的活动窗体 
                        WinMBNotice pWinMBNotice = new WinMBNotice(modelSign.notice);
                        pWinMBNotice.Show();
                        SetActiveWindow(activeForm); // 在把焦点还给之前的活动窗体
                    }

                }
            }
        }


        private async void NewIM_Tick(object sender, EventArgs e)
        {
            if (MagicGlobal.UserInfo.MessageNoticeTime != DateTime.Now.ToString("yyyy-MM-dd"))
            {
                if (hasNew)
                {
                    HttpMessageUnReadParams pParams = new HttpMessageUnReadParams();
                    pParams.userID = MagicGlobal.UserInfo.Id;
                    pParams.userIdentity = 2;
                    pParams.sessionID = null;
                    string jsonStr = DAL.JsonHelper.ToJsonString(pParams);
                    string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrMessageUnReadCount, MagicGlobal.UserInfo.Version, jsonStr));
                    BaseHttpModel<HttpMessageUnRead> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMessageUnRead>>(jsonResult);
                    if (model != null)
                    {
                        if (model.code == 200)
                        {
                            if (model.data.unReadCount > 0)
                            {
                                IntPtr activeForm = GetActiveWindow(); // 先得到当前的活动窗体 
                              
                                WinNewIM pWinNewIM = new WinNewIM(model.data.unReadCount.ToString());
                                pWinNewIM.actionChat = ChatCallBack;
                                pWinNewIM.actionNomore = NomoreCallBack;
                                pWinNewIM.Show();
                                SetActiveWindow(activeForm); // 在把焦点还给之前的活动窗体
                            }
                        }
                    }
                    hasNew = false;
                }
            }
        }

        private void NomoreCallBack()
        {
            notifyFlack(false);
        }

        private async Task ChatCallBack()
        {
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
            this.WindowState = System.Windows.WindowState.Normal;
            notifyFlack(false);
            if (RBIM.IsChecked == false)
            {
                RBIM.IsChecked = true;
                await ucIM.IniRecentChatList();
                await ucIM.UCIMInitial();
            }
            if (ucIM.m_RecentChatList.Count > 0)
            {
                await ucIM.OpenIMControl(ucIM.m_RecentChatList[1].UserID.ToString());
            }
        }
         private async void RecvMessage_Tick(object sender, EventArgs e)
        {
            if (this.WindowState != WindowState.Minimized && this.Visibility == Visibility.Visible)
            {
                if (RBIM.IsChecked == true)
                {
                    await ucIM.RefreshIMRecord(ucIM.curIMConotrol);
                }
            }
        }
        private async void RecvList_Tick(object sender, EventArgs e)
        {
            if (RBIM.IsChecked == true)
            {
                if (ucIM.RBRecnt.IsChecked == true)
                {
                    await ucIM.RefreshRecentChatList();
                }
            }
        }

        private async void RecvIM_Tick(object sender, EventArgs e)
        {

            HttpMessageUnReadParams pParams = new HttpMessageUnReadParams();
            pParams.userID = MagicGlobal.UserInfo.Id;
            pParams.userIdentity = 2;
            pParams.sessionID = null;
            string jsonStr = DAL.JsonHelper.ToJsonString(pParams);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrMessageUnReadCount, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<HttpMessageUnRead> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<HttpMessageUnRead>>(jsonResult);
            if (model != null)
            {
                if (model.code == 200)
                {
                    if (model.data.unReadCount > 0)
                    {
                        ucIM.RecentNew.Visibility = RecentNew.Visibility = Visibility.Visible;
                        tbUnRead.Text = ucIM.tbUnRead.Text = model.data.unReadCount.ToString();
                        
                    }
                    else
                    {
                        ucIM.RecentNew.Visibility = RecentNew.Visibility = Visibility.Collapsed;
                        notifyFlack(false);
                    }
                }
            }

        }

        private void CloseTickCallback()
        {
            timeMessage.Stop();
            timeMessage.Start();
        }
    }
}
