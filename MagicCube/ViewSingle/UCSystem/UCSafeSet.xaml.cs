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
using System.Text.RegularExpressions;

using MagicCube.HttpModel;

using MagicCube.Common;
using MagicCube.View.Message;
using MagicCube.ViewModel;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCSafeSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCSafeSet : UserControl
    {
        public UCSafeSet()
        {
            InitializeComponent();
        }
        string newWorld = string.Empty;
        string oldWorld = string.Empty;
        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.9.4.6.1", "clk");

            if (string.IsNullOrWhiteSpace(this.pbCurrentWord.Password))
            {
                this.CurrentWordValidate = true;
            }
            else
            {
                this.CurrentWordValidate = false;
            }
            if(string.IsNullOrWhiteSpace(this.pbNewWord.Password))
            {
                this.NewWordValidate = true;
            }
            else
            {
                if (this.pbNewWord.Password.Length < 6 || this.pbNewWord.Password.Length > 20)
                    this.NewWordValidate = true;
                else
                {
                    this.NewWordValidate = false;
                }
            }

            //新加“为了您的账号安全，密码请勿与手机号相同”
            if (this.pbNewWord.Password == MagicGlobal.UserInfo.UserAccount)
            {
                this.tbNewPassValidate.Visibility = Visibility.Visible;
                return;
            }



            if(this.pbNewWord.Password != this.pbVerifyWord.Password)
            {
                this.VerifyWordValidate = true;
            }
            else
            {
                this.VerifyWordValidate = false;
            }
            if((!this.VerifyWordValidate) && (!this.NewWordValidate) && (!this.CurrentWordValidate))
            {
                 newWorld = this.pbNewWord.Password;
                 oldWorld = this.pbCurrentWord.Password;
                this.busyCtrl.IsBusy = true;
                await UpdatePassword(newWorld, oldWorld);
           
            }
            
        }

        //private bool ModifyPasswordFc()
        //{
        //    HttpModifyPassword httpModel = new HttpModifyPassword() { oldPassword = this.oldWorld, newPassword = this.newWorld };
        //    string sendStr = MagicCube.DAL.JsonHelper.ToJsonString(httpModel);
        //    string result = DAL.HttpHelper.Instance.HttpPostWebEx(ConfUtil.ServerModifyPassword, sendStr);
        //    if(result.Contains("200"))
        //    {
        //        return true;
        //    }
        //    return false;
        //}


        public bool CurrentWordValidate
        {
            get { return (bool)GetValue(CurrentWordValidateProperty); }
            set { SetValue(CurrentWordValidateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentWordValidate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentWordValidateProperty =
            DependencyProperty.Register("CurrentWordValidate", typeof(bool), typeof(UCSafeSet), new PropertyMetadata(null));



        public bool NewWordValidate
        {
            get { return (bool)GetValue(NewWordValidateProperty); }
            set { SetValue(NewWordValidateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewWordValidate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewWordValidateProperty =
            DependencyProperty.Register("NewWordValidate", typeof(bool), typeof(UCSafeSet), new PropertyMetadata(null));



        public bool VerifyWordValidate
        {
            get { return (bool)GetValue(VerifyWordValidateProperty); }
            set { SetValue(VerifyWordValidateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VerifyWordValidate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerifyWordValidateProperty =
            DependencyProperty.Register("VerifyWordValidate", typeof(bool), typeof(UCSafeSet), new PropertyMetadata(null));




        private void HintPasswordBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy || e.Command == ApplicationCommands.Cut || e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void pbCurrentWord_GotFocus(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.9.4.3.1", "clk");
            this.CurrentWordValidate = false;
        }

        private void pbNewWord_GotFocus(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.9.4.4.1", "clk");

            this.NewWordValidate = false;
            this.tbNewPassValidate.Visibility = Visibility.Collapsed;
        }

        private void pbVerifyWord_GotFocus(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.9.4.5.1", "clk");

            this.VerifyWordValidate = false;
        }



        #region "设置密码panel"

        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtPassword.Password))
            {
                this.rect.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                this.txtError.Visibility = Visibility.Visible;
                return;
            }
            if (this.txtPassword.Password.Length > 20 || this.txtPassword.Password.Length < 6)
            {
                this.rect.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                this.txtError.Visibility = Visibility.Visible;
                return;
            }
            await UpdatePassword(this.txtPassword.Password);
        }



        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            this.txtError.Visibility = Visibility.Hidden;
            this.rect.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
        }

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
                    this.CurrentWordValidate = false;
                    this.NewWordValidate = false;
                    this.VerifyWordValidate = false;
                    this.pbCurrentWord.Password = string.Empty;
                    this.pbNewWord.Password = string.Empty;
                    this.pbVerifyWord.Password = string.Empty;
                    this.gdSetting.Visibility = Visibility.Collapsed;
                    this.gdModify.Visibility = Visibility.Visible;      
                }
                else if (model2.code == -128)
                {
                    this.CurrentWordValidate = true;
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

        private void TrackOperation_Event(object sender, RoutedEventArgs e)
        {

        }
    }
}
