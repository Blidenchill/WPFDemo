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
using MagicCube.Model;
using System.Collections.ObjectModel;
using MagicCube.Common;

using MagicCube.HttpModel;
using MagicCube.TemplateUC;
using System.Threading.Tasks;
using System.Windows.Threading;
using MagicCube.ViewModel;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCInterviewDetail.xaml 的交互逻辑
    /// </summary>
    public partial class UCInterviewDetail : UserControl
    {
        #region "委托"
        public delegate Task delegateReturnHomeAction();
        public delegateReturnHomeAction ReturnHomeAction;
        #endregion

        #region "变量"
        /// <summary>
        /// 预约面试列表
        /// </summary>
        private ObservableCollection<InterviewDetailModel> InterviewList = new ObservableCollection<InterviewDetailModel>();
        /// <summary>
        /// 待确认到访列表
        /// </summary>
        private ObservableCollection<InterviewDetailModel> WaitingList = new ObservableCollection<InterviewDetailModel>();
        /// <summary>
        /// 已到访列表
        /// </summary>
        private ObservableCollection<InterviewDetailModel> CheckInList = new ObservableCollection<InterviewDetailModel>();

        private InterviewDetailModel curInterviewItem;

        /// <summary>
        /// 预约面试列表数目
        /// </summary>
        public string InterviewNum
        {
            get { return (string)GetValue(InterviewNumProperty); }
            set { SetValue(InterviewNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InterviewNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InterviewNumProperty =
            DependencyProperty.Register("InterviewNum", typeof(string), typeof(UCInterviewDetail), new PropertyMetadata(null));
        /// <summary>
        /// 待确认列表数目
        /// </summary>
        public string WaitingNum
        {
            get { return (string)GetValue(WaitingNumProperty); }
            set { SetValue(WaitingNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaitingNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaitingNumProperty =
            DependencyProperty.Register("WaitingNum", typeof(string), typeof(UCInterviewDetail), new PropertyMetadata(null));
        /// <summary>
        /// 已到访列表数目
        /// </summary>
        public string CheckInNum
        {
            get { return (string)GetValue(CheckInNumProperty); }
            set { SetValue(CheckInNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckInNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckInNumProperty =
            DependencyProperty.Register("CheckInNum", typeof(string), typeof(UCInterviewDetail), new PropertyMetadata(null));



        public string StartInterviewTime
        {
            get { return (string)GetValue(StartInterviewTimeProperty); }
            set { SetValue(StartInterviewTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartInterviewTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartInterviewTimeProperty =
            DependencyProperty.Register("StartInterviewTime", typeof(string), typeof(UCInterviewDetail), new PropertyMetadata(null));



        public string RemainInterviewTime
        {
            get { return (string)GetValue(RemainInterviewTimeProperty); }
            set { SetValue(RemainInterviewTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemainInterviewTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemainInterviewTimeProperty =
            DependencyProperty.Register("RemainInterviewTime", typeof(string), typeof(UCInterviewDetail), new PropertyMetadata(null));



        /// <summary>
        /// 当前流程页
        /// </summary>
        public InterviewProcess CurProcess
        {
            get { return (InterviewProcess)GetValue(CurProcessProperty); }
            set { SetValue(CurProcessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurProcess.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurProcessProperty =
            DependencyProperty.Register("CurProcess", typeof(InterviewProcess), typeof(UCInterviewDetail), new PropertyMetadata(null));




        public string sessionRecordID;
        public string jobID;
        public double cost;
        public int onsiteJobTimeSlotId;
        private string tagNewKey = string.Empty;
        Dictionary<string, string> TagsNewList = new Dictionary<string, string>();
        #endregion

        #region "构造函数"
        public UCInterviewDetail()
        {
            InitializeComponent();
            this.InterviewNum = "0";
            this.WaitingNum = "0";
            this.CheckInNum = "0";


        }
        #endregion


        #region "对内函数"

        private void SetTagsNewList()
        {
            this.TagsNewList.Clear();
            foreach (var item in MagicGlobal.currentUserInfo.InterviewIdToUpdateTime)
            {
                string[] listTemp = item.Split(new char[] { '&' });
                this.TagsNewList.Add(listTemp[0] + "&" + listTemp[1], listTemp[2]);
            }
        }

        private void SaveTagsNewList()
        {
            MagicGlobal.currentUserInfo.InterviewIdToUpdateTime.Clear();
            foreach (var item in this.TagsNewList)
            {
                MagicGlobal.currentUserInfo.InterviewIdToUpdateTime.Add(item.Key + "&" + item.Value);
            }
        }


        public async Task IniUCInterviewDetail(string name, string time, string jobId, string sessionId, double cost, InterviewProcess pInterviewProcess)
        {

            switch (pInterviewProcess)
            {
                case InterviewProcess.Interview:
                    RBInterview.IsChecked = true;
                    this.CurProcess = InterviewProcess.Interview;
                    break;
                case InterviewProcess.WaitingOK:
                    RBWaitingOK.IsChecked = true;
                    this.CurProcess = InterviewProcess.WaitingOK;
                    break;
                case InterviewProcess.CheckIn:
                    this.CurProcess = InterviewProcess.CheckIn;
                    RBCheckIn.IsChecked = true;
                    break;
            }


            SetTagsNewList();

            tagNewKey = jobId.ToString() + "&" + time;
            if (TagsNewList.ContainsKey(tagNewKey))
            {
                this.tagDateTime = TagsNewList[tagNewKey];
            }
            else
            {
                TagsNewList.Add(tagNewKey, string.Empty);
                this.tagDateTime = string.Empty;
            }
            //onsiteJobTimeSlotId = id;
            this.cost = cost;
            this.jobID = jobId;
            this.sessionRecordID = sessionId;
            this.txtName.Text = name;
            this.txtTime.Text = time;
            this.StartInterviewTime = time;
            this.RemainInterviewTime = GetRemainTime(time);

            await TimeslotList();
            StartTimeAlive();
        }

        private async Task TimeslotList()
        {
            busyCtrl.IsBusy = true;
            this.SetVoidDataPanel(InterviewProcess.Interview, false);
            this.gdSidebar.Visibility = Visibility.Collapsed;
            svItems.ScrollToTop();
            //时段预约面试

            ViewModel.BaseHttpModel<List<ViewModel.HttpO2OInterview>> model = await GetInterviewListStates(new List<int> { 10, 100,-110, 110, 120, -120 });
            if(model!=null)
            {
                if(model.code == 200)
                {
                    busyCtrl.IsBusy = false;
                    this.InterviewList.Clear();
                    foreach (ViewModel.HttpO2OInterview item in model.data)
                    {
                        this.InterviewList.Add(this.ModelSetting(item, InterviewProcess.Interview));
                    }
                    this.InterviewNum = this.InterviewList.Count().ToString();
                    this.SetInterviewNewTags();
                    if (this.CurProcess == InterviewProcess.Interview)
                    {
                        if (this.InterviewList.Count() == 0)
                        {
                            this.gdVoidDate.Visibility = Visibility.Visible;
                            this.SetVoidDataPanel(InterviewProcess.Interview, true);
                        }
                        this.lstMain.ItemsSource = InterviewList;
                    }
                }
                else
                {

                    this.InterviewNum = "0";
                }
            }
            else
            {

                this.InterviewNum = "0";
            }

            //时段待确认
            ViewModel.BaseHttpModel<List<ViewModel.HttpO2OInterview>> model2 = await GetInterviewListStates(new List<int> {  100, 120});
            if (model2 != null)
            {
                if (model2.code == 200)
                {
                    busyCtrl.IsBusy = false;
                    this.WaitingList.Clear();
                    foreach (ViewModel.HttpO2OInterview item in model2.data)
                    {
                        this.WaitingList.Add(this.ModelSetting(item, InterviewProcess.WaitingOK));
                    }
                    this.WaitingNum = this.WaitingList.Count().ToString();
                    this.SetInterviewNewTags();
                    if (this.CurProcess == InterviewProcess.WaitingOK)
                    {
                        if (this.WaitingList.Count() == 0)
                        {
                            this.gdVoidDate.Visibility = Visibility.Visible;
                            this.SetVoidDataPanel(InterviewProcess.WaitingOK, true);
                        }
                        this.lstMain.ItemsSource = WaitingList;
                    }
                }
                else
                {

                    this.WaitingNum = "0";
                }
            }
            else
            {

                this.WaitingNum = "0";
            }



            //时段确认

            ViewModel.BaseHttpModel<List<ViewModel.HttpO2OInterview>> model3 = await GetInterviewList(Convert.ToInt32(InterviewProcess.CheckIn));
            if (model3 != null)
            {
                if (model3.code == 200)
                {
                    busyCtrl.IsBusy = false;
                    this.CheckInList.Clear();
                    foreach (ViewModel.HttpO2OInterview item in model3.data)
                    {
                        this.CheckInList.Add(this.ModelSetting(item, InterviewProcess.CheckIn));
                    }
                    this.CheckInNum = this.CheckInList.Count().ToString();
                    this.SetInterviewNewTags();
                    if (this.CurProcess == InterviewProcess.CheckIn)
                    {
                        if (this.CheckInList.Count() == 0)
                        {
                            this.gdVoidDate.Visibility = Visibility.Visible;
                            this.SetVoidDataPanel(InterviewProcess.CheckIn, true);
                        }
                        this.lstMain.ItemsSource = CheckInList;
                    }
                }
                else
                {

                    this.CheckInNum = "0";
                }
            }
            else
            {

                this.CheckInNum = "0";
            }     

            busyCtrl.IsBusy = false;

        }

        private async Task WaitingOKListUpdate()
        {
            this.busyCtrl.IsBusy = true;
            this.SetVoidDataPanel(InterviewProcess.WaitingOK, false);
            this.gdSidebar.Visibility = Visibility.Collapsed;
            svItems.ScrollToTop();

            ViewModel.BaseHttpModel<List<ViewModel.HttpO2OInterview>> model = await GetInterviewListStates(new List<int> { 100, 120 });
            if (model != null)
            {
                if (model.code == 200)
                {
                    busyCtrl.IsBusy = false;
                    this.WaitingList.Clear();
                    foreach (ViewModel.HttpO2OInterview item in model.data)
                    {
                        this.WaitingList.Add(this.ModelSetting(item, InterviewProcess.WaitingOK));
                    }
                    this.WaitingNum = this.WaitingList.Count().ToString();
                    this.lstMain.ItemsSource = this.WaitingList;
                    if (this.WaitingList.Count() == 0)
                    {
                        this.SetVoidDataPanel(InterviewProcess.WaitingOK, true);
                    }
                }
                else
                {

                    this.SetVoidDataPanel(InterviewProcess.WaitingOK, true);
                }
            }
            else
            {

                this.SetVoidDataPanel(InterviewProcess.WaitingOK, true);
            }        
        }

        private async Task CheckInListUpdate()
        {
            this.busyCtrl.IsBusy = true;
            this.SetVoidDataPanel(InterviewProcess.CheckIn, false);
            this.gdSidebar.Visibility = Visibility.Collapsed;
            svItems.ScrollToTop();

            ViewModel.BaseHttpModel<List<ViewModel.HttpO2OInterview>> model3 = await GetInterviewList(Convert.ToInt32(InterviewProcess.CheckIn));
            if (model3 != null)
            {
                if (model3.code == 200)
                {
                    busyCtrl.IsBusy = false;
                    this.CheckInList.Clear();
                    foreach (ViewModel.HttpO2OInterview item in model3.data)
                    {
                        this.CheckInList.Add(this.ModelSetting(item, InterviewProcess.CheckIn));
                    }
                    this.CheckInNum = this.CheckInList.Count().ToString();
                    this.lstMain.ItemsSource = this.CheckInList;
                    if (this.CheckInList.Count() == 0)
                    {
                        this.SetVoidDataPanel(InterviewProcess.CheckIn, true);
                    }
                }
                else
                {

                    this.SetVoidDataPanel(InterviewProcess.CheckIn, true);
                }
            }
            else
            {

                this.SetVoidDataPanel(InterviewProcess.CheckIn, true);
            }         
            busyCtrl.IsBusy = false;
        }

        private async Task InterviewListUpdate(bool showBusy=true)
        {
            if(showBusy)
                this.busyCtrl.IsBusy = true;
            if (this.CurProcess == InterviewProcess.Interview)
            {
                this.SetVoidDataPanel(InterviewProcess.Interview, false);
            }
            //时段预约面试

            ViewModel.BaseHttpModel<List<ViewModel.HttpO2OInterview>> model = await GetInterviewListStates(new List<int> { 10, 100,-110, 110, 120, -120 });
            if (model != null)
            {
                if (model.code == 200)
                {

                    busyCtrl.IsBusy = false;
                    this.InterviewList.Clear();
                    foreach (ViewModel.HttpO2OInterview item in model.data)
                    {
                        this.InterviewList.Add(this.ModelSetting(item, InterviewProcess.Interview));
                    }
                    if (this.CurProcess == InterviewProcess.Interview)
                    {
                        this.lstMain.ItemsSource = InterviewList;
                        this.InterviewNum = this.InterviewList.Count().ToString();
                        if (this.InterviewList.Count() == 0)
                        {
                            this.SetVoidDataPanel(InterviewProcess.Interview, true);
                        }
                    }
                }
                else
                {

                    if (this.CurProcess == InterviewProcess.Interview)
                    {
                        this.SetVoidDataPanel(InterviewProcess.Interview, true);
                    }
                }
            }
            else
            {

                if (this.CurProcess == InterviewProcess.Interview)
                {
                    this.SetVoidDataPanel(InterviewProcess.Interview, true);
                }
            }        
            busyCtrl.IsBusy = false;
        }

        private InterviewDetailModel ModelSetting(ViewModel.HttpO2OInterview item, InterviewProcess process)
        {
            InterviewDetailModel temp = new InterviewDetailModel();
            temp.Age = TimeHelper.getAge(item.user.birthdate);
            temp.AvatarUrl = Downloader.LocalPath(item.user.avatar, item.user.name);
            temp.Name = ResumeName.GetName(item.user.name, item.user.userID.ToString());
            if (string.IsNullOrWhiteSpace(temp.Name))
            {
                temp.Name = "姓名未填写";
            }
            temp.Education = item.user.educationDesc;
            temp.WorkingExp = item.user.workingExp;
            string jobName = string.Empty;
            if (!string.IsNullOrWhiteSpace(item.user.exptPositionDesc))
            {
                temp.ExpectJobName = item.user.exptPositionDesc.Replace("$$", "、");
            }
            if (item.user.minSalary > 0 || item.user.maxSalary > 0)
            {
                temp.ExpectSalary = item.user.minSalary.ToString() + "K - " + item.user.maxSalary.ToString() + "K";
            }
            if (!string.IsNullOrWhiteSpace(item.user.careerObjectiveAreaDesc))
            {
                temp.ExpectWorkPlace = item.user.careerObjectiveAreaDesc.Replace("$$", "、").Replace("中国", "");
            }
            
            temp.Gender = item.user.genderDesc;
            temp.InterviewProcessEnum = process;
            temp.Location = item.user.province;
            if (!string.IsNullOrWhiteSpace(item.user.province))
            {
                temp.Location = CityCodeHelper.GetCityName(item.user.province);
            }
            if (!string.IsNullOrWhiteSpace(item.user.city))
            {
                temp.Location = CityCodeHelper.GetCityName(item.user.city);
            }
            if (!string.IsNullOrWhiteSpace(item.user.district))
            {
                temp.Location = CityCodeHelper.GetCityName(item.user.district);
            }
            if (item.totalSore != 0)
            {
                temp.MatchPercent = Convert.ToString(item.score * 100 / item.totalSore) + "%";
            }
            else
            {
                temp.MatchPercent = "100%";
            }

            temp.UpdateTime = item.createTime;
            temp.WorkingExp = item.user.workingExp;
            temp.UserId = item.user.userID.ToString();
            temp.jobId = item.jobID.ToString();
            temp.VerifyTime = item.extraFields.BConfirmTime.ToString("MM-dd HH:mm");
            temp.Status = item.state;
            temp.companyState = item.companyState;
            temp.hasQuestion = item.hasQuestion;
            temp.Mobile = item.user.mobile;
            temp.mB = item.mB;
            return temp;
        }
        private async Task searchReusme(InterviewDetailModel pmodel)
        {
            if (pmodel.InterResume == null)
            {
                SVUCResume.ScrollToTop();
                busyCtrl.IsBusy = true;

                BaseHttpModel<HttpResumeDetial> model = await IMHelper.GetDetialReusme(pmodel.UserId);

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
                pmodel.InterResume = SetModel.SetResumeDetailModel(model.data);
                setReusmeStute();
                ucResumeView.DataContext = pmodel.InterResume;

            }
            else
            {
                setReusmeStute();
                ucResumeView.DataContext = pmodel.InterResume;
            }

        }

        private async Task searchQuesion(InterviewDetailModel model)
        {
            if (model.InterQuestion == null)
            {
                SVUCQuesion.ScrollToTop();
                busyCtrl.IsBusy = true;
                model.InterQuestion = await O2OHelper.getQuestion(model.UserId, model.jobId);
                busyCtrl.IsBusy = false;
                setQuesionStute();
                ucQuesionView.DataContext = model.InterQuestion;

            }
            else
            {
                setQuesionStute();
                ucQuesionView.DataContext = model.InterQuestion;
            }
        }

        private void setReusmeStute()
        {
            gdSidebar.Visibility = Visibility.Visible;
            SVUCQuesion.Visibility = Visibility.Collapsed;
            SVUCResume.Visibility = Visibility.Visible;
        }
        private void setQuesionStute()
        {
            gdSidebar.Visibility = Visibility.Visible;
            SVUCQuesion.Visibility = Visibility.Visible;
            SVUCResume.Visibility = Visibility.Collapsed;
        }

        private string CalcuAge(string dob)
        {
            string temp = string.Empty;
            DateTime dt = Convert.ToDateTime(dob);
            //string[] splits = dob.Split(new string[] { "年" }, StringSplitOptions.RemoveEmptyEntries);
            int year = dt.Year;
            int nowYear = DateTime.Now.Year;
            return Convert.ToString(nowYear - year);
        }

        /// <summary>
        /// 根据时间戳timestamp（单位毫秒）计算日期
        /// </summary>
        private DateTime GetDateFromStamp(long timestamp)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long t = dt1970.Ticks + timestamp * 10000;
            return new DateTime(t);
        }


        private void SetVoidDataPanel(InterviewProcess process, bool isVisible)
        {
            switch (process)
            {
                case InterviewProcess.Interview:
                    this.txtVoidDate1.Text = "暂时没有预约面试的人选";
                    this.txtVoidDate2.Text = "预约现场面试的人会在这里出现哦";
                    break;
                case InterviewProcess.WaitingOK:
                    this.txtVoidDate1.Text = "暂时没有待确认到访的人选";
                    this.txtVoidDate2.Text = "面试结束后需要您确认是否到访的人，会在这里出现哦";
                    break;
                case InterviewProcess.CheckIn:
                    this.txtVoidDate1.Text = "暂时没有已确认到访的人选";
                    this.txtVoidDate2.Text = "您确认到访的人会在这里出现哦";
                    break;
            }
            if (isVisible)
            {
                this.gdVoidDate.Visibility = Visibility.Visible;
            }
            else
            {
                this.gdVoidDate.Visibility = Visibility.Collapsed;
            }
        }

        private string GetRemainTime(string startTime)
        {
            string temp = string.Empty;
            DateTime dt = Convert.ToDateTime(startTime);
            TimeSpan ts = dt - DateTime.Now;
            int day = ts.Days;
            int hour = ts.Hours;
            int minute = ts.Minutes;
            if (day < 0)
                day = 0;
            if (hour < 0)
                hour = 0;
            if (minute < 0)
                minute = 0;
            if (day != 0)
            {
                temp = day.ToString() + " 天 " + hour.ToString() + " 小时 ";
            }
            else
            {
                temp = hour.ToString() + " 小时 " + minute.ToString() + " 分 ";
            }
            return temp;
        }
        private void SetRefuseVisible()
        {
            if (RemainInterviewTime.Contains("天"))
            {
                bdRefuse.Visibility = Visibility.Visible;
                tbRefuse.Visibility = Visibility.Collapsed;
            }
            else if(RemainInterviewTime!= "0 小时 0 分 ")
            { 
                bdRefuse.Visibility = Visibility.Collapsed;
                tbRefuse.Visibility = Visibility.Visible;
            }
            else
            {
                bdRefuse.Visibility = Visibility.Collapsed;
                tbRefuse.Visibility = Visibility.Collapsed;
            }
        }
        private async Task ConfirmArrive()
        {
            busyCtrl.IsBusy = true;
            BaseHttpModel model = await O2OHelper.ConfirmArrive(curInterviewItem.UserId, jobID, sessionRecordID);
            busyCtrl.IsBusy = false;
            if (model == null)
                return;
            if (model.code != 200)
                return;
            this.WaitingList.Remove(curInterviewItem);
            CheckInNum = (Convert.ToInt32(CheckInNum) + 1).ToString();
            WaitingNum = (Convert.ToInt32(WaitingNum) - 1).ToString();
            if (WaitingList.Count == 0)
            {
                this.SetVoidDataPanel(InterviewProcess.WaitingOK, true);
            }
        }

        private async Task UnConfirmArrive(InterviewDetailModel model)
        {
            busyCtrl.IsBusy = true;
            BaseHttpModel modelUnConfirm = await O2OHelper.UnConfirmArrive(curInterviewItem.UserId, jobID, sessionRecordID);
            busyCtrl.IsBusy = false;
            if (modelUnConfirm == null)
                return;
            if (modelUnConfirm.code != 200)
                return;
            await WaitingOKListUpdate();
        }

        private async Task RefuseInterview()
        {
            busyCtrl.IsBusy = true;
            BaseHttpModel modelUnConfirm = await O2OHelper.RefuseArrive(curInterviewItem.UserId, jobID, sessionRecordID, curInterviewItem.Mobile, curInterviewItem.Name);
            busyCtrl.IsBusy = false;
            if (modelUnConfirm == null)
                return;
            if (modelUnConfirm.code != 200)
                return;

            this.gdSidebar.Visibility = Visibility.Collapsed;
            this.InterviewList.Remove(curInterviewItem);
            InterviewNum = (Convert.ToInt32(InterviewNum) - 1).ToString();
            if (InterviewList.Count == 0)
            {
                this.SetVoidDataPanel(InterviewProcess.Interview, true);
            }
            this.InterviewNum = this.InterviewList.Count().ToString();
        }


        private async Task<ViewModel.BaseHttpModel<List<ViewModel.HttpO2OInterview>>> GetInterviewList(int state)
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "sessionRecordID", "jobID", "state", "BConfirmTime", "orderByCreateTimeDesc", "pageSize" },
                  new string[] { sessionRecordID, jobID, state.ToString(),"1","1","1000" });
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewSessionInterview, MagicGlobal.UserInfo.Version, std));
            return DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpO2OInterview>>>(presult);                 
        }
        private async Task<ViewModel.BaseHttpModel<List<ViewModel.HttpO2OInterview>>> GetInterviewListStates(List<int>  states)
        {
            HttpO2Ostates pHttpO2Ostates = new HttpO2Ostates();
            pHttpO2Ostates.sessionRecordID = sessionRecordID;
            pHttpO2Ostates.jobID = jobID;
            pHttpO2Ostates.states = states;
            pHttpO2Ostates.orderByCreateTimeDesc = "1";
            pHttpO2Ostates.pageSize = "1000";
            string std = DAL.JsonHelper.ToJsonString(pHttpO2Ostates);
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewSessionInterview, MagicGlobal.UserInfo.Version, std));
            return DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpO2OInterview>>>(presult);
        }
        #endregion


        #region "事件响应"
        private async void RBInterview_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.4.5.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.4.5.2.1", "pv");
            this.CurProcess = InterviewProcess.Interview;
            this.gdSidebar.Visibility = Visibility.Collapsed;
            svItems.ScrollToTop();
            await this.InterviewListUpdate();

        }

        private async void RBWaitingOK_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.4.8.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.4.8.2.1", "pv");
            this.SaveTagsNewList();
            this.CurProcess = InterviewProcess.WaitingOK;
            if (this.InterviewList.Count > 0)
                this.tagDateTime = this.InterviewList[0].UpdateTime;
            await WaitingOKListUpdate();



        }

        private async void RBCheckIn_Click(object sender, RoutedEventArgs e)
        {
            this.SaveTagsNewList();
            this.CurProcess = InterviewProcess.CheckIn;
            if (this.InterviewList.Count > 0)
                this.tagDateTime = this.InterviewList[0].UpdateTime;
            await this.CheckInListUpdate();
        }

        private async void ReturnHome_Click(object sender, RoutedEventArgs e)
        {
            //释放资源
            //this.tagDateTime = string.Empty;
            if (this.InterviewList.Count > 0)
                this.tagDateTime = this.InterviewList[0].UpdateTime;
            StopTimeAlive();
            //保存tagDateTime键值
            TagsNewList[tagNewKey] = this.tagDateTime;
            SaveTagsNewList();
            if (ReturnHomeAction != null)
            {
               await ReturnHomeAction();
            }
        }

        private void BtnCloseSideBar_Click(object sender, RoutedEventArgs e)
        {
            if (CurProcess == InterviewProcess.Interview)
            {
                Common.TrackHelper2.TrackOperation("5.4.6.1.1", "clk");
            }
            this.gdSidebar.Visibility = Visibility.Collapsed;
        }

        private async void btnResumeQusion_Click(object sender, RoutedEventArgs e)
        {
            if (CurProcess == InterviewProcess.Interview)
            {
                Common.TrackHelper2.TrackOperation("5.4.5.4.1", "clk");
            }
            if (CurProcess == InterviewProcess.WaitingOK)
            {
                Common.TrackHelper2.TrackOperation("5.4.8.4.1", "clk");
            }
            this.SVUCQuesion.Visibility = Visibility.Visible;
            this.SVUCResume.Visibility = Visibility.Collapsed;
            await this.searchQuesion(curInterviewItem);
        }

        private async void btnResumeDetail_Click(object sender, RoutedEventArgs e)
        {
            if(CurProcess == InterviewProcess.Interview)
            {
                Common.TrackHelper2.TrackOperation("5.4.5.3.1", "clk");
            }
            if(CurProcess == InterviewProcess.WaitingOK)
            {
                Common.TrackHelper2.TrackOperation("5.4.8.3.1", "clk");
            }

            this.SVUCResume.Visibility = Visibility.Visible;
            this.SVUCQuesion.Visibility = Visibility.Collapsed;
            await this.searchReusme(curInterviewItem);
        }

        private async void ResumeDetail_Click(object sender, RoutedEventArgs e)
        {
            curInterviewItem = (sender as Button).DataContext as InterviewDetailModel;
            SetRefuseVisible();
            if (curInterviewItem.hasQuestion)
            {
                btneQusion.Visibility = Visibility.Visible;
            }
            else
            {
                btneQusion.Visibility = Visibility.Collapsed;
            }
            await this.searchReusme(curInterviewItem);
        }

        private async void QuesionDetail_Click(object sender, RoutedEventArgs e)
        {
            if(CurProcess == InterviewProcess.Interview)
            {
                Common.TrackHelper2.TrackOperation("5.4.6.2.1", "clk");
            }
            

            curInterviewItem = (sender as Button).DataContext as InterviewDetailModel;
            SetRefuseVisible();
            btneQusion.Visibility = Visibility.Visible;
           await this.searchQuesion(curInterviewItem);
        }



        private async void BtnRefuse_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.4.6.3.1", "clk");

            WinInterviewRefuse pWinArriveConfirm = new WinInterviewRefuse(curInterviewItem.mB);
            pWinArriveConfirm.Owner = Window.GetWindow(this);
            if (pWinArriveConfirm.ShowDialog() == true)
            {
                await RefuseInterview();
            }
            return;

        }

        private async void BtnPresent_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.4.8.5.1", "clk");
            InterviewDetailModel model = (sender as Button).DataContext as InterviewDetailModel;
            this.curInterviewItem = model;
            if (MagicGlobal.currentUserInfo.HandArriveConfirm == false)
            {             
                WinArriveConfirm pWinArriveConfirm = new WinArriveConfirm();
                pWinArriveConfirm.Owner = Window.GetWindow(this);
                if (pWinArriveConfirm.ShowDialog() == true)
                {
                    await ConfirmArrive();
                }
                return;
            }
            else
            {
                await ConfirmArrive();
            }
        }

        private async void BtnUnPresent_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.4.8.6.1", "clk");
            busyCtrl.IsBusy = true;
            InterviewDetailModel model = (sender as Button).DataContext as InterviewDetailModel;
            this.curInterviewItem = model;

            busyCtrl.IsBusy = false;
            WinUnArriveConfirm pWinUnArriveConfirm = new WinUnArriveConfirm(curInterviewItem.mB);
            pWinUnArriveConfirm.Owner = Window.GetWindow(this);
            if (pWinUnArriveConfirm.ShowDialog() == true)
            {
                await UnConfirmArrive(curInterviewItem);
            }
            return;
        }

        #endregion

        #region "定时刷新预约面试列表"
        private DispatcherTimer timeAlive;

        private string tagDateTime = string.Empty;
        private void StartTimeAlive()
        {
            timeAlive = new DispatcherTimer();
            timeAlive.Interval = new TimeSpan(0, 5, 0);   //间隔5秒
            timeAlive.Tick += new EventHandler(timeAlive_Elapsed);
            timeAlive.Start();
        }
        private void StopTimeAlive()
        {
            if (timeAlive != null)
            {
                timeAlive.Stop();
            }

        }

        private void SetInterviewNewTags()
        {
            foreach (var item in InterviewList)
            {
                DateTime dt1;
                if (string.IsNullOrEmpty(this.tagDateTime))
                {
                    dt1 = new DateTime();
                }
                else
                {
                    dt1 = Convert.ToDateTime(this.tagDateTime);
                }

                DateTime dt2 = Convert.ToDateTime(item.UpdateTime);
                if (dt2 > dt1)
                {
                    item.IsNewInsert = true;
                }
            }
            if (this.CurProcess == InterviewProcess.Interview)
            {
                this.lstMain.ItemsSource = this.InterviewList;
            }
        }

        private async void timeAlive_Elapsed(object sender, EventArgs e)
        {

            await this.InterviewListUpdate(false);
            SetInterviewNewTags();
        }
        public void LeiveNewTags()
        {
            if (this.InterviewList.Count > 0)
                this.tagDateTime = this.InterviewList[0].UpdateTime;
            StopTimeAlive();
            //保存tagDateTime键值
            TagsNewList[tagNewKey] = this.tagDateTime;
            SaveTagsNewList();

        }
        #endregion


    }


}
