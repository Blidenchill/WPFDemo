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
using MagicCube.ViewModel;
using MagicCube.HttpModel;
using System.IO;
using System.Collections.ObjectModel;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCIndustry.xaml 的交互逻辑
    /// </summary>
    public partial class UCIndustry : UserControl
    {

        public Action OpenAction;
        public UCIndustry()
        {
            InitializeComponent();
            this.Loaded += UCIndustry_Loaded;
        }

        public ObservableCollection<HttpIndustresItem> industriesList = new ObservableCollection<HttpIndustresItem>();
        public ObservableCollection<HttpIndustresItem> selectIndustry = new ObservableCollection<HttpIndustresItem>();

        private ObservableCollection<HttpIndustresItem> autoAssociateList = new ObservableCollection<HttpIndustresItem>();

        private void  UCIndustry_Loaded(object sender, RoutedEventArgs e)
        {
            string industriesResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "companyInfoDict.json");
            jsonCompanyInfoDictModel jsonCompanyModel = DAL.JsonHelper.ToObject<jsonCompanyInfoDictModel>(industriesResult);
            if (jsonCompanyModel != null)
            {
                industriesList.Clear();
                foreach (var item in jsonCompanyModel.DataDic.element)
                {
                    if (item.name == "industry")
                    {
                        foreach (var industryItem in item.mappingItem)
                        {
                            industriesList.Add(new HttpIndustresItem() { name = industryItem.fullName, code = industryItem.value, isChoose = false });
                        }
                    }
                   
                }
            }
            //this.itemsIndustries.ItemsSource = this.industriesList;
            this.itemSel.ItemsSource = this.selectIndustry;
            this.popupAutoList.ItemsSource = this.autoAssociateList;
        }


        private void IndustryBorder_IsVisibaleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        //private void chkIndustry_Click(object sender, RoutedEventArgs e)
        //{
        //    CheckBox chk = sender as CheckBox;
        //    if ((bool)chk.IsChecked)
        //    {
        //        if (this.selectIndustry.Count == 3)
        //        {
        //            foreach (var item in industriesList)
        //            {
        //                if (item.name == this.selectIndustry[0].name)
        //                {
        //                    item.isChoose = false;
        //                    this.itemsIndustries.ItemsSource = null;
        //                    this.itemsIndustries.ItemsSource = this.industriesList;
        //                    break;
        //                }
        //            }
        //            foreach (var item in industriesList)
        //            {
        //                if (item.name == (chk.Content as TextBlock).Text)
        //                {
        //                    this.selectIndustry.RemoveAt(0);
        //                    this.selectIndustry.Add(item);
        //                }
        //            }

        //        }
        //        else
        //        {
        //            this.selectIndustry.Add(chk.DataContext as HttpIndustresItem);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var item in this.selectIndustry)
        //        {
        //            if (item.name == (chk.Content as TextBlock).Text)
        //            {
        //                this.selectIndustry.Remove(item);
        //                break;
        //            }
        //        }
        //    }
        //}

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = this.txtInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(temp))
            {
                if (this.selectIndustry.Count == 0)
                    this.txtWaterMark.Visibility = Visibility.Visible;
                else
                    this.txtWaterMark.Visibility = Visibility.Collapsed;
                //this.pu.Visibility = Visibility.Collapsed;
                this.popAutoAssociate.IsOpen = false;
                Console.WriteLine("dadian1");
                this.bdNull.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.txtWaterMark.Visibility = Visibility.Collapsed;
                GetAutoAssociate(temp);
                this.popAutoAssociate.IsOpen = true;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            HttpIndustresItem temp = ((sender as Button).DataContext as HttpIndustresItem);
            this.selectIndustry.Remove(temp);
            if(this.selectIndustry.Count < 3)
            {
                this.txtInput.IsEnabled = true;
                foreach (var item in this.industriesList)
                {
                    if (item.name == temp.name)
                    {
                        item.isChoose = false;
                    }
                }
            }
            if(this.selectIndustry.Count > 0)
            {
                this.txtWaterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.txtWaterMark.Visibility = Visibility.Visible;
            }
            this.txtInput.Focus();
        }

        private void IndustrySelectOK_Click(object sender, RoutedEventArgs e)
        {
            this.selectIndustry.Clear();
            foreach(var item in this.industriesList)
            {
                if(item.isChoose)
                {
                    this.selectIndustry.Add(item);
                }
            }
            if (this.selectIndustry.Count > 0)
            {
                this.txtWaterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.txtWaterMark.Visibility = Visibility.Visible;
            }
            this.chkOpen.IsChecked = false;
        }

        private void IndustrySelectCancel_Click(object sender, RoutedEventArgs e)
        {
            this.chkOpen.IsChecked = false;
            foreach (var item in this.industriesList)
            {
                item.isChoose = false;
                foreach(var item2 in selectIndustry)
                {
                    if(item2.name == item.name)
                    {
                        item.isChoose = true;
                    }
                }
            }
            if (this.selectIndustry.Count > 0)
            {
                this.txtWaterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.txtWaterMark.Visibility = Visibility.Visible;
            }
        }

        private void GetAutoAssociate(string temp)
        {
            this.autoAssociateList.Clear();
            foreach(var item in industriesList)
            {
                if(item.name.Contains(temp))
                {
                    this.autoAssociateList.Add(item);
                }
            }
            if(this.autoAssociateList.Count > 0)
            {
                this.bdNull.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.bdNull.Visibility = Visibility.Visible;
            }
        }

        private void tb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HttpIndustresItem temp = ((sender as TextBlock).DataContext as HttpIndustresItem);
            if(this.selectIndustry.Count < 3)
            {
                foreach(var item in this.selectIndustry)
                {
                    if(temp.code == item.code)
                    {
                        return;
                    }
                }
                this.selectIndustry.Add(temp);
                this.txtInput.Clear();
                foreach (var item in this.industriesList)
                {
                    if (item.name == temp.name)
                    {
                        item.isChoose = true;
                    }
                }
            }
            if(this.selectIndustry.Count == 3)
            {
                this.txtInput.IsEnabled = false;
            }

            if (this.selectIndustry.Count > 0)
            {
                this.txtWaterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.txtWaterMark.Visibility = Visibility.Visible;
            }

        }

        private void chkOpen_Click(object sender, RoutedEventArgs e)
        {
            if (OpenAction != null)
                OpenAction();
        }

        private void txtInput_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtInput.Clear();
            this.bdNull.Visibility = Visibility.Collapsed;
        }

        private void txtInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (this.autoAssociateList.Count == 0)
            //    return;
            //if (e.Key == Key.Down)
            //{
            //    string temp = this.txtInput.Text;
            //    this.popAutoAssociate.Focus();
            //    this.popupAutoList.Focus();
            //    this.popAutoAssociate.IsOpen = true;
            //    this.txtInput.Text = temp;
               
            //}
        }

        private void popupAutoList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.selectIndustry.Add(this.popupAutoList.SelectedItem as HttpIndustresItem);
                this.txtWaterMark.Visibility = Visibility.Collapsed;
                this.txtInput.Clear();
                this.popAutoAssociate.IsOpen = false;
                Console.WriteLine("dadian2");
            }
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            bdMain.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            bdMain.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
        }
    }
}
