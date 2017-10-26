using System.Windows;
using System.Windows.Input;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// WinPhoneHint.xaml 的交互逻辑
    /// </summary>
    public partial class WinPhoneCall : Window
    {
        public WinPhoneCall(Window ow)
        {
            
            InitializeComponent();
            if (ow.ActualWidth == SystemParameters.WorkArea.Width && ow.ActualHeight == SystemParameters.WorkArea.Height)
            {
                this.Width = ow.ActualWidth;
                this.Height = ow.ActualHeight;
                this.Left = 0;
                this.Top = 0;
            }
            else
            {
                this.Width = ow.ActualWidth - 20;
                this.Height = ow.ActualHeight - 20;
                this.Left = ow.Left + 10;
                this.Top = ow.Top + 10;
            }
            this.Owner = ow;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
