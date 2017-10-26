using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// WinLoginInDetail.xaml 的交互逻辑
    /// </summary>
    public partial class WinMBNotice : Window
    {
        DispatcherTimer timeClose;
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);
        public WinMBNotice(string notice)
        {
            InitializeComponent();
            //窗口位置
            Rect WorkingAera = SystemParameters.WorkArea;
            this.Top = WorkingAera.Height - 70;
            this.Left = WorkingAera.Width - 331;
            this.tbCount.Text = notice;

            //窗口定时关闭
            timeClose = new DispatcherTimer();
            timeClose.Interval = new TimeSpan(0, 0, 5);   //间隔5秒
            timeClose.Tick += new EventHandler(Close_Tick);
            timeClose.Start();
            this.Loaded += WinMBNotice_Loaded;
        }

        private void WinMBNotice_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            IntPtr HWND = wndHelper.Handle;
            int GWL_EXSTYLE = -20;

            //GetWindowLong(HWND, GWL_EXSTYLE);

            SetWindowLong(HWND, GWL_EXSTYLE, (IntPtr)(0x8000000)); //让当前窗体不获取输
        }

        private void Close_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
