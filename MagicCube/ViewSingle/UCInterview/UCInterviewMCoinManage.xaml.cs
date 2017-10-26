using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// UCInterviewMCoinManage.xaml 的交互逻辑
    /// </summary>
    public partial class UCInterviewMCoinManage : UserControl
    {
        public delegate Task delegateReturnHomeAction();
        public delegateReturnHomeAction ReturnHomeAction;
        public UCInterviewMCoinManage()
        {
            InitializeComponent();
        }

        public async Task InitialMCoinManage()
        {
            this.RBMCoinDetail.IsChecked = true;
            this.ucMCoinDetail.InitialAmount();
            await this.ucMCoinDetail.InitailCoinDetail();
            
        }

        private async void BtnReturnHome_Click(object sender, RoutedEventArgs e)
        {
            if(ReturnHomeAction != null)
            {
              await  ReturnHomeAction();
            }
        }

        private async void RBMCoinDetail_Click(object sender, RoutedEventArgs e)
        {
            this.ucMCoinDetail.InitialAmount();
            await this.ucMCoinDetail.InitailCoinDetail();
            
        }

    }
}
