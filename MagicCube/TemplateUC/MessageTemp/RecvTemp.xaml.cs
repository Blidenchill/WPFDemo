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
    /// SendTemp.xaml 的交互逻辑
    /// </summary>
    public partial class RecvTemp : UserControl
    {
        public RecvTemp()
        {
            InitializeComponent();
        }



        public string UrlPic
        {
            get { return (string)GetValue(UrlPicProperty); }
            set { SetValue(UrlPicProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UrlPic.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UrlPicProperty =
            DependencyProperty.Register("UrlPic", typeof(string), typeof(RecvTemp), new PropertyMetadata(null));


    }
}
