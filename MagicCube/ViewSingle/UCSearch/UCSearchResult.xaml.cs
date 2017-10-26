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
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

using MagicCube.HttpModel;
using MagicCube.Model;
using MagicCube.Common;

using MagicCube.View.Message;
using MagicCube.View.UC;
using MagicCube.TemplateUC;
using MagicCube.ViewModel;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCSearchResult.xaml 的交互逻辑
    /// </summary>
    public partial class UCSearchResult : UserControl
    {
        ResumeDetailModel curResumeDetailModel = new ResumeDetailModel();
        public delegate Task delegateAddV();
        public delegateAddV actionAddV;

        public string searchSession;
        public UCSearchResult()
        {
            InitializeComponent();
            itemsControlResumeImageUrlList.ItemsSource = this.ResumImageUrlList;
            this.cmbPulishJobList.ItemsSource = this.JobPublishList;
            this.cmbPulishJobList.DisplayMemberPath = "jobName";
            this.cmbPulishJobList.SelectedValuePath = "jobName";
            ucThumbNailResum.ResumeMoreShow += ResumeMoreShowCallback;
            ucThumbNailResum.ResumOfferConnectToSingleAction += ResumOfferConnectToSingleCallback;
            ucThumbNailResum.ResumSendOfferAciton += ResumSendOfferCallback;
            ucThumbNailResum.IsCheckedChangedAction += IsCheckedFromThumbnailCallback;
            this.SidebarVisibility(false);
            this.ucRapidSearchCondition.IniSearchingList();
            ucRapidSearchCondition.StartSearchingAction += RapidSearchConditionCallback;
            this.ucPageTurn.PageChange += PageChangeCallback;
            ucResume.ucb.Click += TrackOpeartion_Event;
            ucResume.btnFreePhone.Click += TrackOpeartion_Event;
            ucResume.btnVideo.Click += TrackOpeartion_Event;
            Messaging.Messenger.Default.Register<Messaging.MSJobSearchConditionModel>(this, SearchResumCallback);
         
        }

        #region "委托"
        public Action<SaveSearchCondition> RapidSearchConditionAction;
        #endregion

        #region "变量"
        string uniqueKey = string.Empty;
        string sendMessageStr = "您好，最近有换工作的想法么？可以关注下我发布的{0}({1}K-{2}K)";
        /// <summary>
        /// 进入IM聊天框委托
        /// </summary>
        public Action<int, string> OpenIMAction;
        /// <summary>
        /// 发布新职位委托
        /// </summary>
        public Action OpenJobPublishAction;
        /// <summary>
        /// 上线已下线职位委托
        /// </summary>
        public delegate Task delegateOpenJobGridCloseUCAction();
        public delegateOpenJobGridCloseUCAction OpenJobGridCloseUCAction;
        /// <summary>
        /// 清除收藏
        /// </summary>
        public delegate Task delegateClearCollection();
        public delegateClearCollection actionClearCollection;
        /// <summary>
        /// 职位选择委托
        /// </summary>
        public Action<ObservableCollection<JobPublish>> JobPublishAction;
        public ObservableCollection<JobPublish> JobPublishList = new ObservableCollection<JobPublish>();
        public ObservableCollection<string> JobPublishListcmb = new ObservableCollection<string>();
        public ObservableCollection<string> ResumImageUrlList = new ObservableCollection<string>();
        private SaveSearchCondition saveSearchCondition = new SaveSearchCondition();
        public SearchingTotalResult searchingTotalResult;
        public List<SearchTalentResumModel> searchingTanlentResumModel = new List<SearchTalentResumModel>();
        private List<SearchTalentResumModel> selectedResumModel = new List<SearchTalentResumModel>();
        int totalData = 0;
        int totalPage = 0;
        int curPage = 0;

        private SearchTalentResumModel selectedSearchingResult;

        private SearchConditionSaveModel ConditionSaveModel = new SearchConditionSaveModel();

        private ViewModel.HttpSearchResumModel HttpSearchModel = new HttpSearchResumModel();
        #endregion

        #region "事件"
        /// <summary>
        /// 简历列表双击显示边框简历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void tbResume_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image fe = e.OriginalSource as System.Windows.Controls.Image;
            if (fe != null)
                return;

            this.busyCtrl.IsBusy = true;
            e.Handled = true;
            Point aP = e.GetPosition(this.tbResumeSearch);
            IInputElement obj = this.tbResumeSearch.InputHitTest(aP);
            System.Windows.DependencyObject target = obj as System.Windows.DependencyObject;
            int count = 0;
            while (target != null)
            {
                count++;
                if (target is DataGridRow)
                {
                    selectedSearchingResult = (target as DataGridRow).DataContext as SearchTalentResumModel;
                    SearchTalentResumModel resultItem = selectedSearchingResult;
                    
                    if (resultItem == null)
                        return;
                    await this.UpdateResumeSidebara();
                    resultItem.isRead = true;
                    this.SidebarVisibility(true, 0);
                    return;


                   
                }
                target = VisualTreeHelper.GetParent(target);
                if(count > 20)
                {
                    this.busyCtrl.IsBusy = false;
                    break;
                }
            }
        }

        private async void DatagridRight_MouseClick(object sender, RoutedEventArgs e)
        {
            if (tbResumeSearch.SelectedItem != null)
            {
                SearchTalentResumModel resultItem = tbResumeSearch.SelectedItem as SearchTalentResumModel;
                selectedSearchingResult = resultItem;
                if (resultItem == null)
                    return;
                await this.UpdateResumeSidebara();
                resultItem.isRead = true;
                this.SidebarVisibility(true, 0);
                return;

            }
        }




        private async Task PublishJobMsgCallback(PublishType type)
        {
            switch (type)
            {
                case PublishType.PublishNew:
                    if (this.OpenJobPublishAction != null)
                    {
                        this.OpenJobPublishAction();
                    }
                    break;
                case PublishType.OldReset:
                    if (OpenJobGridCloseUCAction != null)
                    {
                      await  this.OpenJobGridCloseUCAction();
                    }
                    break;
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.4.9.1", "clk");

            (tbResumeSearch.SelectedItem as SearchTalentResumModel).IsChoosed = !(tbResumeSearch.SelectedItem as SearchTalentResumModel).IsChoosed;
            setJudgeAllEnable();
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            List<SearchTalentResumModel> listTemp = tbResumeSearch.ItemsSource as List<SearchTalentResumModel>;
            bool flag = (bool)(sender as CheckBox).IsChecked;
            foreach (SearchTalentResumModel iHttpResumeInterviewer in (tbResumeSearch.ItemsSource as List<SearchTalentResumModel>))
            {
                iHttpResumeInterviewer.IsChoosed = flag;
            }
            setJudgeAllEnable();
        }

        private async void ConnectToAll_Click(object sender, RoutedEventArgs e)
        {

            //this.bdNoticeSend.Visibility = System.Windows.Visibility.Collapsed;
            //this.gdPublishJobPositon.Visibility = System.Windows.Visibility.Visible;
            //this.SendResumOfferSingle = false;
            List<ViewModel.HttpTrackEventDataModel> eventModelList = new List<HttpTrackEventDataModel>();
           
            bool flag = false;
            this.ResumImageUrlList.Clear();
            this.selectedResumModel.Clear();
            int index = 0;
            foreach (var item in this.searchingTanlentResumModel)
            {
                if (item.IsChoosed)
                {
                    this.ResumImageUrlList.Add(item.avatarUrl);
                    this.selectedResumModel.Add(item);
                    ViewModel.HttpTrackEventDataModel eventModel = new HttpTrackEventDataModel();
                    //eventModel.session = Guid.NewGuid().ToString();
                    eventModel.session = this.searchSession;
                    eventModel.algo = "se";
                    eventModel.item = new List<string>();
                    eventModel.item.Add(item.userId.ToString());
                    eventModel.location = index.ToString();
                    eventModelList.Add(eventModel);
                }
                index++;

            }
        

            //埋点
            Common.TrackHelper2.TrackOperation("5.6.4.6.1", "clk",DAL.JsonHelper.ToJsonString(eventModelList));

            if (this.ResumImageUrlList.Count > 0)
            {
                flag = true;
                this.tbPublishOfferNum.Text = this.ResumImageUrlList.Count.ToString();
            }
            //this.txtSendMessage.Text = string.Format(this.sendMessageStr, MagicGlobal.UserInfo.CompanyName, string.Empty);

            if (!flag)
            {
                WinErroTip pWinMessage = new WinErroTip("请至少选择一位候选人");
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                return;
            }

            //获取职位列表
            this.busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "status", "jobType" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "1","2" });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<HttpJobListData> listModel = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(resultStr);
            this.busyCtrl.IsBusy = false;
            if (listModel == null)
            {
                WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                return;
            }
            else
            {
                if(listModel.code == 200)
                {
                    if(listModel.data.data.Count == 0)
                    {
                        WinPublishJobMessage msg = new WinPublishJobMessage();
                        msg.Owner = Window.GetWindow(this);
                        msg.atConfirmTask += PublishJobMsgCallback;
                        msg.ShowDialog();

                        return;
                    }
                    this.JobPublishList.Clear();
                    this.JobPublishListcmb.Clear();
                    foreach (var item in listModel.data.data)
                    {
                        this.JobPublishList.Add(new JobPublish() { id = (int)item.jobID, jobName = item.jobName, minSalary = item.jobMinSalary.ToString(), maxSalary = item.jobMaxSalary.ToString() });
                        this.JobPublishListcmb.Add(item.jobName);
                    }
                    if (listModel.data.data.Count == 1)
                        this.cmbPulishJobList.SelectedIndex = 0;
                    this.SidebarVisibility(true, 1);
                }
                else
                {
                    WinErroTip pWinMessage = new WinErroTip(listModel.msg);
                    pWinMessage.Owner = Window.GetWindow(this);
                    pWinMessage.ShowDialog();
                    return;
                }
            }



            //string gResult = DAL.HttpHelper.Instance.HttpGet(ConfUtil.ServerGetJobPublish);
            //if (gResult.StartsWith("连接失败"))
            //{
            //    LogHelper.WriteLog("人才搜索ConfUtil.ServerGetJobPublish错误：" + gResult);
            //    WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
            //    pWinMessage.Owner = Window.GetWindow(this);
            //    pWinMessage.ShowDialog();
            //    this.Cursor = Cursors.Arrow;
            //    return;
            //}
            //ObservableCollection<JobPublish> tempJobPublish = DAL.JsonHelper.ToObject<ObservableCollection<JobPublish>>(gResult);
            //if (tempJobPublish == null)
            //{
            //    LogHelper.WriteLog("人才搜索ConfUtil.ServerGetJobPublish错误：" + gResult);
            //    WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
            //    pWinMessage.Owner = Window.GetWindow(this);
            //    pWinMessage.ShowDialog();
            //    this.Cursor = Cursors.Arrow;
            //    return;
            //}
            //if (tempJobPublish.Count == 0)
            //{
            //    this.Cursor = Cursors.Arrow;

            //    WinPublishJobMessage msg = new WinPublishJobMessage();
            //    msg.Owner = Window.GetWindow(this);
            //    msg.atConfirm += PublishJobMsgCallback;
            //    msg.ShowDialog();

            //    return;
            //}
            //this.JobPublishList.Clear();
            //this.JobPublishListcmb.Clear();
            //foreach (var item in tempJobPublish)
            //{
            //    this.JobPublishList.Add(item);
            //    this.JobPublishListcmb.Add(item.jobName);
            //}
            //if (tempJobPublish.Count == 1)
            //    this.cmbPulishJobList.SelectedIndex = 0;
            //this.Cursor = Cursors.Arrow;
            //this.SidebarVisibility(true, 1);


        }



        //private void ConnectToSingle_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Cursor = Cursors.Wait;
        //    if (tbResumeSearch.SelectedItem != null)
        //    {
        //        selectedSearchingResult = tbResumeSearch.SelectedItem as SearchTalentResumModel;
        //        this.ResumImageUrlList.Clear();
        //        this.ResumImageUrlList.Add(selectedSearchingResult.avatarUrl);
        //        this.tbPublishOfferNum.Text = this.ResumImageUrlList.Count.ToString();
        //        this.txtSendMessage.Text = string.Format(this.sendMessageStr, MagicGlobal.UserInfo.CompanyName, string.Empty);

        //    }
        //    //this.gdPublishJobPositon.Visibility = System.Windows.Visibility.Visible;
        //    //this.gdPublishOfferMutiple.Visibility = System.Windows.Visibility.Visible;
        //    this.SendResumOfferSingle = true;

        //    string gResult = DAL.HttpHelper.Instance.HttpGet(ConfUtil.ServerGetJobPublish);
        //    if (gResult.StartsWith("连接失败"))
        //    {
        //        LogHelper.WriteLog("人才搜索ConfUtil.ServerGetJobPublish错误：" + gResult);
        //        WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
        //        pWinMessage.Owner = Window.GetWindow(this);
        //        pWinMessage.ShowDialog();
        //        //this.gdPublishJobPositon.Visibility = System.Windows.Visibility.Collapsed;
        //        this.Cursor = Cursors.Arrow;
        //        return;
        //    }
        //    ObservableCollection<JobPublish> tempJobPublish = DAL.JsonHelper.ToObject<ObservableCollection<JobPublish>>(gResult);
        //    if (tempJobPublish == null)
        //    {
        //        LogHelper.WriteLog("人才搜索ConfUtil.ServerGetJobPublish错误：" + gResult);
        //        WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
        //        pWinMessage.Owner = Window.GetWindow(this);
        //        pWinMessage.ShowDialog();
        //        this.Cursor = Cursors.Arrow;
        //        return;
        //    }
        //    if (tempJobPublish.Count == 0)
        //    {
        //        this.Cursor = Cursors.Arrow;
        //        WinPublishJobMessage msg = new WinPublishJobMessage();
        //        msg.Owner = Window.GetWindow(this);
        //        msg.atConfirm += PublishJobMsgCallback;
        //        msg.ShowDialog();
        //        return;
        //    }
        //    this.JobPublishList.Clear();
        //    this.JobPublishListcmb.Clear();
        //    foreach (var item in tempJobPublish)
        //    {
        //        this.JobPublishList.Add(item);
        //        this.JobPublishListcmb.Add(item.jobName);
        //    }
        //    if (tempJobPublish.Count == 1)
        //        this.cmbPulishJobList.SelectedIndex = 0;

        //    this.Cursor = Cursors.Arrow;
        //    this.SidebarVisibility(true, 1);

        //}

        /// <summary>
        /// 批量发送面试邀请确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void JobPublishBatchProcess_Click(object sender, RoutedEventArgs e)
        {

            //ListView lst = sender as ListView;
            if (this.JobPublishList.Count == 0)
            {
                WinConfirmTip wMessage = new WinConfirmTip("您未发布职位无法邀请对方投递或面试，请先发布职位信息。");
                bool flag = (bool)wMessage.ShowDialog();
                if (flag)
                {
                    if (this.OpenJobPublishAction != null)
                    {
                        this.OpenJobPublishAction();
                    }
                }
                return;
            }
            JobPublish jobPublish = this.cmbPulishJobList.SelectedItem as JobPublish;
            if (this.cmbPulishJobList.SelectedItem == null)
            {
                DisappearShow pWinMessage = new DisappearShow("请选择邀请投递的职位", 1);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                return;
            }

            //this.gdPublishJobPositon.Visibility = System.Windows.Visibility.Collapsed;
            //this.gdPublishOfferMutiple.Visibility = System.Windows.Visibility.Collapsed;
            this.SidebarVisibility(false);

            this.busyCtrl.IsBusy = true;

            int jobId = 0;

            if (jobPublish == null)
            {
                this.busyCtrl.IsBusy = false;
                return;
            }

            jobId = jobPublish.id;

            List<HttpSendMessageParams> lstParams = new List<HttpSendMessageParams>();
            foreach (var item in this.selectedResumModel)
            {

                lstParams.Add(IMHelper.GetMessageModel(Convert.ToInt64(item.userId), Convert.ToInt64(jobId),this.txtSendMessage.Text));
            }
            string std = DAL.JsonHelper.ToJsonString(lstParams);
            string url = string.Format(DAL.ConfUtil.AddrBatchInvite, MagicGlobal.UserInfo.Version);
            string result = await DAL.HttpHelper.Instance.HttpPostAsync(url, std);
            this.busyCtrl.IsBusy = false;
            ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel>(result);
            if (model == null)
            {
                WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
            }
            else
            {
                if(model.code == -135)
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
                }
                if (model.code == 200)
                {
                    DisappearShow disappear = new DisappearShow("已发送", 1);
                    disappear.Owner = Window.GetWindow(this);
                    disappear.ShowDialog();
                }
            }

            //if (this.selectedResumModel.Count > 1)
            //{
            //    //批量邀请投递
            //    //string std = string.Empty;
            //    //foreach (var item in this.selectedResumModel)
            //    //{
            //    //    std += DAL.JsonHelper.JsonParamsToString(new string[] { "hrUserID", "type", "jobID", "jhUserID" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "2", jobId.ToString(), item.userId.ToString() }) + ",";

            //    //}
            //    //std = std.TrimEnd(new char[] { ',' });
            //    //string stdNew = string.Format("[{0}]", std);
                
            //}
            //else
            //{
            //    await HrSendOffer(jobId.ToString(), this.selectedResumModel[0].userId.ToString());
            //}

           


            //this.bdNoticeSend.Visibility = System.Windows.Visibility.Visible;

            //ThreadPool.QueueUserWorkItem(JobPublishBatchProcess, jobId);
        }

        //private void JobPublishBatchProcess(object sender)
        //{
        //    try
        //    {
        //        if (this.SendResumOfferSingle)
        //        {
        //            if (selectedSearchingResult != null)
        //            {
        //                string gResult = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerContactRecord, JsonUtil.ContactRecord(Convert.ToInt32(sender), 0, "", selectedSearchingResult.uniqueKey));
        //                //string gResult = DAL.HttpHelper.Instance.HttpGet(string.Format(ConfUtil.ServerGetConstactRecord, selectedSearchingResult.uniqueKey, (int)sender));
        //                Console.WriteLine("人才搜索职位邀请获取contactRecordId结果字符串：" + gResult);
        //                LogHelper.WriteLog("人才搜索职位邀请获取contactRecordId结果字符串：" + gResult);
        //                if (gResult.StartsWith("连接失败"))
        //                {
        //                    return;
        //                }
        //                selectedSearchingResult.isRead = true;
        //                selectedSearchingResult.jobId = Convert.ToString((int)sender);
        //                HttpContactRecordGet constactRecordIdGet = Util.JsonUtil.ToObject<HttpContactRecordGet>(gResult);
        //                if (constactRecordIdGet == null)
        //                    return;

        //                TrackAction.TrackSet("INTERVIEW_REQUESTS", constactRecordIdGet.recordId.ToString());
        //                TrackAction.TrackSet("Face_INTERACTION", uniqueKey);


        //                //GlobalEvent.GlobalEventHandler(this, new GlobalEventArgs(GlobalFlag.UpdateConnectList, constactRecordIdGet.recordId));
        //                selectedSearchingResult = null;
        //            }
        //        }
        //        else
        //        {
        //            foreach (var item in searchingTanlentResumModel)
        //            {
        //                if (item.IsChoosed)
        //                {
        //                    //string gResult = DAL.HttpHelper.Instance.HttpGet(string.Format(ConfUtil.ServerGetConstactRecord, item.uniqueKey, (int)sender));
        //                    string gResult = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerContactRecord, JsonUtil.ContactRecord(Convert.ToInt32(sender), 0, "", item.uniqueKey));
        //                    Console.WriteLine("人才搜索职位邀请获取contactRecordId结果字符串：" + gResult);
        //                    LogHelper.WriteLog("人才搜索职位邀请获取contactRecordId结果字符串：" + gResult);
        //                    if (gResult.StartsWith("连接失败"))
        //                    {
        //                        continue;
        //                    }
        //                    item.isRead = true;
        //                    item.jobId = Convert.ToString((int)sender);
        //                    HttpContactRecordGet constactRecordIdGet = Util.JsonUtil.ToObject<HttpContactRecordGet>(gResult);
        //                    if (constactRecordIdGet == null)
        //                        continue;

        //                    TrackAction.TrackSet("INTERVIEW_REQUESTS", constactRecordIdGet.recordId.ToString());
        //                    TrackAction.TrackSet("Face_INTERACTION", uniqueKey);

        //                    //GlobalEvent.GlobalEventHandler(this, new GlobalEventArgs(GlobalFlag.UpdateConnectList, constactRecordIdGet.recordId));


        //                }
        //            }
        //        }

        //        this.Dispatcher.BeginInvoke(new Action(() =>
        //        {
        //            this.busyCtrl.IsBusy = false;
        //            //Thread thd = new Thread(ShowNoticeSend);
        //            //thd.Start();
        //            DisappearShow disappear = new DisappearShow("已发送", 1);
        //            disappear.Owner = Window.GetWindow(this.gdPublishOfferMutiple);
        //            if (disappear.Owner == null)
        //                return;
        //            disappear.ShowDialog();
        //            return;
        //        }));

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog("人才搜索职位邀请异常捕获");
        //        LogHelper.WriteErrorLog(ex);
        //    }

        //}

        private void cmbPulishJobList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox item = sender as ComboBox;
            JobPublish publish = item.SelectedItem as JobPublish;
            if (publish != null)
                //this.txtSendMessage.Text = string.Format(this.sendMessageStr, MagicGlobal.UserInfo.CompanyName, "就" + publish.jobName + "的职位");
                this.txtSendMessage.Text = string.Format(this.sendMessageStr, publish.jobName, publish.minSalary, publish.maxSalary);

            else
                this.txtSendMessage.Text = "您好，最近有换工作的想法么？";

        }

        #endregion

        #region "对内函数"
        private MagicCube.ViewModel.HttpSearchResumModel GetHttpConditionModel(SearchConditionSaveModel model)
        {
            ViewModel.HttpSearchResumModel httpModel = new ViewModel.HttpSearchResumModel();
            if(model.expCity != null)
            {
                if (model.expCity.Count > 0)
                {
                    if (model.expCity[0].name == null)
                    {
                        httpModel.cityCode = null;
                    }
                    else
                    {
                        if (model.expCity[0].code != "0")
                            httpModel.cityCode = model.expCity[0].code.ToString();
                    }
                }
            }
    

            httpModel.words = model.words.Trim();
            string[] salaryArr = new string[] { "0", "1", "2", "3", "4", "5", "6", "8", "10", "15", "20", "25", "30", "40", "50", "70", "100" };
         
            httpModel.salary = httpModel.RangeConvert(salaryArr[Convert.ToInt32(model.salary.minSalaryCode)], salaryArr[Convert.ToInt32(model.salary.maxSalaryCode)]);

            string[] workingExpArr = new string[] { "不限", "在读学生", "应届毕业生", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            if (model.workExp != null )
            {
                if (model.workExp.minWorkExp !=null && model.workExp.minWorkExp.code == "0")
                {
                    if (model.workExp.maxWorkExp.code == "3" && model.workExp.minWorkExp.code == "0")
                    {
                        httpModel.workExp = "$lte:1";
                    }
                    else
                    {
                        httpModel.workExp = null;
                        httpModel.workExpType = 0;
                    }
                }
                else if (model.workExp.minWorkExp != null && model.workExp.minWorkExp.code == "1")
                {
                    httpModel.workExp = null;
                    httpModel.workExpType = 1;
                }
                else if (model.workExp.minWorkExp != null && model.workExp.minWorkExp.code == "2")
                {
                    httpModel.workExp = null;
                    httpModel.workExpType = 2;
                }
               
                else if (model.workExp.maxWorkExp.code == "0"&&Convert.ToInt32(model.workExp.maxWorkExp.code) > 2)
                {
                    httpModel.workExp = string.Format("$gte:{0}",workingExpArr[Convert.ToInt32(model.workExp.maxWorkExp.code)]);
                }
                else
                {
                    httpModel.workExp = httpModel.RangeConvert(workingExpArr[Convert.ToInt32(model.workExp.minWorkExp.code)], workingExpArr[Convert.ToInt32(model.workExp.maxWorkExp.code)]);
                }
            }
            

           
            httpModel.companyName = model.companyName;
            httpModel.onlyLastCompany = model.onlyLastCompany;

            List<string> TempList = new List<string>();
            foreach (var item in model.jobType)
            {
                TempList.Add(item.code.ToString());
            }
            httpModel.thirdJobCode = httpModel.INConvert(TempList);

            TempList.Clear();
            foreach (var item in model.industry)
            {
                TempList.Add(item.code.ToString());
            }
            httpModel.industry = httpModel.INConvert(TempList);
            if(model.age != null)
            {
                if (!string.IsNullOrEmpty(model.age.minAge) && !string.IsNullOrEmpty(model.age.maxAge))
                    httpModel.age = httpModel.RangeConvert(model.age.minAge, model.age.maxAge);
            }
            if(model.degree !=null)
            {
                if(model.degree.maxDegreeCode == "7")
                {
                    httpModel.education = httpModel.RangeConvert(model.degree.minDegreeCode, "0");
                }
                httpModel.education = httpModel.RangeConvert(model.degree.minDegreeCode, model.degree.maxDegreeCode);
                if(model.degree.expand !=null)
                {
                    foreach (var item in model.degree.expand)
                    {
                        switch (item.code)
                        {
                            case "0":
                                httpModel.group985 = true;
                                break;
                            case "1":
                                httpModel.group211 = true;
                                break;
                            case "2":
                                httpModel.fullTime = true;
                                break;
                            case "3":
                                httpModel.oversea = true;
                                break;
                        }
                    }
                }
               
            }

         

            TempList.Clear();
            
            if(model.address != null)
            {
                foreach (var item in model.address)
                {
                    if (item.name != null)
                    {
                        if(item.code != "0")
                            TempList.Add(item.code.ToString());
                    }
                }
                httpModel.address = httpModel.INConvert(TempList);
            }
            
            httpModel.gender = model.gender;
            if(model.updateTime != null)
            {
                if (model.updateTime == "0")
                {
                    httpModel.updateTime = null;
                }
                else
                {
                    httpModel.updateTime = model.updateTime;
                }
            }
          
            httpModel.school = model.school;
            httpModel.major = model.major;
            if(model.language != null)
            {
                if (model.language == "不限" || model.language == "0")
                {
                    httpModel.language = null;
                }
                else
                {
                    httpModel.language = httpModel.INConvert(new List<string> { model.language}); 
                }
            }
            if(model.status !=null)
            {
                if (model.status == "0")
                    httpModel.status = null;
                else
                    httpModel.status = string.Format("$in:[{0}]", model.status);
            }
           




            return httpModel;
        }

        private void SetConditionModelToHttpSearchModel(int page)
        {
            ViewModel.HttpSearchResumModel model = GetHttpConditionModel(this.ConditionSaveModel);
            model.page = page - 1;
            model.size = DAL.ConfUtil.PageContent;
            model.sort = 0;
            model.userID = MagicGlobal.UserInfo.Id;
            model.appKey = "xiaopin";
            model.properties = ViewModel.ModelTools.SetHttpPropertys<ViewModel.HttpResumeItemModel>();
            model.desc = "fullName";
            this.HttpSearchModel = model;
        }

        private async Task UpdateTable(int page)
        {
            try
            {

                this.busyCtrl.IsBusy = true;
                this.GdNoJob.Visibility = Visibility.Collapsed;
                //searchingTanlentResumModel.Clear();
                //ViewModel.HttpSearchResumModel model = GetHttpConditionModel(this.ConditionSaveModel);
                //model.page = page - 1;
                //model.size = DAL.ConfUtil.PageContent;
                //model.sort = 0;
                //model.userID = MagicGlobal.UserInfo.Id;
                //model.appKey = "xiaopin";
                //model.properties = ViewModel.ModelTools.SetHttpPropertys<ViewModel.HttpResumeItemModel>();
                //model.desc = "fullName";
                //测试
                this.HttpSearchModel.page = page - 1;
                this.HttpSearchModel.size = DAL.ConfUtil.PageContent;
                this.HttpSearchModel.sort = 0;
                this.HttpSearchModel.userID = MagicGlobal.UserInfo.Id;
                this.HttpSearchModel.appKey = "xiaopin";
                this.HttpSearchModel.desc = "fullName";
                string std = Uri.EscapeDataString(DAL.JsonHelper.ToJsonString(this.HttpSearchModel));
                //string std = DAL.JsonHelper.JsonParamsToString(new string[] {"userID", "words", "page","size" }, new string[] { "267", "销售","0","15" }); 
                string temp = string.Format(DAL.ConfUtil.AddrUpdateSearch, MagicGlobal.UserInfo.Version, std);
                string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(temp);
                this.busyCtrl.IsBusy = false;
                ViewModel.BaseHttpModel<ViewModel.HttpSearchResumeList> ResultModel = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpSearchResumeList>>(resultStr);
                if (ResultModel == null)
                {
                    this.GdNoJob.Visibility = Visibility.Visible;
                    this.txtNoSesumTag.Text = "没有搜索到适合的简历，请修改搜索条件试试吧!";
                    return;
                }
                else
                {
                    if (ResultModel.code == 200)
                    {
                        this.GdNoJob.Visibility = Visibility.Collapsed;
                        totalData = ResultModel.data.count;
                        totalPage = ResultModel.data.pageCount;
                        curPage = page;
                        if (totalData == 0)
                            curPage = 0;

                        ucPageTurn.setPageState(page, totalPage, totalData);

                        //获取列表成功
                        //修改
                        foreach (ViewModel.HttpResumeItemModel item in ResultModel.data.resumeList)
                        {
                            if (item.gender == "0")
                                item.avatar = Downloader.LocalPath(item.avatar, item.name, true);
                            else
                                item.avatar = Downloader.LocalPath(item.avatar, item.name, false);
                        }


                        //埋点
                        ViewModel.HttpTrackEventDataModel eventModel = new HttpTrackEventDataModel();
                        eventModel.session = Guid.NewGuid().ToString();
                        this.searchSession = eventModel.session;
                        eventModel.algo = "se";
                        eventModel.page = page.ToString();
                        eventModel.item = new List<string>();
                        //从model到VIEWmodel
                        searchingTanlentResumModel = new List<SearchTalentResumModel>();
                        foreach (ViewModel.HttpResumeItemModel item in ResultModel.data.resumeList)
                        {
                            searchingTanlentResumModel.Add(this.ModelToViewModel(item));
                            eventModel.item.Add(item.userID);

                        }
                        eventModel.userID = MagicGlobal.UserInfo.Id.ToString();

                        Common.TrackHelper2.TrackOperation("5.6.4.2.1", "rv", DAL.JsonHelper.ToJsonString(eventModel));

                        ucThumbNailResum.lstThumbnailResume.ItemsSource = searchingTanlentResumModel;
                        this.tbResumeSearch.ItemsSource = searchingTanlentResumModel;
                        setJudgeAllEnable();


                        if (searchingTanlentResumModel.Count == 0)
                        {

                            this.GdNoJob.Visibility = Visibility.Visible;
                            this.txtNoSesumTag.Text = "没有搜索到适合的简历，请修改搜索条件试试吧!";
                            return;
                        }
                        else
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                ucThumbNailResum.SvConversation.ScrollToTop();
                                tbResumeSearch.ScrollIntoView(tbResumeSearch.Items[0]);
                            }));

                        }

                    }
                    else
                    {
                        this.GdNoJob.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }





            //this.busyCtrl.IsBusy = true;
            //this.GdNoJob.Visibility = System.Windows.Visibility.Collapsed;
            //Action method = delegate
            //{
            //    string pResult = DAL.HttpHelper.Instance.HttpPost(string.Format(url, (page - 1) * ConfUtil.PageContent, ConfUtil.PageContent), JsonUtil.TalentResearch(saveSearchCondition.HttpCondition, page));
            //    this.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        this.busyCtrl.IsBusy = false;
            //    }));
            //    if (pResult.StartsWith("连接失败"))
            //    {
            //        this.Dispatcher.BeginInvoke(new Action(() =>
            //        {
            //            LogHelper.WriteLog("人才搜索列表加载失败：" + pResult);
            //            WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
            //            pWinMessage.Owner = Window.GetWindow(this);
            //            pWinMessage.ShowDialog();
            //        }));

            //        return;
            //    }
            //    searchingTotalResult = Util.JsonUtil.ToObject<SearchingTotalResult>(pResult);
            //    if (searchingTotalResult == null)
            //    {
            //        return;
            //    }

            //    totalData = searchingTotalResult.page.dataTotal;
            //    totalPage = totalData % ConfUtil.PageContent > 0 ? (totalData / ConfUtil.PageContent) + 1 : totalData / ConfUtil.PageContent;
            //    curPage = page;
            //    if (totalData == 0)
            //        curPage = 0;
            //    this.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        ucPageTurn.setPageState(page, totalPage, totalData);
            //    }));

            //    //更新列表数据avatarUrl
            //    //修改
            //    foreach (HttpSearchingResult item in searchingTotalResult.results)
            //    {
            //        item.avatarUrl = Downloader.LocalPath(item.avatarUrl, item.name);
            //    }
            //    //从model到VIEWmodel
            //    searchingTanlentResumModel = new List<SearchTalentResumModel>();
            //    foreach (HttpSearchingResult item in searchingTotalResult.results)
            //    {
            //        searchingTanlentResumModel.Add(this.ModelToViewModel(item));

            //    }
            //    this.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        ucThumbNailResum.lstThumbnailResume.ItemsSource = searchingTanlentResumModel;
            //        this.tbResumeSearch.ItemsSource = searchingTanlentResumModel;
            //        setJudgeAllEnable();
            //    }));




            //    if (searchingTanlentResumModel.Count == 0)
            //    {
            //        this.Dispatcher.BeginInvoke(new Action(() =>
            //        {
            //            this.GdNoJob.Visibility = System.Windows.Visibility.Visible;
            //            if (url == ConfUtil.ServerTelantSearch)
            //                this.txtNoSesumTag.Text = "没有搜索到适合的简历，请修改搜索条件试试吧!";
            //            else if (url == ConfUtil.ServerHistorySearch)
            //            {
            //                this.txtNoSesumTag.Text = "暂无我主动联系的人才.";
            //            }
            //        }));

            //        return;
            //    }
            //    else
            //    {
            //        this.Dispatcher.BeginInvoke(new Action(() =>
            //        {
            //            ucThumbNailResum.SvConversation.ScrollToTop();
            //            tbResumeSearch.ScrollIntoView(tbResumeSearch.Items[0]);
            //        }));

            //    }
            //    return;
            //};
            //method.BeginInvoke(null, null);

        }
        private SearchTalentResumModel ModelToViewModel(ViewModel.HttpResumeItemModel httpSearch)
        {
            try
            {
                SearchTalentResumModel temp = new SearchTalentResumModel();
                //temp.activeTime = httpSearch.activeTime;
                //DateTime birthday = Convert.ToDateTime(httpSearch.birthdate);
                //temp.age = Convert.ToString(DateTime.Now.Year - birthday.Year);
                temp.userId = Convert.ToInt32(httpSearch.userID);
                temp.dob = httpSearch.birthdate;
                temp.name = ResumeName.GetName(httpSearch.name); 
                temp.gender = httpSearch.genderDesc;
                temp.avatarUrl = httpSearch.avatar;
                temp.targetPosition = ViewModel.ModelTools.StringConvertToList(httpSearch.exptPositionDesc);
                if (string.IsNullOrEmpty(httpSearch.resumeUpdateTime))
                {
                    temp.activeTime = string.Empty;
                }
                else
                {
                    DateTime dt = Convert.ToDateTime(httpSearch.resumeUpdateTime);
                    temp.activeTime = dt.ToString("yyyy-MM-dd");

                }

                //最近工作
                List<ViewModel.ExperienceJobModel> ExperienceJob = DAL.JsonHelper.ToObject<List<ViewModel.ExperienceJobModel>>(httpSearch.personExperienceJob);
                if (ExperienceJob != null)
                {
                    if (ExperienceJob.Count > 0)
                    {
                        if (ExperienceJob[0].name != null && ExperienceJob[0].companyName != null)
                            temp.careerStr = ExperienceJob[0].name + "-" + ExperienceJob[0].companyName;
                    }

                    temp.career = new List<Career>();
                    foreach (var item in ExperienceJob)
                    {
                        try
                        {
                            if (item.workEndTime.Contains("9999"))
                            {
                                DateTime dtStart = Convert.ToDateTime(item.workStartTime);
                                temp.career.Add(new Career() { companyName = item.companyName, jobName = item.name, start = dtStart.ToString("yyyy.MM"), end = "至今", jobDescription = item.workDesc });

                            }
                            else if (item.workEndTime == "9999-01-01")
                            {
                                DateTime dtStart = Convert.ToDateTime(item.workStartTime);
                                temp.career.Add(new Career() { companyName = item.companyName, jobName = item.name, start = dtStart.ToString("yyyy.MM"), end = "至今", jobDescription = item.workDesc });
                            }
                            else if (item.workEndTime.Contains("今"))
                            {
                                DateTime dtStart = Convert.ToDateTime(item.workStartTime);
                                temp.career.Add(new Career() { companyName = item.companyName, jobName = item.name, start = dtStart.ToString("yyyy.MM"), end = "至今", jobDescription = item.workDesc });
                            }
                            else
                            {
                                DateTime dtStart = Convert.ToDateTime(item.workStartTime);
                                DateTime dtEnd = Convert.ToDateTime(item.workEndTime);
                                temp.career.Add(new Career() { companyName = item.companyName, jobName = item.name, start = dtStart.ToString("yyyy.MM"), end = dtEnd.ToString("yyyy.MM"), jobDescription = item.workDesc });
                            }
                            if (temp.career.Count >= 2)
                                break;
                        }
                        catch
                        {

                        }
                       
                    }
                }


                temp.degree = httpSearch.educationDesc;
                temp.workingExp = httpSearch.workingExp;                
                if (!string.IsNullOrEmpty(httpSearch.district))
                {
                    temp.location = CityCodeHelper.GetCityName(httpSearch.district);
                }
                else if (!string.IsNullOrEmpty(httpSearch.city))
                {
                    temp.location = CityCodeHelper.GetCityName(httpSearch.city);
                }
                else if (!string.IsNullOrEmpty(httpSearch.province))
                {
                    temp.location = CityCodeHelper.GetCityName(httpSearch.province);
                }

                temp.isRead = Convert.ToBoolean(httpSearch.viewedResume);
              
                return temp;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }

        private void setJudgeAllEnable()
        {
            //if ((tbResumeSearch.ItemsSource as ObservableCollection<SearchingResultBind>).Where(x => x.IsCheck == true).Count() < 1)
            //{
            //    btnJudgeAll.IsEnabled = false;
            //}
            //else
            //{
            //    btnJudgeAll.IsEnabled = true;
            //}
            if ((tbResumeSearch.ItemsSource as List<SearchTalentResumModel>).Count == 0)
            {
                CheckAll.IsChecked = false;
                return;
            }
            if ((tbResumeSearch.ItemsSource as List<SearchTalentResumModel>).Where(x => x.IsChoosed == true).Count() == (tbResumeSearch.ItemsSource as List<SearchTalentResumModel>).Count())
            {
                CheckAll.IsChecked = true;
            }
            else
            {
                CheckAll.IsChecked = false;
            }
        }

        /// <summary>
        /// 打开简历聊天对话框
        /// </summary>
        /// <param name="unikey"></param>
        /// <param name="jobId"></param>
        private void OpenResumIMWindow(string unikey,string jobId)
        {
          
            ////string gResult = DAL.HttpHelper.Instance.HttpGet(string.Format(ConfUtil.ServerGetConstactRecord, unikey, jobId));
            //string gResult = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerContactRecord, JsonUtil.ContactRecord(Convert.ToInt32(jobId), 0, "", unikey));
            //if (gResult.StartsWith("连接失败"))
            //{
            //    this.Dispatcher.Invoke(new Action(() =>
            //    {
            //        WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
            //        pWinMessage.Owner = Window.GetWindow(this);
            //        if (pWinMessage.Owner == null)
            //            return;
            //        pWinMessage.ShowDialog();
            //        this.busyCtrl.IsBusy = false;
            //    }));

            //    return;
            //}
            //HttpContactRecordGet constactRecordIdGet = DAL.JsonHelper.ToObject<HttpContactRecordGet>(gResult);
            //this.Dispatcher.Invoke(new Action(() =>
            //{
            //    selectedSearchingResult.jobId = jobId;
            //    this.busyCtrl.IsBusy = false;

            //    //if (OpenIMAction != null)
            //    //    OpenIMAction(constactRecordIdGet.recordId, content);

            //    TrackAction.TrackSet("INTERVIEW_REQUESTS", constactRecordIdGet.recordId.ToString());
            //    TrackAction.TrackSet("Face_INTERACTION", uniqueKey);

            //    GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk, constactRecordIdGet.recordId.ToString()));

            //    this.busyCtrl.IsBusy = false;
            //}));
        }
        /// <summary>
        /// 侧边栏显示
        /// </summary>
        /// <param name="visible"></param>
        /// <param name="visiblePnl">0：显示简历框，1：显示面试邀请框, 2:显示搜索条件页面</param>
        private void SidebarVisibility(bool visible, int visiblePnl = 0)
        {

            if (visible)
            {
                this.gdSidebar.Visibility = Visibility.Visible;
                switch (visiblePnl)
                {
                    case 0:
                        scrollResum.ScrollToTop();
                        this.gdResum.Visibility = Visibility.Visible;
                        this.gdPublishOfferMutiple.Visibility = Visibility.Collapsed;
                        this.gdConditionSearch.Visibility = Visibility.Collapsed;
                        break;
                    case 1:
                        this.gdResum.Visibility = Visibility.Collapsed;
                        this.gdPublishOfferMutiple.Visibility = Visibility.Visible;
                        this.gdConditionSearch.Visibility = Visibility.Collapsed;
                        break;
                    case 2:
                        this.gdResum.Visibility = Visibility.Collapsed;
                        this.gdPublishOfferMutiple.Visibility = Visibility.Collapsed;
                        this.gdConditionSearch.Visibility = Visibility.Visible;
                        break;

                }
            }
            else
            {
                this.gdSidebar.Visibility = Visibility.Collapsed;
            }
        }


        private async Task UpdateResumeSidebara()
        {
           
            BaseHttpModel<HttpResumeDetial> model = await IMHelper.GetDetialReusme(selectedSearchingResult.userId.ToString());
            busyCtrl.IsBusy = false;
            if (model == null)
            {
                WinErroTip pWinErroTip = new WinErroTip(DAL.ConfUtil.netError);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return;
            }
            if (model.code != 200)
            {
                WinErroTip pWinErroTip = new WinErroTip(model.msg);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return;
            }
            curResumeDetailModel = SetModel.SetResumeDetailModel(model.data);
            ucResume.iniResumeComment();
            ucResume.DataContext = curResumeDetailModel;
            btnCollection.DataContext = curResumeDetailModel;
            //埋点
            ViewModel.HttpTrackEventDataModel eventModel = new HttpTrackEventDataModel();
            eventModel.session = this.searchSession;
            eventModel.algo = "se";
            eventModel.item = new List<string>();
            eventModel.item.Add(selectedSearchingResult.userId.ToString());
            eventModel.page = Convert.ToString(this.curPage - 1);
            for (int i = 0; i < searchingTanlentResumModel.Count; i++)
            {
                if (searchingTanlentResumModel[i].userId == selectedSearchingResult.userId)
                {
                    eventModel.location = i.ToString();
                    break;
                }
            }
            eventModel.userID = MagicGlobal.UserInfo.Id.ToString();
            Common.TrackHelper2.TrackOperation("5.6.4.10.1", "clk", DAL.JsonHelper.ToJsonString(eventModel));

            Common.TrackHelper2.TrackOperation("5.6.5.2.1", "lv", DAL.JsonHelper.ToJsonString(eventModel));

           
            Common.TrackHelper2.TrackOperation("5.6.5.1.1", "pv", DAL.JsonHelper.ToJsonString(eventModel));
        }
        #endregion

        #region "对外函数"
        public async Task LoadingResult(SearchConditionSaveModel saveSearchCondition)
        {
            this.ConditionSaveModel = saveSearchCondition;
            ucPageTurn.tbChangePage.Text = string.Empty;
            this.SidebarVisibility(false);
            this.SetConditionModelToHttpSearchModel(1);
            await this.UpdateTable(1);


        }

        public void InitialVisible()
        {
            //this.gdPublishOfferMutiple.Visibility = Visibility.Collapsed;
            //this.GdShowResum.Visibility = System.Windows.Visibility.Collapsed;
            //if (ResumShadeAction != null)
            //    ResumShadeAction(false);
            //this.GdRusumJobPublish.Visibility = System.Windows.Visibility.Collapsed;
            //AB测试
            if (MagicGlobal.UserInfo.Id % 2 == 0)
            {
                rdoThumbnail.IsChecked = true;
                rdoList.IsChecked = false;
            }
            else
            {
                rdoThumbnail.IsChecked = false;
                rdoList.IsChecked = true;
            }
        }

        public void ReturnRapidSearchCondition(SaveSearchCondition CurrentsaveCondition)
        {
            this.SidebarVisibility(true, 2);
            this.ucRapidSearchCondition.SetSearchListFromStr(CurrentsaveCondition);


        }
        #endregion

        #region "回调函数"
        private async Task ResumeMoreShowCallback(SearchTalentResumModel resultItem)
        {
            selectedSearchingResult = resultItem;
            if (resultItem == null)
                return;

            this.busyCtrl.IsBusy = true;
            await this.UpdateResumeSidebara();
            resultItem.isRead = true;
            this.SidebarVisibility(true, 0);
            return;

        }

        private async Task ResumSendOfferCallback(SearchTalentResumModel resultItem)
        {
            this.popMainMenu.IsOpen = false;
            this.ResumImageUrlList.Clear();
            this.selectedResumModel.Clear();
            this.selectedSearchingResult = resultItem;
            this.ResumImageUrlList.Add(this.selectedSearchingResult.avatarUrl);
            this.selectedResumModel.Add(this.selectedSearchingResult);

            //获取职位列表
            this.busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "status", "jobType" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "1","2" });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<HttpJobListData> listModel = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(resultStr);
            this.busyCtrl.IsBusy = false;
            if (listModel == null)
            {
                WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                return;
            }
            else
            {
                if (listModel.code == 200)
                {
                    if (listModel.data.data.Count == 0)
                    {
                        WinPublishJobMessage msg = new WinPublishJobMessage();
                        msg.Owner = Window.GetWindow(this);
                        msg.atConfirmTask += PublishJobMsgCallback;
                        msg.ShowDialog();

                        return;
                    }
                    this.JobPublishList.Clear();
                    this.JobPublishListcmb.Clear();
                    foreach (var item in listModel.data.data)
                    {
                        this.JobPublishList.Add(new JobPublish() { id = (int)item.jobID, jobName = item.jobName, minSalary = item.jobMinSalary.ToString(), maxSalary = item.jobMaxSalary.ToString() });
                        this.JobPublishListcmb.Add(item.jobName);
                    }
                    if (listModel.data.data.Count == 1)
                        this.cmbPulishJobList.SelectedIndex = 0;
                    this.tbPublishOfferNum.Text = "1";
                    this.SidebarVisibility(true, 1);
                }
                else
                {
                    WinErroTip pWinMessage = new WinErroTip(listModel.msg);
                    pWinMessage.Owner = Window.GetWindow(this);
                    pWinMessage.ShowDialog();
                    return;
                }
            }

        }

        private async Task ResumOfferConnectToSingleCallback(SearchTalentResumModel result)
        {
            this.popMainMenu.IsOpen = false;
            this.busyCtrl.IsBusy = true;
            this.selectedSearchingResult = result;
            BaseHttpModel<HttpRecentList> model = await IMHelper.GetSesionInfo(this.selectedSearchingResult.userId);
            this.busyCtrl.IsBusy = false;
            if (model == null)
            {
                return;
            }
            if (model.code == 200)
            {

                    if (model.data.jobID > 0)
                    {
                        this.selectedSearchingResult.jobId = model.data.jobID.ToString();
                        GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk, new UserJobModel() { jobId = this.selectedSearchingResult.jobId, userId = this.selectedSearchingResult.userId.ToString() }));
                        return;
                    }

                
            }
            ObservableCollection<JobPublish> tempJobPublish = new ObservableCollection<JobPublish>();
            this.busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "status", "jobType" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "1","2" });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<HttpJobListData> listModel = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(resultStr);
            this.busyCtrl.IsBusy = false;
            if (listModel == null)
            {
                WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                return;
            }
            else
            {
                if (listModel.code == 200)
                {
                    foreach (var item in listModel.data.data)
                    {
                        tempJobPublish.Add(new JobPublish() { id = Convert.ToInt32(item.jobID), jobName = item.jobName });
                    }
                    if (tempJobPublish.Count == 0)
                    {
                        WinPublishJobMessage msg = new WinPublishJobMessage();
                        msg.Owner = Window.GetWindow(this);
                        msg.atConfirmTask += PublishJobMsgCallback;
                        msg.ShowDialog();
                        return;
                    }
                    //打开职位列表选择对话框
                    GlobalEvent.GlobalEventHandler(new UserJobModel() { jobId = selectedSearchingResult.jobId, userId = selectedSearchingResult.userId.ToString() }, new GlobalEventArgs(GlobalFlag.JobPublishList, tempJobPublish));
                }
                else
                {
                    WinErroTip pWinMessage = new WinErroTip(listModel.msg);
                    pWinMessage.Owner = Window.GetWindow(this);
                    pWinMessage.ShowDialog();
                    return;
                }
            }


        }

        private void IsCheckedFromThumbnailCallback(bool flag)
        {
            setJudgeAllEnable();
        }

        private void RapidSearchConditionCallback(SaveSearchCondition condition)
        {
            //this.saveSearchCondition = condition;
            //this.UpdateTable(1, ConfUtil.ServerTelantSearch);
            //this.SidebarVisibility(false);
            if (this.RapidSearchConditionAction != null)
            {
                this.RapidSearchConditionAction(condition);
            }
        }

        private async Task PageChangeCallback(int curPage)
        {
            //this.SetConditionModelToHttpSearchModel(curPage);
            await UpdateTable(curPage);
        }

        private async void SearchResumCallback(Messaging.MSJobSearchConditionModel model)
        {
            this.HttpSearchModel = model.model;
            await UpdateTable(1);
        }
        #endregion

        #region "标签"
        //红标事件
        private void chkRedFlag_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    this.Cursor = Cursors.Wait;

            //    CheckBox chk = sender as CheckBox;
            //    SearchTalentResumModel sch = chk.DataContext as SearchTalentResumModel;
            //    sch.RedFlag = (bool)chk.IsChecked;
            //    sch.flag = sch.RedFlag.ToString();
            //    HttpResumeGetTags httpResumeTag = new HttpResumeGetTags() { resumeId = sch.resumeId, flag = sch.RedFlag.ToString(), tags = new Dictionary<string, string>() };
            //    if (sch.tagVs == null)
            //        sch.tagVs = new List<TagV>();
            //    foreach (var item in sch.tagVs)
            //    {
            //        httpResumeTag.tags.Add(item.id.ToString(), item.name);
            //    }
            //    string httpSend = DAL.JsonHelper.ToJsonString(httpResumeTag);
            //    string result = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerResumeRemarkTags, httpSend);
            //    if (!result.Contains("200"))
            //    {
            //        WinErroTip message = new WinErroTip("网络异常，请重试");
            //        message.Owner = Window.GetWindow(this);
            //        message.ShowDialog();
            //        sch.RedFlag = !sch.RedFlag;
            //    }

            //    Console.WriteLine("打红标结果:" + result);
            //    // string sendStr = Util.JsonUtil.ToJsonString(sendHttp);
            //    //string result =  DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerResumeRemarkTags, sendStr);
            //    //Console.WriteLine(result);
            //    this.Cursor = Cursors.Arrow;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("错误信息：" + ex.Message);
            //    this.Cursor = Cursors.Arrow;
            //}

        }
        /// <summary>
        /// 右键红标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRightMenuRedFlag_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    this.Cursor = Cursors.Wait;

            //    CheckBox chk = sender as CheckBox;
            //    SearchTalentResumModel sch = currentTagsEditItem;
            //    sch.RedFlag = (bool)chk.IsChecked;
            //    sch.flag = sch.RedFlag.ToString();
            //    HttpResumeGetTags httpResumeTag = new HttpResumeGetTags() { resumeId = sch.resumeId, flag = sch.RedFlag.ToString(), tags = new Dictionary<string, string>() };
            //    if (sch.tagVs == null)
            //        sch.tagVs = new List<TagV>();
            //    foreach (var item in sch.tagVs)
            //    {
            //        httpResumeTag.tags.Add(item.id.ToString(), item.name);
            //    }
            //    string httpSend = DAL.JsonHelper.ToJsonString(httpResumeTag);
            //    string result = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerResumeRemarkTags, httpSend);
            //    if (!result.Contains("200"))
            //    {
            //        WinErroTip message = new WinErroTip("网络异常，请重试");
            //        message.Owner = Window.GetWindow(this);
            //        message.ShowDialog();
            //        sch.RedFlag = !sch.RedFlag;
            //    }

            //    Console.WriteLine("打红标结果:" + result);
            //    // string sendStr = Util.JsonUtil.ToJsonString(sendHttp);
            //    //string result =  DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerResumeRemarkTags, sendStr);
            //    //Console.WriteLine(result);
            //    this.Cursor = Cursors.Arrow;
            //}
            //catch
            //{
            //    this.Cursor = Cursors.Arrow;
            //}
        }
        private SearchTalentResumModel currentTagsEditItem;
        private List<TagsMenuModel> menuList;
        /// <summary>
        /// 标签编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTagsEdit_Click(object sender, RoutedEventArgs e)
        {
            PopMenu.PlacementTarget = (sender as Button);
            currentTagsEditItem = (sender as Button).DataContext as SearchTalentResumModel;
            menuList = new List<TagsMenuModel>();
            foreach (var item in MagicGlobal.TagsManagerList)
            {
                bool isChecked = false;
                if (currentTagsEditItem.tagVs != null)
                {
                    foreach (var temp in currentTagsEditItem.tagVs)
                    {
                        if (temp == null)
                            continue;
                        if (temp.id == item.Id)
                        {
                            isChecked = true;
                            break;
                        }
                    }
                }
                menuList.Add(new TagsMenuModel() { Id = item.Id, IsChecked = isChecked, TagColor = item.TagColor, TagContent = item.TagContent });
            }
            this.popTagsList.ItemsSource = menuList;
            PopMenu.IsOpen = true;
        }


        //标签事件
        private void chkTagRemark_Click(object sender, RoutedEventArgs e)
        {
            //this.Cursor = Cursors.Wait;
            //if (currentTagsEditItem == null)
            //    return;
            //CheckBox chk = sender as CheckBox;
            //TagsMenuModel tagsModel = chk.DataContext as TagsMenuModel;
            //if (tagsModel != null)
            //{
            //    HttpResumeGetTags resumeGetTags = new HttpResumeGetTags() { tags = new Dictionary<string, string>(), uniqueKey = currentTagsEditItem.uniqueKey };
            //    foreach (var item in menuList)
            //    {
            //        if (item.IsChecked)
            //        {
            //            resumeGetTags.tags.Add(item.Id.ToString(), item.TagContent);
            //        }
            //    }
            //    if (currentTagsEditItem.RedFlag)
            //    {
            //        resumeGetTags.flag = "True";
            //    }
            //    else
            //    {
            //        resumeGetTags.flag = "False";
            //    }

            //    string sendHttp = DAL.JsonHelper.ToJsonString(resumeGetTags);
            //    sendHttp = sendHttp.Replace("\"resumeId\": 0,", "");
            //    string result = DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerResumeRemarkTags, sendHttp);
            //    if (result.Contains("200"))
            //    {
            //        currentTagsEditItem.TagsList.Clear();
            //        if (currentTagsEditItem.tagVs == null)
            //        {
            //            currentTagsEditItem.tagVs = new List<TagV>();
            //        }
            //        currentTagsEditItem.tagVs.Clear();
            //        foreach (var item in menuList)
            //        {
            //            if (item.IsChecked)
            //            {
            //                //currentTagsEditItem.TagsList.Add(item.TagColor);
            //                currentTagsEditItem.TagsList.Add(new TagsManagerModel() { TagColor = item.TagColor, TagContent = item.TagContent });
            //                currentTagsEditItem.tagVs.Add(new TagV() { color = item.TagColor, name = item.TagContent, id = item.Id });
            //            }
            //        }
            //    }
            //    else
            //    {
            //        DisappearShow ds = new DisappearShow("网络异常，添加失败.", 1);
            //        ds.Owner = Window.GetWindow(this);
            //        ds.Show();
            //    }
            //    Console.WriteLine("打标签结果:" + result);
            //}
            //this.Cursor = Cursors.Arrow;
        }
        #endregion
        //标记设置按钮，弹出设置管理框
        private void btnTagManager_Click(object sender, RoutedEventArgs e)
        {
            GlobalEvent.GlobalEventHandler(this, new GlobalEventArgs(GlobalFlag.openTagsManager, null));
            PopMenu.IsOpen = false;
            //this.contextMenu.IsOpen = false;
        }


        /// <summary>
        /// 右键菜单打开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            SearchTalentResumModel sch = tbResumeSearch.SelectedItem as SearchTalentResumModel;
            selectedSearchingResult = sch;
            if (sch == null)
                return;
            currentTagsEditItem = sch;
            menuList = new List<TagsMenuModel>();
            foreach (var item in MagicGlobal.TagsManagerList)
            {
                bool isChecked = false;
                if (sch.tagVs != null)
                {
                    foreach (var temp in sch.tagVs)
                    {
                        if (temp.id == item.Id)
                        {
                            isChecked = true;
                            break;
                        }
                    }
                }
                menuList.Add(new TagsMenuModel() { Id = item.Id, IsChecked = isChecked, TagColor = item.TagColor, TagContent = item.TagContent });
            }
            //this.popTagsList2.ItemsSource = menuList;
            //this.chkRightMenuRedFlag.IsChecked = sch.RedFlag;
        }

        private void btnTagAdd_Click(object sender, RoutedEventArgs e)
        {
            GlobalEvent.GlobalEventHandler(this, new GlobalEventArgs(GlobalFlag.openTagsAdd, null));
            PopMenu.IsOpen = false;
            //this.contextMenu.IsOpen = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.SidebarVisibility(false, 0);
        }

        /// <summary>
        /// 右键菜单免费沟通按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            ViewModel.HttpTrackEventDataModel eventModel = new HttpTrackEventDataModel();
            eventModel.session = this.searchSession;
            eventModel.algo = "se";
            eventModel.item = new List<string>();
            eventModel.item.Add(selectedSearchingResult.userId.ToString());
            eventModel.page = Convert.ToString(this.curPage - 1);
            for (int i = 0; i < searchingTanlentResumModel.Count; i++)
            {
                if (searchingTanlentResumModel[i].userId == selectedSearchingResult.userId)
                {
                    eventModel.location = i.ToString();
                    break;
                }
            }
            Common.TrackHelper2.TrackOperation("5.6.5.7.1", "clk", DAL.JsonHelper.ToJsonString(eventModel));



            this.popMainMenu.IsOpen = false;
            this.busyCtrl.IsBusy = true;
            BaseHttpModel<HttpRecentList> model = await IMHelper.GetSesionInfo(this.selectedSearchingResult.userId);
            this.busyCtrl.IsBusy = false;
            if (model == null)
            {
                return;
            }
            if (model.code == 200)
            {

                    if (model.data.jobID > 0)
                    {
                        selectedSearchingResult.jobId = model.data.jobID.ToString();
                        GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk,new UserJobModel() { jobId = this.selectedSearchingResult .jobId,userId = this.selectedSearchingResult.userId.ToString()}));
                        return;
                    }

                
            }
            ObservableCollection<JobPublish> tempJobPublish = new ObservableCollection<JobPublish>();
            this.busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "status", "jobType" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "1","2" });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<HttpJobListData> listModel = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(resultStr);
            this.busyCtrl.IsBusy = false;
            if (listModel == null)
            {
                WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                return;
            }
            else
            {
                if (listModel.code == 200)
                {
                    foreach (var item in listModel.data.data)
                    {
                        tempJobPublish.Add(new JobPublish() { id = Convert.ToInt32(item.jobID), jobName = item.jobName });
                    }
                    if(tempJobPublish.Count == 0)
                    {
                        WinPublishJobMessage msg = new WinPublishJobMessage();
                        msg.Owner = Window.GetWindow(this);
                        msg.atConfirmTask += PublishJobMsgCallback;
                        msg.ShowDialog();
                        return;
                    }
                    //打开职位列表选择对话框
                    GlobalEvent.GlobalEventHandler(new UserJobModel() { jobId = selectedSearchingResult.jobId, userId = selectedSearchingResult.userId.ToString() }, new GlobalEventArgs(GlobalFlag.JobPublishList, tempJobPublish));
                }
                else
                {
                    WinErroTip pWinMessage = new WinErroTip(listModel.msg);
                    pWinMessage.Owner = Window.GetWindow(this);
                    pWinMessage.ShowDialog();
                    return;
                }
            }


        }



        #region "操作下拉菜单按钮事件"
        private void btnOpenration_Click(object sender, RoutedEventArgs e)
        {

            this.selectedSearchingResult = (sender as Button).DataContext as SearchTalentResumModel;
            this.popMainMenu.IsOpen = true;
            this.popMainMenu.PlacementTarget = sender as Button;
            this.popMainMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint; 
            //this.popMainMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
        }

        private async void OpOpenResum_Click(object sender, RoutedEventArgs e)
        {
            
            if (selectedSearchingResult == null)
                return;
            this.busyCtrl.IsBusy = true;
            this.popMainMenu.IsOpen = false;
            await this.UpdateResumeSidebara();
            selectedSearchingResult.isRead = true;
            this.SidebarVisibility(true, 0);
            ViewModel.HttpTrackEventDataModel eventModel = new HttpTrackEventDataModel();
            eventModel.session = this.searchSession;
            eventModel.algo = "se";
            eventModel.item = new List<string>();
            eventModel.page = Convert.ToString(this.curPage - 1);
            eventModel.item.Add(selectedSearchingResult.userId.ToString());
            for (int i = 0; i < searchingTanlentResumModel.Count; i++)
            {
                if(searchingTanlentResumModel[i].userId == selectedSearchingResult.userId)
                {
                    eventModel.location = i.ToString();
                    
                    break;
                }
            }
            eventModel.userID = MagicGlobal.UserInfo.Id.ToString();
            Common.TrackHelper2.TrackOperation("5.6.4.10.1", "clk", DAL.JsonHelper.ToJsonString(eventModel));

            

        }

        private async void OpIMResum_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            ViewModel.HttpTrackEventDataModel eventModel = new HttpTrackEventDataModel();
            eventModel.session = this.searchSession;
            eventModel.algo = "se";
            eventModel.item = new List<string>();
            eventModel.item.Add(selectedSearchingResult.userId.ToString());
            eventModel.page = Convert.ToString(this.curPage - 1);
            for (int i = 0; i < searchingTanlentResumModel.Count; i++)
            {
                if (searchingTanlentResumModel[i].userId == selectedSearchingResult.userId)
                {
                    eventModel.location = i.ToString();
                    break;
                }
            }
            eventModel.userID = MagicGlobal.UserInfo.Id.ToString();
            Common.TrackHelper2.TrackOperation("5.6.4.12.1", "clk", DAL.JsonHelper.ToJsonString(eventModel));



            this.popMainMenu.IsOpen = false;
            this.busyCtrl.IsBusy = true;
            BaseHttpModel<HttpRecentList> model = await IMHelper.GetSesionInfo(this.selectedSearchingResult.userId);
            this.busyCtrl.IsBusy = false;
            if(model == null)
            {
                return;
            }
            if (model.code == 200)
            {
     
                    if (model.data.jobID > 0)
                    {
                        this.selectedSearchingResult.jobId = model.data.jobID.ToString();

                        GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk, new UserJobModel() { jobId = this.selectedSearchingResult.jobId, userId = this.selectedSearchingResult.userId.ToString() }));
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.data.latestMsgDesc))
                        {
                            this.selectedSearchingResult.jobId = "0";
                            GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openResumTalk, new UserJobModel() { jobId = this.selectedSearchingResult.jobId, userId = this.selectedSearchingResult.userId.ToString()}));
                            return;
                        }
                    }
                                    
            }
                ObservableCollection<JobPublish> tempJobPublish = new ObservableCollection<JobPublish>();
                this.busyCtrl.IsBusy = true;
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "status", "jobType" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "1","2" });
                string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
                ViewModel.BaseHttpModel<HttpJobListData> listModel = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(resultStr);
                this.busyCtrl.IsBusy = false;
                if (listModel == null)
                {
                    WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                    pWinMessage.Owner = Window.GetWindow(this);
                    pWinMessage.ShowDialog();
                    return;
                }
                else
                {
                    if (listModel.code == 200)
                    {
                        foreach (var item in listModel.data.data)
                        {
                            tempJobPublish.Add(new JobPublish() { id = Convert.ToInt32(item.jobID), jobName = item.jobName });
                        }
                    if (tempJobPublish.Count == 0)
                    {
                        WinPublishJobMessage msg = new WinPublishJobMessage();
                        msg.Owner = Window.GetWindow(this);
                        msg.atConfirmTask += PublishJobMsgCallback;
                        msg.ShowDialog();
                        return;
                    }
                    //打开职位列表选择对话框
                    GlobalEvent.GlobalEventHandler(new UserJobModel() { jobId = selectedSearchingResult.jobId, userId = selectedSearchingResult.userId.ToString() }, new GlobalEventArgs(GlobalFlag.JobPublishList, tempJobPublish));
                    }
                    else
                    {
                        WinErroTip pWinMessage = new WinErroTip(listModel.msg);
                        pWinMessage.Owner = Window.GetWindow(this);
                        pWinMessage.ShowDialog();
                        return;
                    }
                }


         
        }

        private async void OpSendOffer_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            ViewModel.HttpTrackEventDataModel eventModel = new HttpTrackEventDataModel();
            eventModel.session = this.searchSession;
            eventModel.algo = "se";
            eventModel.item = new List<string>();
            eventModel.item.Add(selectedSearchingResult.userId.ToString());
            eventModel.page = Convert.ToString(this.curPage - 1);
            for (int i = 0; i < searchingTanlentResumModel.Count; i++)
            {
                if (searchingTanlentResumModel[i].userId == selectedSearchingResult.userId)
                {
                    eventModel.location = i.ToString();
                    break;
                }
            }
            eventModel.userID = MagicGlobal.UserInfo.Id.ToString();
            Common.TrackHelper2.TrackOperation("5.6.4.13.1", "clk", DAL.JsonHelper.ToJsonString(eventModel));



            this.popMainMenu.IsOpen = false;
            this.ResumImageUrlList.Clear();
            this.selectedResumModel.Clear();
            this.ResumImageUrlList.Add(this.selectedSearchingResult.avatarUrl);
            this.selectedResumModel.Add(this.selectedSearchingResult);

            //获取职位列表
            this.busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "status", "jobType" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "1","2" });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<HttpJobListData> listModel = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(resultStr);
            this.busyCtrl.IsBusy = false;
            if (listModel == null)
            {
                WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                return;
            }
            else
            {
                if (listModel.code == 200)
                {
                    if (listModel.data.data.Count == 0)
                    {
                        WinPublishJobMessage msg = new WinPublishJobMessage();
                        msg.Owner = Window.GetWindow(this);
                        msg.atConfirmTask += PublishJobMsgCallback;
                        msg.ShowDialog();

                        return;
                    }
                    this.JobPublishList.Clear();
                    this.JobPublishListcmb.Clear();
                    foreach (var item in listModel.data.data)
                    {
                        this.JobPublishList.Add(new JobPublish() { id = (int)item.jobID, jobName = item.jobName, minSalary = item.jobMinSalary.ToString(),maxSalary = item.jobMaxSalary.ToString()});
                        this.JobPublishListcmb.Add(item.jobName);
                    }
                    if (listModel.data.data.Count == 1)
                        this.cmbPulishJobList.SelectedIndex = 0;
                    this.SidebarVisibility(true, 1);
                    this.tbPublishOfferNum.Text = "1";
                }
                else
                {
                    WinErroTip pWinMessage = new WinErroTip(listModel.msg);
                    pWinMessage.Owner = Window.GetWindow(this);
                    pWinMessage.ShowDialog();
                    return;
                }
            }
            //ObservableCollection<JobPublish> tempJobPublish = new ObservableCollection<JobPublish>();
            //this.busyCtrl.IsBusy = true;
            //string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "status" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "1" });
            //string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
            //ViewModel.BaseHttpModel<HttpJobListData> listModel = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(resultStr);
            //this.busyCtrl.IsBusy = false;
            //if (listModel == null)
            //{
            //    WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
            //    pWinMessage.Owner = Window.GetWindow(this);
            //    pWinMessage.ShowDialog();
            //    return;
            //}
            //else
            //{
            //    if (listModel.code == 200)
            //    {
            //        foreach (var item in listModel.data.data)
            //        {
            //            tempJobPublish.Add(new JobPublish() { id = Convert.ToInt32(item.jobID), jobName = item.jobName });
            //        }
            //        if (listModel.data.data.Count == 0)
            //        {
            //            WinPublishJobMessage msg = new WinPublishJobMessage();
            //            msg.Owner = Window.GetWindow(this);
            //            msg.atConfirm += PublishJobMsgCallback;
            //            msg.ShowDialog();
            //            return;
            //        }
            //        //打开职位列表选择对话框
            //        GlobalEvent.GlobalEventHandler(selectedSearchingResult, new GlobalEventArgs(GlobalFlag.HrSendOffer, tempJobPublish));
            //    }
            //    else
            //    {
            //        WinErroTip pWinMessage = new WinErroTip(listModel.msg);
            //        pWinMessage.Owner = Window.GetWindow(this);
            //        pWinMessage.ShowDialog();
            //        return;
            //    }
            //}
        }

        

        #endregion

        public async Task HrSendOffer(string jobID, string jhUserID)
        {
            List<HttpSendMessageParams> lstParams = new List<HttpSendMessageParams>();
            lstParams.Add(IMHelper.GetMessageModel(Convert.ToInt64(jhUserID), Convert.ToInt64(jobID), this.txtSendMessage.Text));
            
            string std = DAL.JsonHelper.ToJsonString(lstParams);
            string url = string.Format(DAL.ConfUtil.AddrBatchInvite, MagicGlobal.UserInfo.Version);
            string result = await DAL.HttpHelper.Instance.HttpPostAsync(url, std);
            this.busyCtrl.IsBusy = false;
            ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel>(result);
            if (model == null)
            {
                WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
            }
            else
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
                }
                else if (model.code == 200)
                {
                    DisappearShow disappear = new DisappearShow("已发送", 1);
                    disappear.Owner = Window.GetWindow(this);
                    disappear.ShowDialog();
                }
                else
                {
                    WinErroTip pWinMessage = new WinErroTip(model.msg);
                    pWinMessage.Owner = Window.GetWindow(this);
                    pWinMessage.ShowDialog();
                }
            }

            //string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "type", "jobID", "jhUserID" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "2", jobID, jhUserID });
            //string url = string.Format(DAL.ConfUtil.AddrHrSendOffer, MagicGlobal.UserInfo.Version, std);
            //string result = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            //this.busyCtrl.IsBusy = false;
            //ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel>(result);
            //if (model == null)
            //{
            //    WinErroTip pWinMessage = new WinErroTip(ConfUtil.netError);
            //    pWinMessage.Owner = Window.GetWindow(this);
            //    pWinMessage.ShowDialog();
            //}
            //else
            //{
            //    if (model.code == 200)
            //    {
            //        DisappearShow disappear = new DisappearShow("已发送", 1);
            //        disappear.Owner = Window.GetWindow(this.gdPublishOfferMutiple);
            //        disappear.ShowDialog();
            //        return;
            //    }
            //    else if(model.code == -131)
            //    {
            //        DisappearShow disappear = new DisappearShow("已发送", 1);
            //        disappear.Owner = Window.GetWindow(this.gdPublishOfferMutiple);
            //        disappear.ShowDialog();
            //        return;
            //    }
            //    else
            //    {
            //        WinErroTip pWinMessage = new WinErroTip(model.msg);
            //        pWinMessage.Owner = Window.GetWindow(this);
            //        pWinMessage.ShowDialog();
            //    }
            //}
            
        }

        private void TrackOpeartion_Event(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            //埋点
            ViewModel.HttpTrackEventDataModel eventModel = new HttpTrackEventDataModel();
            if (selectedSearchingResult != null)
            {
                eventModel.session = this.searchSession;
                eventModel.algo = "se";
                eventModel.item = new List<string>();
                eventModel.item.Add(selectedSearchingResult.userId.ToString());
                eventModel.page = Convert.ToString(this.curPage - 1);
                for (int i = 0; i < searchingTanlentResumModel.Count; i++)
                {
                    if (searchingTanlentResumModel[i].userId == selectedSearchingResult.userId)
                    {
                        eventModel.location = i.ToString();
                        break;
                    }
                }
            }

            eventModel.userID = MagicGlobal.UserInfo.Id.ToString();
            switch (fe.Name)
            {
                case "rdoList":
                    Common.TrackHelper2.TrackOperation("5.6.4.7.1", "clk");
                    break;
                case "rdoThumbnail":
                    Common.TrackHelper2.TrackOperation("5.6.4.8.1", "clk");
                    break;
                    //简历详情里的查看联系方式按钮
                case "ucb":
                    Common.TrackHelper2.TrackOperation("5.6.5.4.1", "clk", DAL.JsonHelper.ToJsonString(eventModel));

                    break;
                case "btnFreePhone":
                    Common.TrackHelper2.TrackOperation("5.6.5.5.1", "clk", DAL.JsonHelper.ToJsonString(eventModel));
                    break;
                case "btnVideo":
                    Common.TrackHelper2.TrackOperation("5.6.5.6.1", "clk", DAL.JsonHelper.ToJsonString(eventModel));

                    break;
            }
        }

        private async void btnCollection_Click(object sender, RoutedEventArgs e)
        {
            if (curResumeDetailModel.collectResume)
            {
                BaseHttpModel pBaseHttpModel = await IMHelper.CancelCollectResume(curResumeDetailModel.userID);
                if (pBaseHttpModel != null)
                {
                    if (pBaseHttpModel.code == 200)
                    {
                        curResumeDetailModel.collectResume = false;
                        DisappearShow pDisappearShow = new DisappearShow("已取消收藏");
                        pDisappearShow.Owner = Window.GetWindow(this);
                        pDisappearShow.ShowDialog();
                    }
                }


            }
            else
            {
                BaseHttpModel<HttpCollectionCount> pBaseHttpModel = await IMHelper.CollectResume(curResumeDetailModel.userID);
                if (pBaseHttpModel != null)
                {
                    if (pBaseHttpModel.code == 200)
                    {
                        curResumeDetailModel.collectResume = true;
                        if (pBaseHttpModel.data.total == 0)
                        {
                            WinMessageLink link = new WinMessageLink("您收藏的简历可以到「简历管理」-「收藏夹」中查看", "我知道了");
                            link.Owner = Window.GetWindow(this);
                            link.ShowDialog();
                        }
                        else
                        {
                            DisappearShow pDisappearShow = new DisappearShow("已收藏");
                            pDisappearShow.Owner = Window.GetWindow(this);
                            pDisappearShow.ShowDialog();
                        }
                    }
                    else if(pBaseHttpModel.code == -133)
                    {
                        WinClearCollection pWinErroTip = new WinClearCollection();
                        pWinErroTip.Owner = Window.GetWindow(this);
                        if(pWinErroTip.ShowDialog()==true)
                        {
                            if (actionClearCollection != null)
                                await actionClearCollection();
                        }
                    }
                }
            }

        }

        private async void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            if (curResumeDetailModel != null)
            {
                string jobName = string.Empty;
                if (curResumeDetailModel.personExperienceJob != null)
                {
                    if (curResumeDetailModel.personExperienceJob.Count > 0)
                    {
                        jobName = curResumeDetailModel.personExperienceJob[0].name;
                    }
                }
                curResumeDetailModel.downloadType = "talentSearch";
                await DownloadResume.DownLoadResume(curResumeDetailModel, Window.GetWindow(this));
            }
        }

        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (curResumeDetailModel != null)
            {
                WinSendEmail pWinSendEmail = new WinSendEmail(Window.GetWindow(this),curResumeDetailModel.userID, curResumeDetailModel.name);
                //pWinSendEmail.Owner = Window.GetWindow(this);
                pWinSendEmail.ShowDialog();
            }
        }

        private void PrintResume_Click(object sender, RoutedEventArgs e)
        {
            ResumeDetialPrint pResumeDetial = new ResumeDetialPrint();
            pResumeDetial.DataContext = curResumeDetailModel;
            WinResumePrint pWinResumePrint = new WinResumePrint(Window.GetWindow(this));
            if (pWinResumePrint.ShowDialog() == true)
            {
                if (pWinResumePrint.cbCheck.IsChecked == true)
                {
                    pResumeDetial.tbSalary.Visibility = Visibility.Collapsed;
                }
                PrintHelper.print(pResumeDetial);
            }

        }

        private void CommentResume_Click(object sender, RoutedEventArgs e)
        {

            scrollResum.ScrollToBottom();
        }
    }
}
