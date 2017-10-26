 using MagicCube.Index;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Reflection;
using System.Net;
using System.IO;
using System.Text;
using MagicCube.Common;

using System.Threading;
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace MagicCubeShell
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public HttpUpdateInfo updateInfo = new HttpUpdateInfo();
        protected override void OnStartup(StartupEventArgs e)
        {
            //LoadingView loadingView = new LoadingView();
            // Action method = new Action(() =>
            //     this.Dispatcher.Invoke(new Action(() =>
            //loadingView.Show())));
            // method.BeginInvoke(null,null);
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;

            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            base.OnStartup(e);
            LogHelper.SetConfig();
            ////获取更新信息
            //string temp = HttpHelper.HttpGet(ConfUtil.ServerVersionGet);
            //string UpdateVersionID = string.Empty;
            //if (!temp.StartsWith("连接失败"))
            //{
            //    updateInfo = HttpHelper.JSON.parse<HttpUpdateInfo>(temp);
            //    Stream stream = GetResponseStream(updateInfo.latestVersion);
            //    //Stream stream = GetResponseStream("http://192.168.0.162:3100/VersionInfo.txt");
            //    if (stream != null)
            //    {
            //        using (StreamReader sr = new StreamReader(stream))
            //        {
            //            UpdateVersionID = sr.ReadLine();
            //        }
            //    }

            //}
            //Version version = Assembly.GetEntryAssembly().GetName().Version;
            //int major = version.Major;
            //int minor = version.Minor;
            //int builderNum = version.Build;
            //int revision = version.Revision;
            //bool IsForceUpdate = Convert.ToBoolean(GetConfigValue("IsForceUpdate"));

            //string[] updateVersion = UpdateVersionID.Split(new char[] { '.' });
            //if (updateVersion.Length == 4)
            //{
            //    if (major < Int32.Parse(updateVersion[0]))
            //    {
            //        UpdateTip tip = new UpdateTip(updateInfo.downloadUrl);
            //        tip.Show();

            //        if (IsForceUpdate)
            //        {
            //            System.Windows.Application.Current.Shutdown();
            //            return;
            //        }

            //    }
            //}

            //string currentVersionID = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //if (UpdateVersionID != currentVersionID && UpdateVersionID != string.Empty)
            //{
            //    if (Convert.ToBoolean(GetConfigValue("IsForceDownLoadSetupPackage")))
            //    {

            //        UpdateTip tip = new UpdateTip(updateInfo.downloadUrl);
            //        tip.Show();
            //        if (IsForceUpdate)
            //        {
            //            System.Windows.Application.Current.Shutdown();
            //            return;
            //        }
            //    }
            //    try
            //    {
            //        System.Diagnostics.Process pro = System.Diagnostics.Process.Start("AutoUpdateWPF.exe");
            //        pro.WaitForExit();
            //        if (IsForceUpdate)
            //        {
            //            System.Windows.Application.Current.Shutdown();
            //            return;
            //        }
            //    }
            //    catch { }

            //}

            //WinIndex p = new WinIndex();
            //p.Show();

            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).ToList().Count > 1)
            {
                Thread.Sleep(1500);
                if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).ToList().Count > 1)
                {
                    Process proc = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)[0];
                    Console.WriteLine("存在");

                    IntPtr hwnd = MessageHelper.FindWindow(null, "魔方小聘主窗口");
                    IntPtr hwnd2 = MessageHelper.FindWindow(null, "魔方小聘登录");
                    int t = MessageHelper.PostMessage(hwnd, MessageHelper.WM_DOWNLOAD_COMPLETED, IntPtr.Zero, IntPtr.Zero);
                    Console.WriteLine(t);
                    t = MessageHelper.PostMessage(hwnd2, MessageHelper.WM_DOWNLOAD_COMPLETED, IntPtr.Zero, IntPtr.Zero);
                    Console.WriteLine(t);
                    t = MessageHelper.PostMessage(proc.MainWindowHandle, MessageHelper.WM_DOWNLOAD_COMPLETED, IntPtr.Zero, IntPtr.Zero);
                    Console.WriteLine(t);
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                }
            }
            else
            {
                //Console.WriteLine("不存在");
            }

        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            
            LogHelper.WriteErrorLog(e.Exception);
            e.Handled = true;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            
            LogHelper.WriteLog(e.ExceptionObject.ToString());

        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            //新埋点2
            TrackHelper2.TrackOperation("WakeUpSuccess");
        }
    }


    /// <summary>
    /// 从服务器获取更新下载信息
    /// </summary>
    public class HttpUpdateInfo
    {
        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool forcedUpgrade { get; set; }
        /// <summary>
        /// 安装包下载url 
        /// </summary>
        public string downloadUrl { get; set; }
        /// <summary>
        /// 最新版本号url地址
        /// </summary>
        public string versionUrl { get; set; }
        /// <summary>
        /// 是否需要更新
        /// </summary>
        public bool upgrade { get; set; }
        /// <summary>
        /// 更新zip压缩包下载路径
        /// </summary>
        public string updateUrl { get; set; }
        /// <summary>
        /// 最新版本号
        /// </summary>
        public string latestVersion { get; set; }
        /// <summary>
        /// 是否为最近三个版本（是：true，否：false）
        /// </summary>
        public bool latestThreeVersion { get; set; }
    }


}
