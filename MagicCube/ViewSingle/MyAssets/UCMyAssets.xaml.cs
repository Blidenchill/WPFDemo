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
    /// UCMyAssets.xaml 的交互逻辑
    /// </summary>
    public partial class UCMyAssets : UserControl
    {
        public UCMyAssets()
        {
            InitializeComponent();
        }

        public void InitialMyAssets()
        {
            this.rdoDetail.IsChecked = true;
            this.ucAssetsDetail.InitialUCAssetDetail();

        }

        private void scollDetail_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            
        }

        private void scollDetail_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (scollDetail.VerticalOffset == 0)
                return;
            if (scollDetail.VerticalOffset == scollDetail.ScrollableHeight)
            {
                Messaging.Messenger.Default.Send<object, ViewModel.AssetsDetailViewModel>(null);
            }
        }
    }
}
