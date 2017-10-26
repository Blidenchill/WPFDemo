using MagicCube.Common;
using MagicCube.HttpModel;
using MagicCube.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public enum  jobType
    {
        close = -2 ,
        open = 1
    }
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class UCInterviewHomePage : UserControl
    {

        #region 变量
        /// <summary>
        /// 确认到访调整回调
        /// </summary>
        public delegate Task delegateArriveConfirmPage();
        public delegateArriveConfirmPage ArriveConfirmPage;
        /// <summary>
        /// MB回调页面
        /// </summary>
        public delegate Task delegateactionMBPage();
        public delegateactionMBPage actionMBPage;
        /// <summary>
        /// 时间段详情回调
        /// </summary>
        public delegate Task delegateactionJobDetials(string name, string time, string jobId,string sessionId,double cost , InterviewProcess i);
        public delegateactionJobDetials actionJobDetials;
        private jobType mJobType = jobType.open;
        bool handSelectDate = false;
        #endregion

        #region 初始化调用
        public UCInterviewHomePage()
        {
            InitializeComponent();
        }
        public async Task  iniHomePage()
        {
            rbOpen.IsChecked = true;
            rbToday.IsChecked = true;
            mJobType = jobType.open;
            await iniInterviewCount();
        }
        public void iniMB()
        {
            Action action = new Action(() =>
             {
                 string presult = DAL.HttpHelper.Instance.HttpGet(string.Format( DAL.ConfUtil.AddrO2OMB,MagicGlobal.UserInfo.Version,
                   DAL.JsonHelper.JsonParamsToString(new string[] {"userID", "serviceSign" }, new string[] {MagicGlobal.UserInfo.Id.ToString(), "S_Coin" })));

                 ViewModel.BaseHttpModel<HttpMBTotalData> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<HttpMBTotalData>>(presult);
                 if (model != null)
                 {
                     this.Dispatcher.BeginInvoke(new Action(() =>
                     {
                         if (model.code == 200)
                         {
                             tbTotalAmount.Text = Math.Round(model.data.totalAmount, 2).ToString();
                             tbLockedAmount.Text = Math.Round(model.data.lockedAmount, 2).ToString();
                             tbAvailableAmount.Text = Math.Round(model.data.availableAmount, 2).ToString();
                         }
                         //用户没有开通该服务
                         else if (model.code == -2)
                         {
                             tbTotalAmount.Text = "0";
                             tbLockedAmount.Text = "0";
                             tbAvailableAmount.Text = "0";
                         }
                     }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                 }            
             });
            action.BeginInvoke(null, null);
        }
        public async Task iniInterviewCount()
        {
            gdNoJob.Visibility = Visibility.Collapsed;
            gdNoJobOpen.Visibility = Visibility.Collapsed;
            gdNoJobClose.Visibility = Visibility.Collapsed;


            

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewNum, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<ViewModel.HttpInterviewNum> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpInterviewNum>>(presult);
            if (model == null)
            {
                return;
            }
            if (model.code != 200)
            {
                return;
            }
            tbOpenCount.Text = model.data.underwaySessionNum.ToString();
            tbCloseCount.Text = model.data.expiredSessionNum.ToString();

            if (model.data.underwaySessionNum == 0 && model.data.expiredSessionNum == 0)
            {
                gdNoJob.Visibility = Visibility.Visible;
            }
            else
            {
                if (mJobType == jobType.open)
                {
                    if (model.data.underwaySessionNum == 0)
                    {
                        spTimeSelect.Visibility = Visibility.Collapsed;
                        gdNoJobOpen.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        spTimeSelect.Visibility = Visibility.Visible;
                        await iniOpenToday();
                    }
                }
                else if (mJobType == jobType.close)
                {
                    if (model.data.expiredSessionNum == 0)
                    {
                        spTimeSelect.Visibility = Visibility.Collapsed;
                        gdNoJobClose.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        spTimeSelect.Visibility = Visibility.Visible;
                        await updataOtherDay();
                    }
                }
            }

        }
        public void iniUnconfimCount()
        {
            Action action = new Action(() =>
            {
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
                string presult = DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrInterviewUnconfimNum, MagicGlobal.UserInfo.Version, std));
                ViewModel.BaseHttpModel<ViewModel.HttpUnconfimCount> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpUnconfimCount>>(presult);
                if (model == null)
                {
                    return;
                }
                if (model.code != 200)
                {
                    return;
                }
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (model.data.count==0)
                    {
                        btnArriveConfirm.IsEnabled = false;
                    }
                    else
                    {
                        btnArriveConfirm.IsEnabled = true;
                    }
                    tbUnconfimCount.Text = model.data.count.ToString();
                }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
            });
            action.BeginInvoke(null, null);
        }
        #endregion

        #region  事件

        private async void ArriveConfirm_Click(object sender, RoutedEventArgs e)
        {

            Common.TrackHelper2.TrackOperation("5.4.1.3.1", "clk");
            if (ArriveConfirmPage != null)
                await ArriveConfirmPage();
        }
        private async void Today_Click(object sender, RoutedEventArgs e)
        {
            if (rbOpen.IsChecked == true)
            {
                await iniOpenToday();
            }
            else if(rbClose.IsChecked == true)
            {
                setDay(1);
                await updataOtherDay();
            }
        }
        private async void ThreeDay_Click(object sender, RoutedEventArgs e)
        {
            setDay(3);
           await updataOtherDay();
        }
        private async void SevenDay_Click(object sender, RoutedEventArgs e)
        {
            setDay(7);
          await  updataOtherDay();
        }
        private async void ThirtyDay_Click(object sender, RoutedEventArgs e)
        {
            setDay(30);
          await  updataOtherDay();
        }

        private async void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            InterviewJobTreeModel pInterviewJobTreeModel = (sender as Button).DataContext as InterviewJobTreeModel;
            string strStart = getDate(dpStart.Text);
            string strEnd = getEndDate(dpEnd.Text);
            busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "sessionBeginTimeFrom", "sessionBeginTimeTo", "jobIDs", "startRow", "pageSize", "sessionStates" },
                    new string[] { MagicGlobal.UserInfo.Id.ToString(), strStart, strEnd, pInterviewJobTreeModel.jobID,"0","10000", Convert.ToInt32(mJobType).ToString() });
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewSessionList, MagicGlobal.UserInfo.Version, std));
            busyCtrl.IsBusy = false;
            ViewModel.BaseHttpModel<List<ViewModel.HttpSessionList>> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpSessionList>>>(presult);
            if (model == null)
            {
                return;
            }
            if (model.code != 200)
            {
                return;
            }
            ObservableCollection<InterviewSessionListModel> lstInterviewSessionListModel = new ObservableCollection<InterviewSessionListModel>();
            foreach (ViewModel.HttpSessionList iHttpSessionList in model.data)
            {
                if (mJobType == jobType.open)
                    lstInterviewSessionListModel.Add(ModelSettingList(iHttpSessionList, model.extraData.serverTime));
                else
                    lstInterviewSessionListModel.Insert(0, ModelSettingList(iHttpSessionList, model.extraData.serverTime));
            }
            pInterviewJobTreeModel.items = lstInterviewSessionListModel;
            pInterviewJobTreeModel.moreVisibility = Visibility.Collapsed;

        }
        private async void calStart_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (handSelectDate)
            {
                if (calStart.SelectedDate == null)
                    return;

                calStart.DisplayDate = Convert.ToDateTime(calStart.SelectedDate);
                dpStart.Text = calStart.DisplayDate.ToString("yyyy/M/d");

                if (calEnd.DisplayDate < calStart.DisplayDate)
                {
                    calEnd.SelectedDate = calEnd.DisplayDate = calStart.DisplayDate;
                    dpEnd.Text = calEnd.DisplayDate.ToString("yyyy/M/d");
                }
                setDateRbState();
                await updataOtherDay();
                handSelectDate = false;
            }

        }
        private async void calEnd_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (handSelectDate)
            {
                if (calEnd.SelectedDate == null)
                    return;
                calEnd.DisplayDate = Convert.ToDateTime(calEnd.SelectedDate);
                dpEnd.Text = calEnd.DisplayDate.ToString("yyyy/M/d");

                if (calStart.DisplayDate > calEnd.DisplayDate)
                {
                    calStart.SelectedDate =  calStart.DisplayDate = calEnd.DisplayDate;
                    dpStart.Text = calStart.DisplayDate.ToString("yyyy/M/d");
                }
                setDateRbState();
                await updataOtherDay();
                handSelectDate = false;
            }
        }
       
        private void dpStart_Click(object sender, RoutedEventArgs e)
        {
            if (popStart.IsOpen)
            {
                popStart.IsOpen = false;
            }
            else
            {
                popStart.IsOpen = true;
            }
        }
        private void dpEnd_Click(object sender, RoutedEventArgs e)
        {
            if (popEnd.IsOpen)
            {
                popEnd.IsOpen = false;
            }
            else
            {
                popEnd.IsOpen = true;
            }
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popStart.IsOpen = false;
            popEnd.IsOpen = false;
            handSelectDate = true;
        }
        
        private async void rbOpen_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.4.1.5.1", "clk");
            if (mJobType == jobType.open)
                return;
            mJobType = jobType.open;
            rbToday.IsChecked = true;

            await iniInterviewCount();
        }
        private async void rbClose_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.4.1.4.1", "clk");
            if (mJobType == jobType.close)
                return;
            mJobType = jobType.close;
            rbToday.IsChecked = true;
            setDay(1);
            await iniInterviewCount();
        }

        private void MoreLinkButton_Click(object sender, RoutedEventArgs e)
        {
            if(rbOpen.IsChecked==true)
            {
                TrackHelper2.TrackOperation("5.4.1.6.1", "clk");
            }
            else
            {
                TrackHelper2.TrackOperation("5.4.1.6.2", "clk");
            }
            InterviewSessionListModel pSeasonItems = (sender as Button).DataContext as InterviewSessionListModel;
            if (actionJobDetials != null)
            {
                actionJobDetials(pSeasonItems.jobName, pSeasonItems.time, pSeasonItems.jobID, pSeasonItems.sessionRecordID,pSeasonItems.jobMinSalary, InterviewProcess.Interview);
            }
                
        }

        private void SeeMB_Click(object sender, RoutedEventArgs e)
        {
            actionMBPage();
        }

        private void NewlyTimesolt_Click(object sender, RoutedEventArgs e)
        {
            InterviewSessionListModel pSeasonItems = (sender as Button).DataContext as InterviewSessionListModel;
            if (actionJobDetials != null)
            {
                actionJobDetials(pSeasonItems.jobName, pSeasonItems.time, pSeasonItems.jobID, pSeasonItems.sessionRecordID, pSeasonItems.jobMinSalary, InterviewProcess.Interview);
            }
        }

        private void MoreLinkButton_Click(object sender, MouseButtonEventArgs e)
        {
            InterviewSessionListModel pSeasonItems = (sender as TextBlock).DataContext as InterviewSessionListModel;
            if (actionJobDetials != null)
            {
                actionJobDetials(pSeasonItems.jobName, pSeasonItems.time, pSeasonItems.jobID, pSeasonItems.sessionRecordID, pSeasonItems.jobMinSalary, InterviewProcess.Interview);
            }
        }

        private void MoreLinkVisitedButton_Click(object sender, MouseButtonEventArgs e)
        {
            InterviewSessionListModel pSeasonItems = (sender as TextBlock).DataContext as InterviewSessionListModel;
            if (actionJobDetials != null)
            {
                actionJobDetials(pSeasonItems.jobName, pSeasonItems.time, pSeasonItems.jobID, pSeasonItems.sessionRecordID, pSeasonItems.jobMinSalary, InterviewProcess.CheckIn);
            }
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 点击时间按钮时设置时间段显示时间
        /// </summary>
        /// <param name="day"></param>
        private void setDay(int day)
        {
            if (mJobType == jobType.open)
            {
                if (day == 1)
                {
                    calEnd.SelectedDate =  calEnd.DisplayDate = DateTime.Now;
                }
                if (day == 3)
                {
                    calEnd.SelectedDate = calEnd.DisplayDate = DateTime.Now.AddDays(2);
                }
                if (day == 7)
                {
                    calEnd.SelectedDate = calEnd.DisplayDate = DateTime.Now.AddDays(6);
                }
                if (day == 30)
                {
                     calEnd.SelectedDate = calEnd.DisplayDate = DateTime.Now.AddDays(29);
                }

                dpEnd.Text = calEnd.DisplayDate.ToString("yyyy/M/d");

                calStart.SelectedDate = calStart.DisplayDate = DateTime.Now;
                dpStart.Text = calStart.DisplayDate.ToString("yyyy/M/d");
            }
            if (mJobType == jobType.close)
            {
                if (day == 1)
                {
                    calStart.SelectedDate = calStart.DisplayDate = DateTime.Now;
                }
                if (day == 3)
                {
                    calStart.SelectedDate = calStart.DisplayDate = DateTime.Now.AddDays(-2);
                }
                if (day == 7)
                {
                    calStart.SelectedDate = calStart.DisplayDate = DateTime.Now.AddDays(-6);
                }
                if (day == 30)
                {
                    calStart.SelectedDate = calStart.DisplayDate = DateTime.Now.AddDays(-29);
                }
                dpStart.Text = calStart.DisplayDate.ToString("yyyy/M/d");

                calEnd.SelectedDate = calEnd.DisplayDate = DateTime.Now;
                dpEnd.Text = calEnd.DisplayDate.ToString("yyyy/M/d");
            }
        }
        /// <summary>
        /// 进行中的今天
        /// </summary>
        private async Task iniOpenToday()
        {
            svJobTree.Visibility = Visibility.Collapsed;
            svJobList.Visibility = Visibility.Collapsed;
            gdNoResultToday.Visibility = Visibility.Collapsed;
            svJobList.ScrollToTop();
            setDay(1);
            busyCtrl.IsBusy = true;

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "sessionBeginTimeFrom", "sessionBeginTimeTo", "sessionStates" },
                    new string[] { MagicGlobal.UserInfo.Id.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), Convert.ToInt32(mJobType).ToString() });
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewSessionList, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<List<ViewModel.HttpSessionList>> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpSessionList>>>(presult);
            busyCtrl.IsBusy = false;
            if(model == null)
            {
                return;
            }
            if (model.code != 200)
            {
                return;
            }

            if (model.data.Count == 0)
            {
                busyCtrl.IsBusy = true;
                string stdSingle = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "sessionBeginTimeFrom", "startRow", "pageSize", "sessionStates" },
                    new string[] { MagicGlobal.UserInfo.Id.ToString(), DateTime.Now.ToString("yyyy-MM-dd  HH:mm"),"0","1", "1" });
                string presultSingle = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewSessionList, MagicGlobal.UserInfo.Version, stdSingle));
                busyCtrl.IsBusy = false;
                ViewModel.BaseHttpModel<List<ViewModel.HttpSessionList>> modelSingle = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpSessionList>>>(presultSingle);
                if (modelSingle != null)
                {
                    if (modelSingle.code == 200)
                    {
                        if (modelSingle.data.Count > 0)
                        {
                            spNewlyTimesolt.DataContext = ModelSettingList(modelSingle.data[0], modelSingle.extraData.serverTime);
                        }
                    }
                }
                gdNoResultToday.Visibility = Visibility.Visible;
                tbNoResultOpenToday.Visibility = Visibility.Visible;
                tbNoResultOtherToday.Visibility = Visibility.Collapsed;

            }
            else
            {
                List<InterviewSessionListModel> lstInterviewSessionListModel = new List<InterviewSessionListModel>();
                foreach(ViewModel.HttpSessionList iHttpSessionList in model.data)
                {
                    lstInterviewSessionListModel.Add(ModelSettingList(iHttpSessionList,model.extraData.serverTime));
                }
                svJobList.Visibility = Visibility.Visible;
                busyCtrl.IsBusy = false;
                JobList.ItemsSource = lstInterviewSessionListModel;
            }
        }
        /// <summary>
        /// 查询除了进行中的今天 二级列表
        /// </summary>
        private async Task updataOtherDay()
        {
            svJobTree.Visibility = Visibility.Collapsed;
            svJobList.Visibility = Visibility.Collapsed;
            gdNoResultToday.Visibility = Visibility.Collapsed;
            gdNoJobClose.Visibility = Visibility.Collapsed;
            string strStart = getDate(dpStart.Text);
            string strEnd = getEndDate(dpEnd.Text);
            svJobTree.ScrollToTop();
            busyCtrl.IsBusy = true;


            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "sessionBeginTimeFrom", "sessionBeginTimeTo", "sessionStates" },
                  new string[] { MagicGlobal.UserInfo.Id.ToString(), strStart, strEnd,Convert.ToInt32(mJobType).ToString()});
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewJobSessionTree, MagicGlobal.UserInfo.Version, std));
            busyCtrl.IsBusy = true;
            ViewModel.BaseHttpModel<List<ViewModel.HttpJobSessionTreeItem>> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpJobSessionTreeItem>>>(presult);
            busyCtrl.IsBusy = false;
            if (model == null)
            {
                return;
            }
            if (model.code != 200)
            {
                return;
            }
            if (model.data.Count == 0)
            {
                if (mJobType == jobType.open)
                {

                    busyCtrl.IsBusy = true;
                    string stdSingle = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "sessionBeginTimeFrom", "startRow", "pageSize", "sessionStates" },
                                        new string[] { MagicGlobal.UserInfo.Id.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), "0", "1","1" });
                    string presultSingle = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewSessionList, MagicGlobal.UserInfo.Version, stdSingle));
                    busyCtrl.IsBusy = false;
                    ViewModel.BaseHttpModel<List<ViewModel.HttpSessionList>> modelSingle = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpSessionList>>>(presultSingle);
                    if (modelSingle != null)
                    {
                        if (modelSingle.code == 200)
                        {
                            if (modelSingle.data.Count > 0)
                            {
                                spNewlyTimesolt.DataContext = ModelSettingList(modelSingle.data[0], modelSingle.extraData.serverTime);
                            }
                        }
                    }

                    gdNoResultToday.Visibility = Visibility.Visible;
                    tbNoResultOtherToday.Visibility = Visibility.Visible;
                    tbNoResultOpenToday.Visibility = Visibility.Collapsed;
                }
                else
                {
                    gdNoJobClose.Visibility = Visibility.Visible;
                }
            }
            else
            {
                List<InterviewJobTreeModel> lstInterviewJobTreeModel = new List<InterviewJobTreeModel>();
                foreach(ViewModel.HttpJobSessionTreeItem iHttpJobSessionTreeItem in model.data)
                {
                    lstInterviewJobTreeModel.Add(ModelSettingTree(iHttpJobSessionTreeItem, model.extraData.serverTime));
                }
                svJobTree.Visibility = Visibility.Visible;
                JobTree.ItemsSource = lstInterviewJobTreeModel;
            }
        }
        /// <summary>
        /// 时间段选择设置四个按钮反选状态（今天、3天内...）
        /// </summary>
        private void setDateRbState()
        {
            rbToday.IsChecked = false;
            rbThreeDay.IsChecked = false;
            rbSevenDay.IsChecked = false;
            rbThirtyDay.IsChecked = false;
        }
        private string getDate(string time)
        {
            return Convert.ToDateTime(time).ToString("yyyy-MM-dd");
        }
        private string getEndDate(string time)
        {
            return Convert.ToDateTime(time).AddDays(1).ToString("yyyy-MM-dd");
        }

        private InterviewSessionListModel ModelSettingList(ViewModel.HttpSessionList pHttpSessionList, DateTime serverNow)
        {
            InterviewSessionListModel temp = new InterviewSessionListModel();
            temp.time = pHttpSessionList.sessionBeginTime.ToString("yyyy-MM-dd HH:mm");
            temp.sessionRecordID = pHttpSessionList.sessionRecordID;
            temp.jobName = pHttpSessionList.jobName;
            if (pHttpSessionList.flow != null)
            {
                temp.reservedNum = pHttpSessionList.flow.pass + pHttpSessionList.flow.visitTotal;
                temp.visitedNum = pHttpSessionList.flow.visited;
            }
            temp.jobID = pHttpSessionList.jobID;
            temp.surplus = (Convert.ToDateTime(temp.time) - serverNow).TotalSeconds;
            temp.isOpen = mJobType;
            temp.jobMinSalary = pHttpSessionList.customPrice;
            return temp;
        }

        private InterviewJobTreeModel ModelSettingTree(ViewModel.HttpJobSessionTreeItem pHttpJobSessionTreeItem, DateTime serverNow)
        {
            InterviewJobTreeModel temp = new InterviewJobTreeModel();
            temp.jobName = pHttpJobSessionTreeItem.jobName;
            temp.jobID = pHttpJobSessionTreeItem.jobID;
            //if (pHttpJobSessionTreeItem.flowJob != null)
            //{
            //    temp.reservedNum = pHttpJobSessionTreeItem.flowJob.pass + pHttpJobSessionTreeItem.flowJob.visitTotal;
            //    temp.visitedNum = pHttpJobSessionTreeItem.flowJob.visited;

            //}
            //temp.totalCount = pHttpJobSessionTreeItem.totalCount;
            temp.items = new ObservableCollection<InterviewSessionListModel>();
            foreach (ViewModel.HttpJobSessionItem iHttpSessionList in pHttpJobSessionTreeItem.jobSessions)
            {
                InterviewSessionListModel ctemp = new InterviewSessionListModel();
                ctemp.time = iHttpSessionList.sessionBeginTime.ToString("yyyy-MM-dd HH:mm");
                ctemp.jobName = pHttpJobSessionTreeItem.jobName;
                ctemp.sessionRecordID = iHttpSessionList.sessionRecordID;

                if (iHttpSessionList.flowSession != null)
                {
                    ctemp.reservedNum = iHttpSessionList.flowSession.pass + iHttpSessionList.flowSession.visitTotal;
                    ctemp.visitedNum = iHttpSessionList.flowSession.visited;
                    temp.reservedNum += ctemp.reservedNum;
                    temp.visitedNum += ctemp.visitedNum;
                }
                ctemp.jobID = pHttpJobSessionTreeItem.jobID;
                ctemp.surplus = (Convert.ToDateTime(ctemp.time) - serverNow).TotalSeconds;
                ctemp.isOpen = mJobType;
                ctemp.jobMinSalary = pHttpJobSessionTreeItem.customPrice;
                if (temp.items.Count < 3)
                {
                    temp.items.Add(ctemp);
                }
                temp.totalCount += 1;
            }
            if (temp.totalCount < 4)
            {
                temp.moreVisibility = Visibility.Collapsed;
            }
            else
            {
                temp.moreVisibility = Visibility.Visible;
            }
            return temp;
        }
        #endregion

        


    }

    

}
