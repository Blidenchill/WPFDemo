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
    /// UCSystem.xaml 的交互逻辑
    /// </summary>
    public partial class UCSystem : UserControl
    {
        public UCSystem()
        {
            InitializeComponent();
        }

        private void rbBasicSet_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.9.1.2.1", "clk");
            Common.TrackHelper2.TrackOperation("5.9.1.3.1", "pv");
        }

        private void rbIMSet_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.9.2.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.9.2.2.1", "pv");
        }

        private void rbDownloadSet_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.9.3.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.9.3.2.1", "pv");
            //this.ucDownloadSetting.InitialDownload();
        }

        private void rbSafeSet_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.9.4.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.9.4.2.1", "pv");

        }

        private void rbSafeSet_Checked(object sender, RoutedEventArgs e)
        {
            
            this.ucSafeSet.CurrentWordValidate = false;
            this.ucSafeSet.NewWordValidate = false;
            this.ucSafeSet.VerifyWordValidate = false;
            this.ucSafeSet.pbCurrentWord.Password = string.Empty;
            this.ucSafeSet.pbNewWord.Password = string.Empty;
            this.ucSafeSet.pbVerifyWord.Password = string.Empty;
            if (MagicGlobal.UserInfo.PasswordExist)
            {
                this.ucSafeSet.gdModify.Visibility = Visibility.Visible;
                this.ucSafeSet.gdSetting.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ucSafeSet.gdModify.Visibility = Visibility.Collapsed;
                this.ucSafeSet.gdSetting.Visibility = Visibility.Visible;
            }
        }
    }
}
