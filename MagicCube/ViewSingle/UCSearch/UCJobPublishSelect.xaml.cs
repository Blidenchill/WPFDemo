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
using MagicCube.HttpModel;
using System.Collections.ObjectModel;

using MagicCube.Common;
using MagicCube.Model;
using MagicCube.TemplateUC;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCJobPublishSelect.xaml 的交互逻辑
    /// </summary>
    public partial class UCJobPublishSelect : UserControl
    {
        public UCJobPublishSelect()
        {
            InitializeComponent();
        }
        private ObservableCollection<JobPublish> JobPublishList = new ObservableCollection<JobPublish>();
        private UserJobModel selectResumModel;
        #region "委托"
        public Action JobPublishSelectCloseAction;
        public delegate Task JobPubushiAction(UserJobModel model, JobPublishGoto gotoEnum);
        public JobPubushiAction JobPublishSelectOKAction;
        public delegate Task delegateAddV();
        public delegateAddV actionAddV;
        private JobPublishGoto jobPublishGotoEnum;
        //public Action<string> JobPublishSelectOKAction;
        #endregion
        public void JobPublishGet(ObservableCollection<JobPublish> list, UserJobModel selectResumModel, JobPublishGoto gotoEnum)
        {
            JobPublishList = list;
            this.RusumelstJobPublish.ItemsSource = JobPublishList;
            this.selectResumModel = selectResumModel;
            this.jobPublishGotoEnum = gotoEnum;
            switch(gotoEnum)
            {
                case JobPublishGoto.IM:
                    this.tbTitile.Text = "选择沟通职位";
                    this.tbMain.Text = "请选择您想与对方沟通的职位";
                    break;
                case JobPublishGoto.hrSendOffer:
                    this.tbTitile.Text = "选择邀请投递职位";
                    this.tbMain.Text = "请选择您想邀请对方投递的职位";
                    break;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (this.JobPublishSelectCloseAction != null)
                this.JobPublishSelectCloseAction();
        }

        private async void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            JobPublish job = this.RusumelstJobPublish.SelectedItem as JobPublish;
            if (job == null)
            {
                WinErroTip msg = new WinErroTip("请选择一个发布的职位");
                msg.Owner = Window.GetWindow(this);
                msg.ShowDialog();
                this.Cursor = Cursors.Arrow;
                return;
            }
            //string gResult = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerContactRecord, JsonUtil.ContactRecord(job.id, 0, "", selectResumModel.uniqueKey));
            ////string gResult = DAL.HttpHelper.Instance.HttpGet(string.Format(ConfUtil.ServerGetConstactRecord, selectResumModel.uniqueKey, job.id));
            //this.Cursor = Cursors.Arrow;
            //if (gResult.StartsWith("连接失败"))
            //{
            //    this.Cursor = Cursors.Arrow;
            //    this.Dispatcher.Invoke(new Action(() =>
            //    {
            //        WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
            //        pWinMessage.Owner = Window.GetWindow(this);
            //        if (pWinMessage.Owner == null)
            //            return;
            //        pWinMessage.ShowDialog();
            //    }));
            //    return;
            //}
            //this.Cursor = Cursors.Arrow;
            //HttpContactRecordGet constactRecordIdGet = DAL.JsonHelper.ToObject<HttpContactRecordGet>(gResult);
            //selectResumModel.jobId = job.id.ToString();



            //TrackAction.TrackSet("INTERVIEW_REQUESTS", constactRecordIdGet.recordId.ToString());
            //TrackAction.TrackSet("Face_INTERACTION", selectResumModel.uniqueKey);
            //打招呼语
            this.busyCtrl.IsBusy = true;
            ViewModel.BaseHttpModel<ViewModel.HttpMessageRoot>  model = await IMHelper.GetIMRecord(Convert.ToInt64(selectResumModel.userId), job.id);
            this.busyCtrl.IsBusy = false;
            if (model != null)
            {
                if (model.code == -135)
                {
                    WinValidateMessage pWinValidateMessage = new WinValidateMessage("很抱歉，认证后才能与更多候选人主动发起沟通");
                    pWinValidateMessage.Owner = Window.GetWindow(this);
                    if (pWinValidateMessage.ShowDialog() == true)
                    {
                        if (actionAddV != null)
                        {
                            await actionAddV();
                        }
                    }
                    if (this.JobPublishSelectCloseAction != null)
                        this.JobPublishSelectCloseAction();
                    return;
                }
            }
            selectResumModel.jobId = job.id.ToString();

            if (this.JobPublishSelectCloseAction != null)
                this.JobPublishSelectCloseAction();
            if (this.JobPublishSelectOKAction != null)
                await this.JobPublishSelectOKAction(selectResumModel, this.jobPublishGotoEnum);
          

        }
    }

    public enum JobPublishGoto
    {
        IM,
        hrSendOffer,
    }
}
