using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// WinPhoneHint.xaml 的交互逻辑
    /// </summary>
    public partial class WinJobTemplate : Window
    {
        public WinJobTemplate(Window ow)
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
            if (tbTempName.Text.Length < 2 || tbTempName.Text.Length > 20)
            {
                tbError.Visibility = Visibility.Visible;
                bd.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f25751"));
                return;
            }
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

        private void tbTempName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (tbTempName.Text.Length > 1 && tbTempName.Text.Length < 21)
            {
                tbError.Visibility = Visibility.Collapsed;
                bd.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ebebeb"));
                return;
            }
        }
    }
}
