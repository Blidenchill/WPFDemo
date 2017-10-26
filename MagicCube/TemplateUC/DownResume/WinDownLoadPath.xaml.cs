using MagicCube.Common;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// WinPhoneHint.xaml 的交互逻辑
    /// </summary>
    public partial class WinDownLoadPath : Window
    {
        public WinDownLoadPath(Window ow,string type,string name)
        {
            InitializeComponent();
            TrackHelper2.TrackOperation("5.5.8.1.1", "pv");
            if (ow.ActualWidth == SystemParameters.WorkArea.Width && ow.ActualHeight == SystemParameters.WorkArea.Height)
            {
                this.Width = ow.ActualWidth;
                this.Height = ow.ActualHeight;
                this.Left = 0;
                this.Top = 0;
            }
            else
            {
                this.Width = ow.ActualWidth - 20;
                this.Height = ow.ActualHeight - 20;
                this.Left = ow.Left + 10;
                this.Top = ow.Top + 10;
            }
            this.Owner = ow;
            if (!string.IsNullOrWhiteSpace(MagicGlobal.UserInfo.DefaultDownloadPath))
            {
                tbDownloadPath.Text = MagicGlobal.UserInfo.DefaultDownloadPath;
            }
            if(!string.IsNullOrWhiteSpace(tbDownloadPath.Text))
            {
                FreeSpace = GetFreeSpace(tbDownloadPath.Text);
            }
            if (type == "word")
            {
                imgTypeWORD.Visibility = Visibility.Visible;
  
            }
            if (type == "pdf")
            {
                imgTypePDF.Visibility = Visibility.Visible;
            }
            if (type == "html")
            {
                imgTypeHTML.Visibility = Visibility.Visible;
            }
            tbName.Text = name;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.8.6.1", "clk");
            this.DialogResult = false;
            this.Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.8.4.1", "clk");
            if (string.IsNullOrWhiteSpace(tbDownloadPath.Text))
            {
                tbError.Visibility = Visibility.Visible;
                tbError.Text = "请选择保存路径";
                return;
            }
            if (File.Exists(tbDownloadPath.Text.Trim()))
            {
                tbError.Visibility = Visibility.Visible;
                tbError.Text = "保存路径不存在";
                return;
            }
            if(!Common.DownloadResume.IsEnough(tbDownloadPath.Text))
            {
                tbError.Visibility = Visibility.Visible;
                tbError.Text = "剩余空间不足，请重新选择保存路径";
                return;
            }

            if(IsDefultDownload.IsChecked == true)
            {
                MagicGlobal.UserInfo.IsDefaultDownloadPath = true;
                MagicGlobal.UserInfo.DefaultDownloadPath = tbDownloadPath.Text.Trim();
            }

            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.8.5.1", "clk");
            this.DialogResult = false;
            this.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driveDirectoryName"></param>
        /// <returns>剩余容量，单位GB</returns>
        private string GetFreeSpace(string driveDirectoryName)
        {

            DriveInfo drive = new DriveInfo(driveDirectoryName);

            double test = ((double)drive.AvailableFreeSpace) / 1024 / 1024 / 1024;
            return "磁盘剩余空间：" + test.ToString("f2") + "GB";
        }




        public string FreeSpace
        {
            get { return (string)GetValue(FreeSpaceProperty); }
            set { SetValue(FreeSpaceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FreeSpace.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FreeSpaceProperty =
            DependencyProperty.Register("FreeSpace", typeof(string), typeof(WinDownLoadPath), new PropertyMetadata(null));

        private void ScanDialog_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.8.2.1", "clk");
            System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();
            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbDownloadPath.Text = fb.SelectedPath;
                GetFreeSpace(tbDownloadPath.Text);
                tbError.Visibility = Visibility.Collapsed;
            }
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void IsDefultDownload_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.8.3.1", "clk");
        }
    }
}
