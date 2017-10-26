using MagicCube.HttpModel;

using MagicCube.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MagicCube.Model;
using System.Collections.ObjectModel;
using MagicCube.TemplateUC;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// ResumeView.xaml 的交互逻辑
    /// </summary>
    public partial class ResumeDetialNull : UserControl
    {
        public ResumeDetialNull()
        {
            InitializeComponent();
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {

        }   

        private void BtnPhone_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnVideo_Click(object sender, RoutedEventArgs e)
        {
            WinMessageLink pWinConfirmTip = new WinMessageLink("此功能即将上线，敬请期待！", "我知道了");
            pWinConfirmTip.Owner = Window.GetWindow(this);
            pWinConfirmTip.ShowDialog();
        }

    }
}
