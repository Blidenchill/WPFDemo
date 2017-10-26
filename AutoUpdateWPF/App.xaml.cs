using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MFUpdater;
using System.Reflection;

namespace AutoUpdateWPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //if (!AutoUpdater.Instance.ServerVersionCompare())
            //{
            //    Application.Current.Shutdown();
            //}
            VersionInfo info = AutoUpdater.Instance.GetUpdateInfo();
            
        }
    }
}
