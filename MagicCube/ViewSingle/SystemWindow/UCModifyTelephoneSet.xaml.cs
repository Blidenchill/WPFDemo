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
using MagicCube.ViewModel;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Interop;


namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCModifyTelephoneSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCModifyTelephoneSet : UserControl
    {
        public UCModifyTelephoneSet()
        {
            InitializeComponent();
        }

        private string VertyfyType = string.Empty;
        private System.Timers.Timer VertifytimeAlive3;
        private double interval3 = 1000;
        private int TickNum3 = 60;
        #region "对内函数"
        private void SetErroTagsClose()
        {
            this.tbErrorPicVertify.Visibility = Visibility.Collapsed;
            this.tbErrorNewTelephone.Visibility = Visibility.Collapsed;
            this.tbErrorVertifyCode.Visibility = Visibility.Collapsed;

            this.bdNewTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            bdPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
        }
        private async Task<BitmapSource> GetImgVertifyCode(string mobile)
        {
            //获取图形验证码
            string picStd = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "width", "height", "appKey" }, new string[] { mobile, "121", "34", "modify" });
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

        #region "对外函数"
        public void InitialModifyTelephone()
        {
            this.tbTelephone.Text = MagicGlobal.UserInfo.UserMobile;
            SetErroTagsClose();
        }
        #endregion

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
                    this.chkVertifyCode.IsChecked = false;
                }));
                this.VertifytimeAlive3.Stop();
                this.VertifytimeAlive3.Dispose();

            }


        }

        private async void chkVertifyCode_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNewTelephone.Text))
            {
                bdNewTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorNewTelephone.Visibility = Visibility.Visible;
                tbErrorNewTelephone.Text = "手机号不能为空";
                this.chkVertifyCode.IsChecked = false;
                return;
            }

            else
            {
                if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtNewTelephone.Text))
                {
                    bdNewTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorNewTelephone.Visibility = Visibility.Visible;
                    tbErrorNewTelephone.Text = "手机号格式错误";
                    this.chkVertifyCode.IsChecked = false;
                    return;
                }
            }

            //先判断手机号
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtNewTelephone.Text });
            string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (Model == null)
            {
                this.chkVertifyCode.IsChecked = false;
                this.busyCtrl.IsBusy = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
               
                return;

            }
            if (Model.code != 100)
            {
                this.chkVertifyCode.IsChecked = false;
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("手机号已存在", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();
       
                return;
            }





            string std = string.Empty;
            if (this.stkPicVertify.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(this.txtPicVertify.Text))
                {
                    this.chkVertifyCode.IsChecked = false;
                    bdPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorPicVertify.Visibility = Visibility.Visible;
                    tbErrorPicVertify.Text = "图形验证码不能为空";
                  
                    return;
                }
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey", "imageCode" }, new string[] { this.txtNewTelephone.Text, "sms", "modify", this.txtPicVertify.Text });
            }
            else
            {
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtNewTelephone.Text, "sms", "modify" });

            }

            this.busyCtrl.IsBusy = true;
            //string std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtForgetPwTel.Text, "sms", "findPwd" });
            string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrSendVertifyCode, MagicGlobal.UserInfo.Version, std));
            this.busyCtrl.IsBusy = false;
            BaseHttpModel<HttpVertifyCodeModel> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpVertifyCodeModel>>(jsonResult);
            if (model == null)
            {
                this.chkVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
              
                return;
            }
            if (model.code == 200)
            {
                if (model.data.showImageVerifyCode)
                {
                    this.chkVertifyCode.IsChecked = false;
                    ////获取图形验证码
                    imgPicVertify.Source = await GetImgVertifyCode(this.txtNewTelephone.Text);
                    this.busyCtrl.IsBusy = false;
                    this.tbErrorPicVertify.Visibility = Visibility.Visible;
                    this.bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    this.stkPicVertify.Visibility = Visibility.Visible;
                    this.busyCtrl.IsBusy = false;
                    return;
                }

                this.VertyfyType = "sms";
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
                this.chkVertifyCode.IsChecked = false;
                bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorVertifyCode.Text = "获取验证码次数达到上限，如有疑问请联系我们 010-59423287";
                tbErrorVertifyCode.Visibility = Visibility.Visible;
               
            }
            //验证码过期
            else if (model.code == -119)
            {
                this.chkVertifyCode.IsChecked = false;
                bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorVertifyCode.Text = "验证码已过期，请重新获取";
                tbErrorVertifyCode.Visibility = Visibility.Visible;
               
            }
            else if (model.code == -137)
            {
                this.busyCtrl.IsBusy = false;
                this.chkVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                imgPicVertify.Source = await GetImgVertifyCode(this.txtNewTelephone.Text);
            }
            else
            {
                this.chkVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
          
            }
        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            SetErroTagsClose();
        }

        private async void btnPicVertifycodeUpdate_Click(object sender, RoutedEventArgs e)
        {
            imgPicVertify.Source = await GetImgVertifyCode(this.txtNewTelephone.Text);
        }

        private async void BtnVoicCode_Click(object sender, RoutedEventArgs e)
        {

            if (this.chkVertifyCode.IsChecked == true)
            {
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("操作过于频繁", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();
                return;
            }
            if (string.IsNullOrEmpty(this.txtNewTelephone.Text))
            {
                bdNewTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorNewTelephone.Visibility = Visibility.Visible;
                tbErrorNewTelephone.Text = "手机号不能为空";
                return;
            }

            else
            {
                if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtNewTelephone.Text))
                {
                    bdNewTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorNewTelephone.Visibility = Visibility.Visible;
                    tbErrorNewTelephone.Text = "手机号格式错误";
                    return;
                }
            }

            //先判断手机号
            string JsonStr = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile" }, new string[] { this.txtNewTelephone.Text });
            string resultStr = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrCheckMobile, MagicGlobal.UserInfo.Version, JsonStr));
            BaseHttpModel Model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (Model == null)
            {
                this.busyCtrl.IsBusy = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkVertifyCode.IsChecked = false;
                return;

            }
            if(Model.code != 100)
            {
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("手机号已存在", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();
                return;
            }












            string std = string.Empty;
            if (this.stkPicVertify.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(this.txtPicVertify.Text))
                {
                    this.chkVertifyCode.IsChecked = false;
                    bdPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorPicVertify.Visibility = Visibility.Visible;
                    tbErrorPicVertify.Text = "图形验证码不能为空";
                    return;
                }
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey", "imageCode" }, new string[] { this.txtNewTelephone.Text, "voice", "modify", this.txtPicVertify.Text });
            }
            else
            {
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtNewTelephone.Text, "voice", "modify" });

            }

            //string std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.txtForgetPwTel.Text, "voice", "register" });
            string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrSendVertifyCode, MagicGlobal.UserInfo.Version, std));
            this.busyCtrl.IsBusy = false;
            BaseHttpModel<HttpVertifyCodeModel> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpVertifyCodeModel>>(jsonResult);
            if (model.code == 200)
            {
                if (model.data.showImageVerifyCode)
                {
                    this.chkVertifyCode.IsChecked = false;
                    ////获取图形验证码
                    imgPicVertify.Source = await GetImgVertifyCode(this.tbTelephone.Text);
                    this.busyCtrl.IsBusy = false;
                    this.tbErrorPicVertify.Visibility = Visibility.Visible;
                    this.bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    this.stkPicVertify.Visibility = Visibility.Visible;
                    this.busyCtrl.IsBusy = false;
                    return;
                }
                this.VertyfyType = "voice";
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("电话拨打中，请留意来电", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();

                //职位沟通定时器
                VertifytimeAlive3 = new System.Timers.Timer(this.interval3);
                VertifytimeAlive3.Elapsed += VertifytimeAlive_Elapsed3;
                VertifytimeAlive3.Enabled = true;
                this.TickNum3 = 60;
                this.tbClickNum3.Text = "(" + this.TickNum3.ToString() + ")";
                this.chkVertifyCode.IsChecked = true;

                //测试
                Console.WriteLine("验证码：" + model.data.verifyCode.ToString());
            }
            else if (model.code == -2)
            {
                this.chkVertifyCode.IsChecked = false;
                bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorVertifyCode.Text = "获取验证码次数达到上限，如有疑问请联系我们 010-59423287";
                tbErrorVertifyCode.Visibility = Visibility.Visible;
         
            }
            else if (model.code == -137)
            {
                this.busyCtrl.IsBusy = false;
                this.chkVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                imgPicVertify.Source = await GetImgVertifyCode(this.tbTelephone.Text);
            }
            else
            {
                this.chkVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
        
            }
        }

        private async void ModifyOK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNewTelephone.Text))
            {
                bdNewTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorNewTelephone.Visibility = Visibility.Visible;
                tbErrorNewTelephone.Text = "手机号不能为空";
                return;
            }

            else
            {
                if (!MagicCube.Common.CommonValidationMethod.IsValidTel(this.txtNewTelephone.Text))
                {
                    bdNewTelephone.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorNewTelephone.Visibility = Visibility.Visible;
                    tbErrorNewTelephone.Text = "手机号格式错误";
                    return;
                }
            }
            if (string.IsNullOrEmpty(this.txtVertifyCode.Text))
            {
                this.bdVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorVertifyCode.Visibility = Visibility.Visible;
                tbErrorVertifyCode.Text = "验证码不能为空";
                return;
            }

            this.busyCtrl.IsBusy = true;
            string jsonStr = DAL.JsonHelper.JsonParamsToString(
                new string[] { "userID", "mobile", "verifyCode", "verifyType", "appKey" }, 
                new string[] { MagicGlobal.UserInfo.Id.ToString(), this.txtNewTelephone.Text, this.txtVertifyCode.Text, this.VertyfyType, "modify" });
            string url = string.Format(MagicCube.DAL.ConfUtil.AddrModifyTelephone, MagicGlobal.UserInfo.Version, jsonStr);
            string result = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(url);
            this.busyCtrl.IsBusy = false;
            BaseHttpModel model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel>(result);
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
                    MagicGlobal.UserInfo.UserMobile = this.txtNewTelephone.Text;
                    MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink("手机号修改成功，请您重新登录", "去登录");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                    Messaging.Messenger.Default.Send<Messaging.MSChangeAccount, MagicCube.ViewSingle.MainWindow>(new Messaging.MSChangeAccount() { IsDirectChange = true});
                    return;
                }
                else if(model.code == -139)
                {
                    MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("手机号已存在", 1);
                    ds.Owner = Window.GetWindow(this);
                    ds.ShowDialog();
                    return;
                }
                else if(model.code == -140)
                {
                    MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("手机号已存在", 1);
                    ds.Owner = Window.GetWindow(this);
                    ds.ShowDialog();
                    return;
                }
                else
                {
                    TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                    return;
                }
            }

        }
    }
}
