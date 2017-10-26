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

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCSecretButtonIM.xaml 的交互逻辑
    /// </summary>
    public partial class UCSecretButtonIM : UserControl
    {
        public UCSecretButtonIM(string content, string buttonName,string picUrl,string buttonID)
        {
            InitializeComponent();
            this.TxtContent = content;
            this.ButtonName = buttonName;
            this.PicUrl = picUrl;
            this.ButtonID = buttonID;
        }



        public string TxtContent
        {
            get { return (string)GetValue(TxtContentProperty); }
            set { SetValue(TxtContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TxtContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TxtContentProperty =
            DependencyProperty.Register("TxtContent", typeof(string), typeof(UCSecretButtonIM), new PropertyMetadata(null));



        public string ButtonName
        {
            get { return (string)GetValue(ButtonNameProperty); }
            set { SetValue(ButtonNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonNameProperty =
            DependencyProperty.Register("ButtonName", typeof(string), typeof(UCSecretButtonIM), new PropertyMetadata(null));




        public string PicUrl
        {
            get { return (string)GetValue(PicUrlProperty); }
            set { SetValue(PicUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PicUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PicUrlProperty =
            DependencyProperty.Register("PicUrl", typeof(string), typeof(UCSecretButtonIM), new PropertyMetadata(null));




        public string ButtonID = string.Empty;

        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            Messaging.MSBodyBase model = new Messaging.MSBodyBase() { body = null, type = ButtonID };
            Messaging.Messenger.Default.Send<Messaging.MSBodyBase, ViewSingle.MainWindow>(model);
        }

    }
}
