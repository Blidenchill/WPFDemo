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
    /// WinQrCode.xaml 的交互逻辑
    /// </summary>
    public partial class WinQrCode : Window
    {
        public WinQrCode()
        {
            InitializeComponent();
        }


        public string QrCoding
        {
            get { return (string)GetValue(QrCodingProperty); }
            set { SetValue(QrCodingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for QrCoding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QrCodingProperty =
            DependencyProperty.Register("QrCoding", typeof(string), typeof(WinQrCode), new PropertyMetadata(null));

        private void CloseButton_Click(object sender, RoutedEventArgs e)
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
    }
}
