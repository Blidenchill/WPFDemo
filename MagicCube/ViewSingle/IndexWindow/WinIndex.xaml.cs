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
using System.Windows.Shapes;
using MagicCube.ViewModel;
using MagicCube.Common;
using MagicCube.Model;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using System.IO;
using MagicCube.HttpModel;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// WinIndex.xaml 的交互逻辑
    /// </summary>
    public partial class WinIndex : Window
    {
        #region "构造函数"
        public WinIndex()
        {
            InitializeComponent();
            this.bdMainRegist.Visibility = Visibility.Collapsed;
            this.bdMainLogin.Visibility = Visibility.Visible;
            this.bdForgetPassword.Visibility = Visibility.Collapsed;
            MagicGlobal.trackDateTime = DateTime.Now;
            TrackHelper2.FirstTrackOperation("OpenSuccess");
            TrackHelper2.TrackOperation("5.1.1.1.1", "pv");
            //System.Threading.ThreadPool.QueueUserWorkItem(WinIndex_Loaded);

            this.Loaded += WinIndex_Loaded;

        }
        #endregion

        #region "依赖属性"

        #endregion

        #region "变量"
        /// <summary>
        /// 用户账号
        /// </summary>
        private string userAccount = string.Empty;
        /// <summary>
        /// 用户密码
        /// </summary>
        private string passWords = string.Empty;
        /// <summary>
        /// 用户手机号
        /// </summary>
        private string userMobile = string.Empty;
        /// <summary>
        /// 任务栏Icon
        /// </summary>
        private NotifyIcon notifyIcon;
        /// <summary>
        /// 主窗口
        /// </summary>
        private MainWindow newWinMain;
        /// <summary>
        /// 验证码1分钟计时器
        /// </summary>
        private System.Timers.Timer VertifytimeAlive;
        private double interval = 1000;
        private int TickNum = 60;
        /// <summary>
        /// 验证码类型，"sms"验证码，"voice" 语音验证码
        /// </summary>
        private string verifyType = string.Empty;
        private string registerVertyfyType = string.Empty;
        private string forgetPwVertyfyType = string.Empty;

        private bool IsClickSignUpLink = false;
        private bool IsClickPasswordTab = false;

        #endregion

        #region "事件响应"
        /// <summary>
        /// 窗口激活事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Activated(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
        /// <summary>
        /// 点击获取验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void chkGetVertifyCode_Click(object sender, RoutedEventArgs e)
        {
            //新埋点
            TrackHelper2.TrackOperation("5.1.1.4.1", "clk");
            if (string.IsNullOrEmpty(this.txtTelephone.Text))
            {
                this.chkGetVertifyCode.IsChecked = false;
                bdTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErroTelephone.Visibility = Visibility.Visible;
                tbErroTelephone.Text = "手机号不能为空";

                return;
            }
            else if(!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtTelephone.Text))
            {
                this.chkGetVertifyCode.IsChecked = false;
                bdTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErroTelephone.Visibility = Visibility.Visible;
                tbErroTelephone.Text = "手机号格式错误";
                return;
            }
            this.userMobile = this.txtTelephone.Text;
            this.busyCtrl.IsBusy = true;


            //先判断手机号
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtTelephone.Text });
            string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (Model == null)
            {
                this.busyCtrl.IsBusy = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkGetVertifyCode.IsChecked = false;
                return;

            }

            switch (Model.code)
            {
                //手机号未注册
                case 100:
                    this.busyCtrl.IsBusy = false;
                    this.chkGetVertifyCode.IsChecked = false;
                    return;

                //账号b和c冻结
                case -102:
                    this.busyCtrl.IsBusy = false;
                    this.chkGetVertifyCode.IsChecked = false;
                    return;
                //账号B因违规已被冻结
                case -125:
                    this.busyCtrl.IsBusy = false;
                    this.chkGetVertifyCode.IsChecked = false;
                    return;
            }
            string std = string.Empty;

            if(this.stkPicVertify.Visibility == Visibility.Visible)
            {
                if(string.IsNullOrEmpty(this.txtPicVertify.Text))
                {
                    this.busyCtrl.IsBusy = false;
                    this.chkGetVertifyCode.IsChecked = false;
                    bdPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    txtPicVertify.Text = string.Empty;
                    tbErrorPicVertify.Visibility = Visibility.Visible;
                    tbErrorPicVertify.Text = "图形验证码不能为空";
                    return;
                }
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey", "imageCode" }, new string[] { this.txtTelephone.Text, "sms", "login",this.txtPicVertify.Text });
            }
            else
            {
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtTelephone.Text, "sms", "login" });

            }
            string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrSendVertifyCode, MagicGlobal.UserInfo.Version, std));
          
            BaseHttpModel<HttpVertifyCodeModel> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpVertifyCodeModel>>(jsonResult);
            if(model == null)
            {
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkGetVertifyCode.IsChecked = false;
                return;
            }
            if (model.code == 200)
            {
                if(model.data.showImageVerifyCode)
                {
                    this.chkGetVertifyCode.IsChecked = false;
                    ////获取图形验证码
                    imgPicVertify.Source = await GetImgVertifyCode(this.txtTelephone.Text);
                    this.tbErrorPicVertify.Visibility = Visibility.Visible;
                    this.bdPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    this.stkPicVertify.Visibility = Visibility.Visible;
                    return;
                }
                this.busyCtrl.IsBusy = false;
                this.verifyType = "sms";
                //职位沟通定时器
                VertifytimeAlive = new System.Timers.Timer(this.interval);
                VertifytimeAlive.Elapsed += VertifytimeAlive_Elapsed;
                VertifytimeAlive.Enabled = true;
                this.TickNum = 60;
                this.tbClickNum.Text = "(" + this.TickNum.ToString() + ")";

                //测试
                Console.WriteLine("验证码：" + model.data.verifyCode.ToString());
            }
            else if(model.code == -2)
            {
                this.busyCtrl.IsBusy = false;
                bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErroVertifyCode.Text = "获取验证码次数达到上限，如有疑问请联系我们 010-59423287";
                tbErroVertifyCode.Visibility = Visibility.Visible;
                this.chkGetVertifyCode.IsChecked = false;
            }
            //验证码过期
            else if(model.code == -119)
            {
                this.busyCtrl.IsBusy = false;
                bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErroVertifyCode.Text = "验证码已过期，请重新获取";
                tbErroVertifyCode.Visibility = Visibility.Visible;
                this.chkGetVertifyCode.IsChecked = false;
            }
            else if(model.code == -137)
            {
                this.busyCtrl.IsBusy = false;
                this.chkGetVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                imgPicVertify.Source = await GetImgVertifyCode(this.txtTelephone.Text);
                bdPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                txtPicVertify.Text = string.Empty;
                tbErrorPicVertify.Visibility = Visibility.Visible;
                tbErrorPicVertify.Text = "请输入正确的验证码";
            }
            else
            {
                this.busyCtrl.IsBusy = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkGetVertifyCode.IsChecked = false;
            }
        }
        /// <summary>
        /// 点击语音验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnVoicCode_Click(object sender, RoutedEventArgs e)
        {
            //新埋点
            TrackHelper2.TrackOperation("5.1.1.8.1", "clk");
            //判断是否可以验证
            if((bool)this.chkGetVertifyCode.IsChecked)
            {
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("操作过于频繁", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();
                return;
            }

            if (string.IsNullOrEmpty(this.txtTelephone.Text))
            {
                this.chkGetVertifyCode.IsChecked = false;
                bdTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErroTelephone.Visibility = Visibility.Visible;
                tbErroTelephone.Text = "手机号不能为空";

                return;
            }
            else if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtTelephone.Text))
            {
                this.chkGetVertifyCode.IsChecked = false;
                bdTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErroTelephone.Visibility = Visibility.Visible;
                tbErroTelephone.Text = "手机号格式错误";
                return;
            }
            this.busyCtrl.IsBusy = true;

            //先判断手机号
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtTelephone.Text });
            string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            
            if (Model == null)
            {
                this.busyCtrl.IsBusy = false;
                return;
            }
                

            switch (Model.code)
            {
                //手机号未注册
                case 100:
                    this.busyCtrl.IsBusy = false;
                    return;
                    
                //账号b和c冻结
                case -102:
                    this.busyCtrl.IsBusy = false;
                    return;
                //账号B因违规已被冻结
                case -125:
                    this.busyCtrl.IsBusy = false;
                    return;
            }
            string std = string.Empty;

            if (this.stkPicVertify.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(this.txtPicVertify.Text))
                {
                    this.chkGetVertifyCode.IsChecked = false;
                    this.busyCtrl.IsBusy = false;
                    bdPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorPicVertify.Visibility = Visibility.Visible;
                    tbErrorPicVertify.Text = "图形验证码不能为空";
                    return;
                }
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey", "imageCode" }, new string[] { this.txtTelephone.Text, "voice", "login", this.txtPicVertify.Text });
            }
            else
            {
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtTelephone.Text, "voice", "login" });

            }

            string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrSendVertifyCode, MagicGlobal.UserInfo.Version, std));
            
            BaseHttpModel<HttpVertifyCodeModel> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpVertifyCodeModel>>(jsonResult);
 
            if (model.code == 200)
            {
                if (model.data.showImageVerifyCode)
                {
                    this.chkGetVertifyCode.IsChecked = false;
                    this.busyCtrl.IsBusy = false;
                    ////获取图形验证码
                    imgPicVertify.Source = await GetImgVertifyCode(this.txtTelephone.Text);
                    this.tbErrorPicVertify.Visibility = Visibility.Visible;
                    this.bdPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    this.stkPicVertify.Visibility = Visibility.Visible;
                    return;
                }
                this.busyCtrl.IsBusy = false;
                this.verifyType = "voice";
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("电话拨打中，请留意来电", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();
                
                //职位沟通定时器
                VertifytimeAlive = new System.Timers.Timer(this.interval);
                VertifytimeAlive.Elapsed += VertifytimeAlive_Elapsed;
                VertifytimeAlive.Enabled = true;
                this.TickNum = 60;
                this.tbClickNum.Text = "(" + this.TickNum.ToString() + ")";
                this.chkGetVertifyCode.IsChecked = true;

                //测试
                Console.WriteLine("验证码：" + model.data.verifyCode.ToString());
            }
            else if (model.code == -2)
            {
                this.busyCtrl.IsBusy = false;
                bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErroVertifyCode.Text = "获取验证码次数达到上限，如有疑问请联系我们 010-59423287";
                tbErroVertifyCode.Visibility = Visibility.Visible;
                this.chkGetVertifyCode.IsChecked = false;
            }
            else if (model.code == -137)
            {
                this.busyCtrl.IsBusy = false;
                this.chkGetVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                imgPicVertify.Source = await GetImgVertifyCode(this.txtTelephone.Text);
            }
            else
            {
                this.busyCtrl.IsBusy = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkGetVertifyCode.IsChecked = false;
            }
        }
        /// <summary>
        /// 自动登录check控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            if((bool)this.rbPhoneLogin.IsChecked)
            TrackHelper2.TrackOperation("5.1.1.6.1", "clk");
            else
                TrackHelper2.TrackOperation("5.1.2.4.1", "clk");
            //switch ((sender as System.Windows.Controls.CheckBox).Name)
            //{
            //    case "RememberPassword":
            //        //if (RememberPassword.IsChecked == false)
            //        //{
            //        //    AutoLogin.IsChecked = false;
            //        //}
            //        ////新埋点
            //        //TrackHelper2.TrackOperation("5.1.1.6.1", "clk");
            //        break;
            //    case "AutoLogin":
            //        //if (AutoLogin.IsChecked == true)
            //        //{
            //        //    RememberPassword.IsChecked = true;

            //        //}
            //        //新埋点

            //        break;
            //}
        }
        /// <summary>
        /// 注册按钮&& 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkClick(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.Button).Content.ToString() == "注册账号")
            {
                if(this.rbPhoneLogin.IsChecked == true)
                {
                    //System.Diagnostics.Process.Start(MagicCube.DAL.ConfUtil.ServerIERegister + "&spm=5.1.1.9.1");
                    //新埋点2
                    TrackHelper2.TrackOperation("5.1.1.9.1", "clk");
                }
                else
                {
                    //System.Diagnostics.Process.Start(MagicCube.DAL.ConfUtil.ServerIERegister + "&spm=5.1.2.7.1");
                    //新埋点2
                    TrackHelper2.TrackOperation("5.1.2.7.1", "clk");
                }
                this.bdMainLogin.Visibility = Visibility.Collapsed;
                this.bdMainRegist.Visibility = Visibility.Visible;
                this.bdForgetPassword.Visibility = Visibility.Collapsed;
            
            }
            else
            {
                ////找回密码
                //System.Diagnostics.Process.Start(string.Format( MagicCube.DAL.ConfUtil.ServerIEGetPassword, this.txtUserAccount.Text) + "&spm=5.1.2.5.1");
                ////新埋点2
                this.bdMainLogin.Visibility = Visibility.Collapsed;
                this.bdMainRegist.Visibility = Visibility.Collapsed;
                this.bdForgetPassword.Visibility = Visibility.Visible;
                this.txtForgetPwTel.Text = this.txtUserAccount.Text;
                TrackHelper2.TrackOperation("5.1.2.5.1", "clk");

            }
            this.txtTelephone.Focus();
        }
        /// <summary>
        /// 登录button事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SignClick(object sender, RoutedEventArgs e)
        {
            try
            {

                //新埋点
                if ((bool)this.rbPhoneLogin.IsChecked)
                {
                    TrackHelper2.TrackOperation("5.1.1.7.1", "clk");
                }
                else
                {
                    TrackHelper2.TrackOperation("5.1.2.6.1", "clk");
                }


                //登陆
                

                await LoginAsyn();
               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// 最小化Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        /// <summary>
        /// 关闭button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (notifyIcon != null)
            {
                this.notifyIcon.Visible = false;
                this.notifyIcon.Dispose();
                this.notifyIcon = null;
            }
            Environment.Exit(0);
            System.Windows.Application.Current.Shutdown();
        }
        /// <summary>
        /// 密码输入textbox按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void cPassWord_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(this.bdMainLogin.Visibility == Visibility.Visible)
                {
                    btnSend.Focus();
                    if (!busyCtrl.IsBusy)
                    {
                        await LoginAsyn();
                    }
                   
                }
                else if(this.bdMainRegist.Visibility == Visibility.Visible)
                {
                    btnRegist.Focus();
                    if (!busyCtrl.IsBusy)
                    {
                        await RegisterStart();
                    }
                }
               
            }
        }
        /// <summary>
        /// 窗口拖拽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        /// <summary>
        /// 界面四个texbox获取焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {

            //bdError.Visibility = System.Windows.Visibility.Collapsed;
            this.SetErroTagsClose();
            FrameworkElement fe = sender as FrameworkElement;

            switch(fe.Name)
            {
                case "txtUserAccount":
                    //新埋点2
                    TrackHelper2.TrackOperation("5.1.2.2.1", "clk");
                    break;
                case "txtPassWord":
                    //新埋点2
                    TrackHelper2.TrackOperation("5.1.2.3.1", "clk");
                    break;
                case "txtTelephone":
                    TrackHelper2.TrackOperation("5.1.1.3.1", "clk");
                    break;
                case "txtVertifyCode":
                    TrackHelper2.TrackOperation("5.1.1.5.1", "clk");
                    break;
                case "txtRegistTel":
                    TrackHelper2.TrackOperation("5.1.4.2.1", "clk");
                    break;
                case "txtRegistVertifyCode":
                    TrackHelper2.TrackOperation("5.1.4.3.1", "clk");
                    break;
                case "txtRegistNewPassword":
                    TrackHelper2.TrackOperation("5.1.4.6.1", "clk");
                    break;
                case "txtForgetPwTel":
                    TrackHelper2.TrackOperation("5.1.5.2.1", "clk");
                    break;
                case "txtForegetPwVertifyCode":
                    TrackHelper2.TrackOperation("5.1.5.3.1", "clk");
                    break;
                case "txtForegetPwNewPassword":
                    TrackHelper2.TrackOperation("5.1.5.6.1", "clk");
                    break;
                case "txtForegetPwConfirmPassword":
                    TrackHelper2.TrackOperation("5.1.5.7.1", "clk");

                    break;

            }          
        }
        /// <summary>
        /// 验证码登录tab事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbPhoneLogin_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.2.1.1", "clk");
            this.txtTelephone.Focus();
        }
        /// <summary>
        /// 密码登录tab事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbPasswordLogin_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.1.2.1", "clk");
            if(!string.IsNullOrEmpty(this.txtTelephone.Text))
            {
                this.txtUserAccount.Text = this.txtTelephone.Text;
            }
        }
        /// <summary>
        /// 手机号输入框失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void txtTelephone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.IsClickSignUpLink)
                return;
            if (this.IsClickPasswordTab)
                return;
            if (string.IsNullOrWhiteSpace(this.txtTelephone.Text))
                return;
            if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtTelephone.Text))
                return;
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtTelephone.Text });
            string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (Model == null)
                return;

            switch (Model.code)
            {
                //手机号未注册
                case 100:
                    TemplateUC.WinMessageLink linkError = new TemplateUC.WinMessageLink("亲，您的手机号尚未注册，请先去注册", "去注册");
                    linkError.Owner = Window.GetWindow(this);
                    if(!linkError.IsActive)
                    {
                        if ((bool)linkError.ShowDialog())
                        {
                            //System.Diagnostics.Process.Start(MagicCube.DAL.ConfUtil.ServerIERegister);
                            this.bdMainLogin.Visibility = Visibility.Collapsed;
                            this.bdMainRegist.Visibility = Visibility.Visible;
                            this.bdForgetPassword.Visibility = Visibility.Collapsed;
                            this.txtRegistTel.Text = this.txtTelephone.Text;
                        }
                    }
                    
                    this.txtTelephone.Focus();
                    break;
                //账号b和c冻结
                case -102:
                    TemplateUC.WinAccountLock2 lock2 = new TemplateUC.WinAccountLock2();
                    lock2.Owner = Window.GetWindow(this);
                    lock2.ShowDialog();
                    this.txtTelephone.Focus();
                    break;
                //账号B因违规已被冻结
                case -125:
                    TemplateUC.WinAccountLock1 lock1 = new TemplateUC.WinAccountLock1(string.Empty);
                    lock1.Owner = Window.GetWindow(this);
                    lock1.ShowDialog();
                    this.txtTelephone.Focus();
                    break;
            }
        }
        /// <summary>
        /// 账号输入框失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void txtUserAccount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.IsClickSignUpLink)
                return;
            if (string.IsNullOrWhiteSpace(this.txtUserAccount.Text))
                return;
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtUserAccount.Text });
            string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (Model == null)
                return;
            switch (Model.code)
            {
                ////手机号未注册
                //case 100:
                //    TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink("账号不存在，请先去魔方官网注册", "去注册");
                //    link.Owner = Window.GetWindow(this);
                //    if ((bool)link.ShowDialog())
                //    {
                //        System.Diagnostics.Process.Start(MagicCube.DAL.ConfUtil.ServerRegister);
                //    }
                //    break;
                //账号b和c冻结
                case -102:
                    TemplateUC.WinAccountLock1 lock1 = new TemplateUC.WinAccountLock1(string.Empty);
                    lock1.Owner = Window.GetWindow(this);
                    lock1.ShowDialog();
                    break;
                //账号B因违规已被冻结
                case -125:
                    TemplateUC.WinAccountLock2 lock2 = new TemplateUC.WinAccountLock2();
                    lock2.Owner = Window.GetWindow(this);
                    lock2.ShowDialog();
                    break;
            }
        }

        #endregion

        #region "方法"
        /// <summary>
        /// 初始化任务栏通知图标
        /// </summary>
        private void IniIcon()
        {
            try
            {
                this.notifyIcon = new NotifyIcon();
                this.notifyIcon.Text = "魔方小聘";
                this.notifyIcon.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "GreyIcon.ico");
                this.notifyIcon.Visible = true;
                //打开菜单项
                System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("打开");
                open.Click += new EventHandler(Show);
                //退出菜单项
                System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
                exit.Click += new EventHandler(Close);
                //关联托盘控件
                System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
                notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

                this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
                {
                    if (e.Button == MouseButtons.Left) this.Show(o, e);
                });
            }
            catch { }

        }
        /// <summary>
        /// 初始化用户信息
        /// </summary>
        public async Task InitUser()
        {
            if (MagicGlobal.UserInfo == null)
                MagicGlobal.UserInfo = XmlClassData.ReadDataFromXml<UserSetting>(DAL.ConfUtil.LocalHomePath + "userinfo.db");
            if (MagicGlobal.UserInfo != null)
            {
                txtUserAccount.Text = MagicGlobal.UserInfo.UserAccount;
                txtPassWord.Password = MagicGlobal.UserInfo.Password;
                AutoLogin.IsChecked = MagicGlobal.UserInfo.AutoLogin;
                this.txtTelephone.Text = MagicGlobal.UserInfo.UserMobile;
                if (MagicGlobal.UserInfo.AutoLogin)
                {
                    await AutoLoginAsyn();
                }
            }
            else
            {
                MagicGlobal.UserInfo = new UserSetting();
                MagicGlobal.UserInfo.AutoRunSetting = true;
                MagicGlobal.UserInfo.ExistAppTip = true;
                MagicGlobal.UserInfo.WindowsMinWhenClose = true;
                MagicGlobal.UserInfo.EnterSendChoose = true;
                MagicGlobal.UserInfo.CtrlEnterSendChoose = false;
                MagicGlobal.UserInfo.IsQuipReplySend = false;
                AutoLogin.IsChecked = true;
                
            }
            
            MagicGlobal.UserInfo.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //MagicGlobal.UserInfo.Version = "1.4";
            this.txtTelephone.Focus();
            this.txtTelephone.SelectionStart = this.txtTelephone.Text.Length;
            //newWinMain = new MainWindow();
            //newWinMain.Show();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        private async Task LoginAsyn()
        {
            //新埋点
            TrackHelper2.TrackOperation("5.1.1.9.1", "clk");
            if ((bool)this.rbPasswordLogin.IsChecked)
            {
                if (txtPassWord.Password == null)
                {
                    txtPassWord.Password = string.Empty;
                }
                userAccount = txtUserAccount.Text.Trim();
                passWords = txtPassWord.Password;

                if (userAccount.Trim() == string.Empty)
                {
                    bdUserAccount.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErroAccount.Text = "账号不能为空";
                    tbErroAccount.Visibility = Visibility.Visible;
                    return;
                }

                if (passWords == string.Empty)
                {
                    bdPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErroPassword.Text = "密码不能为空";
                    tbErroPassword.Visibility = Visibility.Visible;
                    return;
                }
            }
            else
            {
                userAccount = txtTelephone.Text.Trim();
                if (string.IsNullOrEmpty(this.txtTelephone.Text))
                {
                    bdTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErroTelephone.Text = "手机号不能为空";
                    tbErroTelephone.Visibility = Visibility.Visible;
                    return;

                }
                else if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtTelephone.Text))
                {
                    this.chkGetVertifyCode.IsChecked = false;
                    bdTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErroTelephone.Visibility = Visibility.Visible;
                    tbErroTelephone.Text = "手机号格式错误";
                    return;
                }
                else if (string.IsNullOrEmpty(txtVertifyCode.Text))
                {
                    bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErroVertifyCode.Text = "验证码不能为空";
                    tbErroVertifyCode.Visibility = Visibility.Visible;
                    return;
                }
                else if(this.stkPicVertify.Visibility == Visibility.Visible)
                {
                    if(string.IsNullOrEmpty(txtPicVertify.Text))
                    {
                        bdPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                        tbErrorPicVertify.Text = "图形验证码不能为空";
                        tbErrorPicVertify.Visibility = Visibility.Visible;
                        return;
                    }
                }
            }



            busyCtrl.IsBusy = true;
            uiSign.Visibility = Visibility.Hidden;
            busyCtrl.Text = "登录中,请稍候...";

            //MagicGlobal.UserInfo.RememberPassword = (bool)RememberPassword.IsChecked;
            //实例化小聘2.0版新界面

            bool isLoginSuccess = false;
            if ((bool)this.rbPasswordLogin.IsChecked)
            {
                //账号密码登录
                isLoginSuccess = await this.CheckSignPasswordAsyn();
            }
            else
            {
                //手机号验证码登录
                isLoginSuccess = await this.CheckSignTelephoneAsyn();
            }
          
            
            if (isLoginSuccess)
            {
                MagicGlobal.UserInfo.AutoLogin = (bool)AutoLogin.IsChecked;
                MagicGlobal.UserInfo.UserAccount = this.userAccount;
                if (await GetUserBasicInfo())
                {

                    //Func<string, Task<string>> asyncLambda = async name => {
                    //    //获取本地XML文件中CurrentUserInfo信息
                    //    this.ReadCurrentUserInfoXml();
                    //    //
                    //    //创建数据库
                    //    //SQLiteHelper.CreatTable(MagicGlobal.UserInfo.UserAccount);
                    //    newWinMain.iniBeforLoaded();
                    //    await TaskEx.Delay(1);
                    //    return string.Empty;
                    //};
                    //await asyncLambda(null);

                    await TaskEx.Run(()=> {
                        //获取本地XML文件中CurrentUserInfo信息
                        this.ReadCurrentUserInfoXml();
                        //
                        //创建数据库
                        //SQLiteHelper.CreatTable(MagicGlobal.UserInfo.UserAccount);
                        newWinMain.iniBeforLoaded();
                    });


                    this.Hide();
                    //主窗口初始化前加载工作
                    //newWinMain = new ViewSingle.MainWindow();
                 
                  
                    this.ShowInTaskbar = false;
                    this.notifyIcon.Dispose();
                    //newWinMain.LoginMessage = loginTip;
                    newWinMain.Show();
                    this.Close();
                    
                }

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
            this.busyCtrl.IsBusy = false;
            this.uiSign.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 获取用户注册完善信息
        /// </summary>
        /// <returns>如果用户信息未完善，返回false，否则，返回true</returns>
        private async Task<bool> GetUserBasicInfo()
        {
            string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpUserBasicInfoModel>();
            string std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] {"userID", "properties" }, new string[] {MagicGlobal.UserInfo.Id.ToString(), propertys });
            string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrGetUserBasicInfo, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpUserBasicInfoModel> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpUserBasicInfoModel>>(jsonResult);
            this.busyCtrl.IsBusy = false;
            if(model == null)
            {
                return false;
            }
            if (model.code == 200)
            {
                MagicGlobal.UserInfo.RealName = model.data.name;
                MagicGlobal.UserInfo.avatarUrl = model.data.avatar;
                MagicGlobal.UserInfo.validStatus = model.data.isHRAuth == 1;
                MagicGlobal.UserInfo.Email = model.data.hrEmail;
                MagicGlobal.UserInfo.briefName = model.data.companyShortName;
                MagicGlobal.UserInfo.CompanyId = Convert.ToInt32(model.data.companyID);
                MagicGlobal.UserInfo.CompanyName = model.data.companyName;
                MagicGlobal.UserInfo.UserPosition = model.data.position;
                MagicGlobal.UserInfo.IsHRAuth = model.data.isHRAuth;
                MagicGlobal.UserInfo.IsSimilarAuth = model.data.isHRAuth;
                MagicGlobal.isHRAuth =  Convert.ToBoolean(model.data.isHRAuth);

                //埋单获取userId和Unique
                Common.TrackHelper2.UserId = MagicGlobal.UserInfo.Id.ToString();
                Common.TrackHelper2.UniqueKey = Common.TrackHelper2.MD5Encrypt(MagicGlobal.UserInfo.UserMobile);

                if (model.data.isUserInfoComplete)
                {
                    newWinMain.btnMenu.Visibility = Visibility.Collapsed;
                    return true;
                }
                else
                {
                    MagicGlobal.isHRAuth = false;
                    //个人信息不完整
                    if(string.IsNullOrEmpty(model.data.name) || string.IsNullOrEmpty(model.data.avatar) || string.IsNullOrEmpty(model.data.hrEmail) || string.IsNullOrEmpty(model.data.companyName) || string.IsNullOrEmpty(model.data.position))
                    {
                        //newWinMain = new MainWindow();
                        //newWinMain.gdMain.Visibility = Visibility.Collapsed;
                       
                        newWinMain.ucRegister.rbSelfInfo.IsChecked = true;

                        newWinMain.ucRegister.RegisterViewModel.UserName = model.data.name;
                        newWinMain.ucRegister.RegisterViewModel.AvatarUrl = model.data.avatar;
                        newWinMain.ucRegister.RegisterViewModel.Email = model.data.hrEmail;
                        newWinMain.ucRegister.RegisterViewModel.Position = model.data.position;
                        newWinMain.ucRegister.RegisterViewModel.CompanyName = model.data.companyName;
                        newWinMain.iniBeforLoaded();
                        this.busyCtrl.IsBusy = false;
                        this.Hide();
                        this.ShowInTaskbar = false;
                        this.notifyIcon.Dispose();
                        //newWinMain.LoginMessage = loginTip;
                        newWinMain.IsRegister = true;
                        newWinMain.Show();
                        
                        this.Close();
                        return false;

                    }
                    //公司信息不完整情况
                    else if (string.IsNullOrEmpty(model.data.companyID)||string.IsNullOrEmpty(model.data.companyName)||string.IsNullOrEmpty(model.data.companyLogo)
                        ||string.IsNullOrEmpty(model.data.companyShortName)||string.IsNullOrEmpty(model.data.companyIndustry)||string.IsNullOrEmpty(model.data.companyCharact)
                        ||string.IsNullOrEmpty(model.data.companyScale)||string.IsNullOrEmpty(model.data.companyCity)||string.IsNullOrEmpty(model.data.companyAddress)
                        ||string.IsNullOrEmpty(model.data.companyIntro)||string.IsNullOrEmpty(model.data.companyBenefit))
                    {
                        //newWinMain = new MainWindow();
                       
                        newWinMain.ucRegister.rbCompanyInfo.IsChecked = true;
                        newWinMain.iniBeforLoaded();
                        this.busyCtrl.IsBusy = false;
                        this.Hide();
                        this.ShowInTaskbar = false;
                        this.notifyIcon.Dispose();
                        //newWinMain.LoginMessage = loginTip;
                        newWinMain.IsRegister = true;
                        newWinMain.Show();
                        
                        this.Close();

                        return false;
                    }
                    newWinMain.btnMenu.Visibility = Visibility.Collapsed;
       
                    return true;
                }

            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckSignPasswordAsyn()
        {
            string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpLogin>();
            string jsonResult = string.Empty;
            string std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "account", "password", "properties","appKey" }, new string[] { this.userAccount, this.passWords, propertys, "login" });
            jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrLoginByPassword,MagicGlobal.UserInfo.Version, std));

            BaseHttpModel<HttpLogin> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpLogin>>(jsonResult);
            MagicCube.TemplateUC.WinMessageLink link;
            if (model == null)
            {
                link = new TemplateUC.WinMessageLink("网络异常,请稍后重试", "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                return false;

            }
            //code码判断
            bool flag = false;
           
            switch (model.code)
            {
                case 200:
                    MagicGlobal.UserInfo.Id = model.data.userID;
                    MagicGlobal.UserInfo.UserMobile = model.data.mobile;
                    flag = true;
                    break;
                //账号不存在
                case  -103:
                    link = new TemplateUC.WinMessageLink("亲，您的手机号尚未注册，请先去注册", "去注册");
                    link.Owner = Window.GetWindow(this);
                    if((bool)link.ShowDialog())
                    {
                        this.bdMainLogin.Visibility = Visibility.Collapsed;
                        this.bdMainRegist.Visibility = Visibility.Visible;
                        this.bdForgetPassword.Visibility = Visibility.Collapsed;
                        this.txtRegistTel.Text = this.txtTelephone.Text;
                    }
                    flag = false;
                    break;
                //账号不存在或密码错误
                case -105:
                    bdPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErroPassword.Visibility = Visibility.Visible;
                    tbErroPassword.Text = model.msg;
                    flag = false;
                    break;
                //您的账号没有设置密码，请使用验证码登陆
                case -116:
                    link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                    this.rbPhoneLogin.IsChecked = true;
                    flag = false;
                    break;
                  
                default:
                    link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                    flag = false;
                    break;
            }
            return flag;


        }
        /// <summary>
        /// 手机号验证码登录
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckSignTelephoneAsyn()
        {
            string proertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpLogin>();
            string jsonResult = string.Empty;
            string imagecode = string.Empty;
            
            string std = MagicCube.DAL.JsonHelper.JsonParamsToString(
                  new string[] { "mobile", "verifyType", "verifyCode", "properties", "appKey" },
                  new string[] { this.txtTelephone.Text, this.verifyType, this.txtVertifyCode.Text, proertys, "login" });

            jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrLoginByMobile, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpLogin> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpLogin>>(jsonResult);
            MagicCube.TemplateUC.WinMessageLink link;
            if (model == null)
            {
                link = new TemplateUC.WinMessageLink("网络异常,请稍后重试", "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                return false;

            }
            bool flag = false;
            switch(model.code)
            {
                case 200:
                    MagicGlobal.UserInfo.Id = model.data.userID;
                    MagicGlobal.UserInfo.UserMobile = model.data.mobile;
                    flag = true; 
                    break;
                //验证码错误
                case -104:
                    bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErroVertifyCode.Visibility = Visibility.Visible;
                    tbErroVertifyCode.Text = model.msg;
                    flag = false;
                    break;
                //账号不存在
                case -103:
                    //if (linkError == null)
                    //    linkError = new TemplateUC.WinMessageLink("账号不存在，请先去魔方官网注册", "去注册");
                    //linkError.Owner = Window.GetWindow(this);
                    //if (!linkError.IsActive)
                    //{
                    //    if ((bool)linkError.ShowDialog())
                    //    {
                    //        System.Diagnostics.Process.Start(MagicCube.DAL.ConfUtil.ServerIERegister);
                    //    }
                    //}
                    //link = new TemplateUC.WinMessageLink("亲，您的手机号尚未注册，请先去注册", "去注册");
                    //link.Owner = Window.GetWindow(this);
                    //if ((bool)link.ShowDialog())
                    //{
                    //    System.Diagnostics.Process.Start(MagicCube.DAL.ConfUtil.ServerIERegister);
                    //}
                    flag = false;
                    break;
                //短信验证码请求超过3次，请使用语音验证码
                case -111:
                    bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErroVertifyCode.Visibility = Visibility.Visible;
                    tbErroVertifyCode.Text = model.msg;
                    flag = false;
                    break;
                //验证码请求次数超过6次
                case -115:
                    bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErroVertifyCode.Visibility = Visibility.Visible;
                    tbErroVertifyCode.Text = model.msg;
                    flag = false;
                    break;
                default:
                    link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                    flag = false;
                    break;
            }

            return flag;



            
        }


        /// <summary>
        /// 自动登录
        /// </summary>
        /// <returns></returns>
        private async Task AutoLoginAsyn()
        {
            this.busyCtrl.IsBusy = true;
            this.uiSign.Visibility = Visibility.Hidden;

            //获取本地存储的cookie
            if (MagicCube.DAL.HttpHelper.Instance.ReadCookie())
            {
                //自动登录接口
                string std1 = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "properties" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), string.Empty });
                string jsonResult1 = await MagicCube.DAL.HttpHelper.Instance.HttpFirstAutoLoginGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrLoginByUserId, MagicGlobal.UserInfo.Version, std1));
                BaseHttpModel model1 = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(jsonResult1);
                if (await GetUserBasicInfo())
                {
                    
                    this.busyCtrl.IsBusy = false;
                    //获取本地XML文件中CurrentUserInfo信息
                    this.ReadCurrentUserInfoXml();
                    //
                    //创建数据库
                    //SQLiteHelper.CreatTable(MagicGlobal.UserInfo.UserAccount);
                    //主窗口初始化前加载工作
                    newWinMain.iniBeforLoaded();
                    this.busyCtrl.IsBusy = false;
                    this.Hide();
                    this.ShowInTaskbar = false;
                    this.notifyIcon.Dispose();
                    //newWinMain.LoginMessage = loginTip;
                    newWinMain.Show();
                    this.Close();
                }
                else
                {
                    this.uiSign.Visibility = Visibility.Visible;
                    this.busyCtrl.IsBusy = false;
                    return;
                }
                this.busyCtrl.IsBusy = false;

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



        }
        /// <summary>
        /// 获取xml文件中currentUserInfo对象
        /// </summary>
        private  void ReadCurrentUserInfoXml()
        {
            if (!Directory.Exists(DAL.ConfUtil.LocalHomePath + MagicGlobal.UserInfo.UserAccount))
            {
                Directory.CreateDirectory(DAL.ConfUtil.LocalHomePath + MagicGlobal.UserInfo.UserAccount);
            }
            //读xml文件
            if (File.Exists(DAL.ConfUtil.LocalHomePath + MagicGlobal.UserInfo.UserAccount + "//CurrentUserInfo.xml"))
            {
                MagicGlobal.currentUserInfo = XmlClassData.ReadDataFromXml<CurrentUserInfo>(DAL.ConfUtil.LocalHomePath + MagicGlobal.UserInfo.UserAccount + "//CurrentUserInfo.xml");
                if (MagicGlobal.currentUserInfo == null)
                {
                    MagicGlobal.currentUserInfo = new CurrentUserInfo();
                }
            }
            else
            {
                MagicGlobal.currentUserInfo = new CurrentUserInfo();
                MagicGlobal.currentUserInfo.InterviewIdToUpdateTime = new List<string>();
            }
            if (MagicGlobal.currentUserInfo.InterviewIdToUpdateTime != null)
            {
                //List<string> keyStr = new List<string>();
                //foreach (var item in MagicGlobal.currentUserInfo.InterviewIdToUpdateTime)
                //{
                //    string[] tempSplit = item.Split(new char[] { '&' });
                //    if (tempSplit.Length == 3)
                //    {
                //        string timeDate = tempSplit[1];
                //        DateTime dtTemp = Convert.ToDateTime(timeDate);
                //        if (dtTemp < DateTime.Now)
                //        {
                //            keyStr.Add(item);
                //        }
                //    }
                //}
                //foreach (var item in keyStr)
                //{
                //    MagicGlobal.currentUserInfo.InterviewIdToUpdateTime.Remove(item);
                //}
            }
            else
            {
                MagicGlobal.currentUserInfo.InterviewIdToUpdateTime = new List<string>();
            }
        }

        private async Task<BitmapSource> GetImgVertifyCode(string mobile)
        {
            //获取图形验证码
            string picStd = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "width", "height", "appKey" }, new string[] { mobile, "121", "34", "login" });
            string picResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrImgVertifyCode, MagicGlobal.UserInfo.Version, picStd));
            this.busyCtrl.IsBusy = false;
            BaseHttpModel<HttpImgVertfiyCodeModel> picModel = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpImgVertfiyCodeModel>>(picResult);
            if (picModel != null)
            {
                if (picModel.code == 200)
                {
                    byte[] arr = Convert.FromBase64String(picModel.data.picture);
                    MemoryStream ms = new MemoryStream(arr);
                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);
                    BitmapSource source = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    //BitmapImage bmp = new BitmapImage();
                    //bmp.StreamSource = ms;
                    //imgPicVertify.Source = source;
                    return source;
                }
            }
            return null;
        }

        #endregion

        #region "对内功能函数"
        /// <summary>
        /// 全局钩子windowsAPI
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == MessageHelper.WM_DOWNLOAD_COMPLETED)
            {
                Console.WriteLine("收到");
                this.WindowState = System.Windows.WindowState.Normal;
                this.Show();
            }

            return hwnd;
        }
        /// <summary>
        /// 清空界面所有错误提示显示
        /// </summary>
        private void SetErroTagsClose()
        {
            this.tbErroAccount.Visibility = Visibility.Collapsed;
            this.tbErroPassword.Visibility = Visibility.Collapsed;
            this.tbErroTelephone.Visibility = Visibility.Collapsed;
            this.tbErroVertifyCode.Visibility = Visibility.Collapsed;
            this.tbErrorPicVertify.Visibility = Visibility.Collapsed;
        

            this.tbRegistErrorVertifyCode.Visibility = Visibility.Collapsed;
            this.tbErrowRegistTel.Visibility = Visibility.Collapsed;
            this.tbErrorRegistNewPassword.Visibility = Visibility.Collapsed;
            this.tbErrorRegistPicVertify.Visibility = Visibility.Collapsed;

            this.bdPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdUserAccount.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdRegistPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));

            this.bdRegistTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdRegistVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdRegistNewPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));

            this.tbErrorForgetPw.Visibility = Visibility.Collapsed;
            this.tbErrorForegetPwVertifyCode.Visibility = Visibility.Collapsed;
            this.tbErrorForegetPwNewPassword.Visibility = Visibility.Collapsed;
            this.tbErrorForegetPwConfirmPassword.Visibility = Visibility.Collapsed;
            this.tbErrorForegetPwPicVertify.Visibility = Visibility.Collapsed;

            this.bdForgetPwTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdForegetPwVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7)); 
            this.bdForegetPwNewPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdForegetPwConfirmPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdForgetPwPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));

        }

        #endregion

        #region "回调函数"
        /// <summary>
        /// 验证码1分钟等待定时器回调函数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void VertifytimeAlive_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            this.TickNum = this.TickNum - 1;
            if(this.TickNum != 0)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.tbClickNum.Text = "(" + this.TickNum.ToString() + ")";
                }));
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.chkGetVertifyCode.IsChecked = false;
                }));
                this.VertifytimeAlive.Stop();
                this.VertifytimeAlive.Dispose();
               
            }
            

        }

        /// <summary>
        /// 窗口Loaded回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void WinIndex_Loaded(object sender, EventArgs e)
        {
    


            this.Activate();
            IniIcon();
            newWinMain = new ViewSingle.MainWindow();
            await InitUser();
            MagicCube.DAL.SocketHelper.Instance.DeviceID = MagicGlobal.UserInfo.PushDeviceID;
            MagicCube.DAL.SocketHelper.Instance.SecureKey = MagicGlobal.UserInfo.PushSecureKey;
            await MagicCube.DAL.SocketHelper.Instance.Start();
            //添加窗口全局钩子
            (PresentationSource.FromVisual(this) as HwndSource).AddHook(new HwndSourceHook(this.WndProc));
           
           
        }
        /// <summary>
        /// 任务栏图标单击显示主窗口回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show(object sender, EventArgs e)
        {
            if (this.IsLoaded)
            {
                this.Show();
                this.Activate();
                this.WindowState = System.Windows.WindowState.Normal;
            }
            else
            {
                if (notifyIcon != null)
                {
                    this.notifyIcon.Visible = false;
                    this.notifyIcon.Dispose();
                    this.notifyIcon = null;
                }
            }
        }
        /// <summary>
        /// 任务栏图标按钮关闭回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, EventArgs e)
        {
            if (notifyIcon != null)
            {
                this.notifyIcon.Visible = false;
                this.notifyIcon.Dispose();
                this.notifyIcon = null;
            }
            Environment.Exit(0);
        }

        private void btnLoginUpLink_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.IsClickSignUpLink = true;
        }

        private void btnLoginUpLink_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.IsClickSignUpLink = false;
        }




        #endregion

        private void rbPasswordLogin_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.IsClickPasswordTab = true;
        }

        private void rbPasswordLogin_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.IsClickPasswordTab = false;
        }



        #region "注册模块"

        /// <summary>
        /// 验证码1分钟计时器
        /// </summary>
        private System.Timers.Timer VertifytimeAlive2;
        private double interval2 = 1000;
        private int TickNum2 = 60;
        private async void chkRegistVertifyCode_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.4.4.1", "clk");
            if (string.IsNullOrEmpty(this.txtRegistTel.Text))
            {
                this.chkRegistVertifyCode.IsChecked = false;
                bdRegistTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrowRegistTel.Visibility = Visibility.Visible;
                tbErrowRegistTel.Text = "手机号不能为空";

                return;
            }
            else if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtRegistTel.Text))
            {
                this.chkRegistVertifyCode.IsChecked = false;
                bdRegistTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrowRegistTel.Visibility = Visibility.Visible;
                tbErrowRegistTel.Text = "手机号格式错误";
                return;
            }
            this.busyCtrl.IsBusy = true;
            //判断是否注册过
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtRegistTel.Text });
            string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (Model == null)
            {
                this.busyCtrl.IsBusy = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkRegistVertifyCode.IsChecked = false;
                return;
            }
            switch (Model.code)
            {
                //手机号未注册
                case 100:
                    break;
                default:
                    this.busyCtrl.IsBusy = false;
                    TemplateUC.WinGotoLogin link = new TemplateUC.WinGotoLogin();
                    link.Owner = Window.GetWindow(this);
                    if(link.ShowDialog() == true)
                    {
                        this.bdMainLogin.Visibility = Visibility.Visible;
                        this.bdMainRegist.Visibility = Visibility.Collapsed;
                        this.bdForgetPassword.Visibility = Visibility.Collapsed;
                        this.txtTelephone.Text = this.txtRegistTel.Text;
                    }
                    else
                    {
                        this.txtRegistTel.Clear();
                    }
                    this.chkRegistVertifyCode.IsChecked = false;
                    return;
            }



         
            string std = string.Empty;
            if (this.stkRegistPicVertify.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(this.txtRegistPicVertify.Text))
                {
                    this.busyCtrl.IsBusy = false;
                    this.chkRegistVertifyCode.IsChecked = false;
                    bdRegistPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorRegistPicVertify.Visibility = Visibility.Visible;
                    tbErrorRegistPicVertify.Text = "图形验证码不能为空";
                    return;
                }
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey", "imageCode" }, new string[] { this.txtRegistTel.Text, "sms", "register", this.txtRegistPicVertify.Text });
            }
            else
            {
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtRegistTel.Text, "sms", "register" });

            }
            //string std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtRegistTel.Text, "sms", "register" });
            string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrSendVertifyCode, MagicGlobal.UserInfo.Version, std));
       
            BaseHttpModel<HttpVertifyCodeModel> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpVertifyCodeModel>>(jsonResult);
            if (model == null)
            {
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkRegistVertifyCode.IsChecked = false;
                this.busyCtrl.IsBusy = false;
                return;
            }
            if (model.code == 200)
            {
                if (model.data.showImageVerifyCode)
                {
                    this.chkRegistVertifyCode.IsChecked = false;
                    ////获取图形验证码
                    imgRegistPicVertify.Source = await GetImgVertifyCode(this.txtRegistTel.Text);
                    this.busyCtrl.IsBusy = false;
                    this.tbErrorRegistPicVertify.Visibility = Visibility.Visible;
                    this.bdRegistPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    this.stkRegistPicVertify.Visibility = Visibility.Visible;
                    return;
                }
                this.registerVertyfyType = "sms";
                //职位沟通定时器
                VertifytimeAlive2 = new System.Timers.Timer(this.interval2);
                VertifytimeAlive2.Elapsed += VertifytimeAlive_Elapsed2;
                VertifytimeAlive2.Enabled = true;
                this.TickNum2 = 60;
                this.tbClickNum2.Text = "(" + this.TickNum2.ToString() + ")";

                //测试
                Console.WriteLine("验证码：" + model.data.verifyCode.ToString());
                this.busyCtrl.IsBusy = false;
            }
            else if (model.code == -2)
            {
                this.busyCtrl.IsBusy = false;
                bdRegistVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbRegistErrorVertifyCode.Text = "获取验证码次数达到上限，如有疑问请联系我们 010-59423287";
                tbRegistErrorVertifyCode.Visibility = Visibility.Visible;
                this.chkRegistVertifyCode.IsChecked = false;
            }
            //验证码过期
            else if (model.code == -119)
            {
                this.busyCtrl.IsBusy = false;
                bdRegistVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbRegistErrorVertifyCode.Text = "验证码已过期，请重新获取";
                tbRegistErrorVertifyCode.Visibility = Visibility.Visible;
                this.chkRegistVertifyCode.IsChecked = false;
            }
            else if (model.code == -137)
            {
                this.busyCtrl.IsBusy = false;
                this.chkRegistVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                imgRegistPicVertify.Source = await GetImgVertifyCode(this.txtRegistTel.Text);
            }
            else
            {
                this.busyCtrl.IsBusy = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkRegistVertifyCode.IsChecked = false;
            }
        }

        /// <summary>
        /// 点击语音验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnRegistVoicCode_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.4.5.1", "clk");
            //判断是否可以验证
            if ((bool)this.chkRegistVertifyCode.IsChecked)
            {
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("操作过于频繁", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();
                return;
            }

            if (string.IsNullOrEmpty(this.txtRegistTel.Text))
            {
                this.chkRegistVertifyCode.IsChecked = false;
                bdRegistTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrowRegistTel.Visibility = Visibility.Visible;
                tbErrowRegistTel.Text = "手机号不能为空";

                return;
            }
            else if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtRegistTel.Text))
            {
                this.chkRegistVertifyCode.IsChecked = false;
                bdRegistTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrowRegistTel.Visibility = Visibility.Visible;
                tbErrowRegistTel.Text = "手机号不能为空";
                return;
            }
            this.busyCtrl.IsBusy = true;

            //判断是否注册过
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtRegistTel.Text });
            string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (Model == null)
                return;

            switch (Model.code)
            {
                //手机号未注册
                case 100:
                    break;
                default:
                    this.busyCtrl.IsBusy = false;
                    TemplateUC.WinGotoLogin link = new TemplateUC.WinGotoLogin();
                    if (link.ShowDialog() == true)
                    {
                        this.bdMainLogin.Visibility = Visibility.Visible;
                        this.bdMainRegist.Visibility = Visibility.Collapsed;
                        this.bdForgetPassword.Visibility = Visibility.Collapsed;
                        this.txtTelephone.Text = this.txtRegistTel.Text;
                    }
                    else
                    {
                        this.txtRegistTel.Clear();
                    }

                    return;
            }

            string std = string.Empty;
            if (this.stkRegistPicVertify.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(this.txtRegistPicVertify.Text))
                {
                    this.busyCtrl.IsBusy = false;
                    this.chkRegistVertifyCode.IsChecked = false;
                    bdRegistPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorRegistPicVertify.Visibility = Visibility.Visible;
                    tbErrorRegistPicVertify.Text = "图形验证码不能为空";
                    return;
                }
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey", "imageCode" }, new string[] { this.txtRegistTel.Text, "voice", "register", this.txtRegistPicVertify.Text });
            }
            else
            {
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtRegistTel.Text, "voice", "register" });

            }

            //string std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtRegistTel.Text, "voice", "register" });
            string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrSendVertifyCode, MagicGlobal.UserInfo.Version, std));
            
            BaseHttpModel<HttpVertifyCodeModel> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpVertifyCodeModel>>(jsonResult);
            if (model.code == 200)
            {
                if (model.data.showImageVerifyCode)
                {
                    this.chkRegistVertifyCode.IsChecked = false;
                    ////获取图形验证码
                    imgRegistPicVertify.Source = await GetImgVertifyCode(this.txtRegistTel.Text);
                    this.busyCtrl.IsBusy = false;
                    this.tbErrorRegistPicVertify.Visibility = Visibility.Visible;
                    this.bdRegistPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    this.stkRegistPicVertify.Visibility = Visibility.Visible;
                    this.busyCtrl.IsBusy = false;
                    return;
                }
                this.registerVertyfyType = "voice";
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("电话拨打中，请留意来电", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();

                //职位沟通定时器
                VertifytimeAlive2 = new System.Timers.Timer(this.interval2);
                VertifytimeAlive2.Elapsed += VertifytimeAlive_Elapsed2;
                VertifytimeAlive2.Enabled = true;
                this.TickNum2 = 60;
                this.tbClickNum2.Text = "(" + this.TickNum2.ToString() + ")";
                this.chkRegistVertifyCode.IsChecked = true;

                //测试
                Console.WriteLine("验证码：" + model.data.verifyCode.ToString());
                this.busyCtrl.IsBusy = false;
            }
            else if (model.code == -2)
            {
                this.busyCtrl.IsBusy = false;
                bdRegistVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbRegistErrorVertifyCode.Text = "获取验证码次数达到上限，如有疑问请联系我们 010-59423287";
                tbRegistErrorVertifyCode.Visibility = Visibility.Visible;
                this.chkRegistVertifyCode.IsChecked = false;
            }
            else if (model.code == -137)
            {
                this.busyCtrl.IsBusy = false;
                this.chkRegistVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                imgRegistPicVertify.Source = await GetImgVertifyCode(this.txtRegistTel.Text);
            }
            else
            {
                this.busyCtrl.IsBusy = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkRegistVertifyCode.IsChecked = false;
            }
        }

        private void VertifytimeAlive_Elapsed2(object source, System.Timers.ElapsedEventArgs e)
        {
            this.TickNum2 = this.TickNum2 - 1;
            if (this.TickNum2 != 0)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.tbClickNum2.Text = "(" + this.TickNum2.ToString() + ")";
                }));
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.chkRegistVertifyCode.IsChecked = false;
                }));
                this.VertifytimeAlive2.Stop();
                this.VertifytimeAlive2.Dispose();

            }


        }

        private async void btnRegist_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.4.8.1", "clk");

            await RegisterStart();
        }

        private async Task RegisterStart()
        {
            if (string.IsNullOrEmpty(this.txtRegistTel.Text))
            {
                bdRegistTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrowRegistTel.Text = "手机号不能为空";
                tbErrowRegistTel.Visibility = Visibility.Visible;
                return;

            }
            else if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtRegistTel.Text))
            {
                this.chkRegistVertifyCode.IsChecked = false;
                bdRegistTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrowRegistTel.Visibility = Visibility.Visible;
                tbErrowRegistTel.Text = "手机号格式错误";
                return;
            }
            else if (string.IsNullOrEmpty(txtRegistVertifyCode.Text))
            {
                bdRegistVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbRegistErrorVertifyCode.Text = "验证码不能为空";
                tbRegistErrorVertifyCode.Visibility = Visibility.Visible;
                return;
            }
            if (string.IsNullOrEmpty(this.txtRegistNewPassword.Password))
            {
                this.tbErrorRegistNewPassword.Visibility = Visibility.Visible;
                this.tbErrorRegistNewPassword.Text = "密码不能为空";
                this.bdRegistNewPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                return;
            }
            else
            {
                if (this.txtRegistNewPassword.Password.Length < 6 || this.txtRegistNewPassword.Password.Length > 20)
                {
                    this.tbErrorRegistNewPassword.Visibility = Visibility.Visible;
                    this.tbErrorRegistNewPassword.Text = "请输入6-20位字母、数字或特殊字符";
                    this.bdRegistNewPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    return;
                }
            }


            if (this.chkReadAgree.IsChecked == false)
            {
                return;
            }

            TemplateUC.WinMessageLink link;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "channel", "mobile", "password", "verifyType", "verifyCode", "appKey" },
                new string[] {"xiaopin", this.txtRegistTel.Text, this.txtRegistNewPassword.Password, this.registerVertyfyType, this.txtRegistVertifyCode.Text, "register" });
            string urlStr = string.Format(DAL.ConfUtil.AddrRegister, MagicGlobal.UserInfo.Version, std);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(urlStr);
            BaseHttpModel<HttpResigterModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpResigterModel>>(resultStr);
            if (model == null)
            {
                link = new TemplateUC.WinMessageLink("网络异常,请稍后重试", "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                return;
            }
            else
            {
                if (model.code == 200)
                {
                    MagicGlobal.UserInfo.Id = Convert.ToInt32(model.data.userID);
                    MagicGlobal.UserInfo.UserAccount = model.data.mobile;
                    MagicGlobal.UserInfo.UserMobile = model.data.mobile;
                    MagicGlobal.UserInfo.IsHRAuth = 0;
                    MagicGlobal.UserInfo.validStatus = false;
                    MagicGlobal.UserInfo.avatarUrl = null;
                    MagicGlobal.UserInfo.UserMobile = model.data.mobile;
                    this.ReadCurrentUserInfoXml();
                    newWinMain = new MainWindow();
                    newWinMain.SetRegistPanel(true);
                    this.ShowInTaskbar = false;
                    this.notifyIcon.Dispose();
                    newWinMain.Show();
                    this.Close();
                }
                else if (model.code == -1)
                {
                    TemplateUC.WinGotoLogin link2 = new TemplateUC.WinGotoLogin();
                    link2.Owner = Window.GetWindow(this);
                    if (link2.ShowDialog() == true)
                    {
                        this.bdMainLogin.Visibility = Visibility.Visible;
                        this.bdMainRegist.Visibility = Visibility.Collapsed;
                        this.bdForgetPassword.Visibility = Visibility.Collapsed;
                        this.txtTelephone.Text = this.txtRegistTel.Text;
                        
                    }
                    else
                    {
                        this.txtRegistTel.Clear();
                        this.txtRegistVertifyCode.Clear();
                        this.txtRegistNewPassword.Password = string.Empty;
                    }
                    return;
                }
                else
                {
                    link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                    return;
                }
            }

        }
        private void BtnReturnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (this.bdMainRegist.Visibility == Visibility.Visible)
                TrackHelper2.TrackOperation("5.1.4.9.1", "clk");
            else if (this.bdForgetPassword.Visibility == Visibility.Visible)
                TrackHelper2.TrackOperation("5.1.5.9.1", "clk");


            this.bdMainLogin.Visibility = Visibility.Visible;
            this.bdMainRegist.Visibility = Visibility.Collapsed;
            this.bdForgetPassword.Visibility = Visibility.Collapsed;
            this.txtTelephone.Focus();
        }
        #endregion


        #region "忘记密码"
        private System.Timers.Timer VertifytimeAlive3;
        private double interval3 = 1000;
        private int TickNum3 = 60;


        private async void btnModifyPwAndLogin_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.5.8.1", "clk");
            if (string.IsNullOrEmpty(this.txtForgetPwTel.Text))
            {
                bdForgetPwTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForgetPw.Visibility = Visibility.Visible;
                tbErrorForgetPw.Text = "手机号不能为空";
                return;
            }
            
            else
            {
                if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtForgetPwTel.Text))
                {
                    bdForgetPwTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForgetPw.Visibility = Visibility.Visible;
                    tbErrorForgetPw.Text = "手机号格式错误";
                    return;
                }
            } 
           
            if (string.IsNullOrEmpty(this.txtForegetPwVertifyCode.Text))
            {
                this.bdForegetPwVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwVertifyCode.Visibility = Visibility.Visible;
                tbErrorForegetPwVertifyCode.Text = "验证码不能为空";
                return;
            }
            if (string.IsNullOrEmpty(this.txtForegetPwNewPassword.Password))
            {
                this.bdForegetPwNewPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwNewPassword.Visibility = Visibility.Visible;
                tbErrorForegetPwNewPassword.Text = "密码不能为空";
                return;
            }
            else
            {
                if(this.txtForegetPwNewPassword.Password.Length < 6 || this.txtForegetPwNewPassword.Password.Length >20)
                {
                    this.bdForegetPwNewPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForegetPwNewPassword.Visibility = Visibility.Visible;
                    tbErrorForegetPwNewPassword.Text = "请输入6-20位字母、数字或字符";
                    return;
                }
            
            }
            if(string.IsNullOrEmpty(this.txtForegetPwConfirmPassword.Password))
            {
                this.bdForegetPwConfirmPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwConfirmPassword.Visibility = Visibility.Visible;
                tbErrorForegetPwConfirmPassword.Text = "请再次输入新密码";
                return;
            }
            else
            {
                if (this.txtForegetPwNewPassword.Password != this.txtForegetPwConfirmPassword.Password)
                {
                    this.bdForegetPwConfirmPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForegetPwConfirmPassword.Visibility = Visibility.Visible;
                    tbErrorForegetPwConfirmPassword.Text = "两次输入的密码不一致，请重新输入";
                    return;
                }
            }

            //判断是否注册过
            this.busyCtrl.IsBusy = true;
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtForgetPwTel.Text });
            string resultStr2 = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr2);
            if (Model == null)
            {
                this.busyCtrl.IsBusy = false;
                return;
            }
           
            switch (Model.code)
            {
                case 100:
                    this.busyCtrl.IsBusy = false;
                    TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink("该手机号未注册", "去注册");
                    link.Owner = Window.GetWindow(this);
                    if (link.ShowDialog() == true)
                    {
                        this.bdMainLogin.Visibility = Visibility.Collapsed;
                        this.bdMainRegist.Visibility = Visibility.Visible;
                        this.bdForgetPassword.Visibility = Visibility.Collapsed;
                        this.txtRegistTel.Text = this.txtForgetPwTel.Text;
                    }
                    else
                    {
                        this.txtForgetPwTel.Clear();
                        this.txtForegetPwConfirmPassword.Password = string.Empty;
                        this.txtForegetPwNewPassword.Password = string.Empty;
                        this.txtForegetPwVertifyCode.Clear();
                        
                    }
                    this.chkForegetPwVertifyCode.IsChecked = false;
                    return;
            }



            //修改密码
            string jsonStr = DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "password", "verifyCode", "verifyType", "appKey" },
                new string[] { this.txtForgetPwTel.Text, this.txtForegetPwNewPassword.Password,this.txtForegetPwVertifyCode.Text, this.forgetPwVertyfyType, "findPwd" });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrForgetPassword, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<HttpForgetPasswordAndLogin> model2 = DAL.JsonHelper.ToObject<BaseHttpModel<HttpForgetPasswordAndLogin>>(resultStr);
            if (model2 == null)
            {
                this.busyCtrl.IsBusy = false;
                TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                return;
            }
            else
            {
                this.busyCtrl.IsBusy = false;
                if (model2.code != 200)
                {
                    if(model2.code == -104)
                    {
                        TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model2.msg, "我知道了");
                        link.Owner = Window.GetWindow(this);
                        link.ShowDialog();
                  
                    }
                    else
                    {
                        TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                        link.Owner = Window.GetWindow(this);
                        link.ShowDialog();
                    }             
                    return;
                }

                else
                {
                    this.busyCtrl.IsBusy = false;
                    MagicGlobal.UserInfo.AutoLogin = (bool)AutoLogin.IsChecked;
                    MagicGlobal.UserInfo.UserAccount = model2.data.mobile;
                    MagicGlobal.UserInfo.UserMobile = model2.data.mobile;
                    MagicGlobal.UserInfo.Id = Convert.ToInt32(model2.data.userID);
                    Console.WriteLine("修改密码成功");
                    //登录已成功
                    if (await GetUserBasicInfo())
                    {
                        //获取本地XML文件中CurrentUserInfo信息
                        this.ReadCurrentUserInfoXml();
                        //
                        //创建数据库
                        //SQLiteHelper.CreatTable(MagicGlobal.UserInfo.UserAccount);
                        //主窗口初始化前加载工作
                        newWinMain = new MainWindow();
                        newWinMain.iniBeforLoaded();
                        this.Hide();
                        this.ShowInTaskbar = false;
                        this.notifyIcon.Dispose();
                        //newWinMain.LoginMessage = loginTip;
                        newWinMain.Show();
                        this.Close();
                    }
                    this.busyCtrl.IsBusy = false;

                }

            }




        }

        /// <summary>
        /// 点击语音验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnForgetPwVoicCode_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.5.5.1", "clk");
            //判断是否可以验证
            if ((bool)this.chkForegetPwVertifyCode.IsChecked)
            {
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("操作过于频繁", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();
                return;
            }

            if (string.IsNullOrEmpty(this.txtForgetPwTel.Text))
            {
                this.chkForegetPwVertifyCode.IsChecked = false;
                bdForgetPwTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForgetPw.Visibility = Visibility.Visible;
                tbErrorForgetPw.Text = "手机号不能为空";

                return;
            }
            else if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtForgetPwTel.Text))
            {
                this.chkForegetPwVertifyCode.IsChecked = false;
                bdForgetPwTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForgetPw.Visibility = Visibility.Visible;
                tbErrorForgetPw.Text = "请输入正确手机号";
                return;
            }
            this.busyCtrl.IsBusy = true;

            //判断是否注册过
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtForgetPwTel.Text });
            string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (Model == null)
            {
                this.busyCtrl.IsBusy = false;
                return;
            }
            switch (Model.code)
            {
                case 100:
                    this.busyCtrl.IsBusy = false;
                    TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink("该手机号未注册", "去注册");
                    link.Owner = Window.GetWindow(this);
                    if (link.ShowDialog() == true)
                    {
                        this.bdMainLogin.Visibility = Visibility.Collapsed;
                        this.bdMainRegist.Visibility = Visibility.Visible;
                        this.bdForgetPassword.Visibility = Visibility.Collapsed;
                        this.txtRegistTel.Text = this.txtForgetPwTel.Text;
                    }
                    else
                    {
                        this.txtForgetPwTel.Clear();
                        this.txtForegetPwConfirmPassword.Password = string.Empty;
                        this.txtForegetPwNewPassword.Password = string.Empty;
                        this.txtForegetPwVertifyCode.Clear();
                    }
                    this.chkForegetPwVertifyCode.IsChecked = false;
                    return;
            }
            string std = string.Empty;
            if (this.stkForgetPwPicVertify.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(this.txtForgetPwPicVertify.Text))
                {
                    this.busyCtrl.IsBusy = false;
                    this.chkForegetPwVertifyCode.IsChecked = false;
                    bdForgetPwPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForegetPwPicVertify.Visibility = Visibility.Visible;
                    tbErrorForegetPwPicVertify.Text = "图形验证码不能为空";
                    return;
                }
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey", "imageCode" }, new string[] { this.txtForgetPwTel.Text, "voice", "findPwd", this.txtForgetPwPicVertify.Text });
            }
            else
            {
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtForgetPwTel.Text, "voice", "findPwd" });

            }

            //string std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtForgetPwTel.Text, "voice", "register" });
            string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrSendVertifyCode, MagicGlobal.UserInfo.Version, std));
            this.busyCtrl.IsBusy = false;
            BaseHttpModel<HttpVertifyCodeModel> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpVertifyCodeModel>>(jsonResult);
            if (model.code == 200)
            {
                if (model.data.showImageVerifyCode)
                {
                    this.chkForegetPwVertifyCode.IsChecked = false;
                    ////获取图形验证码
                    imgForgetPwPicVertify.Source = await GetImgVertifyCode(this.txtForgetPwTel.Text);
                    this.busyCtrl.IsBusy = false;
                    this.tbErrorForegetPwPicVertify.Visibility = Visibility.Visible;
                    this.bdForgetPwPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    this.stkForgetPwPicVertify.Visibility = Visibility.Visible;
                    this.busyCtrl.IsBusy = false;
                    return;
                }
                this.busyCtrl.IsBusy = false;
                this.forgetPwVertyfyType = "voice";
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("电话拨打中，请留意来电", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();

                //职位沟通定时器
                VertifytimeAlive3 = new System.Timers.Timer(this.interval3);
                VertifytimeAlive3.Elapsed += VertifytimeAlive_Elapsed3;
                VertifytimeAlive3.Enabled = true;
                this.TickNum3 = 60;
                this.tbClickNum3.Text = "(" + this.TickNum3.ToString() + ")";
                this.chkForegetPwVertifyCode.IsChecked = true;

                //测试
                Console.WriteLine("验证码：" + model.data.verifyCode.ToString());
            }
            else if (model.code == -2)
            {
                this.busyCtrl.IsBusy = false;
                bdForegetPwVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwVertifyCode.Text = "获取验证码次数达到上限，如有疑问请联系我们 010-59423287";
                tbErrorForegetPwVertifyCode.Visibility = Visibility.Visible;
                this.chkForegetPwVertifyCode.IsChecked = false;
            }
            else if (model.code == -137)
            {
                this.busyCtrl.IsBusy = false;
                this.chkForegetPwVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                imgForgetPwPicVertify.Source = await GetImgVertifyCode(this.txtForgetPwTel.Text);
            }
            else
            {
                this.busyCtrl.IsBusy = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkForegetPwVertifyCode.IsChecked = false;
            }
        }

        private void VertifytimeAlive_Elapsed3(object source, System.Timers.ElapsedEventArgs e)
        {
            this.TickNum3 = this.TickNum3 - 1;
            if (this.TickNum3 != 0)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.tbClickNum3.Text = "(" + this.TickNum3.ToString() + ")";
                }));
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.chkForegetPwVertifyCode.IsChecked = false;
                }));
                this.VertifytimeAlive3.Stop();
                this.VertifytimeAlive3.Dispose();

            }


        }

        private async void chkForegetPwVertifyCode_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.5.4.1", "clk");
            if (string.IsNullOrEmpty(this.txtForgetPwTel.Text))
            {
                this.chkForegetPwVertifyCode.IsChecked = false;
                bdForgetPwTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForgetPw.Visibility = Visibility.Visible;
                tbErrorForgetPw.Text = "手机号不能为空";

                return;
            }
            else if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtForgetPwTel.Text))
            {
                this.chkForegetPwVertifyCode.IsChecked = false;
                bdForgetPwTel.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForgetPw.Visibility = Visibility.Visible;
                tbErrorForgetPw.Text = "手机号格式错误";
                return;
            }

            //判断是否注册过
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtForgetPwTel.Text });
            string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (Model == null)
                return;
            switch (Model.code)
            {
                case 100:
                    TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink("该手机号未注册","去注册");
                    link.Owner = Window.GetWindow(this);
                    if (link.ShowDialog() == true)
                    {
                        this.bdMainLogin.Visibility = Visibility.Collapsed;
                        this.bdMainRegist.Visibility = Visibility.Visible;
                        this.bdForgetPassword.Visibility = Visibility.Collapsed;
                        this.txtRegistTel.Text = this.txtForgetPwTel.Text;
                    }
                    else
                    {
                        this.txtForgetPwTel.Clear();
                        this.txtForegetPwConfirmPassword.Password = string.Empty;
                        this.txtForegetPwNewPassword.Password = string.Empty;
                        this.txtForegetPwVertifyCode.Clear();
                    }
                    this.chkForegetPwVertifyCode.IsChecked = false;
                    return;
            }

            string std = string.Empty;
            if (this.stkForgetPwPicVertify.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(this.txtForgetPwPicVertify.Text))
                {
                    this.chkForegetPwVertifyCode.IsChecked = false;
                    bdForgetPwPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForegetPwPicVertify.Visibility = Visibility.Visible;
                    tbErrorForegetPwPicVertify.Text = "图形验证码不能为空";
                    return;
                }
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey", "imageCode" }, new string[] { this.txtForgetPwTel.Text, "sms", "findPwd", this.txtForgetPwPicVertify.Text });
            }
            else
            {
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtForgetPwTel.Text, "sms", "findPwd" });

            }

            this.busyCtrl.IsBusy = true;
            //string std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtForgetPwTel.Text, "sms", "findPwd" });
            string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrSendVertifyCode, MagicGlobal.UserInfo.Version, std));
            this.busyCtrl.IsBusy = false;
            BaseHttpModel<HttpVertifyCodeModel> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpVertifyCodeModel>>(jsonResult);
            if (model == null)
            {
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkForegetPwVertifyCode.IsChecked = false;
                return;
            }
            if (model.code == 200)
            {
                if (model.data.showImageVerifyCode)
                {
                    this.chkForegetPwVertifyCode.IsChecked = false;
                    ////获取图形验证码
                    imgForgetPwPicVertify.Source = await GetImgVertifyCode(this.txtForgetPwTel.Text);
                    this.busyCtrl.IsBusy = false;
                    this.tbErrorForegetPwPicVertify.Visibility = Visibility.Visible;
                    this.bdForgetPwPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    this.stkForgetPwPicVertify.Visibility = Visibility.Visible;
                    this.busyCtrl.IsBusy = false;
                    return;
                }

                this.forgetPwVertyfyType = "sms";
                //职位沟通定时器
                VertifytimeAlive3 = new System.Timers.Timer(this.interval3);
                VertifytimeAlive3.Elapsed += VertifytimeAlive_Elapsed3;
                VertifytimeAlive3.Enabled = true;
                this.TickNum3 = 60;
                this.tbClickNum3.Text = "(" + this.TickNum3.ToString() + ")";

                //测试
                Console.WriteLine("验证码：" + model.data.verifyCode.ToString());
            }
            else if (model.code == -2)
            {
                bdForegetPwVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwVertifyCode.Text = "获取验证码次数达到上限，如有疑问请联系我们 010-59423287";
                tbErrorForegetPwVertifyCode.Visibility = Visibility.Visible;
                this.chkForegetPwVertifyCode.IsChecked = false;
            }
            //验证码过期
            else if (model.code == -119)
            {
                bdForegetPwVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwVertifyCode.Text = "验证码已过期，请重新获取";
                tbErrorForegetPwVertifyCode.Visibility = Visibility.Visible;
                this.chkForegetPwVertifyCode.IsChecked = false;
            }
            else if (model.code == -137)
            {
                this.busyCtrl.IsBusy = false;
                this.chkForegetPwVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                imgForgetPwPicVertify.Source = await GetImgVertifyCode(this.txtForgetPwTel.Text);
            }
            else
            {
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkForegetPwVertifyCode.IsChecked = false;
            }
        }

        #endregion



        /// <summary>
        /// 魔方用户协议网页跳转。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadAgree_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.4.7.1", "clk");
            string webSite = "https://www.mofanghr.com/agreement";
            System.Diagnostics.Process.Start(webSite);
        }

        private void chkReadAgree_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void bdMainRegist_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(this.bdMainRegist.Visibility == Visibility.Visible)
            {
                TrackHelper2.TrackOperation("5.1.4.1.1", "pv");
            }
        }

        private void bdForgetPassword_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.bdForgetPassword.Visibility == Visibility.Visible)
            {
                TrackHelper2.TrackOperation("5.1.5.1.1", "pv");
            }
        }

        private async void btnPicVertifycodeUpdate_Click(object sender, RoutedEventArgs e)
        {
            imgPicVertify.Source = await this.GetImgVertifyCode(this.txtTelephone.Text);
        }

        private async void btnRegistPicVertifycodeUpdate_Click(object sender, RoutedEventArgs e)
        {
            imgRegistPicVertify.Source = await this.GetImgVertifyCode(this.txtRegistTel.Text);
        }

        private async void btnForgetPwPicVertifycodeUpdate_Click(object sender, RoutedEventArgs e)
        {
            imgForgetPwPicVertify.Source = await this.GetImgVertifyCode(this.txtForgetPwTel.Text);
        }
    }
}
