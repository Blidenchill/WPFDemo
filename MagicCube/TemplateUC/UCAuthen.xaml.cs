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
using MagicCube.Common;
using MagicCube.ViewModel;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCAuthen.xaml 的交互逻辑
    /// </summary>
    public partial class UCAuthen : UserControl
    {
        public UCAuthen()
        {
            InitializeComponent();
            ucPictureUpload.OKAction += PictureUploadOKCallback;
            ucPictureUpload.CancelAction += PictureCancelCallback;
        }

        public Action UploadAuthOKAction;
        public Action GotoJobAction;


        public string AuthPicture
        {
            get { return (string)GetValue(AuthPictureProperty); }
            set { SetValue(AuthPictureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AuthPicture.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthPictureProperty =
            DependencyProperty.Register("AuthPicture", typeof(string), typeof(UCAuthen), new PropertyMetadata(null));



        public Visibility GoToJobIsVisible
        {
            get { return (Visibility)GetValue(GoToJobIsVisibleProperty); }
            set { SetValue(GoToJobIsVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GoToJobIsVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoToJobIsVisibleProperty =
            DependencyProperty.Register("GoToJobIsVisible", typeof(Visibility), typeof(UCAuthen), new PropertyMetadata(null));



        public string FailReason
        {
            get { return (string)GetValue(FailReasonProperty); }
            set { SetValue(FailReasonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FailReason.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FailReasonProperty =
            DependencyProperty.Register("FailReason", typeof(string), typeof(UCAuthen), new PropertyMetadata(null));



        public string CompanyName
        {
            get { return (string)GetValue(CompanyNameProperty); }
            set { SetValue(CompanyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CompanyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompanyNameProperty =
            DependencyProperty.Register("CompanyName", typeof(string), typeof(UCAuthen), new PropertyMetadata(null));





        private async void btnAddAuthenPicture_Click(object sender, RoutedEventArgs e)
        {
            //TrackHelper2.TrackOperation("5.1.8.2.1", "clk");
            //System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            //openFile.Filter = "(*.jpg,*.png)|*.jpg;*.png";
            //if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    this.ucPictureUpload.GetImagePath(openFile.FileName);
            //    this.ucPictureUpload.Visibility = Visibility.Visible;
            //}

            //改为直接上传图片，不裁剪
            string picturePath = string.Empty;
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Filter = "(*.jpg,*.png)|*.jpg;*.png";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                picturePath = openFile.FileName;
                string tempPath = DAL.ConfUtil.LocalHomePath + "/" + openFile.SafeFileName;
                bool compressFlag = ImageCompress.CompressTo1M(picturePath, tempPath);
                if (compressFlag)
                {
                    picturePath = tempPath;
                }
                busyCtrl.IsBusy = true;



                using (FileStream fs = new FileStream(picturePath, FileMode.Open, FileAccess.Read))
                {
                    string std = DAL.JsonHelper.JsonParamsToString(new string[] { "appKey", "fileName" }, new string[] { "head", "head" });
                    string resultStr = await DAL.HttpHelper.Instance.HttpUploadFile(string.Format(DAL.ConfUtil.AddrUploadFile, MagicGlobal.UserInfo.Version, std), fs, "head");
                    busyCtrl.IsBusy = false;
                    BaseHttpModel<HttpUploadFileModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpUploadFileModel>>(resultStr);
                    if (model == null)
                    {
                        MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                        winLink.Owner = Window.GetWindow(this);
                        winLink.ShowDialog();
                        return;
                    }
                    else
                    {
                        if (model.code == 200)
                        {
                            AuthPicture = model.data.url;
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
                }
            }


        }

        private async void PictureUploadOKCallback(BitmapSource bs)
        {
            this.busyCtrl.IsBusy = true;
            this.ucPictureUpload.Visibility = Visibility.Collapsed;
            this.gdAuthMain.Visibility = Visibility.Visible;
            MemoryStream stream = ImageProcessor.SaveImageToStream(bs);

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "appKey", "fileName" }, new string[] { "head", "head" });
            string resultStr = await DAL.HttpHelper.Instance.HttpUploadFile(string.Format(DAL.ConfUtil.AddrUploadFile, MagicGlobal.UserInfo.Version, std), stream, "head");
            this.busyCtrl.IsBusy = false;
            BaseHttpModel<HttpUploadFileModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpUploadFileModel>>(resultStr);
            if (model == null)
            {
                MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(DAL.ConfUtil.netError, "我知道了");
                winLink.Owner = Window.GetWindow(this);
                winLink.ShowDialog();
                return;
            }
            else
            {
                if (model.code == 200)
                {
                    AuthPicture = model.data.url + "?w=140&h=140";
                }
                else
                {
                    MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink(model.msg, "我知道了");
                    winLink.Owner = Window.GetWindow(this);
                    winLink.ShowDialog();
                    return;
                }
            }



        }
        private void PictureCancelCallback()
        {
            this.ucPictureUpload.Visibility = Visibility.Collapsed;
            this.gdAuthMain.Visibility = Visibility.Visible;
        }

        private async void btnUpLoadOK_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.8.3.1", "clk");
            if(string.IsNullOrEmpty(this.AuthPicture))
            {
                MagicCube.View.Message.DisappearShow ps = new View.Message.DisappearShow("请上传认证照片", 1);
                ps.Owner = Window.GetWindow(this);
                ps.Show();
                return;
            }
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "companyID", "imageUrl" },
                new string[] { MagicGlobal.UserInfo.Id.ToString(), MagicGlobal.UserInfo.CompanyId.ToString(), this.AuthPicture });
            string url = string.Format(DAL.ConfUtil.AddrUploadAuthen, MagicGlobal.UserInfo.Version, std);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<BaseHttpModel>(resultStr);
            if(model == null)
            {
                MagicCube.TemplateUC.WinMessageLink winLink = new TemplateUC.WinMessageLink("网络异常，请重试", "我知道了");
                winLink.Owner = Window.GetWindow(this);
                winLink.ShowDialog();
                return;
            }
            else
            {
                if(model.code == 200)
                {
                    if (this.UploadAuthOKAction != null)
                        this.UploadAuthOKAction();
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
        }

        private void btnGotoJob_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.1.8.4.1", "clk");
            if (this.GotoJobAction != null)
                this.GotoJobAction();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">0:待审核，1：认证通过，-1认证未通过，9：上传状态</param>
        public void SetPanel(int code)
        {
            switch(code.ToString())
            {
                //待审核
                case "0":
                    this.gdAuthCheck.Visibility = Visibility.Visible;
                    this.gdAuthMain.Visibility = Visibility.Collapsed;
                    this.gdAuthSuccess.Visibility = Visibility.Collapsed;
                    break;
                    //认证通过
                case "1":
                    this.gdAuthSuccess.Visibility = Visibility.Visible;
                    this.gdAuthMain.Visibility = Visibility.Collapsed;
                    this.gdAuthCheck.Visibility = Visibility.Collapsed;
                    break;
                    //认证未通过
                case "-1":
                    this.gdAuthSuccess.Visibility = Visibility.Collapsed;
                    this.gdAuthMain.Visibility = Visibility.Visible;
                    this.stkFail.Visibility = Visibility.Visible;
                    this.tbFourPic.Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66));
                    this.gdAuthCheck.Visibility = Visibility.Collapsed;
                    break;
                    //上传状态
                case "9":
                    this.gdAuthSuccess.Visibility = Visibility.Collapsed;
                    this.gdAuthMain.Visibility = Visibility.Visible;
                    this.stkFail.Visibility = Visibility.Collapsed;
                    this.tbFourPic.Foreground = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                    this.gdAuthCheck.Visibility = Visibility.Collapsed;
                    break;

            }
        }

        private void btnSeeExample1_Click(object sender, RoutedEventArgs e)
        {
         
            //this.popExample1.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
            this.popExample1.PlacementTarget = sender as Button;
            this.popExample1.StaysOpen = false;
            this.popExample1.IsOpen = true;
        }

        private void btnSeeExample2_Click(object sender, RoutedEventArgs e)
        {
            //this.popExample2.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
            this.popExample2.PlacementTarget = sender as Button;
            this.popExample2.StaysOpen = false;
            this.popExample2.IsOpen = true;
        }

        private void btnSeeExample3_Click(object sender, RoutedEventArgs e)
        {
            //this.popExample3.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
            this.popExample3.PlacementTarget = sender as Button;
            this.popExample3.StaysOpen = false;
            this.popExample3.IsOpen = true;
        }

        private void btnSeeExample4_Click(object sender, RoutedEventArgs e)
        {
            //this.popExample4.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
            this.popExample4.PlacementTarget = sender as Button;
            this.popExample4.StaysOpen = false;
            this.popExample4.IsOpen = true;
        }
    }
}
