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
using MagicCube.Common;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCBasicSetting.xaml 的交互逻辑
    /// </summary>
    public partial class UCBasicSetting : UserControl
    {
        public UCBasicSetting()
        {
            InitializeComponent();
            this.gdMain.DataContext = MagicGlobal.UserInfo;
        }

        #region "委托"
        #endregion

        #region "时间"
        private void BOX_Checked(object sender, RoutedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.CheckBox).Name)
            {
                case "RememberPassword":
                    if (RememberPassword.IsChecked == false)
                    {
                        AutoLogin.IsChecked = false;
                    }
                    break;
                case "AutoLogin":
                    //埋点
                    Common.TrackHelper2.TrackOperation("5.9.1.5.1", "clk");
                    if (AutoLogin.IsChecked == true)
                    {
                        RememberPassword.IsChecked = true;
                    }
                    break;
                case "chkAutoRun":
                    Common.TrackHelper2.TrackOperation("5.9.1.6.1", "clk");
                    AutoRunSetting.SetAutoRun((bool)chkAutoRun.IsChecked);
                    break;
                case "chkWindowsFront":
                    //ChangeWinMost((bool)chkWindowsFront.IsChecked);
                    MagicCube.Common.GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.WindowMost, (bool)chkWindowsFront.IsChecked));
                    break;
               
            }
            //MagicGlobal.UserInfo.RememberPassword = (bool)RememberPassword.IsChecked;
            //MagicGlobal.UserInfo.AutoLogin = (bool)AutoLogin.IsChecked;

            //MagicGlobal.UserInfo.AutoRunSetting = (bool)chkAutoRun.IsChecked;


            //MagicGlobal.UserInfo.ExistAppTip = (bool)chkExistTip.IsChecked;
            //MagicGlobal.UserInfo.WindowsMinWhenClose = (bool)chkWindowsMin.IsChecked;
            //MagicGlobal.UserInfo.WindowsFrontMost = (bool)chkWindowsFront.IsChecked;
            //MagicGlobal.UserInfo.EnterSendChoose = (bool)chkEnterSend.IsChecked;
            //MagicGlobal.UserInfo.CtrlEnterSendChoose = (bool)chkCtrlEnterSend.IsChecked;


        }
        #endregion

        private void AutoLogin_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
