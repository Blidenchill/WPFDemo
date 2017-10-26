using MagicCube.Common;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// WinLoginInDetail.xaml 的交互逻辑
    /// </summary>
    public partial class WinNewIM : Window
    {
        DispatcherTimer timeClose;
        public delegate Task delegateChat();
        public delegateChat actionChat;
        public Action actionNomore;

        public WinNewIM(string count)
        {
            InitializeComponent();
            tbCount.Text = count;
            
            TrackHelper2.TrackOperation("5.10.2.1.1", "pv");
            //窗口位置
            Rect WorkingAera = SystemParameters.WorkArea;
            this.Top = WorkingAera.Height - 87;
            this.Left = WorkingAera.Width - 331;
            timeClose = new DispatcherTimer();
            timeClose.Interval = new TimeSpan(0, 0, 5);   //间隔5秒
            timeClose.Tick += new EventHandler(Close_Tick);
            timeClose.Start();
        }


        private void Close_Tick(object sender, EventArgs e)
        {
            this.Close();        
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.10.2.3.1", "clk");
            this.Close();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
           
            timeClose.Stop();
        }

        private async void Reply_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.10.2.7.1", "clk");
            if (actionChat != null)
                await actionChat();
            this.Close();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.10.2.4.1", "clk");
            PopMenu.IsOpen = true;
            PopMenu.PlacementTarget = btnSetting;
        }

        private void Today_MenuClick(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.10.2.6.1", "clk");
            PopMenu.IsOpen = false;
            MagicGlobal.UserInfo.MessageNoticeTime = DateTime.Now.ToString("yyyy-MM-dd");
            this.Close();
        }

        private void Ignore_MenuClick(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.10.2.5.1", "clk");
            PopMenu.IsOpen = false;
            if (actionNomore != null)
                actionNomore();
            this.Close();
        }
    }
}
