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

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// WinLoginInDetail.xaml 的交互逻辑
    /// </summary>
    public partial class WinLoginInDetail : Window
    {
        private WinLoginInDetail()
        {
            InitializeComponent();
        }
        private System.Timers.Timer timeAlive;
        private double interval = 10 * 1000;

        public WinLoginInDetail(int remainDays, string account)
        {
            InitializeComponent();
            //窗口位置
            Rect WorkingAera = SystemParameters.WorkArea;
            this.Top = WorkingAera.Height - 70;
            this.Left = WorkingAera.Width - 331;
            this.tbCount.Text = account;
            this.tbCount2.Text = account;
            if (remainDays == 5)
            {
                this.stk1.Visibility = Visibility.Collapsed;
                this.stk2.Visibility = Visibility.Visible;
            }
            else
            {
                this.stk1.Visibility = Visibility.Visible;
                this.stk2.Visibility = Visibility.Collapsed;
                this.tbDay.Text = remainDays.ToString();
                
            }

            //窗口定时关闭
            timeAlive = new System.Timers.Timer(this.interval);
            timeAlive.Elapsed += timeAlive_Elapsed;
            timeAlive.Enabled = true;
        }

        private void timeAlive_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Close();
            }));
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
