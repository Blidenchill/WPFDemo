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
using MagicCube.Common;
using MagicCube.HttpModel;
using MagicCube.Model;
using MagicCube.View;

using MagicCube.TemplateUC;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// WinTipBox.xaml 的交互逻辑
    /// </summary>
    public partial class WinTipBox : Window
    {
        public WinTipBox()
        {
            InitializeComponent();
        }
        private List<IMControlModel> jobDynamicModelList = new List<IMControlModel>();

        private System.Timers.Timer timeAlive;
        private double interval = 10 * 1000;


        public WinTipBox(List<IMControlModel> jobDynamicModelList)
        {
            InitializeComponent();
            //foreach(var item in jobDynamicModelList)
            //{
            //    if(item.IsJobRead)
            //    {
            //        continue;
            //    }
            //    this.jobDynamicModelList.Add(item);
            //}
            this.jobDynamicModelList.AddRange(jobDynamicModelList);
            //this.jobDynamicModelList = jobDynamicModelList;
            if (jobDynamicModelList != null)
            {
                this.tbCount.Text = jobDynamicModelList.Count.ToString();

            }
            //窗口位置
            Rect WorkingAera = SystemParameters.WorkArea;
            this.Top = WorkingAera.Height - 70;
            this.Left = WorkingAera.Width - 331;
            LogHelper.WriteLog("托盘提示结果弹出");

            //窗口定时关闭
            timeAlive = new System.Timers.Timer(this.interval);
            timeAlive.Elapsed += timeAlive_Elapsed;
            timeAlive.Enabled = true;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnCheckView_Click(object sender, RoutedEventArgs e)
        {
            GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.TipOpenDynamicUI, null));

            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public void UpdateList(IEnumerable<IMControlModel> collection)
        {
            //foreach(var item in collection)
            //{
            //    if (item.IsJobRead)
            //        continue;
            //    jobDynamicModelList.Add(item);
            //}
            jobDynamicModelList.Clear();
            jobDynamicModelList.AddRange(collection);
            this.tbCount.Text = jobDynamicModelList.Count.ToString();
            LogHelper.WriteLog("托盘提示结果更新列表");
        }

        private void timeAlive_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Close();
            }));
        }
    }
}
