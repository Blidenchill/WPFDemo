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
using System.Windows.Media.Animation;
using System.Threading.Tasks;

using MagicCube.HttpModel;
using MagicCube.Model;
using MagicCube.BindingConvert;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCResumeCollection.xaml 的交互逻辑
    /// </summary>
    public partial class UCResumeCollection : UserControl
    {
        List<SearchTalentResumModel> test = new List<SearchTalentResumModel>();
        SearchConditionSaveModel CurrentsaveCondition = new SearchConditionSaveModel();
        public Action<bool> ResumShadeAction;
        public UCResumeCollection()
        {
            InitializeComponent();
            this.cmbSaveSearchList.ItemsSource = MagicGlobal.saveSearchConditionList;
            this.cmbSaveSearchList.SetBinding(ComboBox.VisibilityProperty, new Binding() { Source = MagicGlobal.saveSearchConditionList, Path = new PropertyPath("Count"), Converter = new ListCount0ToCollapseConverter() });
            this.rctHidenSearchConditionName.SetBinding(Rectangle.VisibilityProperty, new Binding() { Source = MagicGlobal.saveSearchConditionList, Path = new PropertyPath("Count"), Converter = new ListCount0ToVisibleConverter() });
            //this.ucSearchCondition.StartSearchingAction += StartSearchingCallback;
            //this.ucSearchResult.RapidSearchConditionAction += StartSearchingCallback;
            this.ucSearchCondition.SearchConditionAction += StartSearchingCallback;
            //ucSearchCondition.IniSearchingList();
            //埋点
            //Common.TrackHelper2.TrackOperation("5.5.1.1.1", "pv");
            Messaging.Messenger.Default.Register<Messaging.MSJobSearchConditionModel>(this, SearchResumCallback);

        }

        #region "事件"

        private async void btnReturnTalentSearch_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.4.4.1", "clk");

            this.ucSearchCondition.Visibility = Visibility.Visible;
            //this.SearchConditionPnlCollapse(false);
            //this.ucSearchCondition.SearchConditionPnlAnimation(SystemParameters.PrimaryScreenHeight);
            //this.grdUCSearchCondition.Height = SystemParameters.PrimaryScreenHeight;

            this.ucSearchResult.Visibility = Visibility.Collapsed;
            if (CurrentsaveCondition != null)
                await this.ucSearchCondition.InitialUCSearchCondition();
            this.ucSearchCondition.SetRetunCondition(CurrentsaveCondition);
            //if (CurrentsaveCondition != null)
                //this.ucSearchCondition.SetSearchListFromStr(CurrentsaveCondition.HttpCondition);
            //this.ucSearchCondition.IniSearchingList();
            //this.cmbSaveSearchList.SelectedIndex = -1;
        }

        private void btnReturnRapidSearchCondition(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.4.5.1", "clk");
            //this.ucSearchResult.ReturnRapidSearchCondition(CurrentsaveCondition);
            this.ucSearchResult.Visibility = Visibility.Collapsed;
            this.ucSearchCondition.Visibility = Visibility.Visible;

            this.ucSearchCondition.SetRetunCondition(CurrentsaveCondition);
        }
        private void cmbSaveSearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ComboBox cmb = sender as ComboBox;
            //CurrentsaveCondition = cmb.SelectedItem as SaveSearchCondition;
            //if (CurrentsaveCondition == null)
            //    return;
            //this.lblCurrentCondition.Text = CurrentsaveCondition.LocalCondition;
            //this.ucSearchResult.LoadingResult(CurrentsaveCondition);
        }

        #endregion

        #region "公共方法"
        public async  Task InitalCollection()
        {
            //Window.GetWindow(this).Title = "人才搜索";
            //ucSearchCondition.IniSearchingList();
            await ucSearchCondition.InitialUCSearchCondition();
            this.ucSearchCondition.Visibility = Visibility.Visible;
            this.ucSearchResult.Visibility = Visibility.Collapsed;
            this.ucSearchResult.InitialVisible();

        }
       
        public void InitalUCVisible()
        {
            //Window.GetWindow(this).Title = "人才搜索";
            if (this.ActualHeight == 0)
                return;
            //this.ucSearchCondition.SearchConditionPnlAnimation(SystemParameters.PrimaryScreenHeight);
            this.ucSearchCondition.Visibility = Visibility.Visible;
            if (CurrentsaveCondition != null)
                //this.ucSearchCondition.SetSearchListFromStr(CurrentsaveCondition.HttpCondition);
            this.cmbSaveSearchList.SelectedIndex = -1;
            this.ucSearchResult.InitialVisible();
        }
        #endregion

        #region "回调函数"
        private async Task StartSearchingCallback(SearchConditionSaveModel saveSearchCondition)
        {
            //新埋点
            Common.TrackHelper2.TrackOperation("5.6.4.1.1", "pv");
            this.ucSearchResult.Visibility = Visibility.Visible;
            this.ucSearchCondition.Visibility = Visibility.Collapsed;
            this.lblCurrentCondition.Text = saveSearchCondition.conditionHeadShow;
            CurrentsaveCondition = saveSearchCondition;

            await this.ucSearchResult.LoadingResult(saveSearchCondition);


        }

        private void ResumShadeActionCallback(bool flag)
        {
            //if (flag)
            //{
            //    this.gdShade.Visibility = System.Windows.Visibility.Visible;
            //    if (ResumShadeAction != null)
            //        ResumShadeAction(true);
            //}
            //el
            //{
            //    this.gdShade.Visibility = System.Windows.Visibility.Collapsed;
            //    if (ResumShadeAction != null)
            //        ResumShadeAction(false);
            //}
        }

        private  void SearchResumCallback(Messaging.MSJobSearchConditionModel model)
        {
            this.lblCurrentCondition.Text = model.ConditionHeadShow;
            this.ucSearchResult.Visibility = Visibility.Visible;
            this.ucSearchCondition.Visibility = Visibility.Collapsed;
            CurrentsaveCondition = null;
        }

        #endregion

        #region "内部函数"

        #endregion

        private void btnReturnSearchConditionPnl_Click(object sender, RoutedEventArgs e)
        {
            //this.ucSearchCondition.SearchConditionPnlAnimation(SystemParameters.PrimaryScreenHeight);
            this.ucSearchCondition.Visibility = Visibility.Visible;
            if (CurrentsaveCondition != null)
                //this.ucSearchCondition.SetSearchListFromStr(CurrentsaveCondition.HttpCondition);
            this.cmbSaveSearchList.SelectedIndex = -1;
            this.ucSearchResult.Visibility = Visibility.Collapsed;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window win = Window.GetWindow(this);
        }
    }
}
