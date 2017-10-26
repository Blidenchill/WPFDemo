using MagicCube.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace MagicCube.ViewSingle
{
    public partial class MainWindow : Window
    {

        private System.Windows.Forms.NotifyIcon notifyIcon;

        private bool bNotifyFlack = true;

        private bool bNewMessage = false;

        //private WinNewMsg  mWinNewMsg = new WinNewMsg();

        DispatcherTimer icoTimer;


        //private NotifyIconMouseHelper notifyHelper;
        //初始化小任务栏
        private void IniNotifyIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "魔方小聘";

            notifyIcon.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "Icon.ico");
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            icoTimer = new DispatcherTimer();
            icoTimer.Interval = TimeSpan.FromSeconds(0.3);
            icoTimer.Tick += new EventHandler(IcoTimer_Tick);

            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("打开主页面");
            open.Click += new EventHandler(NotifyShow);

            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(Close);


            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] {open, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

        }

        private object notifyLock = new object();
        private async void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (bNewMessage) 
                {
                    bool isIMCur = (bool)RBIM.IsChecked;
                    if (true)
                    {
                        RBIM.IsChecked = true;
                        await ucIM.IniRecentChatList();
                        await ucIM.UCIMInitial();
                    }
                    //if (ucIM.m_RecentChatList.Count > 0)
                    //{
                    //    await ucIM.OpenIMControl(ucIM.m_RecentChatList[1].UserID.ToString());
                    //}
                }
                NotifyShow(null, null);

            }

        }


        private void NotifyShow(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
            this.WindowState = System.Windows.WindowState.Normal;
        }

        private void Close(object sender, EventArgs e)
        {
            DAL.HttpHelper.Instance.SaveCookie();
            MagicGlobal.SaveSeting();
            this.SaveInfo();
            Environment.Exit(0);
            System.Windows.Application.Current.Shutdown();

        }

        public void IcoTimer_Tick(object sender, EventArgs e)
        {

            if (bNotifyFlack)
            {
                this.notifyIcon.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "TransparentIcon.ico");
                bNotifyFlack = false;
            }
            else
            {
                this.notifyIcon.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "Icon.ico");
                bNotifyFlack = true;
            }
        }

        public void notifyFlack(bool isStart)
        {
            if (isStart)
            {
                bNewMessage = true;
                icoTimer.Start();

            }
            else
            {
                bNewMessage = false;
                icoTimer.Stop();
                this.notifyIcon.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "Icon.ico");
            }
        }
    }
}
