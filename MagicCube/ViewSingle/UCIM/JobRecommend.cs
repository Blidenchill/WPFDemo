using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Threading.Tasks;
using MagicCube.ViewModel;
using MagicCube.Common;
using MagicCube.Model;

namespace MagicCube.ViewSingle
{
    public partial class UCIM : UserControl
    {
        ObservableCollection<IMControlModel> jobRecommendModelList;

        List<IMControlModel> jobRecommendUpdateList = new List<IMControlModel>();
        IMControlModel curJobRecommendModel = new IMControlModel();
        public string jobRecommendSession;
        public long curRecommendJobID = -1;
        private string algorithmID = string.Empty;

        #region "公共方法"
        public async Task IniRecommendList()
        {     
            jobRecommendModelList = new ObservableCollection<IMControlModel>();
            await SetRecommendList();
            if (RBRecommend.IsChecked == true)
            {
                if (jobRecommendModelList.Count > 0)
                {
                    lstRecommend.SelectedIndex = 0;
                    IMControlModel temp = lstRecommend.SelectedItem as IMControlModel;

                    SVUCResume.ScrollToTop();
                    temp.IsOpenResum = true;
                    this.curJobRecommendModel = temp;
                    this.curIMConotrol = temp;
                    this.curIMConotrol.ResumeDetail = await openResume(temp);
                    btnCollection.DataContext = this.curIMConotrol.ResumeDetail;
                }
            }
        }
        private async Task SetRecommendList()
        {
            
            string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<MagicCube.ViewModel.HttpRecommendResume>();

            List<string> pkey = new List<string> { "userID", "properties", "desc", "scrollID", "startRow", "pageSize" };
            List<string> pvalue = new List<string>  { MagicGlobal.UserInfo.Id.ToString(), propertys, "fullName", DateTime.Now.ToString(), "0", "100" };

            if (curRecommendJobID != -1)
            {
                pkey.Add("jobID");
                pvalue.Add(curRecommendJobID.ToString());
            }
            tabHintRecommend.Visibility = Visibility.Collapsed;
            RecommendResult.Visibility = Visibility.Collapsed;
            RecommendLoading.Visibility = Visibility.Visible;
            svRecommend.Visibility = Visibility.Collapsed;
            string std = DAL.JsonHelper.JsonParamsToString(pkey, pvalue);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrResumeRecommend,MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpRecommend> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpRecommend>>(jsonResult);
            RecommendLoading.Visibility = Visibility.Collapsed;
            svRecommend.Visibility = Visibility.Visible;
            if (model != null)
            {
                if (model.code == 200)
                {
                    jobRecommendModelList.Clear();

                    ViewModel.HttpTrackEventDataModel eventDataModel = new ViewModel.HttpTrackEventDataModel();
                    eventDataModel.session = Guid.NewGuid().ToString();
                    this.jobRecommendSession = eventDataModel.session;
                    eventDataModel.algo = model.data.algorithmID;
                    this.algorithmID = model.data.algorithmID;
                    if(string.IsNullOrWhiteSpace(this.algorithmID))
                    {
                        this.algorithmID = "0";
                    }
                    eventDataModel.item = new List<string>();

                    foreach (var item in model.data.resumeList)
                    {
                        IMControlModel temp = new IMControlModel();
                        temp.name = item.name;
                        temp.job = item.jobName;
                        temp.gender = item.gender.ToString();
                        temp.avatarUrl = Downloader.LocalPath(item.avatar, item.name);
                        //temp.RecommendTime = Convert.ToDateTime(item.updatedTime);
                        temp.jobId = item.jobID;
                        temp.gender = item.genderDesc;
                        temp.workingExp = item.workingExp;
                        temp.degree = item.educationDesc;
                        temp.UserID = item.userID;
                        temp.name = ResumeName.GetName(temp.name, temp.UserID.ToString()); 
                        this.jobRecommendModelList.Add(temp);
                        eventDataModel.item.Add(item.userID.ToString());
                    }
                    eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();
                    //推荐埋点
                    Common.TrackHelper2.TrackOperation("5.2.5.15.1", "rv", DAL.JsonHelper.ToJsonString(eventDataModel));

                    this.lstRecommend.ItemsSource = jobRecommendModelList;

                    if (jobRecommendModelList.Count == 0)
                    {
                        if (curRecommendJobID > -1)
                        {
                            RecommendResult.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            tabHintRecommend.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
          
        }

     
        #endregion




        private async void bgdRecommend_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IMControlModel temp = (sender as Grid).DataContext as IMControlModel;

            //埋点
            ViewModel.HttpTrackEventDataModel eventDataModel = new ViewModel.HttpTrackEventDataModel();
            //eventDataModel.session = Guid.NewGuid().ToString();
            eventDataModel.session = this.jobRecommendSession;
            eventDataModel.algo = this.algorithmID;
            eventDataModel.item = new List<string>();
            eventDataModel.item.Add(temp.UserID.ToString());
            for(int i = 0; i < jobRecommendModelList.Count; i ++)
            {
                if(jobRecommendModelList[i].UserID == temp.UserID)
                {
                    eventDataModel.location = i.ToString();
                    break;
                }
            }
            eventDataModel.userID = MagicGlobal.UserInfo.Id.ToString();
            Common.TrackHelper2.TrackOperation("5.2.5.3.1", "clk", DAL.JsonHelper.ToJsonString(eventDataModel));

            Common.TrackHelper2.TrackOperation("5.2.5.5.1", "lv", DAL.JsonHelper.ToJsonString(eventDataModel));

            Common.TrackHelper2.TrackOperation("5.2.6.6.1", "pv");

            SVUCResume.ScrollToTop();
            temp.IsOpenResum = true;
            this.curJobRecommendModel = temp;
            this.lstRecommend.SelectedItem = temp;
            this.curIMConotrol = temp;
            this.curIMConotrol.ResumeDetail = await openResume(temp);
            btnCollection.DataContext = this.curIMConotrol.ResumeDetail;
        }

        private void bgdRecommend_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private async void gdLstRecommend_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool visible = (bool)e.NewValue;
            if (visible)
            {
                this.gdMessageAndResume.SetBinding(Grid.DataContextProperty, new Binding("SelectedItem") { Source = this.lstRecommend });
                if (lstRecommend.SelectedItem != null)
                {
                    curIMConotrol = lstRecommend.SelectedItem as IMControlModel;
                    if (curIMConotrol != null)
                        curIMConotrol.IsOpenResum = true;
                }
                if (jobRecommendModelList.Count > 0)
                {
                    lstRecommend.SelectedIndex = 0;
                    IMControlModel temp = lstRecommend.SelectedItem as IMControlModel;

                    SVUCResume.ScrollToTop();
                    temp.IsOpenResum = true;
                    this.curJobRecommendModel = temp;
                    this.curIMConotrol = temp;
                    this.curIMConotrol.ResumeDetail = await openResume(temp);
                    btnCollection.DataContext = this.curIMConotrol.ResumeDetail;
                }
            }
        }

        private async void cbJobRecommend_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbJobRecommend.SelectedItem == null)
                return;
            if ((cbJobRecommend.SelectedItem as HttpJobList).jobID == curRecommendJobID)
                return;
            curRecommendJobID = (cbJobRecommend.SelectedItem as HttpJobList).jobID;
            await IniRecommendList();

            
        }

    }
}
