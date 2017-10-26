using MagicCube.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Collections.ObjectModel;
using MagicCube.HttpModel;
using MagicCube.Model;

using System.Diagnostics;
using System.Reflection;
using MagicCube.TemplateUC;
using System.Threading.Tasks;
using MagicCube.ViewModel;

namespace MagicCube.ViewSingle
{
    enum eLimit
    {
        F_LimitCommunicate,
        F_LimitManageJob,
        F_LimitManageResume,
        F_LimitSearch,
        F_LimitUpdateInfo,
        F_UseManageInterview
    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string LoginMessage = string.Empty;
        MainHintBinding mMainHintBinding;
        private const int SIGNDAY = 5;
        public WinSystemSet winSysSet;

        public bool IsRegister = false;

        public MainWindow()
        {
            
            InitializeComponent();
            this.NormalOrMax = "Normal";
            GlobalEvent.GlobalEventHandler += GlobalEventCallback;
            this.ucJobPublishSelect.JobPublishSelectCloseAction += JobPublishSelectCloseCallback;
            this.ucJobPublishSelect.JobPublishSelectOKAction += JobPublishSelectOKCallback;
            this.ucTagsSetting.CloseAction += UcTagsCloseCallback;
                     
            this.ucIM.actionOpenPublish = openPublish;
            this.ucIM.actionSearch = SearchCallback;
            this.ucIM.actionHRInfo = HRInfoCallback;
            this.ucResume.ucResumeGridView.actionSearch = SearchCallback;
            this.ucResume.ucResumeGridView.actionChat = ChatCallback;
            this.ucResume.actionOpenPublish = openPublish;
            this.ucJob.actionOpenResume = openResume;
            this.ucJob.ucJobPublish.actionSearch = SearchCallback;
            this.ucSearch.ucResumeCollection.ucSearchResult.OpenJobPublishAction += openPublish;
            this.ucSearch.ucResumeCollection.ucSearchResult.OpenJobGridCloseUCAction += OpenJobGridClosePublish;

            this.ucResume.ucCollectionTable.OpenJobPublishAction = openPublish;
            this.ucResume.ucCollectionTable.OpenJobGridCloseUCAction = OpenJobGridClosePublish;

            this.ucSearch.ucResumeCollection.ucSearchCondition.OpenPublishJobAction += openPublish;
            this.ucSearch.ucResumeCollection.ucSearchResult.actionClearCollection = OpenCollectionPage;
            ucIM.actionClearCollection = OpenCollectionPage;

            ucResume.ucCollectionTable.actionAddV = AddVCallBack;
            ucJob.ucJobClose.actionAddV = AddVCallBack;
            ucJob.ucJobOpen.actionAddV = AddVCallBack;
            ucJob.ucJobPublish.actionAddV = AddVCallBack; 
            ucIM.actionAddV = AddVCallBack;
            this.ucJobPublishSelect.actionAddV = AddVCallBack;
            ucSearch.ucResumeCollection.ucSearchResult.actionAddV = AddVCallBack;

            this.ContentRendered += MainWindow_ContentRendered;
            this.Loaded += MainWindow_Loaded;

            DAL.HttpHelper.Instance.AutoLoginAction += AutoLoginCallbackAction;
            DAL.HttpHelper.Instance.autoLoginActionDelegate += AutoLoginCallbackDelegate;

            ucRegister.CloseRegistAction += CloseRegistCallback;
            ucRegister.GotJobPublishAction += CloseRegistCallback;
            ucInfoSet.GotoJobAction += openPublish;

            DAL.SocketHelper.Instance.LoginAction += SocketLoginCallback;

            ucIM.actionCloseTick = CloseTickCallback;

            Messaging.Messenger.Default.Register<Messaging.MSBodyBase>(this, SecretaryButtonMsgCallbcak);
        }



        async void MainWindow_ContentRendered(object sender, EventArgs e)
        {
           
            await IniLimit();
            if (!MagicGlobal.GLimit.Contains(eLimit.F_LimitCommunicate.ToString()))
            {
                
                await ucIM.IniRecentChatList();
                await ucIM.UCIMInitial();
                await InitalTagsManage();
                await IniAccountSignIn();
            }
        }

