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
    /// UCIMSetting.xaml 的交互逻辑
    /// </summary>
    public partial class UCIMSetting : UserControl
    {
        public UCIMSetting()
        {
            InitializeComponent();
            this.gdMain.DataContext = MagicGlobal.UserInfo;
        }
        private void BOX_Checked(object sender, RoutedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.CheckBox).Name)
            {

                case "chkEnterSend":
                    //埋点
                    Common.TrackHelper2.TrackOperation("5.9.2.3.1", "clk");
                    MagicGlobal.UserInfo.EnterSendChoose = (bool)this.chkEnterSend.IsChecked;
                    break;
                case "chkCtrlEnterSend":
                    Common.TrackHelper2.TrackOperation("5.9.2.4.1", "clk");
                    MagicGlobal.UserInfo.CtrlEnterSendChoose = (bool)this.chkCtrlEnterSend.IsChecked;
                    break;
            }
        }
    }
}
