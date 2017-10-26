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
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;
using System.Threading;

namespace InstallPackageWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private VersionInfo versionInfo = new VersionInfo();
        private OpenFileDialog openFile = new OpenFileDialog();
        private SaveFileDialog saveFile = new SaveFileDialog();
        private string versionInfoName = "version.txt";
        string tempVersionPath = "";
        public MainWindow()
        {
            InitializeComponent();
            this.grdMain.DataContext = versionInfo;
            this.grdFileInfo.ItemsSource = versionInfo.UpdateFileList;
            openFile.Filter = "所有文件|*.*";
            openFile.Multiselect = true;
            saveFile.Filter = "zip |*.zip";
            tempVersionPath = System.IO.Path.GetTempPath() + "\\" + versionInfoName;
        }
        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            if(openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach(var item in openFile.FileNames)
                {
                    this.versionInfo.UpdateFileList.Add(new VersionFileInfo() { FileName = System.IO.Path.GetFileName(item), FileSize = FileSize(item), RelativePath = System.IO.Path.GetFileName(item), LocalRealPath=item});
                    this.versionInfo.TotalFileSize += FileSize(item);
                }
            }
        }
        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            this.versionInfo.UpdateFileList.RemoveAt(this.grdFileInfo.SelectedIndex);
            long size = 0;
            foreach (var item in versionInfo.UpdateFileList)
            {
                size += item.FileSize;
            }
            versionInfo.TotalFileSize = size;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtSavePath.Text = saveFile.FileName;
            }
        }

        private void Running_Click(object sender, RoutedEventArgs e)
        {
             
            try
            {
                string folder = System.IO.Path.GetDirectoryName(this.txtSavePath.Text);
                if (!System.IO.Directory.Exists(folder))
                {
                    System.Windows.MessageBox.Show("保存路径错误！", "错误信息");
                    return;
                }
                if(versionInfo.VersionName == null || versionInfo.VersionName == string.Empty)
                {
                    System.Windows.MessageBox.Show("请输入版本号", "错误信息");
                    return;
                }
                if(versionInfo.Description == null || versionInfo.Description == string.Empty)
                {
                    System.Windows.MessageBox.Show("请输入更新日志", "错误信息");
                    return;
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("保存路径错误！", "错误信息");
                return;
            }
            this.RunState(true);
            if (this.cmbIsForceUpdate.SelectedIndex == 0)
                this.versionInfo.IsForceUpdate = true;
            else
                this.versionInfo.IsForceUpdate = false;
            if (this.cmbUpdateStyle.SelectedIndex == 0)
                this.versionInfo.IsForceDownLoadSetupPackage = true;
            else
                this.versionInfo.IsForceDownLoadSetupPackage = false;
            this.CreateVersinInfoTxt(this.versionInfo);
            ThreadPool.QueueUserWorkItem(this.RunningZip, this.txtSavePath.Text);
            
        }

        private long FileSize(string path)
        {
            FileInfo info = new FileInfo(path);
            return info.Length;
        }
        private void CreateVersinInfoTxt(VersionInfo info)
        {
            using (FileStream fs = new FileStream(tempVersionPath, FileMode.Create, FileAccess.Write))
            {
                using(StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(this.versionInfo.VersionName + "\r\n");
                    sw.Write(this.versionInfo.IsForceUpdate.ToString() + "\r\n");
                    sw.Write(this.versionInfo.IsForceDownLoadSetupPackage.ToString() + "\r\n");
                    sw.Write(this.versionInfo.Description + "\r\n");
                    sw.Write(this.versionInfo.TotalFileSize.ToString() +"\r\n");
                    foreach(var item in this.versionInfo.UpdateFileList)
                    {
                        sw.Write(item.FileName + "," + item.RelativePath + "," + item.FileOperateType.ToString() + "," + item.FileSize.ToString() + "\r\n");
                    }
                }
            }
        }

        private void RunningZip(object SavePath)
        {
            ZipFile zip = new ZipFile();
            foreach (var item in this.versionInfo.UpdateFileList)
            {
                zip.AddFile(item.LocalRealPath, "");
            }
            zip.AddFile(tempVersionPath, "");
            zip.Save(SavePath.ToString());
            //FileInfo info = new FileInfo(saveFile.ToString());
            DirectoryInfo info = Directory.GetParent(saveFile.FileName);

            File.Copy(tempVersionPath, info +  "\\" + versionInfoName, true);
            File.Delete(tempVersionPath);
            this.Dispatcher.Invoke(new Action(()=>RunState(false)));
            System.Windows.MessageBox.Show("安装包制作完成！", "提示");
        }
        private void RunState(bool state)
        {
            if(state)
                this.progressBar.Visibility = Visibility.Visible;
            else
                this.progressBar.Visibility = Visibility.Hidden;
            this.btnRun.IsEnabled = !state;
            this.txtSavePath.IsEnabled = !state;

        }
    }
}
