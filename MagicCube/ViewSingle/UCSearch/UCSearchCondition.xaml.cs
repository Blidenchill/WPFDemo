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
using System.Reflection;

using MagicCube.HttpModel;
using MagicCube.Common;

using MagicCube.BindingConvert;
using System.Windows.Media.Animation;
using System.IO;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCSearchCondition.xaml 的交互逻辑
    /// </summary>
    public partial class UCSearchCondition : UserControl
    {
        public UCSearchCondition()
        {
            InitializeComponent();

            //this.Height = SystemParameters.PrimaryScreenHeight;
            //绑定
            this.SVSearchItem.DataContext = httpSearchList;
            this.lstSaveSearchCondition.ItemsSource = MagicGlobal.saveSearchConditionList;

            this.cmbTimeInterval.ItemsSource = timeIntervalList;
            this.cmbTargetSalary.ItemsSource = targetSalaryList;
            this.cmbTargetWorkLocation.ItemsSource = targetWorkLocationList;
            this.cmbWorkExp.ItemsSource = workingExpList;
            this.cmbMinDegree.ItemsSource = minDegreeList;
            this.cmbMaxDegree.ItemsSource = maxDegreeList;
            this.cmbLocation.ItemsSource = locationList;
            this.stackPanelSaveCondition.SetBinding(StackPanel.VisibilityProperty, new Binding() { Source = MagicGlobal.saveSearchConditionList, Path = new PropertyPath("Count"), Converter = new ListCount0ToCollapseConverter() });
        }
        #region "委托"
        public Action<SaveSearchCondition> StartSearchingAction;
        #endregion

        #region "变量"
        public HttpSearchList httpSearchList = new HttpSearchList();
        public SearchingCmbListItems searchingCmbListItems;
        public ObservableCollection<string> minDegreeList = new ObservableCollection<string>();
        public ObservableCollection<string> maxDegreeList = new ObservableCollection<string>();

        public ObservableCollection<string> timeIntervalList = new ObservableCollection<string>();
        public ObservableCollection<string> targetSalaryList = new ObservableCollection<string>();
        public ObservableCollection<string> targetWorkLocationList = new ObservableCollection<string>();
        public ObservableCollection<string> workingExpList = new ObservableCollection<string>();
        public ObservableCollection<string> locationList = new ObservableCollection<string>();
        private string localSearchConditionStr = string.Empty;
        private string httpSearchConditionStr = string.Empty;

        private SaveSearchCondition saveSearchCondition = new SaveSearchCondition();
        #endregion

        #region "对内方法"
        public void ObservableCollectionAddRange(IEnumerable<string> collection, ObservableCollection<string> observable)
        {
            foreach (var item in collection)
            {
                observable.Add(item);
            }
        }

        public void ObservableCollectionRemoveRange(ObservableCollection<string> observable, int index, int count)
        {
            if (observable.Count < count)
                return;
            for (int i = 1; i < count; i++)
            {
                observable.RemoveAt(0);
            }
        }

        private bool GetSearchParamStr(HttpSearchList list)
        {
            httpSearchConditionStr = string.Empty;
            localSearchConditionStr = string.Empty;
            bool flag = false;
            bool IsAgeSetting = false;
            bool IsDegreeSetting = false;

            if (list.partialValue != null && list.partialValue != string.Empty)
            {
                string temp = list.partialValue.Trim();
                if (temp != string.Empty)
                {
                    httpSearchConditionStr += "partialValue=" + list.partialValue + "&";
                    localSearchConditionStr += list.partialValue + "/";
                    flag = true;
                }
            }
            if (list.workingExp != null && list.workingExp != "不限")
            {
                httpSearchConditionStr += "workingExp=" + list.workingExp + "&";
                localSearchConditionStr += list.workingExp + "/";
                flag = true;

            }
            if (list.minDegree != null && list.minDegree != "不限")
            {
                httpSearchConditionStr += "minDegree=" + list.minDegree + "&";
                localSearchConditionStr += list.minDegree + "-";
                flag = true;
                IsDegreeSetting = true;
            }

            if (list.maxDegree != null && list.maxDegree != "不限")
            {
                httpSearchConditionStr += "maxDegree=" + list.maxDegree + "&";
                localSearchConditionStr += list.maxDegree + "/";
                flag = true;
            }
            else
            {
                if (IsDegreeSetting)
                {
                    localSearchConditionStr.Remove(localSearchConditionStr.Length - 1);
                    localSearchConditionStr += "/";
                }
            }
            if (list.minAge != null && list.minAge != string.Empty)
            {
                httpSearchConditionStr += "minAge=" + this.txtMinAge.Text + "&";
                localSearchConditionStr += this.txtMinAge.Text + "岁-";
                flag = true;
                IsAgeSetting = true;

            }
            if (list.maxAge != null && list.maxAge != string.Empty)
            {
                httpSearchConditionStr += "maxAge=" + list.maxAge + "&";
                localSearchConditionStr += list.maxAge + "岁/";
                flag = true;
            }
            else
            {
                if (IsAgeSetting)
                {
                    localSearchConditionStr.Remove(localSearchConditionStr.Length - 1);
                    localSearchConditionStr += "/";
                }

            }
            if (list.targetWorkLocation != null && list.targetWorkLocation != "不限")
            {
                httpSearchConditionStr += "targetWorkLocation=" + list.targetWorkLocation + "&";
                localSearchConditionStr += list.targetWorkLocation + "/";
                flag = true;

            }
            if (list.targetSalary != null && list.targetSalary != "不限")
            {
                httpSearchConditionStr += "targetSalary=" + list.targetSalary + "&";
                localSearchConditionStr += list.targetSalary + "/";
                flag = true;

            }
            if (list.location != null && list.location != "不限")
            {
                httpSearchConditionStr += "location=" + list.location + "&";
                localSearchConditionStr += list.location + "/";
                flag = true;

            }
            if (list.timeInterval != null && list.timeInterval != "不限")
            {
                httpSearchConditionStr += "timeInterval=" + list.timeInterval;
                localSearchConditionStr += list.timeInterval;
                flag = true;

            }
            localSearchConditionStr = localSearchConditionStr.TrimEnd(new char[] { '/' });
            httpSearchConditionStr = httpSearchConditionStr.TrimEnd(new char[] { '&' });
            //string conditionShow = localSearchConditionStr.Replace('/', '+');
            this.saveSearchCondition.LocalCondition = localSearchConditionStr.TrimEnd(new char[] { '/' });
            this.saveSearchCondition.HttpCondition = httpSearchConditionStr.TrimEnd(new char[] { '/' });
            this.saveSearchCondition.LocalHead = SaveConditionConvert(localSearchConditionStr);
            return flag;
        }
        private string SaveConditionConvert(string localCondition)
        {
            string temp = null;
            //string[] items = localCondition.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            //if (items.Length != 0)
            //{
            //    temp = items[0];
            //}
            
            if((bool)this.chkSaveCondition.IsChecked)
            {
                if (string.IsNullOrEmpty(this.txtSaveConditionHead.Text.Trim()))
                {
                    temp = localCondition;
                }
                else
                {
                    temp = this.txtSaveConditionHead.Text;
                }
            }
            else
            {
                temp = localCondition;
            }
            return temp;
        }

        private void InitalSearchTable()
        {
            httpSearchList.maxAge = null;
            httpSearchList.minAge = null;
            httpSearchList.maxDegree = "不限";
            httpSearchList.minDegree = "不限";
            httpSearchList.partialValue = null;
            httpSearchList.targetSalary = "不限";
            httpSearchList.timeInterval = "不限";
            httpSearchList.targetWorkLocation = "不限";
            httpSearchList.workingExp = "不限";
            httpSearchList.location = "不限";
        }

        #endregion

        #region "公共方法"
        public void IniSearchingList()
        {
            Action method = new Action(() =>
            {
                ////初始化服务器获取设置项列表。
                //string iResult = DAL.HttpHelper.Instance.HttpGet(ConfUtil.ServerConditionGet);
                string iResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "SearchResult.json");
                InitalSearchTable();

                if (!iResult.StartsWith("连接失败"))
                {
                    this.searchingCmbListItems = DAL.JsonHelper.ToObject<SearchingCmbListItems>(iResult);

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.timeIntervalList.Clear();
                        this.timeIntervalList.Add("不限");
                        //UCResumSearch.timeIntervalList.AddRange(UCResumSearch.searchingCmbListItems.timeInterval);
                        this.ObservableCollectionAddRange(this.searchingCmbListItems.timeInterval, this.timeIntervalList);
                        this.targetSalaryList.Clear();
                        this.targetSalaryList.Add("不限");
                        //UCResumSearch.targetSalaryList.AddRange(UCResumSearch.searchingCmbListItems.targetSalary);
                        this.ObservableCollectionAddRange(this.searchingCmbListItems.targetSalary, this.targetSalaryList);
                        this.targetWorkLocationList.Clear();
                        this.targetWorkLocationList.Add("不限");
                        //UCResumSearch.targetWorkLocationList.AddRange(UCResumSearch.searchingCmbListItems.targetWorkLocation);
                        this.ObservableCollectionAddRange(this.searchingCmbListItems.targetWorkLocation, this.targetWorkLocationList);
                        this.locationList.Clear();
                        this.locationList.Add("不限");
                        //UCResumSearch.locationList.AddRange(UCResumSearch.searchingCmbListItems.targetWorkLocation);
                        this.ObservableCollectionAddRange(this.searchingCmbListItems.targetWorkLocation, this.locationList);
                        this.workingExpList.Clear();
                        this.workingExpList.Add("不限");
                        //UCResumSearch.workingExpList.AddRange(UCResumSearch.searchingCmbListItems.workingExp);
                        this.ObservableCollectionAddRange(this.searchingCmbListItems.workingExp, this.workingExpList);

                        this.minDegreeList.Clear();
                        this.minDegreeList.Add("不限");
                        this.ObservableCollectionAddRange(this.searchingCmbListItems.degree, this.minDegreeList);
                        this.maxDegreeList.Clear();
                        this.maxDegreeList.Add("不限");
                        this.ObservableCollectionAddRange(this.searchingCmbListItems.degree, this.maxDegreeList);
                        this.cmbTimeInterval.SelectedIndex = 0;
                        this.cmbTargetSalary.SelectedIndex = 0;
                        this.cmbTargetWorkLocation.SelectedIndex = 0;
                        this.cmbWorkExp.SelectedIndex = 0;
                        this.cmbMinDegree.SelectedIndex = 0;
                        this.cmbMaxDegree.SelectedIndex = 0;
                        this.cmbLocation.SelectedIndex = 0;
                        //this.txtMaxAge.Clear();
                        //this.txtMinAge.Clear();
                        //this.txtPartialValue.Clear();
                        this.httpSearchList.partialValue = null;
                        this.httpSearchList.maxAge = null;
                        this.httpSearchList.minAge = null;
                    })
                    );
                }
            });
            method.BeginInvoke(null, null);

        }

        public void SetSearchListFromStr(string httpStr)
        {
            InitalSearchTable();
            string[] strItems = httpStr.Split(new char[] { '&' });
            foreach (var item in strItems)
            {
                string[] temp = item.Split(new char[] { '=' });
                if (temp.Length < 2)
                    continue;
                Type type = httpSearchList.GetType();
                PropertyInfo TeaNameProperty = type.GetProperty(temp[0]);
                if (TeaNameProperty == null)
                    return;
                TeaNameProperty.SetValue(httpSearchList, temp[1], null);
            }
        }

        //public void SearchConditionPnlAnimation(double height)
        //{
        //    DoubleAnimation heightAnimation = new DoubleAnimation();
        //    //if (flag)
        //    //{
        //    //    //DoubleAnimation widthAnimation = new DoubleAnimation();
        //    //    //widthAnimation.To = 100;
        //    //    //widthAnimation.Duration = TimeSpan.FromMilliseconds(100);
        //    //    //this.grdTest.BeginAnimation(Grid.WidthProperty, widthAnimation);
        //    //    //heightAnimation.To = 1;
        //    //    //heightAnimation.Duration = TimeSpan.FromMilliseconds(300);
        //    //    //this.grdSearchPanel.BeginAnimation(Grid.HeightProperty, heightAnimation);
        //    //    this.grdSearchPanel.Height = 0;

        //    //}
        //    //else
        //    //{
        //    //    heightAnimation.To = height;
        //    //    heightAnimation.Duration = TimeSpan.FromMilliseconds(500);
        //    //    this.grdSearchPanel.BeginAnimation(Grid.HeightProperty, heightAnimation);
        //    //}
        //    heightAnimation.To = height;
        //    heightAnimation.Duration = TimeSpan.FromMilliseconds(300);
        //    this.grdSearchPanel.BeginAnimation(Grid.HeightProperty, heightAnimation);
        //}
        #endregion



        #region "事件"
        private void ClearTextBlock_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                switch (btn.Name)
                {
                    case "btnPartialValueClear":
                        //this.txtPartialValue.Text = string.Empty;
                        //this.txtPartialValue.Clear();
                        //this.txtPartialValue.ClearValue(TextBox.TextProperty);
                        httpSearchList.partialValue = null;
                        break;
                    case "btnMinAgeClear":
                        httpSearchList.minAge = null;
                        break;
                    case "btnMaxAgeClear":
                        httpSearchList.maxAge = null;
                        break;
                }
            }
        }

        private void Degree_DataChanged(object sender, SelectionChangedEventArgs args)
        {
            ComboBox listBoxTemp = sender as ComboBox;
            if (listBoxTemp == null)
                return;
            if (listBoxTemp.SelectedItem == null)
                return;
            // ListBoxItem lbi = (listBoxTemp.SelectedItem as ListBoxItem);
            string content = listBoxTemp.SelectedItem.ToString();
            if (content == "不限")
            {
                maxDegreeList.Clear();
                maxDegreeList.Add("不限");
                //this.ObservableCollectionAddRange(searchingCmbListItems.degree, maxDegreeList);
                //this.cmbMaxDegree.SelectedIndex = 1;
                this.cmbMaxDegree.SelectedIndex = 0;
                return;
            }
            if (content == "其他")
            {
                maxDegreeList.Clear();
                maxDegreeList.Add("其他");
                this.cmbMaxDegree.SelectedIndex = 0;
                return;
            }
            this.maxDegreeList.Clear();
            maxDegreeList.Add("不限");
            this.ObservableCollectionAddRange(searchingCmbListItems.degree, maxDegreeList);
            maxDegreeList.Remove("其他");
            for (int i = 0; i < minDegreeList.Count; i++)
            {
                if (minDegreeList[i] == content)
                {
                    this.ObservableCollectionRemoveRange(maxDegreeList, 0, i + 1);
                    maxDegreeList.Insert(0, "及以上");
                    break;
                }
            }
            this.cmbMaxDegree.SelectedIndex = 1;
            this.cmbMaxDegree.SelectedIndex = 0;

        }

        private void ClearHistorySearchCondition_Click(object sender, RoutedEventArgs e)
        {
            SaveSearchCondition itemCondition = new SaveSearchCondition();
            itemCondition = (sender as Button).DataContext as SaveSearchCondition;
            if (itemCondition != null)
                MagicGlobal.saveSearchConditionList.Remove(itemCondition);

        }

        private void txt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Back)
            {
                //if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
                //{
                //    e.Handled = true;
                //}
                //if (textBox.Text.Length > 1 && e.Key != Key.Back)
                //{
                //    e.Handled = true;
                //}
                //if (textBox.Text.Length == 0 && e.Key == Key.D0)
                //{
                //    e.Handled = true;
                //}
                //if (textBox.Text.Length == 0 && e.Key == Key.NumPad0)
                //{
                //    e.Handled = true;
                //}
            }
            else
                e.Handled = true;
            //if (textBox.Text.Length == 2)
            //    if (textBox.SelectedText.Length != 2)
            //        e.Handled = true;
        }

        private void SearchResumStart(object sender, RoutedEventArgs e)
        {
            MatchAgeSetting();

            GetSearchParamStr(this.httpSearchList);
            if((bool)this.chkSaveCondition.IsChecked)
            {
                this.SaveCollectionCondition();
                chkSaveCondition.IsChecked = false;
            }
            if (this.StartSearchingAction != null)
            {
                StartSearchingAction(this.saveSearchCondition);
            }
        }
        private void SaveCollectionCondition()
        {
            bool flag = this.GetSearchParamStr(httpSearchList);
            if (!flag)
                return;
            if (MagicGlobal.saveSearchConditionList.Count < 10)
            {
                MagicGlobal.saveSearchConditionList.Add(new SaveSearchCondition() { HttpCondition = httpSearchConditionStr, LocalCondition = localSearchConditionStr, LocalHead = SaveConditionConvert(localSearchConditionStr) });
            }
            else
            {
                MagicGlobal.saveSearchConditionList.RemoveAt(0);
                MagicGlobal.saveSearchConditionList.Add(new SaveSearchCondition() { HttpCondition = httpSearchConditionStr, LocalCondition = localSearchConditionStr, LocalHead = SaveConditionConvert(localSearchConditionStr) });
            }

        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SaveSearchCondition itemCondition = new SaveSearchCondition();
            SaveSearchCondition temp = (sender as StackPanel).DataContext as SaveSearchCondition;
            itemCondition.HttpCondition = temp.HttpCondition;
            itemCondition.LocalCondition = temp.LocalCondition;
            itemCondition.LocalHead = temp.LocalHead;


            if (this.StartSearchingAction != null)
            {
                StartSearchingAction(itemCondition);
            }
        }
        /// <summary>
        /// 清除当前搜索条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSearchCondition(object sender, RoutedEventArgs e)
        {
            IniSearchingList();
        }
        #endregion

        private void txtMinAge_LostFocus(object sender, RoutedEventArgs e)
        {
            MatchAgeSetting();
        }

        private void txtMaxAge_LostFocus(object sender, RoutedEventArgs e)
        {
            MatchAgeSetting();
        }

        private void MatchAgeSetting()
        {
            if(string.IsNullOrEmpty(this.httpSearchList.minAge) && string.IsNullOrEmpty(this.httpSearchList.maxAge))
            {
                return;
            }
            if(string.IsNullOrEmpty(this.httpSearchList.minAge))
            {
                this.httpSearchList.minAge = "1";
            }
            if(string.IsNullOrEmpty(this.httpSearchList.maxAge))
            {
                this.httpSearchList.maxAge = "99";
            }
            if(Convert.ToInt32(this.httpSearchList.minAge) > Convert.ToInt32(this.httpSearchList.maxAge))
            {
                string temp = this.httpSearchList.minAge;
                this.httpSearchList.minAge = this.httpSearchList.maxAge;
                this.httpSearchList.maxAge = temp;
            }
        }
    }
}
