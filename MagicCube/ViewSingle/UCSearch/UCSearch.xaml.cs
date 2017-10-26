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

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCSearch.xaml 的交互逻辑
    /// </summary>
    public partial class UCSearch : UserControl
    {
        public UCSearch()
        {
            InitializeComponent();
        }

        private async void rbSearch_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.1.2.1", "clk");
            await this.ucResumeCollection.InitalCollection();
        }

        private async void rbBuy_Click(object sender, RoutedEventArgs e)
        {
            await ucResumeBuy.IniTable();
        }
    }
}
