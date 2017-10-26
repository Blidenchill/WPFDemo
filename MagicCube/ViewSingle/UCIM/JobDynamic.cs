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
        ObservableCollection<IMControlModel> jobDynamicModelList;
        List<IMControlModel> jobDynamicUpdateList = new List<IMControlModel>();
        IMControlModel curJobDynamicModel = new IMControlModel();
        public long curDynamicJobID = -1;
        public string jobDynamicSession = string.Empty;


        public async Task IniIniJobDynamic()
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "userIdentity", "relationSign" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "2", "viewJobReverse,collectJobReverse" });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrRedFunction, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpCollectViewRed> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpCollectViewRed>>(jsonResult);
            if(model!=null)
            {
                if(model.code == 200)
                {
                    if(RBInterest.IsChecked == false)
                    {
                        if(model.data.viewJobReverse == true || model.data.collectJobReverse == true)
                        {
                            InterestNew.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            InterestNew.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }


        #region "公共方法"
        private async Task IniJobDynamicList()
        {
            jobDynamicModelList = new ObservableCollection<IMControlModel>();
            await SetJobDynamicList();
            if (RBInterest.IsChecked == true)
            {
                if (jobDynamicModelList.Count > 0)
                {

                    lstJobDynamic.SelectedIndex = 0;
                    IMControlModel temp = lstJobDynamic.SelectedItem as IMControlModel;

                    SVUCResume.ScrollToTop();
                    temp.IsOpenResum = true;
                    this.curJobDynamicModel = temp;
                    this.curIMConotrol = temp;
                    this.curIMConotrol.ResumeDetail = await openResume(temp);
                    btnCollection.DataContext = this.curIMConotrol.ResumeDetail;
                    this.curIMConotrol.ResumeDetail.timeType = this.curIMConotrol.IsCollectJob ? "收藏" : "查看";
                    this.curIMConotrol.ResumeDetail.resumeTime = this.curIMConotrol.CreateTime.ToString("yyyy-MM-dd");
                }
            }
        }

        private async Task SetJobDynamicList()
        {
          
            List<string> pkey = new List<string> { "userID", "startRow", "pageSize","jobType" };
            List<string> pvalue = new List<string> { MagicGlobal.UserInfo.Id.ToString(), "0", "100","2" };

            if (curDynamicJobID != -1)
            {
                pkey.Add("jobID");
                pvalue.Add(curDynamicJobID.ToString());
            }
            tabHintDynamic.Visibility = Visibility.Collapsed;
            DynamicResult.Visibility = Visibility.Collapsed;
            DynamicLoading.Visibility = Visibility.Visible;
            svDynamic.Visibility = Visibility.Collapsed;

            string std = DAL.JsonHelper.JsonParamsToString(pkey, pvalue);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrViewAndColectList, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<List<HttpCollect>> model = DAL.JsonHelper.ToObject<BaseHttpModel<List<HttpCollect>>>(jsonResult);

            DynamicLoading.Visibility = Visibility.Collapsed;
            svDynamic.Visibility = Visibility.Visible;
            if (model != null)
            {
                if (model.code == 200)
                {
                    this.jobDynamicModelList.Clear();
                    ViewModel.HttpTrackEventDataModel eventModel = new HttpTrackEventDataModel();
                    eventModel.session = Guid.NewGuid().ToString();
                    this.jobDynamicSession = eventModel.session;
                    eventModel.algo = "0";
                    eventModel.item = new List<string>();
                    eventModel.userID = MagicGlobal.UserInfo.Id.ToString();


                    foreach (HttpCollect iHttpCollect in model.data)
                    {

                        IMControlModel temp = new IMControlModel();
                        if (iHttpCollect.resume != null)
                        {
                            temp.gender = iHttpCollect.resume.gender.ToString();
                            temp.name = iHttpCollect.resume.name;
                            temp.avatarUrl = Downloader.LocalPath(iHttpCollect.resume.avatar, temp.name);
                            temp.UserID = iHttpCollect.resume.userID;
                            temp.CreateTime = iHttpCollect.relation.updateTime;
                            temp.name = ResumeName.GetName(iHttpCollect.resume.name, temp.UserID.ToString());
                            if (model.extraData != null)
                            {
                                if (TimeHelper.GetTime(model.extraData.previousTime.ToString()) < iHttpCollect.relation.updateTime)
                                {
                                    temp.IsJobRead = false;
                                }
                            }


                        }
                        if (iHttpCollect.job != null)
                        {
                            temp.job = iHttpCollect.job.jobName;
                            temp.jobId = iHttpCollect.job.jobID;
                        }

                        temp.IsCollectJob = (iHttpCollect.relation.relationSign == "collectJobReverse") ? true : false;
                        eventModel.item.Add(temp.UserID.ToString());
                        this.jobDynamicModelList.Add(temp);
                    }
                    lstJobDynamic.ItemsSource = jobDynamicModelList;
                    if (jobDynamicModelList.Count == 0)
                    {
                        if (curDynamicJobID > -1)
                        {
                            DynamicResult.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            tabHintDynamic.Visibility = Visibility.Visible;
                        }
                    }
                    //查看感兴趣列表埋点
                    Common.TrackHelper2.TrackOperation("5.2.3.14.1", "rv", DAL.JsonHelper.ToJsonString(eventModel));
                }
            }

        }
        
        #endregion

        #region "事件"
        private async  void gdJobDynamic_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool visible = (bool)e.NewValue;
            if (visible)
            {

                this.gdMessageAndResume.SetBinding(Grid.DataContextProperty, new Binding("SelectedItem") { Source = this.lstJobDynamic });
                if (lstJobDynamic.SelectedItem != null)
                {
                    curIMConotrol = lstJobDynamic.SelectedItem as IMControlModel;
                    if (curIMConotrol!=null)
                        curIMConotrol.IsOpenResum = true;
                }

                if (jobDynamicModelList.Count > 0)
                {

                    lstJobDynamic.SelectedIndex = 0;
                    IMControlModel temp = lstJobDynamic.SelectedItem as IMControlModel;

                    SVUCResume.ScrollToTop();
                    temp.IsOpenResum = true;
                    this.curJobDynamicModel = temp;
                    this.curIMConotrol = temp;
                    this.curIMConotrol.ResumeDetail = await openResume(temp);
                    btnCollection.DataContext = this.curIMConotrol.ResumeDetail;
                    this.curIMConotrol.ResumeDetail.timeType = this.curIMConotrol.IsCollectJob ? "收藏" : "查看";
                    this.curIMConotrol.ResumeDetail.resumeTime = this.curIMConotrol.CreateTime.ToString("yyyy-MM-dd");
                }

                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "userIdentity", "relationSign" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "2", "viewJobReverse,collectJobReverse" });
                string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrClearRedFunction, MagicGlobal.UserInfo.Version, std));
            }
            else
            {
                InterestNew.Visibility = Visibility.Collapsed;
            }
        }


        private async void bgdDynamic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IMControlModel temp = (sender as Grid).DataContext as IMControlModel;

            //埋点
            Common.TrackHelper2.TrackOperation("5.2.4.1.1", "pv");

            SVUCResume.ScrollToTop();
            temp.IsOpenResum = true;
            temp.IsJobRead = true;
            this.curJobDynamicModel = temp;
            this.curIMConotrol = temp;
            this.lstJobDynamic.SelectedItem = temp;
            this.curIMConotrol.ResumeDetail = await openResume(temp);
            btnCollection.DataContext = this.curIMConotrol.ResumeDetail;
            this.curIMConotrol.ResumeDetail.timeType = this.curIMConotrol.IsCollectJob ? "收藏" : "查看";
            this.curIMConotrol.ResumeDetail.resumeTime = this.curIMConotrol.CreateTime.ToString("yyyy-MM-dd");
        }
           

        private void bgdDynamic_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
           
        }

        private async void cbJobDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbJobDynamic.SelectedItem == null)
                return;
            if ((cbJobDynamic.SelectedItem as HttpJobList).jobID == curDynamicJobID)
                return;
            curDynamicJobID = (cbJobDynamic.SelectedItem as HttpJobList).jobID;
            await IniJobDynamicList();


        }
        #endregion
    }
}