        private async Task iniIntegral()
        {
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetMB, MagicGlobal.UserInfo.Version,
                 DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "serviceSign" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "S_Point" })));

            if (presult.StartsWith("连接失败"))
            {
                return;
            }
            HttpMBTotalRoot pObject = DAL.JsonHelper.ToObject<HttpMBTotalRoot>(presult);
            if (pObject.code == 200)
            {
                tbIntegralShow.Text = Math.Round(pObject.data.totalAmount, 2).ToString();
                Messaging.MSBodyBase<string> temp = new Messaging.MSBodyBase<string>() { type = "Intergral", body = pObject.data.totalAmount.ToString() };
                Messaging.Messenger.Default.Send<Messaging.MSBodyBase<string>, ViewModel.AssetsDetailViewModel>(temp);
            }
            
        }
        private async Task iniMB()
        {
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetMB, MagicGlobal.UserInfo.Version,
                    DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "serviceSign" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "S_Coin" })));

            if (presult.StartsWith("连接失败"))
            {
                return;
            }
            HttpMBTotalRoot pObject = DAL.JsonHelper.ToObject<HttpMBTotalRoot>(presult);
            if (pObject.code == 200)
            {
                tbMBShow.Text = Math.Round(pObject.data.totalAmount, 2).ToString();
                Messaging.Messenger.Default.Send<HttpMBTotalRoot, ViewModel.AssetsDetailViewModel>(pObject);
            }
                
        }
        private async Task iniViewContact()
        {
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetMB, MagicGlobal.UserInfo.Version,
                     DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "serviceSign" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "S_ViewContact" })));

            if (presult.StartsWith("连接失败"))
            {
                return;
            }
            HttpMBTotalRoot pObject = DAL.JsonHelper.ToObject<HttpMBTotalRoot>(presult);
            spViewCantect.Visibility = Visibility.Collapsed;
            if (pObject != null)
            {
                if (pObject.code == 200)
                {
                    if (pObject.data.totalAmount > 0)
                    {
                        spViewCantect.Visibility = Visibility.Visible;
                        tbCountShow.Text = Math.Round(pObject.data.totalAmount).ToString();
                        Messaging.MSBodyBase<string> temp = new Messaging.MSBodyBase<string>() { type = "download", body = pObject.data.totalAmount.ToString() };
                        Messaging.Messenger.Default.Send<Messaging.MSBodyBase<string>, ViewModel.AssetsDetailViewModel>(temp);
                    }
                }
            }
        }
        private void  MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsRegister)
                SetRegistPanel(true);

            //判断是否有密码
            PasswordIsExist();

            HrInfo.DataContext = MagicGlobal.UserInfo;

            IniNotifyIcon();

            iniSaveSearchConditionList();

            //包含修改资料、发布职位、简历管理、人才搜索、IM沟通功能
            if (!(MagicGlobal.GLimit.Contains(eLimit.F_LimitCommunicate.ToString())
                && MagicGlobal.GLimit.Contains(eLimit.F_LimitManageJob.ToString())
                 && MagicGlobal.GLimit.Contains(eLimit.F_LimitManageResume.ToString())
                 && MagicGlobal.GLimit.Contains(eLimit.F_LimitSearch.ToString())
                 && MagicGlobal.GLimit.Contains(eLimit.F_LimitUpdateInfo.ToString())))
            {
                mMainHintBinding = new MainHintBinding();
                mMainHintBinding.OpenPhblish = true;
                ucIM.tabHintRecent.DataContext = mMainHintBinding;
                ucIM.tabHintDynamic.DataContext = mMainHintBinding;
                ucIM.tabHintRecommend.DataContext = mMainHintBinding;
                initTextChat();         
                
                setOpenJob();
                
            }

            (PresentationSource.FromVisual(this) as HwndSource).AddHook(new HwndSourceHook(this.WndProc));
            Messaging.Messenger.Default.Register<Messaging.MSCloseWindow>(this, CloseCallback);
            Messaging.Messenger.Default.Register<Messaging.MSChangeAccount>(this, ChangeAccountCallback);

            

        }
        private async Task IniLimit()
        {
            if (MagicGlobal.GLimit.Contains(eLimit.F_LimitUpdateInfo.ToString()))
            {
                RBInfo.IsEnabled = false;
            }
            if (MagicGlobal.GLimit.Contains(eLimit.F_LimitCommunicate.ToString()))
            {
                RBIM.Visibility = Visibility.Collapsed;               
            }
            if (MagicGlobal.GLimit.Contains(eLimit.F_LimitManageResume.ToString()))
            {

                RBResume.Visibility = Visibility.Collapsed;
            }
            if (MagicGlobal.GLimit.Contains(eLimit.F_LimitManageJob.ToString()))
            {

                RBJob.Visibility = Visibility.Collapsed;
            }
            if (MagicGlobal.GLimit.Contains(eLimit.F_LimitSearch.ToString()))
            {

                RBSearch.Visibility = Visibility.Collapsed;
            }
            if (MagicGlobal.GLimit.Contains(eLimit.F_UseManageInterview.ToString()))
            {
                RBInterview.IsChecked = true;
               await ucInterview.IniInterview(true);
            }
        }

        public void iniBeforLoaded()
        {
            //IniHRAuth();
            GetLimit();
        }

        public void IniHRAuth()
        {
            string stdHRAuth = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string resultHRAuth =  DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrGetAuthenState, MagicGlobal.UserInfo.Version, stdHRAuth));

            ViewModel.BaseHttpModel<ViewModel.HttpAuthStateModel> modelHRAuth = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpAuthStateModel>>(resultHRAuth);
            if (modelHRAuth == null)
            {
                return;
            }
            else
            {
                if (modelHRAuth.code == 200)
                {
                    MagicGlobal.UserInfo.validStatus = modelHRAuth.data.auditStatus == "1";
                }
            }
        }

        /// <summary>
        /// 初始化签到
        /// </summary>
        /// <returns></returns>
        private async Task IniAccountSignIn()
        {

            if (MagicGlobal.UserInfo.IsHRAuth == 0)
                return;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });

            string uriVertify = string.Format(DAL.ConfUtil.AddrSignInVertify, MagicGlobal.UserInfo.Version, std);
            string resultVertify = await DAL.HttpHelper.Instance.HttpGetAsync(uriVertify);
            ViewModel.BaseHttpModel modelVertify = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel>(resultVertify);
            if(modelVertify != null)
            {
                if(modelVertify.code == 200)
                {
                    if((bool)modelVertify.data)
                    {
                        //return;
                    }
                }
            }
            string url = string.Format(DAL.ConfUtil.AddrSignIn, MagicGlobal.UserInfo.Version, std);
            string resultUrl = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            ViewModel.BaseHttpModel<HttpLoginQuantity> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<HttpLoginQuantity>>(resultUrl);
            if(model !=null)
            {
                string account = string.Empty;
                account = model.data.quantity;
                if(model.code == 200)
                {
                    string urlDetail = string.Format(DAL.ConfUtil.AddrSignInDetail, MagicGlobal.UserInfo.Version, std);
                    string resultDetail = await DAL.HttpHelper.Instance.HttpGetAsync(urlDetail);
                    ViewModel.BaseHttpModel<HttpSignInDetailModel> modelDetail = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<HttpSignInDetailModel>>(resultDetail);
                    if(modelDetail != null)
                    {
                        if(modelDetail.code == 200)
                        {
                            IntPtr activeForm = GetActiveWindow(); // 先得到当前的活动窗体 
                            WinLoginInDetail win;
                            //bool flag;
                            //if (MagicGlobal.UserInfo.IsHRAuth == 1)
                            //    flag = true;
                            //else
                            //    flag = false;
                            if (MagicGlobal.UserInfo.IsHRAuth == 0)
                                return;
                            int day;
                            int.TryParse(modelDetail.data.conSignInNum, out day);
                            if(modelDetail.data.conSignInNum == "0")
                            {
                                win = new WinLoginInDetail(5, account);
                            }
                            else
                            {
                                int remainDay = 5 - day % 5;
                                if (remainDay == 5)
                                    return;
                                win = new WinLoginInDetail(remainDay, account);
                            }
                            win.Show();
                            SetActiveWindow(activeForm); // 在把焦点还给之前的活动窗体
                        }
                    }
                }

            }
        }

        public void GetLimit()
        {
            MagicGlobal.GLimit = new List<string>();
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string result = DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrUserFunction, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<List<HttpLimitData>> model = DAL.JsonHelper.ToObject<BaseHttpModel<List<HttpLimitData>>>(result);
            if (model != null)
            {
                if (model.data != null)
                {
                    foreach (HttpLimitData iHttpLimitData in model.data)
                    {
                        MagicGlobal.GLimit.Add(iHttpLimitData.functionSign);
                    }
                }
            }
        }

        private async Task InitalTagsManage()
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string url = string.Format(DAL.ConfUtil.AddrResumTagsList, MagicGlobal.UserInfo.Version, std);
            string result = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            BaseHttpModel<List<HttpResumTags>> model = DAL.JsonHelper.ToObject<BaseHttpModel<List<HttpResumTags>>>(result);
            if (model == null)
                return;
            else
            {
                if(model.code == 200)
                {
                    MagicGlobal.TagsManagerList.Clear();
                    foreach(var item in model.data)
                    {
                        MagicGlobal.TagsManagerList.Add(new TagsManagerModel() { Id = item.userSignID, TagColor = item.userSignColor, TagContent = item.userSignName });
                    }
                    if(MagicGlobal.TagsManagerList.Count == 0)
                    {
                        string std1 = Uri.EscapeDataString(DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "userSignName", "userSignColor" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "未接电话", "#FFC540" }));
                        string url1 = string.Format(DAL.ConfUtil.AddrResumTagsAdd, MagicGlobal.UserInfo.Version, std1);
                        string result1 = await DAL.HttpHelper.Instance.HttpGetAsync(url1);

                        string std2 = Uri.EscapeDataString(DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "userSignName", "userSignColor" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "我很欣赏你", "#94A1FA" }));
                        string url2 = string.Format(DAL.ConfUtil.AddrResumTagsAdd, MagicGlobal.UserInfo.Version, std2);
                        string result2 = await DAL.HttpHelper.Instance.HttpGetAsync(url2);

                        string std3 = Uri.EscapeDataString(DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "userSignName", "userSignColor" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "待沟通", "#7FD3D8" }));
                        string url3 = string.Format(DAL.ConfUtil.AddrResumTagsAdd, MagicGlobal.UserInfo.Version, std3);
                        string result3 = await DAL.HttpHelper.Instance.HttpGetAsync(url3);

                        await InitalTagsManage();
                    }
                }
            }
           
           

        }

        private async void UpdatePassword(string password)
        {
            HttpSetPasswordModel model = new HttpSetPasswordModel() { userID = MagicGlobal.UserInfo.Id.ToString(), password = password, type = "setPassword" };
            string jsonStr = DAL.JsonHelper.ToJsonString(model);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrUpdatePassword, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel model2 = DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (model2 == null)
            {
                WinMessageLink link = new WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = GetWindow(this);
                link.ShowDialog();
                return;
            }
            else
            {
                if (model2.code != 200)
                {
                    WinMessageLink link = new WinMessageLink(model2.msg, "我知道了");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                    return;
                }
                MagicGlobal.UserInfo.PasswordExist = true;
                View.Message.DisappearShow dis = new View.Message.DisappearShow("修改成功", 1);
                dis.Owner = Window.GetWindow(this);
                dis.ShowDialog();
            }

        }

        private void  PasswordIsExist()
        {
            Action mehtod = delegate
            {
                string jsonStr = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "properties" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "password" });
                string resultStr = DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrGetUser, MagicGlobal.UserInfo.Version, jsonStr));
                BaseHttpModel<HttpGetUserModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpGetUserModel>>(resultStr);
                if (model == null)
                    return;
                else
                {
                    if (model.code == 200)
                    {
                        MagicGlobal.UserInfo.PasswordExist = model.data.existPwd;
                        if (!model.data.existPwd)
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                TemplateUC.WinCompletePassword win = new TemplateUC.WinCompletePassword();
                                win.Owner = Window.GetWindow(this);
                                if ((bool)win.ShowDialog())
                                {
                                    UpdatePassword(win.Password);
                                }
                            }));
                            
                        }
                    }
                }
            };
            mehtod.BeginInvoke(null, null);
           
        }
        

        #region window操作
        private HwndSource hwndSource;
        private Rect rcnormal;
        private double rcnWidth = 0;
        public string NormalOrMax
        {
            get { return (string)GetValue(NormalOrMaxProperty); }
            set { SetValue(NormalOrMaxProperty, value); }
        }
        public static readonly DependencyProperty NormalOrMaxProperty =
            DependencyProperty.Register("NormalOrMax", typeof(string), typeof(MainWindow), new PropertyMetadata(null));
        private void BtnNomalOrMax_Click(object sender, RoutedEventArgs e)
        {
            //CheckBox chk = sender as CheckBox;
            if (NormalOrMax == "Normal")
            {
                NormalOrMax = "Max";
                rcnormal = new Rect(this.Left, this.Top, this.Width, this.Height);//保存下当前位置与大小
                Rect rc = SystemParameters.WorkArea;//获取工作区大小
                //获取任务栏停靠位置
                int width, height;
                Windows32Operation.windowsLocation location = Windows32Operation.GetWindowsBarLocation(out width, out height);
                switch (location)
                {
                    case Windows32Operation.windowsLocation.bottom:
                        this.Left = 0;//设置位置
                        this.Top = 0;
                        this.Width = rc.Width;
                        this.Height = rc.Height;
                        break;
                    case Windows32Operation.windowsLocation.left:
                        this.Left = width;
                        this.Top = 0;
                        this.Width = rc.Width;
                        this.Height = rc.Height;
                        break;
                    case Windows32Operation.windowsLocation.right:
                        this.Left = 0;//设置位置
                        this.Top = 0;
                        this.Width = rc.Width;
                        this.Height = rc.Height;
                        break;
                    case Windows32Operation.windowsLocation.top:
                        this.Left = 0;
                        this.Top = height;
                        this.Width = rc.Width;
                        this.Height = rc.Height;
                        break;
                }
                this.ResizeMode = System.Windows.ResizeMode.NoResize;
                this.gdMain.Margin = new Thickness(0);
            }
            else
            {
                NormalOrMax = "Normal";
                this.Left = rcnormal.Left;
                this.Top = rcnormal.Top;
                this.Width = rcnormal.Width;
                this.Height = rcnormal.Height;
                //this.ResizeMode = System.Windows.ResizeMode.CanResize;
                this.gdMain.Margin = new Thickness(10);

            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void winMain_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.Width = this.rcnWidth;
                this.MinWidth = 1015;
            }
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (NormalOrMax == "Max")
                return;
            FrameworkElement fe = e.OriginalSource as FrameworkElement;
            if (fe == null)
                return;
            string strTemp = fe.Name;
            switch (strTemp)
            {
                case "topRec":
                    this.Cursor = Cursors.SizeNS;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61443), IntPtr.Zero);
                    break;
                case "botRec":
                    this.Cursor = Cursors.SizeNS;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61446), IntPtr.Zero);
                    break;
                case "rightRec":
                    this.Cursor = Cursors.SizeWE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61442), IntPtr.Zero);
                    break;
                case "leftRec":
                    this.Cursor = Cursors.SizeWE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61441), IntPtr.Zero);
                    break;
                case "nwseUpRec":
                    this.Cursor = Cursors.SizeNWSE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61444), IntPtr.Zero);
                    break;
                case "nwseDownRec":
                    this.Cursor = Cursors.SizeNWSE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61448), IntPtr.Zero);
                    break;
                case "neswDownRec":
                    this.Cursor = Cursors.SizeNESW;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61447), IntPtr.Zero);
                    break;
                case "neswUpRec":
                    this.Cursor = Cursors.SizeNESW;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61445), IntPtr.Zero);
                    break;
                default:
                    this.Cursor = Cursors.Arrow;
                    break;

            }
        }


        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.rcnWidth = this.Width;//保存下当前位置的宽度大小
            this.MinWidth = 0;
            this.Width = 0;
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //if (MagicGlobal.UserInfo.WindowsMinWhenClose)
            //{
            //    this.ShowInTaskbar = false;
            //    this.Hide();
            //    return;
            //}
            //if (MagicGlobal.UserInfo.ExistAppTip)
            //{
            //    if (!ClosingTip())
            //    {
            //        return;
            //    }
            //}
            //this.Close();
            //Environment.Exit(0);


            //Application.Current.Shutdown();
        }
        #endregion

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.NormalOrMax == "Normal")
                    this.DragMove();
            }
        }

        #region "全局事件"
        private async void GlobalEventCallback(object sender, GlobalEventArgs e)
        {
            switch (e.Flag)
            {
                case GlobalFlag.JobPublishList:
                    this.ucJobPublishSelect.JobPublishGet(e.Opacity as ObservableCollection<JobPublish>, sender as UserJobModel, JobPublishGoto.IM);
                    this.ucJobPublishSelect.Visibility = Visibility.Visible;
                    break;
                case GlobalFlag.openResumTalk:
                    UserJobModel model = new UserJobModel();
                    model = e.Opacity as UserJobModel;
                    await JobPublishSelectOKCallback(model, JobPublishGoto.IM);
                    break;
                case GlobalFlag.WindowMost:
                    this.Topmost = (bool)e.Opacity;
                    break;
                case GlobalFlag.openTagsManager:
                    this.ucTagsSetting.Visibility = Visibility.Visible;
                    this.ucTagsSetting.ChoosePanel(true);
                    break;
                case GlobalFlag.openTagsAdd:
                    this.ucTagsSetting.Visibility = Visibility.Visible;
                    this.ucTagsSetting.ChoosePanel(false);
                    break;
                case GlobalFlag.IsOpenQuipReplySetting:
                    if ((bool)e.Opacity)
                    {
                        this.ucQuipReplySetting.InitialQuipReplyList();
                        this.ucQuipReplySetting.Visibility = Visibility.Visible;
                    }
                    else
                        this.ucQuipReplySetting.Visibility = Visibility.Collapsed;
                    break;
                case GlobalFlag.TipOpenDynamicUI:
                    this.Show();
                    this.Activate();
                    this.WindowState = System.Windows.WindowState.Normal;
                    this.RBIM.IsChecked = true;
                    this.ucIM.RBInterest.IsChecked = true;
                    break;
                case GlobalFlag.TipOpenRecommandUI:
                    this.Show();
                    this.Activate();
                    this.WindowState = System.Windows.WindowState.Normal;
                    this.RBIM.IsChecked = true;
                    this.ucIM.RBRecommend.IsChecked = true;
                    break;
                case GlobalFlag.HrSendOffer:
                    this.ucJobPublishSelect.JobPublishGet(e.Opacity as ObservableCollection<JobPublish>, sender as UserJobModel, JobPublishGoto.hrSendOffer);
                    this.ucJobPublishSelect.Visibility = Visibility.Visible;
                    break;

            }
        }
        #endregion

        #region "回调函数"
        private void JobPublishSelectCloseCallback()
        {
            this.ucJobPublishSelect.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 打开IM聊天框
        /// </summary>
        /// <param name="sessionId"> contactRecordId</param>
        private async Task JobPublishSelectOKCallback(UserJobModel model, JobPublishGoto gotoEnum)
        {
            switch (gotoEnum)
            {
                case JobPublishGoto.IM:
                    this.Cursor = Cursors.Arrow;
                    RBIM.IsChecked = true;
                    await ucIM.OpenIMControl(model.userId.ToString(), model.jobId.ToString());
                    await ucIM.UCIMInitial();
                    break;
                case JobPublishGoto.hrSendOffer:
                    await this.ucSearch.ucResumeCollection.ucSearchResult.HrSendOffer(model.jobId, model.userId.ToString());
                    break;

            }

        }   

        private void UcTagsCloseCallback()
        {
            this.ucTagsSetting.Visibility = Visibility.Collapsed;
        }

        private async Task ChatCallback(string id,string jobID)
        {
            if (await ucIM.OpenIMControl(id, jobID))
            {
                RBIM.IsChecked = true;
                await ucIM.UCIMInitial();
            }
            else
            {
                WinErroTip pWinErroTip = new WinErroTip(DAL.ConfUtil.serverError);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
            }
            //ucIM.IniResume(id, 1);

        }



        private void MessageCallback(string content, int type)
        {
            if (content == "与智联账号的连接已断开，建议您重新绑定。")
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    WinErroTip msg = new WinErroTip(content);
                    msg.Owner = this;
                    msg.ShowDialog();
                    this.AccountBindingManageOpen();
                }));
            }
            else
            {
                if (type == 0)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        WinErroTip msg = new WinErroTip(content);
                        msg.ShowDialog();
                    }));
                }
            }
        }


        private void openPublish()
        {
            RBJob.IsChecked = true;
            ucJob.RBPublish.IsChecked = true;
            ucJob.ucJobPublish.iniPublish();
        }

        private async Task OpenJobGridClosePublish()
        {
            RBJob.IsChecked = true;
            ucJob.RBClose.IsChecked = true;
            await ucJob.ucJobClose.iniJobClose();
        }

        private void openResume(string arg1, long arg2)
        {
            RBResume.IsChecked = true;
            ucResume.iniResumeModuleById(arg1, arg2);
        }

        private async Task SearchCallback()
        {
            RBSearch.IsChecked = true;
            this.ucSearch.rbSearch.IsChecked = true;
       
            await this.ucSearch.ucResumeCollection.InitalCollection();
        }

        private void HRInfoCallback()
        {
            RBInfo.IsChecked = true;
            this.ucInfoSet.ucSelfInfo.InitialSelfInfo();
            this.ucInfoSet.rbSelfInfo.IsChecked = true;
        }

        private void AutoLoginCallbackAction()
        {
            Action method = delegate
            {
                //自动登录接口
                string std1 = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "properties" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), string.Empty });
                string jsonResult1 = DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrLoginByUserId, MagicGlobal.UserInfo.Version, std1));
            };
            method.BeginInvoke(null, null);
        }
        private async Task AutoLoginCallbackDelegate()
        {
            //自动登录接口
            string std1 = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "properties" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), string.Empty });
            string jsonResult1 = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrLoginByUserId, MagicGlobal.UserInfo.Version, std1));
        }


        private void CloseRegistCallback()
        {
            this.SetRegistPanel(false);
        }

        private async Task OpenCollectionPage()
        {
            RBResume.IsChecked = true;
            await ucResume.OpenCollection();
        }

        private async Task AddVCallBack()
        {
            RBInfo.IsChecked = true;
            ucInfoSet.rbAuthen.IsChecked = true;
            await ucInfoSet.IniAuthen();
        }

        private void SocketLoginCallback()
        {
            //推送系统登录
            MagicGlobal.UserInfo.PushDeviceID = DAL.SocketHelper.Instance.DeviceID;
            MagicGlobal.UserInfo.PushSecureKey = DAL.SocketHelper.Instance.SecureKey;
            HttpSocketBaseModel<HttpSocketLoginModel> model = new HttpSocketBaseModel<HttpSocketLoginModel>();
            model.cmd = "USER_LOGIN";
            model.data = new HttpSocketLoginModel();
            model.data.deviceID = MagicGlobal.UserInfo.PushDeviceID;
            model.data.userIdentity = "2";
            model.data.userID = MagicGlobal.UserInfo.Id.ToString();
            model.data.platform = "1004";
            model.data.version = MagicGlobal.UserInfo.Version;

            string jsonStr = DAL.JsonHelper.ToJsonString(model);
            DAL.SocketHelper.Instance.Send(jsonStr);
        }

        private async void CloseCallback(Messaging.MSCloseWindow sender)
        {
            MagicCube.DAL.HttpHelper.Instance.SaveCookie();
            MagicGlobal.SaveSeting();
            this.SaveInfo();

            //推送系统退出登录
            MagicGlobal.UserInfo.PushDeviceID = DAL.SocketHelper.Instance.DeviceID;
            MagicGlobal.UserInfo.PushSecureKey = DAL.SocketHelper.Instance.SecureKey;
            HttpSocketBaseModel<HttpSocketLoginModel> model = new HttpSocketBaseModel<HttpSocketLoginModel>();
            model.cmd = "USER_LOGOUT";
            model.data = new HttpSocketLoginModel();
            model.data.deviceID = MagicGlobal.UserInfo.PushDeviceID;
            model.data.userIdentity = "2";
            model.data.userID = MagicGlobal.UserInfo.Id.ToString();
            string jsonStr = DAL.JsonHelper.ToJsonString(model);
            DAL.SocketHelper.Instance.Send(jsonStr);
            DAL.SocketHelper.Instance.Close();
            string url = string.Format(DAL.ConfUtil.AddrLogOut, MagicGlobal.UserInfo.Version);
            await DAL.HttpHelper.Instance.HttpLogoutGetAsync(url);

            Environment.Exit(0);
            System.Windows.Application.Current.Shutdown();
            

        }

        private async void ChangeAccountCallback(Messaging.MSChangeAccount sender)
        {
            //if(sender != null)
            //{
            //    if(!sender.IsDirectChange)
            //    {
            //        WinConfirmTip tip = new WinConfirmTip("是否确定切换账号?");
            //        tip.Owner = this;
            //        if (!(bool)tip.ShowDialog())
            //            return;
            //    }
            //}
            //else
            //{
            //    WinConfirmTip tip = new WinConfirmTip("是否确定切换账号?");
            //    tip.Owner = this;
            //    if (!(bool)tip.ShowDialog())
            //        return;
            //}
            
            MagicCube.DAL.HttpHelper.Instance.ClearCookieFile();
            MagicGlobal.UserInfo.AutoLogin = false;
            MagicGlobal.SaveSeting();

            //推送系统退出登录
            MagicGlobal.UserInfo.PushDeviceID = DAL.SocketHelper.Instance.DeviceID;
            MagicGlobal.UserInfo.PushSecureKey = DAL.SocketHelper.Instance.SecureKey;
            HttpSocketBaseModel<HttpSocketLoginModel> model = new HttpSocketBaseModel<HttpSocketLoginModel>();
            model.cmd = "USER_LOGOUT";
            model.data = new HttpSocketLoginModel();
            model.data.deviceID = MagicGlobal.UserInfo.PushDeviceID;
            model.data.userIdentity = "2";
            model.data.userID = MagicGlobal.UserInfo.Id.ToString();
            string jsonStr = DAL.JsonHelper.ToJsonString(model);
            DAL.SocketHelper.Instance.Send(jsonStr);
            DAL.SocketHelper.Instance.Close();
            string url = string.Format(DAL.ConfUtil.AddrLogOut, MagicGlobal.UserInfo.Version);
            await DAL.HttpHelper.Instance.HttpLogoutGetAsync(url);

            logout();
        }

        public async void SecretaryButtonMsgCallbcak(Messaging.MSBodyBase sender)
        {
            switch(sender.type)
            {
                //跳转认证页面
                case "2001":
                    this.RBInfo.IsChecked = true;
                    this.ucInfoSet.rbAuthen.IsChecked = true;
                    await ucInfoSet.IniAuthen();
                    break;
                //职位管理页
                case "2002":
                    this.RBJob.IsChecked = true;
                    this.ucJob.RBPublish.IsChecked = true;
                    this.ucJob.ucJobPublish.iniPublish();
                    break;

                    
            }
        }
        #endregion

        #region "对内函数"

        private void AccountBindingManageOpen()
        {
            //this.RBSystemSetting.IsChecked = true;
            this.ucInfoSet.rbZhilianBinding.IsChecked = true;
        }


        private void RabbitMqAccountLogInCallback(string str)
        {
            //Console.WriteLine(str);
            //HttpLogoutTime HttpLogoutTime = DAL.JsonHelper.ToObject<HttpLogoutTime>(str);
            //if (HttpLogoutTime.nonce != MagicGlobal.GNonce)
            //{
            //    DateTime dt = DateTime.Now;
            //    string presult = DAL.HttpHelper.Instance.HttpGet(ConfUtil.ServerTime);
            //    if (!presult.StartsWith("连接失败"))
            //    {
            //        HttpServerTime pHttpServerTime = DAL.JsonHelper.ToObject<HttpServerTime>(presult);
            //        dt = Convert.ToDateTime(pHttpServerTime.result);
            //    }

            //    TimeSpan ND = dt - Convert.ToDateTime(HttpLogoutTime.responseDate);
            //    if (ND.TotalMinutes <= 1)
            //    {
            //        isOnLine = false;
            //        rabbitMqAccountLogIn.Close();

            //        this.Dispatcher.BeginInvoke((Action)delegate()
            //        {
            //            //timeAlive.Stop();
            //            //timeOpenTipBox.Stop();
            //            //AccountBindingLogin.Instance.StopAliveListen();
            //            //icoTimer.Stop();
            //            //WinConfirmTip tip = new WinConfirmTip("您的账号已在另一地点登录，如需重登请在1分钟后尝试。");
            //            MagicCube.TemplateUC.WinMessageLink tip = new TemplateUC.WinMessageLink("您的账号已在另一地点登录，如需重登请在1分钟后尝试。", "我知道了");
            //            tip.Owner = this;
            //            tip.ShowDialog();
            //            logout();
            //        });

            //    }
            //}
        }

        private void logout()
        {
            SaveInfo();
            Process.Start(Assembly.GetEntryAssembly().Location);
            Environment.Exit(0);

        }

        private void SaveInfo()
        {
            //存最近聊天列表
            //saveRecentList();
            //关闭托盘
            if (notifyIcon != null)
            {
                this.notifyIcon.Visible = false;
                this.notifyIcon.Dispose();
            }

            //using (FileStream fs = new FileStream(ConfUtil.dataPath + MagicGlobal.UserInfo.UserAccount + "//" + "SaveSearchConditionList.xml", FileMode.Create, FileAccess.Write))
            //{
            //    using (StreamWriter sw = new StreamWriter(fs))
            //    {
            //        foreach (var item in MagicGlobal.saveSearchConditionList)
            //        {
            //            sw.WriteLine(item.HttpCondition + "," + item.LocalCondition + "," + item.LocalHead);
            //        }
            //    }
            //}
            //MagicGlobal.SaveSeting();
        }


        private bool ClosingTip()
        {
            if (MagicGlobal.UserInfo.ExistAppTip)
            {
                if (RecentNew.Visibility == Visibility.Collapsed)
                {
                    WinCloseMessageTip tip = new WinCloseMessageTip("您确定要退出魔方小聘吗");
                    tip.Owner = this;
                    return (bool)tip.ShowDialog();
                }
                else
                {
                    WinCloseMessageTip tip = new WinCloseMessageTip("您有未读消息尚未处理，是否确定退出");
                    tip.Owner = this;
                    return (bool)tip.ShowDialog();
                }
            }
            else
                return true;
        }

        private void iniSaveSearchConditionList()
        {
            //if (File.Exists(ConfUtil.dataPath + MagicGlobal.UserInfo.UserAccount + "//" + "SaveSearchConditionList.xml"))
            //{
            //    using (FileStream fs = new FileStream(ConfUtil.dataPath + MagicGlobal.UserInfo.UserAccount + "//" + "SaveSearchConditionList.xml", FileMode.Open, FileAccess.Read))
            //    {
            //        using (StreamReader sr = new StreamReader(fs))
            //        {
            //            MagicGlobal.saveSearchConditionList.Clear();
            //            while (!sr.EndOfStream)
            //            {
            //                string line = sr.ReadLine();
            //                string[] item = line.Split(new char[] { ',' });
            //                //string[] headtemp = item[1].Split(new char[] { '/' });
            //                //string head = null;
            //                //if (headtemp.Length != 0)
            //                //{
            //                //    head = headtemp[0];
            //                //}
            //                if (item.Length == 3)
            //                    MagicGlobal.saveSearchConditionList.Add(new SaveSearchCondition() { HttpCondition = item[0], LocalCondition = item[1], LocalHead = item[2] });
            //            }
            //        }
            //    }
            //}
        }


        public void SetRegistPanel(bool isRegist)
        {
            if (isRegist)
            {
                TrackHelper2.TrackOperation("5.1.6.1.1", "pv");
                this.ucRegister.rbSelfInfo.IsChecked = true;
                //this.RBIM.Visibility = Visibility.Collapsed;

                //this.RBJob.Visibility = Visibility.Collapsed;
                //this.RBResume.Visibility = Visibility.Collapsed;
                //this.RBInterview.Visibility = Visibility.Collapsed;
                //this.RBSearch.Visibility = Visibility.Collapsed;
                this.RBSystemSetting.Visibility = Visibility.Collapsed;
                this.bdRigstShade.Visibility = Visibility.Visible;
                this.gdMBShow.Visibility = Visibility.Collapsed;
                this.bdMenu.Height = 40;
                this.btnChangeAccout.Visibility = Visibility.Collapsed;
                this.btnModifyPassword.Visibility = Visibility.Collapsed;
                this.rectMainSplit.Visibility = Visibility.Collapsed;
                gdRegister.Visibility = Visibility.Visible;
                this.gdNewRed.Visibility = Visibility.Collapsed;

                this.btnMenu.Visibility = Visibility.Visible;
            }
            else
            {
                //this.RBIM.Visibility = Visibility.Visible;

                //this.RBJob.Visibility = Visibility.Visible;
                //this.RBResume.Visibility = Visibility.Visible;
                //this.RBInterview.Visibility = Visibility.Visible;
                //this.RBSearch.Visibility = Visibility.Visible;
                this.RBSystemSetting.Visibility = Visibility.Visible;
                this.bdRigstShade.Visibility = Visibility.Collapsed;

                this.gdMBShow.Visibility = Visibility.Visible;
                this.bdMenu.Height = 111;
                this.btnChangeAccout.Visibility = Visibility.Visible;
                this.btnModifyPassword.Visibility = Visibility.Visible;
                this.rectMainSplit.Visibility = Visibility.Visible;

                gdRegister.Visibility = Visibility.Collapsed;
                this.btnMenu.Visibility = Visibility.Collapsed;
                this.gdNewRed.Visibility = Visibility.Visible;
                

                ////打开发布职位页面
                RBJob.IsChecked = true;
                ucJob.RBPublish.IsChecked = true;
                ucJob.ucJobPublish.iniPublish();
            }
        }

        #endregion

        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == MessageHelper.WM_DOWNLOAD_COMPLETED)
            {
                Console.WriteLine("收到全局钩子");
                this.Show();
                this.Activate();
                this.WindowState = System.Windows.WindowState.Normal;
            }
            if (msg ==Convert.ToInt32(XPMsg.XPVIDEO_LOGIN))
            {
                if((int)wParam == 1)
                    MessageBox.Show("收到登录成功");
                else
                    MessageBox.Show("收到登录失败");
            }
            return hwnd;
        }

        private async void RBResume_Click(object sender, RoutedEventArgs e)
        {

            Common.TrackHelper2.TrackOperation("5.5.1.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.5.1.4.1", "pv");
            await ucResume.iniResumeModule();
        }


        private async void RBJob_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.1.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.3.1.2.1", "pv");
            //ucJob.iniJobCount();
            ucJob.RBOpen.IsChecked = true;
            await ucJob.ucJobOpen.iniJobOpen();
            ucJob.tbTitle.Text = "发布中的职位";
            ucJob.SetJobCount();
        }

        private async void RBIM_Click(object sender, RoutedEventArgs e)
        {
            if (this.RBIM.IsChecked == true)
                this.RBIMIsChecked = true;
            //埋点
            Common.TrackHelper2.TrackOperation("5.2.1.2.1", "clk");
            Common.TrackHelper2.TrackOperation("5.2.1.1.1", "clk");

            //新埋点
            Common.TrackHelper2.TrackOperation("5.2.1.1.1", "pv");

            setOpenJob();

            await ucIM.IniRecentChatList();
            await ucIM.UCIMInitial();
            
        }

        private void winMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            DAL.HttpHelper.Instance.SaveCookie();
            MagicGlobal.SaveSeting();
            this.ShowInTaskbar = false;
            this.Hide();
            e.Cancel = true;
            //string url = string.Format(DAL.ConfUtil.AddrLogOut, MagicGlobal.UserInfo.Version);
            //await DAL.HttpHelper.Instance.HttpGetAsync(url);
            return;


        }
        private void setOpenJob()
        {
            Action action = new Action(() =>
            {
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
                string jsonResult = DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrGetJobAccount, MagicGlobal.UserInfo.Version, std));
                BaseHttpModel<JobTotal> model = DAL.JsonHelper.ToObject<BaseHttpModel<JobTotal>>(jsonResult);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (model != null)
                    {
                        if (model.code == 200)
                        {
                            if (model.data.onlineJobNum > 0 || model.data.offlineJobNum > 0)
                            {
                                mMainHintBinding.OpenPhblish = true;
                            }
                            else
                            {
                                mMainHintBinding.OpenPhblish = false;
                            }
                        }
                    }


                }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            });
            action.BeginInvoke(null, null);
        }


        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag.ToString())
            {

                case "ModifyPassword":
                    //this.RBSystemSetting.IsChecked = true;
                    //this.ucSystem.rbSafeSet.IsChecked = false;
                    //this.ucSystem.rbSafeSet.IsChecked = true;
                    this.PopMenu.IsOpen = false;
                    //埋点
                    Common.TrackHelper2.TrackOperation("5.8.1.3.1", "clk");
                    break;

                case "menu":
                    if (PopMenu.IsOpen)
                    {
                        PopMenu.IsOpen = false;
                    }
                    else
                    {
                        PopMenu.IsOpen = true;
                        //埋点
                        Common.TrackHelper2.TrackOperation("5.8.1.1.1", "clk");
                        Common.TrackHelper2.TrackOperation("5.8.1.2.1", "pv");
                    }

                    break;

            }
        }

        private void BtnCancellation_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.8.1.4.1", "clk");

            //WinConfirmTip tip = new WinConfirmTip("是否确定切换账号?");
            //tip.Owner = this;
            //if (!(bool)tip.ShowDialog())
            //    return;
            DAL.HttpHelper.Instance.ClearCookieFile();
            MagicGlobal.UserInfo.AutoLogin = false;
            MagicGlobal.SaveSeting();
          
            logout();
        }

        private async void BtnStrongClose_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.8.1.5.1", "clk");
            DAL.HttpHelper.Instance.SaveCookie();
            MagicGlobal.SaveSeting();
            this.SaveInfo();

            //推送系统退出登录
            MagicGlobal.UserInfo.PushDeviceID = DAL.SocketHelper.Instance.DeviceID;
            MagicGlobal.UserInfo.PushSecureKey = DAL.SocketHelper.Instance.SecureKey;
            HttpSocketBaseModel<HttpSocketLoginModel> model = new HttpSocketBaseModel<HttpSocketLoginModel>();
            model.cmd = "USER_LOGOUT";
            model.data = new HttpSocketLoginModel();
            model.data.deviceID = MagicGlobal.UserInfo.PushDeviceID;
            model.data.userIdentity = "2";
            model.data.userID = MagicGlobal.UserInfo.Id.ToString();
            string jsonStr = DAL.JsonHelper.ToJsonString(model);
            DAL.SocketHelper.Instance.Send(jsonStr);
            DAL.SocketHelper.Instance.Close();
            string url = string.Format(DAL.ConfUtil.AddrLogOut, MagicGlobal.UserInfo.Version);
            await DAL.HttpHelper.Instance.HttpLogoutGetAsync(url);

            Environment.Exit(0);
            System.Windows.Application.Current.Shutdown();

        }

        private void RBInfo_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.1.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.7.1.3.1", "pv");
            this.ucInfoSet.ucSelfInfo.InitialSelfInfo();
            this.ucInfoSet.rbSelfInfo.IsChecked = true;
        }

        private void RBSystemSetting_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.9.1.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.9.1.3.1", "pv");
            Messaging.Messenger.Default.Send<string, ViewModel.SysAccontSetModel>("UpdateAccountSet");
           
            if (winSysSet == null)
            {
                winSysSet = new WinSystemSet();
                winSysSet.Owner = this;
                winSysSet.Show();
            }
            else
            {
                try
                {
                    winSysSet.Show();
                    winSysSet.Left = this.Left + this.ActualWidth / 2 - 600 / 2;
                    winSysSet.Top = this.Top + this.ActualHeight / 2 - 540 / 2;
                    winSysSet.rdoAccountSet.IsChecked = true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
             
                winSysSet.Activate();
                winSysSet.WindowState = System.Windows.WindowState.Normal;
            }




            //this.ucSystem.rbBasicSet.IsChecked = true;
        }

        private async void RBSearch_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.1.1.1", "clk");
            //this.ucSearch.ucResumeCollection.ucSearchCondition.IniSearchingList();
            this.ucSearch.rbSearch.IsChecked = true;
            await this.ucSearch.ucResumeCollection.InitalCollection();
            
        }
        private async void RBInterview_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.4.1.1.1", "clk");
            if (MagicGlobal.GLimit.Contains(eLimit.F_UseManageInterview.ToString()))
            {
                await ucInterview.IniInterview(true);
            }
            else
            {
                await ucInterview.IniInterview(false);
            }
        }
        private void winMain_Activated(object sender, EventArgs e)
        {

   
        }

        private void MB_Click(object sender, RoutedEventArgs e)
        {
            this.ucMyAssets.InitialMyAssets();
            //PopMenuMB.IsOpen = true;
            //iniMB();
            //iniIntegral();
            //iniViewContact();
        }
        private void AddVPop_MouseEnter(object sender, MouseEventArgs e)
        {
            addVPopUpPosition.IsChecked = true;
        }

        private async void AddV_Click(object sender, RoutedEventArgs e)
        {
            addVPopUpPosition.IsChecked = false;
            await AddVCallBack();
        }

        private void RBIM_Checked(object sender, RoutedEventArgs e)
        {
            this.RBIMIsChecked = true;
        }


        private void addVPopUpPosition_Click(object sender, RoutedEventArgs e)
        {
            addVPopUpPosition.IsChecked = true;
        }

        private bool mbMouseEnter = false;
        private async void btnMB_MouseEnter(object sender, MouseEventArgs e)
        {
            //RadioButton btn = sender as RadioButton;
            //Console.WriteLine(btn.IsMouseOver);
            mbMouseEnter = true;
            if (mbMouseEnter == false)
                return;
            bdMenuMB.Visibility = Visibility.Visible;
            mbMouseEnter = false;
            await TaskEx.WhenAll(iniMB(), iniIntegral(), iniViewContact());



        }

        private void btnMB_MouseLeave(object sender, MouseEventArgs e)
        {
            //RadioButton btn = sender as RadioButton;
            //Console.WriteLine(btn.IsMouseOver);
            mbMouseEnter = false;
            bdMenuMB.Visibility = Visibility.Collapsed;
        }
    }
}
