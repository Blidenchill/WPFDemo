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

namespace MagicCube.Index
{
    /// <summary>
    /// UpdateTip.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateTip : Window
    {
        private string url = string.Empty;
        public static bool OKChoose = false;
        public UpdateTip(string url)
        {
            InitializeComponent();
            this.url = url;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(url);
            OKChoose = true;
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
    }
}
