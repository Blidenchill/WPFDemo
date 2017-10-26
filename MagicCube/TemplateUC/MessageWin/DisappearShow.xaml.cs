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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace MagicCube.View.Message
{
    /// <summary>
    /// DisappearShow.xaml 的交互逻辑
    /// </summary>
    public partial class DisappearShow : Window
    {
        private DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public DisappearShow(string content, int timer = 2)
        {
            InitializeComponent();
            timer = 2;
            this.txtShow.Text = content;
            DoubleAnimation daV2 = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(timer)));
            this.bdNoticeSend.BeginAnimation(UIElement.OpacityProperty, daV2);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, timer);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.Close();
            dispatcherTimer.Stop();
            
        }

    }
}
