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

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCDownloadSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCDownloadSet : UserControl
    {
        public UCDownloadSet()
        {
            InitializeComponent();
        }
        public void InitialDownload()
        {
            FreeSpace = GetFreeSpace(MagicGlobal.UserInfo.DefaultDownloadPath);
            this.ucDownload.DataContext = MagicGlobal.UserInfo;
            RememberPassword.IsChecked = MagicGlobal.UserInfo.IsDefaultDownloadPath;
            tbPath.Text = MagicGlobal.UserInfo.DefaultDownloadPath;
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
            DependencyProperty.Register("FreeSpace", typeof(string), typeof(UCDownloadSet), new PropertyMetadata(null));

        private void ScanDialog_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.9.3.4.1", "clk");

            System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();
            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbPath.Text = MagicGlobal.UserInfo.DefaultDownloadPath = fb.SelectedPath;
            }
        }

        private void RememberPassword_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.9.3.5.1", "clk");
            if (RememberPassword.IsChecked == true)
            {
                MagicGlobal.UserInfo.IsDefaultDownloadPath = true;
            }
            else
            {
                MagicGlobal.UserInfo.IsDefaultDownloadPath = false;
            }
        }
    }
}
