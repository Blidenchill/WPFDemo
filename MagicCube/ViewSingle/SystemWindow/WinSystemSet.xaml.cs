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
using System.Windows.Shapes;
using MagicCube.ViewModel;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// WinSystemSet.xaml 的交互逻辑
    /// </summary>
    public partial class WinSystemSet : Window
    {
        public WinSystemSet()
        {
            InitializeComponent();
            this.Loaded += WinLoaded; 
        }
        private SysAccontSetModel sysAccontModel = new SysAccontSetModel();

        private void WinLoaded(object sender, RoutedEventArgs e)
        {
            sysAccontModel.AvatarUrl = MagicGlobal.UserInfo.avatarUrl;
            sysAccontModel.HrName = MagicGlobal.UserInfo.RealName;
            sysAccontModel.HrPosition = MagicGlobal.UserInfo.UserPosition;

            this.ucAccountSet.DataContext = sysAccontModel;
        }

        private void rdoDownloadSet_Click(object sender, RoutedEventArgs e)
        {
            this.ucDownloadSet.InitialDownload();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.Owner.Activate();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void rdoBasicSet_Click(object sender, RoutedEventArgs e)
        {
            this.ucBasicSet.chkEnterSend.IsChecked = MagicGlobal.UserInfo.EnterSendChoose;
            
        }

        private void rdoModifyPasswordSet_Click(object sender, RoutedEventArgs e)
        {
            this.ucModifyPasswordSet.InitialMofityPassword();
            if (MagicGlobal.UserInfo.PasswordExist)
            {
                this.ucModifyPasswordSet.gdModify.Visibility = Visibility.Visible;
                this.ucModifyPasswordSet.gdSetting.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ucModifyPasswordSet.gdModify.Visibility = Visibility.Collapsed;
                this.ucModifyPasswordSet.gdSetting.Visibility = Visibility.Visible;
            }
        }

        private void rdoModifyTelephoneSet_Click(object sender, RoutedEventArgs e)
        {
            this.ucModifyTelephoneSet.InitialModifyTelephone();
        }
    }
}
