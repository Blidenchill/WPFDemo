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
using MagicCube.Index;

using System.Threading;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Net;
using MagicCube.ViewSingle;
using MagicCube.DAL;
using System.Threading.Tasks;

namespace MagicCubeShell
{
    /// <summary>
    /// LoadingView.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingView : Window
    {
        public LoadingView()
        {
            InitializeComponent();
            //Thread thd = new Thread(this.InitialLoading);
            //thd.Start();
            TaskEx.Run(()=>
            {
                InitialLoading();
            });
            
        }
        public HttpUpdateInfo updateInfo = new HttpUpdateInfo();
        private static string pathVersion = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\mofanghr\updateVersion";
        private static string pathInfo = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\mofanghr\updateInfo";

        string UpdateVersionID = string.Empty;
        string LowingVersionID = string.Empty;
        bool IsForceUpdate;
        bool IsForceDownLoadSetupPackage;
        private void InitialLoading()
        {
            try
            {
                //打开登录界面
                   this.Dispatcher.Invoke(new Action(() =>
                   {
                       //MainWindow pMainWindow = new MainWindow();
                       //pMainWindow.Show();
                      
                       //IndexWindow p = new IndexWindow();
                       WinIndex p = new WinIndex();
                       p.Show();
                       this.Hide();
                       this.Close();
                   }
                ));
             

                //////////////////////////////////上线前需修改
                //在线获取更新信息
                string url = @"https://desktop-application.mofanghr.com/version.json?version=" + Assembly.GetEntryAssembly().GetName().Version.ToString();
                string temp =  HttpHelper.Instance.HttpGet(url);

                MagicCube.ViewModel.BaseHttpModel<HttpUpdateInfo> model = MagicCube.DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<HttpUpdateInfo>>(temp);
                if (model == null)
                    return;
                else
                {
                    if(model.code == 200)
                    {
                        updateInfo = model.data;
                        Stream stream = GetResponseStream(updateInfo.versionUrl);
                        if (stream != null)
                        {
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                try
                                {
                                    UpdateVersionID = sr.ReadLine();
                                    IsForceUpdate = Convert.ToBoolean(sr.ReadLine());
                                    IsForceDownLoadSetupPackage = Convert.ToBoolean(sr.ReadLine());
                                    LowingVersionID = sr.ReadLine();
                                }
                                catch { }
                            }
                        }
                    }
                }
                

                //判断是否为最新三个版本
                if(!updateInfo.latestThreeVersion)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Hide();
                        UpdateTip tip = new UpdateTip(updateInfo.downloadUrl);
                        tip.ShowDialog();
                        this.Dispatcher.Invoke(new Action(() => this.Close()));
                        System.Windows.Application.Current.Shutdown();
                        return;
                    }));
                    return;
                }

                //判断版本号
                Version version = Assembly.GetEntryAssembly().GetName().Version;
                int major = version.Major;
                int minor = version.Minor;
                int builderNum = version.Build;
                int revision = version.Revision;
                string[] updateVersion = UpdateVersionID.Split(new char[] { '.' });
                string[] lowestVersion = LowingVersionID.Split(new char[] { '.' });

                long currentFlag = 0;
                long updateFlag = 0;
                long lowestFlag = 0;
                if (updateVersion.Length == 4)
                {
                     currentFlag = major * 1000 + minor * 100 + builderNum * 10 + revision;
                     updateFlag = Int32.Parse(updateVersion[0]) * 1000 + Int32.Parse(updateVersion[1]) * 100 + Int32.Parse(updateVersion[2]) * 10 + Int32.Parse(updateVersion[3]);
                     lowestFlag = Int32.Parse(lowestVersion[0]) * 1000 + Int32.Parse(lowestVersion[1]) * 100 + Int32.Parse(lowestVersion[2]) * 10 + Int32.Parse(lowestVersion[3]);
                    if (currentFlag >= updateFlag)
                        return;
                }
                else
                    return;

                //如果低于等于最低版本，直接安装包下载更新
               if(currentFlag <= lowestFlag)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Hide();
                        UpdateTip tip = new UpdateTip(updateInfo.downloadUrl);
                        tip.ShowDialog();
                        this.Dispatcher.Invoke(new Action(() => this.Close()));
                        System.Windows.Application.Current.Shutdown();
                        return;
                    }));
                    return;
                }

                if (IsForceDownLoadSetupPackage)
                {
                    //下载安装包升级
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Hide();
                        UpdateTip tip = new UpdateTip(updateInfo.downloadUrl);
                        tip.ShowDialog();
                        if (UpdateTip.OKChoose)
                        {
                            this.Dispatcher.Invoke(new Action(() => this.Close()));
                            System.Windows.Application.Current.Shutdown();
                            return;
                        }
                        if (IsForceUpdate)
                        {
                            this.Dispatcher.Invoke(new Action(() => this.Close()));
                            System.Windows.Application.Current.Shutdown();
                            return;
                        }
                    }));
                   
                }
                else
                {
                    try
                    {
                        //zip包自动升级
                        SaveFile(updateInfo.versionUrl);
                        MagicCube.Common.XmlClassData.WriteDataToXml<HttpUpdateInfo>(updateInfo, pathInfo);
                        this.Dispatcher.Invoke(new Action(() => this.Hide()));
                        System.Diagnostics.Process pro = System.Diagnostics.Process.Start("AutoUpdateWPF.exe");
                        //pro.WaitForExit();
                        Thread.Sleep(2000);
                        if (IsForceUpdate)
                        {
                            Environment.Exit(0);
                        }

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        
                        if (IsForceUpdate)
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }
            finally
            {
            }
            
           
          
        }

        private static Stream GetResponseStream(string picUrl)
        {
            WebResponse response = null;
            Stream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
                request.Timeout = 10000;
                response = request.GetResponse();
                stream = response.GetResponseStream();
            }
            catch
            {
            }
            return stream; ;
        }

        private static void SaveFile(string picUrl)
        {
            WebResponse response = null;
            Stream stream = null;
            try
            {
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
                request.Timeout = 10000;
                response = request.GetResponse();
                stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                FileStream fs = new FileStream(pathVersion, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(sr.ReadToEnd());
                sw.Flush();
                sw.Close();
                sr.Close();
                fs.Close();
            }
            catch { }
        }

       


        private static string GetConfigValue(string appKey)
        {
            string appValue = ConfigurationManager.AppSettings[appKey];
            return appValue;
        }

    }
}
