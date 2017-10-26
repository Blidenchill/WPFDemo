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
using MagicCube.ViewSingle;
using System.Collections.ObjectModel;
using MagicCube.Model;

using MagicCube.Common;
using MagicCube.HttpModel;
using MagicCube.TemplateUC;
using System.Threading.Tasks;
using MagicCube.ViewModel;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCInterviewUnconfirm.xaml 的交互逻辑
    /// </summary>
    public partial class UCInterviewUnconfirm : UserControl
    {
        public delegate Task delegateactionReturn();
        public delegateactionReturn actionReturn;
        private InterviewDetailModel curInterviewDetailModel;
        ObservableCollection<InterviewSessionTreeModel> curInterviewSessionTree;
        public UCInterviewUnconfirm()
        {
            InitializeComponent();
        }

        public async Task iniUCInterviewUnconfirm()
        {
            svjob.ScrollToTop();
            busyCtrl.IsBusy = true;
            gdResumeView.Visibility = Visibility.Collapsed;
            gdNoResult.Visibility = Visibility.Collapsed;
            int itotal = 0;

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" },
                   new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewAffirm, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<List<ViewModel.HttpSessionInterview>> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpSessionInterview>>>(presult);
            busyCtrl.IsBusy = false;
            if (model == null)
            {
                gdNoResult.Visibility = Visibility.Visible;
                return;
            }
            if (model.code != 200)
            {
                gdNoResult.Visibility = Visibility.Visible;
                return;
            }
            if (model.data.Count < 1)
            {
                gdNoResult.Visibility = Visibility.Visible;
            }
            else
            {
                curInterviewSessionTree = new ObservableCollection<InterviewSessionTreeModel>();
                foreach (ViewModel.HttpSessionInterview iHttpSessionInterview in model.data)
                {
                    itotal += iHttpSessionInterview.totalCount;
                    curInterviewSessionTree.Add(ModelSettingTree(iHttpSessionInterview));
                }
            }
            busyCtrl.IsBusy = false;
            tbTotal.Text = itotal.ToString();
            lstJob.ItemsSource = curInterviewSessionTree;
        }

        private async void MoreButton_Click(object sender, RoutedEventArgs e)
        {

            InterviewSessionTreeModel pHttpUnconfirm = (sender as Button).DataContext as InterviewSessionTreeModel;
            await FindInterviewerByTimesoltID(pHttpUnconfirm,10000);
        
        }

        private async void Return_Click(object sender, RoutedEventArgs e)
        {
           await actionReturn();
        }

        private async void Result_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.4.2.4.1", "clk");
            InterviewDetailModel pHttpUnconfirmItem = (sender as Button).DataContext as InterviewDetailModel;
            busyCtrl.IsBusy = true;
            if (MagicGlobal.currentUserInfo.HandArriveConfirm == false)
            {
                WinArriveConfirm pWinArriveConfirm = new WinArriveConfirm(false);
                pWinArriveConfirm.Owner = Window.GetWindow(this);
                if (pWinArriveConfirm.ShowDialog() == true)
                {
                    await ConfirmArrive(pHttpUnconfirmItem);
                }
            }
            else
            {
                await ConfirmArrive(pHttpUnconfirmItem);
            }
            busyCtrl.IsBusy = false;
        }
        private async void ReviewResult_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.4.2.5.1", "clk");
            busyCtrl.IsBusy = true;
            InterviewDetailModel pHttpUnconfirmItem = (sender as Button).DataContext as InterviewDetailModel;


            WinUnArriveConfirm pWinUnArriveConfirm = new WinUnArriveConfirm(pHttpUnconfirmItem.mB,false);
            pWinUnArriveConfirm.Owner = Window.GetWindow(this);
            if (pWinUnArriveConfirm.ShowDialog() == true)
            {
                await UnConfirmArrive(pHttpUnconfirmItem);
            }
            busyCtrl.IsBusy = false;
        }

        #region 私有方法
        private async Task ConfirmArrive(InterviewDetailModel pItem)
        {
            busyCtrl.IsBusy = true;
            BaseHttpModel model = await O2OHelper.ConfirmArrive(pItem.UserId, pItem.jobId, pItem.sessionRecordID);
            busyCtrl.IsBusy = false;
            if (model == null)
                return;
            if (model.code != 200)
                return;
            InterviewSessionTreeModel pTree = GetHttpUnconfirmbySoltid(pItem.sessionRecordID);
            await FindInterviewerByTimesoltID(pTree, pTree.items.Count());

        }
        private async Task UnConfirmArrive(InterviewDetailModel pItem)
        {
            busyCtrl.IsBusy = true;
            BaseHttpModel model = await O2OHelper.UnConfirmArrive(pItem.UserId, pItem.jobId, pItem.sessionRecordID);
            busyCtrl.IsBusy = false;
            if (model == null)
                return;
            if (model.code != 200)
                return;
            InterviewSessionTreeModel pTree = GetHttpUnconfirmbySoltid(pItem.sessionRecordID);
            await FindInterviewerByTimesoltID(pTree, pTree.items.Count());

        }
        private async Task FindInterviewerByTimesoltID(InterviewSessionTreeModel pSession, int count)
        {

            busyCtrl.IsBusy = true;

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "sessionRecordID", "startRow", "pageSize" }, new string[] { pSession.sessionRecordID, "0", "1000" });
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrInterviewSessionInterviewList, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<List<HttpO2OInterview>> model = DAL.JsonHelper.ToObject<BaseHttpModel<List<HttpO2OInterview>>>(presult);
            busyCtrl.IsBusy = false;
            ObservableCollection<InterviewDetailModel> ocInterviewDetailModel = new ObservableCollection<InterviewDetailModel>();
            if (model == null)
                return;
            if (model.code != 200)
                return;

            int i = 0;
            foreach (ViewModel.HttpO2OInterview item in model.data)
            {
                if (i < count)
                {                    
                    ocInterviewDetailModel.Add(this.ModelSettingInterview(item, pSession.cost));
                }
                i++;
            }
            pSession.totalCount = model.data.Count;
            pSession.items = ocInterviewDetailModel;
            if (model.data.Count == 0)
            {
                pSession.NoResuleVisibility = Visibility.Visible;
            }
            else
            {
                pSession.NoResuleVisibility = Visibility.Collapsed;
            }
            if (ocInterviewDetailModel.Count  < pSession.totalCount)
            {
                pSession.moreVisibility = Visibility.Visible;
            }
            else
            {
                pSession.moreVisibility = Visibility.Collapsed;
            }

            int itotal = 0;
            foreach (InterviewSessionTreeModel iHttpUnconfirm in curInterviewSessionTree)
            {
                if (iHttpUnconfirm != null)
                {
                    itotal += iHttpUnconfirm.totalCount;
                }
            }
            tbTotal.Text = itotal.ToString();

        }

        private InterviewSessionTreeModel GetHttpUnconfirmbySoltid(string id )
        {
            foreach (InterviewSessionTreeModel iHttpUnconfirm in curInterviewSessionTree)
            {

                if (iHttpUnconfirm.sessionRecordID == id)
                {
                    return iHttpUnconfirm;
                }
            }
            return null;

                    
        }
        #endregion

        #region 简历答题
        private async void Resume_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.4.2.2.1", "clk");
            curInterviewDetailModel = (sender as Button).DataContext as InterviewDetailModel;
            await searchReusme();
            if(curInterviewDetailModel.hasQuestion)
            {
                btnQusion.Visibility = Visibility.Visible;
            }
            else
            {
                btnQusion.Visibility = Visibility.Collapsed;
            }
        }

        private async void Quesion_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.4.2.3.1", "clk");
            curInterviewDetailModel = (sender as Button).DataContext as InterviewDetailModel;
            btnQusion.Visibility = Visibility.Visible;
            await searchQuesion();
        }

        private void BtnResumeViewClose_Click(object sender, RoutedEventArgs e)
        {
            gdResumeView.Visibility = Visibility.Collapsed;
        }
        private async void btnResumeQusion_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ResumeQusionButton).Text == "答题详情")
            {
               await searchQuesion();
            }
            else
            {
               await searchReusme();
            }
        }
        private void setQuesionStute()
        {
            gdResumeView.Visibility = Visibility.Visible;
            SVUCQuesion.Visibility = Visibility.Visible;
            SVUCResume.Visibility = Visibility.Collapsed;
        }

        private async Task searchQuesion()
        {
            if (curInterviewDetailModel.InterQuestion == null)
            {
                SVUCQuesion.ScrollToTop();
                busyCtrl.IsBusy = true;
                curInterviewDetailModel.InterQuestion = await O2OHelper.getQuestion(curInterviewDetailModel.UserId, curInterviewDetailModel.jobId);
                busyCtrl.IsBusy = false;
                setQuesionStute();
                ucQuesionView.DataContext = curInterviewDetailModel.InterQuestion;
              
            }
            else
            {
                setQuesionStute();
                ucQuesionView.DataContext = curInterviewDetailModel.InterQuestion;
            }
        }

        private void setReusmeStute()
        {
            gdResumeView.Visibility = Visibility.Visible;
            SVUCQuesion.Visibility = Visibility.Collapsed;
            SVUCResume.Visibility = Visibility.Visible;
        }

        private async Task searchReusme()
        {
            if (curInterviewDetailModel.InterResume == null)
            {
                SVUCResume.ScrollToTop();
                busyCtrl.IsBusy = true;

                BaseHttpModel<HttpResumeDetial> model =await IMHelper.GetDetialReusme(curInterviewDetailModel.UserId);

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
                curInterviewDetailModel.InterResume = SetModel.SetResumeDetailModel(model.data);
                setReusmeStute();
                ucResumeView.DataContext = curInterviewDetailModel.InterResume;

            }
            else
            {
                setReusmeStute();
                ucResumeView.DataContext = curInterviewDetailModel.InterResume;
            }
        }
        #endregion


        private InterviewSessionTreeModel ModelSettingTree(ViewModel.HttpSessionInterview pHttpSessionInterview)
        {
            InterviewSessionTreeModel temp = new InterviewSessionTreeModel();
            temp.time = pHttpSessionInterview.sessionBeginTime.ToString("yyyy-MM-dd HH:mm");
            temp.totalCount = pHttpSessionInterview.totalCount;
            temp.cost = pHttpSessionInterview.customPrice;
            temp.sessionRecordID = pHttpSessionInterview.sessionRecordID;
            temp.reservedNum = pHttpSessionInterview.statistics.visitTotal+ pHttpSessionInterview.statistics.pass;
            temp.visitedNum = pHttpSessionInterview.statistics.visited;
            temp.items = new ObservableCollection<InterviewDetailModel>();
            temp.jobName = pHttpSessionInterview.jobName;
            foreach (ViewModel.HttpO2OInterview iHttpO2OInterview in pHttpSessionInterview.flows)
            {
                temp.items.Add(ModelSettingInterview(iHttpO2OInterview, pHttpSessionInterview.jobMinSalary));
            }
            if(temp.totalCount > temp.items.Count)
            {
                temp.moreVisibility = Visibility.Visible;
            }
            else
            {
                temp.moreVisibility = Visibility.Collapsed;
            }
            if(temp.totalCount<1)
            {
                temp.NoResuleVisibility = Visibility.Visible;
            }
            else
            {
                temp.NoResuleVisibility = Visibility.Collapsed;
            }
            return temp;
        }

        private InterviewDetailModel ModelSettingInterview(ViewModel.HttpO2OInterview item,double cost)
        {
            InterviewDetailModel temp = new InterviewDetailModel();
            temp.cost = cost;
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
            temp.VerifyTime = item.interviewTime.ToString("MM-dd HH:mm");
            temp.Status = item.state;
            temp.companyState = item.companyState;
            temp.hasQuestion = item.hasQuestion;
            temp.sessionRecordID = item.sessionRecordID;
            temp.Mobile = item.user.mobile;
            temp.mB = item.mB;
            return temp;
        }
    }




}
