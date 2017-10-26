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

    public enum ResumeTableType
    {
        initiative = 1,
        invite = 2,
        pass = 3,
        fail = 4,
        autoFilter = 5
    }
    /// <summary>
    /// UCJobxaml.xaml 的交互逻辑
    /// </summary>
    public partial class UCResume : UserControl
    {
        
        public Action actionOpenPublish;
        public UCResume()
        {
            InitializeComponent();
            ucResumeGridView.UpdataCount = aUpdataCount;
            ucResumeGridView.actionOpenPublish = aOpenPublish;
            ucCollectionTable.UpdataCount = aUpdataCountCollection;
        }

        private void aUpdataCountCollection(int obj)
        {
            RBCollection.Count = obj.ToString();
        }

        private void aOpenPublish()
        {
            actionOpenPublish();
        }


        private void aUpdataCount()
        {
            iniResumeCount();
        }
        private async void ResumeType_Click(object sender, RoutedEventArgs e)
        {
            gdResumeDeliver.Visibility = Visibility.Visible;
            gdResumeCollection.Visibility = Visibility.Collapsed;
            //initiative invitation
            switch ((sender as RadioButton).Name)
            {
                case "rbOrderResume":
                    Common.TrackHelper2.TrackOperation("5.5.1.2.1", "clk");
                    Common.TrackHelper2.TrackOperation("5.5.1.4.1", "pv");
                    await ucResumeGridView.iniTable(ResumeTableType.initiative);
                    rbActive.IsChecked = true;
                    gdSelect.Visibility = Visibility.Visible;
                    tbTtile.Text = "候选人-待筛选";
                    break;
                case "rbOrderResumepass":
                    Common.TrackHelper2.TrackOperation("5.5.3.1.1", "clk");
                    Common.TrackHelper2.TrackOperation("5.5.3.2.1", "pv");
                    await ucResumeGridView.iniTable(ResumeTableType.pass);
                    gdSelect.Visibility = Visibility.Collapsed;
                    tbTtile.Text = "候选人-合适";
                    break;
                case "rbOrderResumefail":
                    Common.TrackHelper2.TrackOperation("5.5.4.1.1", "clk");
                    Common.TrackHelper2.TrackOperation("5.5.4.2.1", "pv");
                    await ucResumeGridView.iniTable(ResumeTableType.fail);
                    gdSelect.Visibility = Visibility.Collapsed;
                    tbTtile.Text = "候选人-不合适";
                    break;
                case "rbOrderResumereserve":
                    Common.TrackHelper2.TrackOperation("5.5.5.1.1", "clk");
                    Common.TrackHelper2.TrackOperation("5.5.5.2.1", "pv");
                    await ucResumeGridView.iniTable(ResumeTableType.autoFilter);
                    gdSelect.Visibility = Visibility.Collapsed;
                    tbTtile.Text = "候选人-自动过滤";
                    break;
            }
        }

        public async void iniResumeModuleById(string type, long id)
        {
            gdResumeDeliver.Visibility = Visibility.Visible;
            gdResumeCollection.Visibility = Visibility.Collapsed;
            iniResumeCount();
            iniCollectionCount();
            cb1.IsChecked = true;
            switch (type)
            {
                case "initiative":
                    Common.TrackHelper2.TrackOperation("5.5.1.4.1", "pv");
                    await ucResumeGridView.iniTable(ResumeTableType.initiative, id);
                    rbActive.IsChecked = true;
                    rbOrderResume.IsChecked = true;
                    gdSelect.Visibility = Visibility.Visible;
                    tbTtile.Text = "候选人-待筛选";
                    break;
                case "invite":
                    Common.TrackHelper2.TrackOperation("5.5.2.2.1", "pv");
                    await ucResumeGridView.iniTable(ResumeTableType.invite, id);
                    rbInvite.IsChecked = true;
                    rbOrderResume.IsChecked = true;
                    gdSelect.Visibility = Visibility.Visible;
                    tbTtile.Text = "候选人-待筛选";
                    break;
                case "pass":
                    Common.TrackHelper2.TrackOperation("5.5.3.2.1", "pv");
                    await ucResumeGridView.iniTable(ResumeTableType.pass, id);
                    rbOrderResumepass.IsChecked = true;
                    gdSelect.Visibility = Visibility.Collapsed;
                    tbTtile.Text = "候选人-合适";
                    break;
                case "fail":
                    Common.TrackHelper2.TrackOperation("5.5.4.2.1", "pv");
                    await ucResumeGridView.iniTable(ResumeTableType.fail, id);
                    rbOrderResumefail.IsChecked = true;
                    gdSelect.Visibility = Visibility.Collapsed;
                    tbTtile.Text = "候选人-不合适";
                    break;
                case "reserve_fail":
                    Common.TrackHelper2.TrackOperation("5.5.5.2.1", "pv");
                    await ucResumeGridView.iniTable(ResumeTableType.autoFilter, id);
                    rbOrderResumereserve.IsChecked = true;
                    gdSelect.Visibility = Visibility.Collapsed;
                    tbTtile.Text = "候选人-自动过滤";
                    break;
            }
        }

        public void iniResumeCount()
        {
            Action action = new Action(() =>
            {
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID"}, new string[] { MagicGlobal.UserInfo.Id.ToString() });
                string jsonResult = DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrDeliveryCount, MagicGlobal.UserInfo.Version, std));
                BaseHttpModel<MagicCube.ViewModel.HttpResumeCount> model = DAL.JsonHelper.ToObject<BaseHttpModel<MagicCube.ViewModel.HttpResumeCount>>(jsonResult);
               

                  
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (model != null)
                    {
                        if (model.code == 200)
                        {
                            cb1.Count = (model.data.initiative + model.data.invite + model.data.pass + model.data.fail + model.data.autoFilter).ToString();
                            rbActive.Count = model.data.initiative.ToString();
                            rbInvite.Count = model.data.invite.ToString();
                            rbOrderResume.Count = (model.data.initiative + model.data.invite).ToString();
                            rbOrderResumepass.Count = model.data.pass.ToString();
                            rbOrderResumefail.Count = model.data.fail.ToString();
                            rbOrderResumereserve.Count = model.data.autoFilter.ToString();

                        }
                    }
                }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            });
            action.BeginInvoke(null, null);
        }

        public void iniCollectionCount()
        {
            Action action = new Action(() =>
            {
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
                string jsonResult = DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrCollectCount, MagicGlobal.UserInfo.Version, std));
                BaseHttpModel<MagicCube.ViewModel.HttpCollectionCount> model = DAL.JsonHelper.ToObject<BaseHttpModel<MagicCube.ViewModel.HttpCollectionCount>>(jsonResult);



                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (model != null)
                    {
                        if (model.code == 200)
                        {
                            RBCollection.Count = (model.data.total).ToString();
                            

                        }
                    }
                }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            });
            action.BeginInvoke(null, null);
        }

        public async Task iniResumeModule()
        {
            gdResumeDeliver.Visibility = Visibility.Visible;
            gdResumeCollection.Visibility = Visibility.Collapsed;
            iniResumeCount();
            iniCollectionCount();
            cb1.IsChecked = true;
            rbOrderResume.IsChecked = true;
            rbActive.IsChecked = true;
            await ucResumeGridView.iniTable(ResumeTableType.initiative);
            tbTtile.Text = "候选人-待筛选";
            gdSelect.Visibility = Visibility.Visible;
        }

        private async void rbActive_Checked(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.1.3.1", "clk");
            TrackHelper2.TrackOperation("5.5.1.4.1", "pv");
            await ucResumeGridView.iniTable(ResumeTableType.initiative);
            tbTtile.Text = "候选人-待筛选";
        }

        private async void rbInvite_Checked(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.2.1.1", "clk");
            TrackHelper2.TrackOperation("5.5.2.2.1", "pv");
            await ucResumeGridView.iniTable(ResumeTableType.invite);
            tbTtile.Text = "候选人-待筛选";
        }

        private async void RBCollection_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.6.1.1", "clk");
            TrackHelper2.TrackOperation("5.5.6.2.1", "pv");
            tbTtile.Text = "收藏夹";
            gdResumeDeliver.Visibility = Visibility.Collapsed;
            gdResumeCollection.Visibility = Visibility.Visible;
            await ucCollectionTable.IniTable();
        }
        public async Task OpenCollection()
        {
            TrackHelper2.TrackOperation("5.5.6.2.1", "pv");
            RBCollection.IsChecked = true;
            tbTtile.Text = "收藏夹";
            iniResumeCount();
            iniCollectionCount();
            gdResumeDeliver.Visibility = Visibility.Collapsed;
            gdResumeCollection.Visibility = Visibility.Visible;
            await ucCollectionTable.IniTable();
        }
    }
}
