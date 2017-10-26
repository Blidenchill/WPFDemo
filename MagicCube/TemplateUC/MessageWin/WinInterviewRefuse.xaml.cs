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
    /// WinInterviewRefuse.xaml 的交互逻辑
    /// </summary>
    public partial class WinInterviewRefuse : Window
    {
        public WinInterviewRefuse()
        {
            InitializeComponent();
        }
        public WinInterviewRefuse(double price)
        {
            InitializeComponent();
            string temp1 =Math.Round(price * 0.2,2).ToString();
            string temp2 = Math.Round(price, 2).ToString();
            this.tbTemp1.Text = string.Format("拒绝后将会短信通知对方，并扣除您{0}M币", temp1);
            this.tbTemp2.Text = string.Format("同时会返还您已冻结的{0}M币", temp2);

        }

        private void BtnRefuse_Click(object sender, RoutedEventArgs e)
        {

            Common.TrackHelper2.TrackOperation("5.4.7.1.1", "clk");

            this.DialogResult = true;
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.4.7.3.1", "clk");
            this.DialogResult = false;
            this.Close();
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // 获取鼠标相对标题栏位置  
            Point position = e.GetPosition((FrameworkElement)sender);


            // 如果鼠标位置在标题栏内，允许拖动  
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (position.X >= 0 && position.X < ((FrameworkElement)sender).ActualWidth && position.Y >= 0 &&
                    position.Y < ((FrameworkElement)sender).ActualHeight)
                {
                    this.DragMove();
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.4.7.2.1", "clk");
            this.DialogResult = false;
            this.Close();
        }
    }
}
