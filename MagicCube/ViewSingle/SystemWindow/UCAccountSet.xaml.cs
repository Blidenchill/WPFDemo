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
using MagicCube.ViewModel;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCAccountSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCAccountSet : UserControl
    {
        private SysAccontSetModel ViewModel
        {
            get { return DataContext as SysAccontSetModel; }
        }
        public UCAccountSet()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LinkButton_Click(object sender, RoutedEventArgs e)
        {
           TemplateUC.WinConfirmTip tip = new TemplateUC.WinConfirmTip("是否确定切换账号?");
            tip.Owner = Window.GetWindow(this);
            if (!(bool)tip.ShowDialog())
                return;
            else
            {
                ViewModel.ChangeAccoutCommand.Execute(null);
            }

        }
    }
}
