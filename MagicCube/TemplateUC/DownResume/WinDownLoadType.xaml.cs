using System.Windows;
using System.Windows.Input;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// WinPhoneHint.xaml 的交互逻辑
    /// </summary>
    public partial class WinDownLoadType : Window
    {
        public WinDownLoadType(Window ow)
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
            MagicCube.Common.TrackHelper2.TrackOperation("5.5.7.1.1", "pv");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            MagicCube.Common.TrackHelper2.TrackOperation("5.5.7.7.1", "clk");
            this.DialogResult = false;
            this.Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            MagicCube.Common.TrackHelper2.TrackOperation("5.5.7.5.1", "clk");
            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MagicCube.Common.TrackHelper2.TrackOperation("5.5.7.6.1", "clk");
            this.DialogResult = false;
            this.Close();
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Track_Event(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            switch(fe.Name)
            {
                case "typeWord":
                    MagicCube.Common.TrackHelper2.TrackOperation("5.5.7.2.1", "clk");
                    break;
                case "typePDF":
                    MagicCube.Common.TrackHelper2.TrackOperation("5.5.7.3.1", "clk");
                    break;
                case "typeHtml":
                    MagicCube.Common.TrackHelper2.TrackOperation("5.5.7.4.1", "clk");
                    break;
            }
        }
    }
}
