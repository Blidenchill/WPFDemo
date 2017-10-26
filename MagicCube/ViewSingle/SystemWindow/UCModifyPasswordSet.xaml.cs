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
    /// UCModifyPasswordSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCModifyPasswordSet : UserControl
    {
        public UCModifyPasswordSet()
        {
            InitializeComponent();
        }
        string newWorld = string.Empty;
        string oldWorld = string.Empty;
        private string forgetPwVertyfyType = string.Empty;
        private System.Timers.Timer VertifytimeAlive3;
        private double interval3 = 1000;
        private int TickNum3 = 60;
        #region "设置密码panel"





        private async Task UpdatePassword(string password)
        {
            this.busyCtrl.IsBusy = true;
            HttpSetPasswordModel model = new HttpSetPasswordModel() { userID = MagicGlobal.UserInfo.Id.ToString(), password = password, type = "setPassword" };
            string jsonStr = DAL.JsonHelper.ToJsonString(model);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrUpdatePassword, MagicGlobal.UserInfo.Version, jsonStr));
            this.busyCtrl.IsBusy = false;
            BaseHttpModel model2 = DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (model2 == null)
            {
                TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                return;
            }
            else
            {
                if (model2.code != 200)
                {
                    TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model2.msg, "我知道了");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                    return;
                }
                MagicGlobal.UserInfo.PasswordExist = true;
                View.Message.DisappearShow dis = new View.Message.DisappearShow("修改成功", 1);
                dis.Owner = Window.GetWindow(this);
                dis.ShowDialog();
                this.gdSetting.Visibility = Visibility.Collapsed;
                this.gdModify.Visibility = Visibility.Visible;
            }

        }

        private async Task UpdatePassword(string newPassword, string oldPassword)
        {
            HttpSetPasswordModel model = new HttpSetPasswordModel() { userID = MagicGlobal.UserInfo.Id.ToString(), password = newPassword, oldPassword = oldPassword, type = "oldPassword" };
            string jsonStr = DAL.JsonHelper.ToJsonString(model);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrUpdatePassword, MagicGlobal.UserInfo.Version, jsonStr));
            this.busyCtrl.IsBusy = false;
            BaseHttpModel model2 = DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if (model2 == null)
            {
                TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                return;
            }

            else
            {
                if (model2.code == 200)
                {
                    MagicGlobal.UserInfo.PasswordExist = true;
                    View.Message.DisappearShow dis = new View.Message.DisappearShow("修改成功", 1);
                    dis.Owner = Window.GetWindow(this);
                    dis.ShowDialog();
                    //this.CurrentWordValidate = false;
                    //this.NewWordValidate = false;
                    //this.VerifyWordValidate = false;
                    //this.pbCurrentWord.Password = string.Empty;
                    //this.pbNewWord.Password = string.Empty;
                    //this.pbVerifyWord.Password = string.Empty;
                    this.txtForegetPwNewPassword.Password = string.Empty;
                    this.txtForegetPwConfirmPassword.Password = string.Empty;
                    this.gdSetting.Visibility = Visibility.Collapsed;
                    this.gdModify.Visibility = Visibility.Visible;
                }
                else if (model2.code == -128)
                {
                    //this.CurrentWordValidate = true;
                }
                else
                {
                    TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model2.msg, "我知道了");
                    link.Owner = Window.GetWindow(this);
                    link.ShowDialog();
                }



            }
        }
        #endregion
        #region "对内函数"
        private void SetErroTagsClose()
        {
            this.tbErrorForegetPwVertifyCode.Visibility = Visibility.Collapsed;
            this.tbErrorForegetPwPicVertify.Visibility = Visibility.Collapsed;
            this.tbErrorForegetPwNewPassword.Visibility = Visibility.Collapsed;
            this.tbErrorForegetPwConfirmPassword.Visibility = Visibility.Collapsed;

            this.bdForegetPwVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.bdForegetPwVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.rectNewWord.Stroke = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            this.rectVertifyWord.Stroke = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
            bdForgetPwPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe7, 0xe7, 0xe7));
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

        #region "对外函数"
        public void InitialMofityPassword()
        {
            this.tbTelephone.Text = MagicGlobal.UserInfo.UserMobile;
            SetErroTagsClose();
            
        }
        #endregion

        private async void chkForegetPwVertifyCode_Click(object sender, RoutedEventArgs e)
        {
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
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey", "imageCode" }, new string[] { this.tbTelephone.Text, "sms", "findPwd", this.txtForgetPwPicVertify.Text });
            }
            else
            {
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.tbTelephone.Text, "sms", "findPwd" });

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
                    imgForgetPwPicVertify.Source = await GetImgVertifyCode(this.tbTelephone.Text);
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
                this.chkForegetPwVertifyCode.IsChecked = false;
                bdForegetPwVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwVertifyCode.Text = "获取验证码次数达到上限，如有疑问请联系我们 010-59423287";
                tbErrorForegetPwVertifyCode.Visibility = Visibility.Visible;
             
            }
            //验证码过期
            else if (model.code == -119)
            {
                this.chkForegetPwVertifyCode.IsChecked = false;
                bdForegetPwVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwVertifyCode.Text = "验证码已过期，请重新获取";
                tbErrorForegetPwVertifyCode.Visibility = Visibility.Visible;
             
            }
            else if (model.code == -137)
            {
                this.busyCtrl.IsBusy = false;
                this.chkForegetPwVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                imgForgetPwPicVertify.Source = await GetImgVertifyCode(this.tbTelephone.Text);
            }
            else
            {
                this.chkForegetPwVertifyCode.IsChecked = false;
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
          
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

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            this.SetErroTagsClose();
        }



        private async void BtnForgetPwVoicCode_Click(object sender, RoutedEventArgs e)
        {
            string std = string.Empty;
            if ((bool)this.chkForegetPwVertifyCode.IsChecked)
            {
                MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("操作过于频繁", 1);
                ds.Owner = Window.GetWindow(this);
                ds.ShowDialog();
                return;
            }


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
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey", "imageCode" }, new string[] { this.tbTelephone.Text, "voice", "findPwd", this.txtForgetPwPicVertify.Text });
            }
            else
            {
                std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "verifyType", "appKey" }, new string[] { this.tbTelephone.Text, "voice", "findPwd" });

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
                    imgForgetPwPicVertify.Source = await GetImgVertifyCode(this.tbTelephone.Text);
                    this.busyCtrl.IsBusy = false;
                    this.tbErrorForegetPwPicVertify.Visibility = Visibility.Visible;
                    this.bdForgetPwPicVertify.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    this.stkForgetPwPicVertify.Visibility = Visibility.Visible;
                    this.busyCtrl.IsBusy = false;
                    return;
                }
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
                imgForgetPwPicVertify.Source = await GetImgVertifyCode(this.tbTelephone.Text);
            }
            else
            {
                MagicCube.TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.chkForegetPwVertifyCode.IsChecked = false;
            }
        }



        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtForegetPwVertifyCode.Text))
            {
                this.bdForegetPwVertifyCode.BorderBrush = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwVertifyCode.Visibility = Visibility.Visible;
                tbErrorForegetPwVertifyCode.Text = "验证码不能为空";
                return;
            }
            if (string.IsNullOrEmpty(this.txtForegetPwNewPassword.Password))
            {
                this.rectNewWord.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwNewPassword.Visibility = Visibility.Visible;
                tbErrorForegetPwNewPassword.Text = "密码不能为空";
                return;
            }
            else
            {
                if (this.txtForegetPwNewPassword.Password.Length < 6 || this.txtForegetPwNewPassword.Password.Length > 20)
                {
                    this.rectNewWord.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForegetPwNewPassword.Visibility = Visibility.Visible;
                    tbErrorForegetPwNewPassword.Text = "请输入6-20位字母、数字或字符";
                    return;
                }
                if(this.txtForegetPwNewPassword.Password == this.tbTelephone.Text)
                {
                    this.rectNewWord.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForegetPwNewPassword.Visibility = Visibility.Visible;
                    tbErrorForegetPwNewPassword.Text = "为了您的账号安全，密码请勿与手机号相同";
                    return;
                }

            }
            if (string.IsNullOrEmpty(this.txtForegetPwConfirmPassword.Password))
            {
                this.rectVertifyWord.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwConfirmPassword.Visibility = Visibility.Visible;
                tbErrorForegetPwConfirmPassword.Text = "请再次输入新密码";
                return;
            }
            else
            {
                if (this.txtForegetPwNewPassword.Password != this.txtForegetPwConfirmPassword.Password)
                {
                    this.rectVertifyWord.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForegetPwConfirmPassword.Visibility = Visibility.Visible;
                    tbErrorForegetPwConfirmPassword.Text = "两次输入的密码不一致，请重新输入";
                    return;
                }
            }


            //修改密码
            this.busyCtrl.IsBusy = true;
            string jsonStr = DAL.JsonHelper.JsonParamsToString(new string[] { "mobile", "password", "verifyCode", "verifyType", "appKey" },
                new string[] { this.tbTelephone.Text, this.txtForegetPwNewPassword.Password, this.txtForegetPwVertifyCode.Text, this.forgetPwVertyfyType, "findPwd" });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrForgetPassword, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<HttpForgetPasswordAndLogin> model2 = DAL.JsonHelper.ToObject<BaseHttpModel<HttpForgetPasswordAndLogin>>(resultStr);
            if (model2 == null)
            {
                TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                link.Owner = Window.GetWindow(this);
                link.ShowDialog();
                this.busyCtrl.IsBusy = false;
                return;
            }
            else
            {
                if (model2.code != 200)
                {
                    if (model2.code == -104)
                    {
                        this.busyCtrl.IsBusy = false;
                        TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(model2.msg, "我知道了");
                        link.Owner = Window.GetWindow(this);
                        link.ShowDialog();
                       
                    }
                    else
                    {
                        this.busyCtrl.IsBusy = false;
                        TemplateUC.WinMessageLink link = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                        link.Owner = Window.GetWindow(this);
                        link.ShowDialog();
                     
                    }

                    return;
                }

                else
                {
                    this.chkForegetPwVertifyCode.IsChecked = false;
                    this.VertifytimeAlive3.Enabled = false;
                    this.busyCtrl.IsBusy = false;
                    this.txtForgetPwPicVertify.Text = string.Empty;
                    this.txtForegetPwVertifyCode.Text = string.Empty;
                    this.txtForegetPwNewPassword.Password = string.Empty;
                    this.txtForegetPwConfirmPassword.Password = string.Empty;
                    MagicCube.View.Message.DisappearShow ds = new View.Message.DisappearShow("修改成功", 1);
                    ds.Owner = Window.GetWindow(this);
                    ds.ShowDialog();
               
                    
                    Console.WriteLine("修改密码成功");
                 

                }
            }
        }

        private async void btnForgetPwPicVertifycodeUpdate_Click(object sender, RoutedEventArgs e)
        {
            imgForgetPwPicVertify.Source = await GetImgVertifyCode(this.tbTelephone.Text);
        }

        private async void SetPsOK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtForegetPwNewPassword2.Password))
            {
                this.rectNewWord2.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwNewPassword2.Visibility = Visibility.Visible;
                tbErrorForegetPwNewPassword2.Text = "密码不能为空";
                return;
            }
            else
            {
                if (this.txtForegetPwNewPassword2.Password.Length < 6 || this.txtForegetPwNewPassword2.Password.Length > 20)
                {
                    this.rectNewWord2.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForegetPwNewPassword2.Visibility = Visibility.Visible;
                    tbErrorForegetPwNewPassword2.Text = "请输入6-20位字母、数字或字符";
                    return;
                }
                if (this.txtForegetPwNewPassword2.Password == this.tbTelephone2.Text)
                {
                    this.rectNewWord2.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForegetPwNewPassword2.Visibility = Visibility.Visible;
                    tbErrorForegetPwNewPassword2.Text = "两次输入的密码不一致，请重新输入";
                    return;
                }

            }
            if (string.IsNullOrEmpty(this.txtForegetPwConfirmPassword2.Password))
            {
                this.rectVertifyWord2.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                tbErrorForegetPwConfirmPassword2.Visibility = Visibility.Visible;
                tbErrorForegetPwConfirmPassword2.Text = "请再次输入新密码";
                return;
            }
            else
            {
                if (this.txtForegetPwNewPassword2.Password != this.txtForegetPwConfirmPassword2.Password)
                {
                    this.rectVertifyWord2.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    tbErrorForegetPwConfirmPassword2.Visibility = Visibility.Visible;
                    tbErrorForegetPwConfirmPassword2.Text = "两次输入的密码不一致，请重新输入";
                    return;
                }
            }

            await UpdatePassword(this.txtForegetPwConfirmPassword2.Password);
        }


    }
}
