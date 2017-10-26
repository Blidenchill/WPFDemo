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
    /// WinActiveTip.xaml 的交互逻辑
    /// </summary>
    public partial class WinUnArriveConfirm : Window
    {
        bool mIsSingle;
        public WinUnArriveConfirm(double mb = 0,bool isSingle = true)
        {
            InitializeComponent();
            tbMB.Text = Math.Round(mb, 2).ToString();
            mIsSingle = isSingle;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (mIsSingle)
            {
                Common.TrackHelper2.TrackOperation("5.4.4.1.1", "clk");
            }
            else
            {
                Common.TrackHelper2.TrackOperation("5.4.10.1.1", "clk");
            }
            this.DialogResult = true;
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (mIsSingle)
            {
                Common.TrackHelper2.TrackOperation("5.4.4.3.1", "clk");
            }
            else
            {
                Common.TrackHelper2.TrackOperation("5.4.10.3.1", "clk");
            }
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
            if (mIsSingle)
            {
                Common.TrackHelper2.TrackOperation("5.4.4.2.1", "clk");
            }
            else
            {
                Common.TrackHelper2.TrackOperation("5.4.10.2.1", "clk");
            }
            this.DialogResult = false;
            this.Close();
        }
    }
}
