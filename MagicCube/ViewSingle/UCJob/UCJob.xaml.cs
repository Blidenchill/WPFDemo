using MagicCube.Common;
using MagicCube.HttpModel;

using MagicCube.ViewModel;
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
    /// UCJobxaml.xaml 的交互逻辑
    /// </summary>
    public partial class UCJob : UserControl
    {
        public Action<string, long> actionOpenResume;
        public UCJob()
        {
            InitializeComponent();
            ucJobOpen.actionEdit = actionEdit;
            ucJobClose.actionEdit = actionEditClose;
            ucJobOpen.actionOpenResume = aOpenResume;
            ucJobClose.actionOpenResume = aOpenResume;
            ucJobOpen.actionUpdataCount = SetJobCount;
            ucJobClose.actionUpdataCount = SetJobCount;
            ucJobOpen.actionOpenPublish = OpenPublish;
            ucJobClose.actionPublishJob = OpenPublishClose;
            ucJobPublish.actionUpdataCount = SetJobCount;
            ucJobPublish.actionReturn = ReturnCallback;
        }

       

        public void SetJobCount()
        {
            Action action = new Action(() =>
            {
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "jobType" }, new string[] { MagicGlobal.UserInfo.Id.ToString() ,"2"});
                string jsonResult = DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrGetJobAccount, MagicGlobal.UserInfo.Version, std));
                BaseHttpModel<JobTotal> model = DAL.JsonHelper.ToObject<BaseHttpModel<JobTotal>>(jsonResult);
                 this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if(model != null)
                    {
                        if (model.code == 200)
                        {
                            RBOpen.Count = model.data.onlineJobNum.ToString();
                            RBClose.Count = model.data.offlineJobNum.ToString();
                        }
                    }
                    

                }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            });
            action.BeginInvoke(null, null);
           
        }

        private void UpdataCount(string online,string offline)
        {
            RBOpen.Count = online;
            RBClose.Count = offline;
        }
        private void aOpenResume(string arg1, long arg2)
        {
            actionOpenResume(arg1, arg2);
        }

        private void OpenPublish()
        {
            RBPublish.IsChecked = true;
            ucJobPublish.iniPublish();
            tbTitle.Text = "发布职位";
        }
        private void OpenPublishClose()
        {
            RBPublish.IsChecked = true;
            ucJobPublish.iniPublish(true);
            tbTitle.Text = "发布职位";
        }
        private async void RBOpen_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.2.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.3.2.2.1", "pv");
            await ucJobOpen.iniJobOpen();
            tbTitle.Text = "发布中的职位";
        }

        private async void RBClose_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.4.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.3.4.2.1", "pv");
            await ucJobClose.iniJobClose();
            tbTitle.Text = "已下线的职位";
        }

        private void RBPublish_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.3.1.3.1", "clk");
            Common.TrackHelper2.TrackOperation("5.3.1.4.1", "pv");
            ucJobPublish.iniPublish();
            tbTitle.Text = "发布职位";
        }

        public async Task actionEdit(long id)
        {
            RBPublish.IsChecked = true;
            tbTitle.Text = "发布职位";
            await ucJobPublish.iniEdit(id);


        }
        private async Task actionEditClose(long id)
        {
            RBPublish.IsChecked = true;
            tbTitle.Text = "发布职位";
            await ucJobPublish.iniEdit(id,true);
        }

        private async Task ReturnCallback(bool isFromClose)
        {
            if(isFromClose)
            {
                RBClose.IsChecked = true;
                await ucJobClose.iniJobClose();
                tbTitle.Text = "已下线的职位";
            }
            else
            {
                RBOpen.IsChecked = true;
                await ucJobOpen.iniJobOpen();
                tbTitle.Text = "发布中的职位";
            }
        }

    }
}
