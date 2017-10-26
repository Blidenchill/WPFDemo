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
using System.IO;
using MagicCube.Model;
using MagicCube.HttpModel;

using MagicCube.Common;
using MagicCube.View.Message;
using System.Threading.Tasks;
using MagicCube.ViewModel;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCSelfInfo.xaml 的交互逻辑
    /// </summary>
    public partial class UCSelfInfo : UserControl
    {
        string picturePath = string.Empty;
     
        private bool IsNameActivePanelClickOK = false;
        private bool IsPositionActivePanelClickOK = false;
        
        InfomationModel infomationModel = new InfomationModel();

        #region "属性"


        public bool IsActivate
        {
            get { return (bool)GetValue(IsActivateProperty); }
            set { SetValue(IsActivateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActivate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActivateProperty =
            DependencyProperty.Register("IsActivate", typeof(bool), typeof(UCSelfInfo), new PropertyMetadata(null));




        public bool IsUserPositionIsEdit
        {
            get { return (bool)GetValue(IsUserPositionIsEditProperty); }
            set { SetValue(IsUserPositionIsEditProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsUserPositionIsNull.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUserPositionIsEditProperty =
            DependencyProperty.Register("IsUserPositionIsEdit", typeof(bool), typeof(UCSelfInfo), new PropertyMetadata(null));



        public bool IsUserNameIsEdit
        {
            get { return (bool)GetValue(IsUserNameIsEditProperty); }
            set { SetValue(IsUserNameIsEditProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsUserNameIsEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUserNameIsEditProperty =
            DependencyProperty.Register("IsUserNameIsEdit", typeof(bool), typeof(UCSelfInfo), new PropertyMetadata(null));




        #endregion
        public UCSelfInfo()
        {
            InitializeComponent();
            
        }


        #region "个人信息"



        private void btnModifyInfo_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.1.4.1", "clk");
            Common.TrackHelper2.TrackOperation("5.7.2.2.1", "pv");
            this.gdPerson.Visibility = Visibility.Collapsed;
            this.gdEditPerson.Visibility = Visibility.Visible;
            IsUserNameIsEdit = false;
            //this.Cursor = Cursors.Wait;
            //this.JudgeActivate();
            //this.Cursor = Cursors.Arrow;
        }

        private void btnAddHeadPicture_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.2.3.1", "clk");
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "(*.jpg,*.png)|*.jpg;*.png";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ucSelfImageUpload.GetImagePath(openFile.FileName);
                this.ucSelfImageUpload.Visibility = Visibility.Visible;
                this.gdEditPerson.Visibility = Visibility.Collapsed;
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.2.10.1", "clk");

            bool flag = this.PersonValidateTrigger();
            if(string.IsNullOrEmpty(this.txtAreaCode.Text) && (!string.IsNullOrEmpty(this.txtOfficePhone.Text)))
            {
                flag = false;
                this.rctAreaCode.Stroke = new SolidColorBrush(Colors.Red);
                this.txtAreaCodeValidate.Text = "区号不能为空";
                this.stkTelephoneValidate.Visibility = Visibility.Visible;
                this.txtAreaCodeValidate.Visibility = Visibility.Visible;
            }
            else if((!string.IsNullOrEmpty(this.txtAreaCode.Text)) && string.IsNullOrEmpty(this.txtOfficePhone.Text))
            {
                flag = false;
                this.rctOfficePhone.Stroke = new SolidColorBrush(Colors.Red);
                this.txtOfficePhoneValidate.Text = "座机号不能为空";
                this.stkTelephoneValidate.Visibility = Visibility.Visible;
                this.txtOfficePhoneValidate.Visibility = Visibility.Visible;
            }
            else if(string.IsNullOrEmpty(this.txtAreaCode.Text)&&string.IsNullOrEmpty(this.txtOfficePhone.Text) && (!string.IsNullOrEmpty(this.txtExtensionNumber.Text)))
            {

                flag = false;
                this.rctOfficePhone.Stroke = new SolidColorBrush(Colors.Red);
                this.rctAreaCode.Stroke = new SolidColorBrush(Colors.Red);
                this.txtAreaCodeValidate.Text = "区号不能为空";
                this.txtAreaCodeValidate.Visibility = Visibility.Visible;
                this.txtOfficePhoneValidate.Text = "座机号不能为空";
                this.txtOfficePhoneValidate.Visibility = Visibility.Visible;
                this.stkTelephoneValidate.Visibility = Visibility.Visible;
            }

            if(this.txtAreaCode.Text.Length < 3 && (!string.IsNullOrEmpty(this.txtAreaCode.Text)))
            {
                flag = false;
                this.rctAreaCode.Stroke = new SolidColorBrush(Colors.Red);
                this.stkTelephoneValidate.Visibility = Visibility.Visible;
                this.txtAreaCodeValidate.Visibility = Visibility.Visible;
                this.txtAreaCodeValidate.Text = "区号不得少于3位";
            }
            if(this.txtOfficePhone.Text.Length < 7 && (!string.IsNullOrEmpty(this.txtOfficePhone.Text)))
            {
                flag = false;
                this.rctOfficePhone.Stroke = new SolidColorBrush(Colors.Red);
                this.stkTelephoneValidate.Visibility = Visibility.Visible;
                this.txtOfficePhoneValidate.Visibility = Visibility.Visible;
                this.txtOfficePhoneValidate.Text = "座机号不得少于7位";
            }

            
            if (!flag)
            {
                DisappearShow show = new DisappearShow("您有不正确填写项", 1);
                show.Owner = Window.GetWindow(this);
                show.ShowDialog();
                return;
            }
            if(string.IsNullOrEmpty(infomationModel.AvatarUrl))
            {
                MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink("请上传头像", "我知道了");
                winLink.Owner = Window.GetWindow(this);
                winLink.ShowDialog();
                return;
            }

            if(MagicGlobal.isHRAuth)
            {
                if (IsUserPositionIsEdit)
                    this.IsPositionActivePanelClickOK = false;
                if (this.IsNameActivePanelClickOK || this.IsPositionActivePanelClickOK)
                {
                    MagicCube.TemplateUC.WinActiveTip tip = new TemplateUC.WinActiveTip();
                    tip.Owner = Window.GetWindow(this);
                    if ((bool)tip.ShowDialog())
                    {
                        IsActivate = false;
                        MagicGlobal.isHRAuth = false;
                        MagicGlobal.UserInfo.validStatus = false;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            //新接口
            this.busyCtrl.IsBusy = true;
            bool isSuccess = await this.SaveInfo();
            this.busyCtrl.IsBusy = false;
            if(isSuccess)
            {
                MagicGlobal.UserInfo.avatarUrl = infomationModel.AvatarUrl;
                DisappearShow show = new DisappearShow("个人信息保存成功", 1);
                show.Owner = Window.GetWindow(this);
                show.ShowDialog();
                this.UpdateGlobalInfo();
                this.txtFullOfficePhone.Text = this.infomationModel.FullOfficePhone;
                this.gdEditPerson.Visibility = Visibility.Collapsed;
                this.gdPerson.Visibility = Visibility.Visible;
            }
            else
            {
                DisappearShow show = new DisappearShow("网络异常，保存失败", 1);
                show.Owner = Window.GetWindow(this);
                show.ShowDialog();
            }



            //this.busyCtrl.IsBusy = true;
            //MagicGlobal.UserInfo.avatarUrl = infomationModel.AvatarUrl;
            //Action method = delegate
            //{

            //    //将保存的信息上传服务器
            //    HttpSelfInfo httpSelfInfo = new HttpSelfInfo();
            //    httpSelfInfo.avatarUrl = infomationModel.AvatarUrl;
            //    httpSelfInfo.realName = infomationModel.RealName.Replace(" ","");
            //    httpSelfInfo.areaCode = infomationModel.AreaCode;
            //    httpSelfInfo.officePhone = infomationModel.OfficePhone;
            //    httpSelfInfo.extensionNumber = infomationModel.ExtensionNumber;
            //    httpSelfInfo.email = infomationModel.Email;
            //    httpSelfInfo.position = infomationModel.UserPosition;
            //    httpSelfInfo.isAccountActivate = MagicGlobal.isAccountActivate;



            //    string str = MagicCube.Util.JsonUtil.ToJsonString(httpSelfInfo);
            //   string strResult =  DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerSelfInfoSave, str);

            //    Action message = delegate
            //    {
            //        this.busyCtrl.IsBusy = false;
            //        if (strResult.StartsWith("{\"status\": 200,"))
            //        {
            //            DisappearShow show = new DisappearShow("个人信息保存成功", 1);
            //            show.Owner = Window.GetWindow(this);
            //            show.ShowDialog();
            //            this.UpdateGlobalInfo();
            //            this.txtFullOfficePhone.Text = this.infomationModel.FullOfficePhone;
            //            //this.IsUserPositionIsEdit = false;
            //            this.gdEditPerson.Visibility = Visibility.Collapsed;
            //            this.gdPerson.Visibility = Visibility.Visible;
            //        }
            //        else
            //        {
            //            DisappearShow show = new DisappearShow("网络异常，保存失败", 1);
            //            show.Owner = Window.GetWindow(this);
            //            show.ShowDialog();
            //        }

            //    };
            //    this.Dispatcher.BeginInvoke(message, System.Windows.Threading.DispatcherPriority.Normal, null);
            //};
            //method.BeginInvoke(null, null);

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.2.11.1", "clk");

            this.gdEditPerson.Visibility = Visibility.Collapsed;
            this.gdPerson.Visibility = Visibility.Visible;
            this.txtFullOfficePhone.Text = this.infomationModel.FullOfficePhone;
            this.InitalPersonInfo();
        }


        private void txt_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;




            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
                {
                    e.Handled = true;
                }
            }
            else
                e.Handled = true;
            //  if(textBox.Name == "txtAreaCode")
            //  {
            //      if (this.txtAreaCode.Text.Length >= 5)
            //      {
            //          if (e.Key == Key.Back)
            //              return;
            //          e.Handled = true;
            //      }
            //  }
            // if(textBox.Name == "txtOfficePhone")
            //  {
            //      if (this.txtOfficePhone.Text.Length >= 8)
            //      {
            //          if (e.Key == Key.Back)
            //              return;
            //          e.Handled = true;
            //      }
            //  }
            //if(textBox.Name == "txtExtensionNumber")
            //  {
            //      if (this.txtExtensionNumber.Text.Length >= 5)
            //      {
            //          if (e.Key == Key.Back)
            //              return;
            //          e.Handled = true;
            //      }
            //  }


        }



        #endregion

        #region "对内函数"
        private async void InitalPersonInfo()
        {
            await UpdatePersonInfo();
            infomationModel.validStatus = MagicGlobal.UserInfo.validStatus;
            infomationModel.Telephone = MagicGlobal.UserInfo.UserMobile;
            infomationModel.Email = MagicGlobal.UserInfo.Email;
            infomationModel.RealName = MagicGlobal.UserInfo.RealName;
            infomationModel.AreaCode = MagicGlobal.UserInfo.AreaCode;
            infomationModel.OfficePhone = MagicGlobal.UserInfo.OfficePhone;
            infomationModel.ExtensionNumber = MagicGlobal.UserInfo.ExtensionNumber;
            infomationModel.AvatarUrl = MagicGlobal.UserInfo.avatarUrl;
            infomationModel.UserPosition = MagicGlobal.UserInfo.UserPosition;

           


            //if(string.IsNullOrEmpty(infomationModel.UserPosition))
            //{
            //    this.IsUserPositionIsEdit = true;
            //}
            //else
            //{
            //    this.IsUserPositionIsEdit = false;
            //}
        }

        private async Task UpdatePersonInfo()
        {
            string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpUserBasicInfoModel>();
            string std = MagicCube.DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "properties" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), propertys });
            string jsonResult = await MagicCube.DAL.HttpHelper.Instance.HttpGetAsync(string.Format(MagicCube.DAL.ConfUtil.AddrGetUserBasicInfo, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpUserBasicInfoModel> model = MagicCube.DAL.JsonHelper.ToObject<BaseHttpModel<HttpUserBasicInfoModel>>(jsonResult);
            this.busyCtrl.IsBusy = false;
            if (model == null)
            {
                return;
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
                MagicGlobal.isHRAuth = Convert.ToBoolean(model.data.isHRAuth);

                //埋单获取userId和Unique
                Common.TrackHelper2.UserId = MagicGlobal.UserInfo.Id.ToString();
                Common.TrackHelper2.UniqueKey = Common.TrackHelper2.MD5Encrypt(MagicGlobal.UserInfo.UserMobile);
            }
        }

        private void UpdateGlobalInfo()
        {
            MagicGlobal.UserInfo.validStatus = infomationModel.validStatus;
            MagicGlobal.UserInfo.Email = infomationModel.Email;
            MagicGlobal.UserInfo.RealName = infomationModel.RealName;
            MagicGlobal.UserInfo.AreaCode = infomationModel.AreaCode;
            MagicGlobal.UserInfo.OfficePhone = infomationModel.OfficePhone;
            MagicGlobal.UserInfo.ExtensionNumber = infomationModel.ExtensionNumber;
            MagicGlobal.UserInfo.avatarUrl = infomationModel.AvatarUrl;
            MagicGlobal.UserInfo.UserPosition = infomationModel.UserPosition;
            MagicGlobal.UserInfo.Email = infomationModel.Email;
        }


        public bool PersonValidateTrigger()
        {
            this.infomationModel.IsPersoonValidation = true;
            string temp = "dd";
            string temp2 = "11@11.com";
            string content;

            content = this.infomationModel.AreaCode;
            this.infomationModel.AreaCode = temp;
            this.infomationModel.AreaCode = content;

            content = this.infomationModel.OfficePhone;
            this.infomationModel.OfficePhone = temp;
            this.infomationModel.OfficePhone = content;

            content = this.infomationModel.ExtensionNumber;
            this.infomationModel.ExtensionNumber = temp;
            this.infomationModel.ExtensionNumber = content;

            content = this.infomationModel.RealName;
            this.infomationModel.RealName = temp;
            this.infomationModel.RealName = content;

            content = infomationModel.UserPosition;
            infomationModel.UserPosition = temp;
            infomationModel.UserPosition = content;


            content = this.infomationModel.Email;
            this.infomationModel.Email = temp2;
            this.infomationModel.Email = content;

            return this.infomationModel.IsPersoonValidation;
        }

        private async Task JudgeActivate()
        {
            this.IsActivate = MagicGlobal.isHRAuth;
            if(!MagicGlobal.isHRAuth)
            {
                string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpUserBasicInfoModel>();
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "properties" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), propertys });
                string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetUserBasicInfo, MagicGlobal.UserInfo.Version, std));
                BaseHttpModel<HttpUserBasicInfoModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpUserBasicInfoModel>>(jsonResult);
                if (model == null)
                {
                    return;
                }
                if (model.code == 200)
                {
                    this.IsActivate = Convert.ToBoolean(model.data.isHRAuth);
                }
                if(this.IsActivate)
                {
                    MagicGlobal.UserInfo.validStatus = true;
                }
            }

            //if (!MagicGlobal.isAccountActivate)
            //{
            //    string pCanPublicResult = DAL.HttpHelper.Instance.HttpGet(ConfUtil.ServerCanPublic);
            //    Console.WriteLine(pCanPublicResult);

            //    if (!pCanPublicResult.Contains("200"))
            //    {
            //        if (pCanPublicResult.Contains("10090"))
            //        {
            //            this.IsActivate = false;
                      
            //        }
            //        else if (pCanPublicResult.Contains("10093"))
            //        {
            //            this.IsActivate = true;
                     
            //        }
            //        else if (pCanPublicResult.Contains("10094"))
            //        {

            //            this.IsActivate = false;
             
            //        }
            //    }
            //    if (pCanPublicResult.Contains("200"))
            //    {
            //        MagicGlobal.isAccountActivate = true;
            //        MagicGlobal.UserInfo.validStatus = true;
            //    }
            //}
            

            

            if (this.IsActivate)
            {
                this.IsUserPositionIsEdit = false;
                this.IsUserNameIsEdit = false;
                if (string.IsNullOrEmpty(infomationModel.UserPosition))
                {
                    this.IsUserPositionIsEdit = true;
                }
                else
                {
                    this.IsUserPositionIsEdit = false;
                }
            }
            else
            {
                this.IsUserPositionIsEdit = true;
                this.IsUserNameIsEdit = true;
                
            }
            
        }

        private async Task<bool> SaveInfo()
        {
            bool isSuccess = false;
            MagicCube.ViewModel.HttpSelfInfoModel model = new ViewModel.HttpSelfInfoModel();
            model.name = this.infomationModel.RealName;
            model.avatar = this.infomationModel.AvatarUrl;
            model.hrEmail = this.infomationModel.Email;
            model.position = this.infomationModel.UserPosition;
            model.userID = MagicGlobal.UserInfo.Id.ToString();
            if(this.infomationModel.FullOfficePhone != "未填写")
                model.telNr = this.infomationModel.FullOfficePhone;

            string jsonStr = DAL.JsonHelper.ToJsonString(model);
            string jsonResult = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrSaveUserInfo, MagicGlobal.UserInfo.Version), jsonStr);
            MagicCube.ViewModel.BaseHttpModel<HttpSelfInfoModel> modelResult = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<HttpSelfInfoModel>>(jsonResult);
            if(modelResult == null)
            {
                return false;
            }
            if(modelResult.code == 200)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
        }
            
        #endregion
        #region "回调函数"
        private async void ImageUploadOKCallback(BitmapSource bs)
        {
            this.busyCtrl.IsBusy = true;
            this.gdEditPerson.Visibility = Visibility.Visible;
            this.ucSelfImageUpload.Visibility = Visibility.Collapsed;
            MemoryStream stream = ImageProcessor.SaveImageToStream(bs);
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "appKey", "fileName" }, new string[] { "head", "head" });
            //FileStream fs = new FileStream("D:\\ted.png", FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream("D:\\td.png", FileMode.Create, FileAccess.Write);
            //byte[] byteTem = new byte[3024];
            //stream.Write(byteTem, 1000, 3024);
     



                string resultStr = await DAL.HttpHelper.Instance.HttpUploadFile(string.Format(DAL.ConfUtil.AddrUploadFile, MagicGlobal.UserInfo.Version, std), stream, "head");
            BaseHttpModel<HttpUploadFileModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpUploadFileModel>>(resultStr);
            this.busyCtrl.IsBusy = false;
            if(model == null)
            {
                MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                winLink.Owner = Window.GetWindow(this);
                winLink.ShowDialog();
                return;
            }
            else
            {
                if(model.code == 200)
                {
                    infomationModel.AvatarUrl = model.data.url;
                    return;
                }
                else
                {

                    MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    winLink.Owner = Window.GetWindow(this);
                    winLink.ShowDialog();
                    return;
                }

            }
            //Action method = delegate
            //{
            //    //string strTemp = HttpHelper.HttpUploadFile(string.Format(ConfUtil.ServerUploadFile, "test1"), picturePath, null);
            //    string strTemp = HttpHelper.HttpUploadStream(string.Format(ConfUtil.ServerUploadFile, "test1"), stream, null, Encoding.UTF8);
            //    if (strTemp != null)
            //    {
            //        HttpReturnUploadPictureUrl urlJson = DAL.JsonHelper.ToObject<HttpReturnUploadPictureUrl>(strTemp);
            //        if (urlJson != null)
            //        {
            //            infomationModel.AvatarUrl = urlJson.url;
            //            //MagicGlobal.UserInfo.avatarUrl = urlJson.url;

            //        }
            //        ////将保存的信息上传服务器
            //        //HttpSelfInfo httpSelfInfo = new HttpSelfInfo();
            //        //httpSelfInfo.avatarUrl = infomationModel.AvatarUrl;
            //        //httpSelfInfo.realName = infomationModel.RealName;
            //        //httpSelfInfo.areaCode = infomationModel.AreaCode;
            //        //httpSelfInfo.officePhone = infomationModel.OfficePhone;
            //        //httpSelfInfo.extensionNumber = infomationModel.ExtensionNumber;
            //        //string str = MagicCube.Util.JsonUtil.ToJsonString(httpSelfInfo);
            //        //DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerSelfInfoSave, str);
            //    }
            //    Action message = delegate
            //    {
            //        this.busyCtrl.IsBusy = false;
            //        //DisappearShow show = new DisappearShow("上传头像成功", 1);
            //        //Window win = Window.GetWindow(this);
            //        //show.Owner = win;
            //        //show.Show();
            //        //show.ShowDialog();
            //        //win.Activate();
            //        //this.gdUploadPersonPicture.Visibility = System.Windows.Visibility.Collapsed;
            //    };
            //    this.Dispatcher.BeginInvoke(message, System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            //};
            //method.BeginInvoke(null, null);

         

        }
        private void ImageUploadCancelCallback()
        {
            this.gdEditPerson.Visibility = Visibility.Visible;
            this.ucSelfImageUpload.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region "对外功能函数"
        public void InitialSelfInfo()
        {
            this.gdEditPerson.Visibility = Visibility.Collapsed;
            this.ucSelfImageUpload.Visibility = Visibility.Collapsed;
            this.gdPerson.Visibility = Visibility.Visible;
          
            this.InitalPersonInfo();
            this.gdMain.DataContext = this.infomationModel;
            this.ucSelfImageUpload.OKAction += ImageUploadOKCallback;
            this.ucSelfImageUpload.CancelAction += ImageUploadCancelCallback;
        }
        #endregion

        private void BtnNameEditClose_Click(object sender, RoutedEventArgs e)
        {
            this.gdNameEdit.Visibility = Visibility.Collapsed;
            this.gdPositionEdit.Visibility = Visibility.Collapsed;
        }

        private void BtnPositionModify_Click(object sender, RoutedEventArgs e)
        {
            MagicCube.TemplateUC.WinNameEdit nameEdit = new TemplateUC.WinNameEdit(infomationModel.UserPosition, IsActivate, false);
            nameEdit.Owner = Window.GetWindow(this);
            if ((bool)nameEdit.ShowDialog())
            {
                this.infomationModel.UserPosition = nameEdit.PositionShow;
                this.IsPositionActivePanelClickOK = true;

            }
        }

        private void BtnNameModify_Click(object sender, RoutedEventArgs e)
        {
            MagicCube.TemplateUC.WinNameEdit nameEdit = new TemplateUC.WinNameEdit(infomationModel.RealName, IsActivate, true);
            nameEdit.Owner = Window.GetWindow(this);
            if((bool)nameEdit.ShowDialog())
            {
                this.infomationModel.RealName = nameEdit.NameShow;
                this.IsNameActivePanelClickOK = true;
            }
           
        }
    
        private void BtnNameActiveOK_Click(object sender, RoutedEventArgs e)
        {
            this.IsNameActivePanelClickOK = true;
            this.gdNameEdit.Visibility = Visibility.Collapsed;

        }

        private void BtnPositionActiveOK_Click(object sender, RoutedEventArgs e)
        {
            this.IsPositionActivePanelClickOK = true;
            this.gdPositionEdit.Visibility = Visibility.Collapsed;
        }

        private void txtAreaCode_GotFocus(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.2.7.1", "clk");

            this.rctAreaCode.Stroke = new SolidColorBrush(Color.FromRgb(229,229,229));
            this.rctOfficePhone.Stroke = new SolidColorBrush(Color.FromRgb(229, 229, 229));
            this.stkTelephoneValidate.Visibility = Visibility.Collapsed;
            this.txtOfficePhoneValidate.Visibility = Visibility.Hidden;
            this.txtAreaCodeValidate.Visibility = Visibility.Hidden;
        }

        private void txtOfficePhone_GotFocus(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.2.8.1", "clk");

            this.rctOfficePhone.Stroke = new SolidColorBrush(Color.FromRgb(229, 229, 229));
            this.rctAreaCode.Stroke = new SolidColorBrush(Color.FromRgb(229, 229, 229));
            this.stkTelephoneValidate.Visibility = Visibility.Collapsed;
            this.txtOfficePhoneValidate.Visibility = Visibility.Hidden;
            this.txtAreaCodeValidate.Visibility = Visibility.Hidden;
        }

        private void txtExtensionNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.2.9.1", "clk");

            this.rctOfficePhone.Stroke = new SolidColorBrush(Color.FromRgb(229, 229, 229));
            this.rctAreaCode.Stroke = new SolidColorBrush(Color.FromRgb(229, 229, 229));
            this.stkTelephoneValidate.Visibility = Visibility.Collapsed;
            this.txtOfficePhoneValidate.Visibility = Visibility.Hidden;
            this.txtAreaCodeValidate.Visibility = Visibility.Hidden;
        }

        private void TrackOperation_Event(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            switch(fe.Name)
            {
                case "txtNameEdit":
                    Common.TrackHelper2.TrackOperation("5.7.2.4.1", "clk");
                    break;
                case "txtPosition":
                    Common.TrackHelper2.TrackOperation("5.7.2.5.1", "clk");
                    break;
                case "txtEmail":
                    Common.TrackHelper2.TrackOperation("5.7.2.6.1", "clk");
                    break;
            }
        }
    }
}
