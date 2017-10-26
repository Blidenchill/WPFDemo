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
using MagicCube.ViewModel;
using System.IO;


namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCAutAssociation.xaml 的交互逻辑
    /// </summary>
    public partial class UCAutAssociation : UserControl
    {
        HttpJobCatagaryCodeRoot pJobCatagaryRoot;
        public UCAutAssociation()
        {
            InitializeComponent();
            string pResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "jobCatagary.json");
            pJobCatagaryRoot = DAL.JsonHelper.ToObject<HttpJobCatagaryCodeRoot>(pResult);
            itemSel.ItemsSource = this.SelectedVals;
        }
        public Action<bool> OpenAciton;


        public ObservableCollection<ValCommon1> SelectedVals = new ObservableCollection<ValCommon1>();


        private ObservableCollection<ValCommon1> TreeList = new ObservableCollection<ValCommon1>();

        private void TwoVal_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ValCommon1 temp = ((sender as TextBlock).DataContext as ValCommon1);

            if (this.SelectedVals.Count == 3)
                return;

            this.txtInput.Clear();
            foreach(var item in this.SelectedVals)
            {
                if (item.code == temp.code)
                    return;
            }
            this.SelectedVals.Add(temp);
            this.txtWaterMark.Visibility = Visibility.Collapsed;
            if (this.SelectedVals.Count == 3)
            {
                this.txtInput.IsEnabled = false;
            }
            
            
        }

        private void ThreeVal_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ValCommon2 temp = ((sender as TextBlock).DataContext as ValCommon2);
           

            if (this.SelectedVals.Count == 3)
                return;
            this.txtInput.Clear();
            foreach (var item in this.SelectedVals)
            {
                if (item.code == temp.code1)
                    return;
            }
            this.SelectedVals.Add(new ValCommon1() { name = temp.name2, code = temp.code1, parentCode = temp.parentCode1 });
            this.txtWaterMark.Visibility = Visibility.Collapsed;
            if (this.SelectedVals.Count == 3)
            {
                this.txtInput.IsEnabled = false;
            }
        }

        private void btnOpenSel_Click(object sender, RoutedEventArgs e)
        {
            if(this.OpenAciton != null)
            {
                this.OpenAciton((bool)this.chkOpenSel.IsChecked);
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = this.txtInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(temp))
            {
                //this.pu.Visibility = Visibility.Collapsed;
                this.popupMenu.IsOpen = false;
                this.txtWaterMark.Visibility = Visibility.Visible;
                this.bdNull.Visibility = Visibility.Collapsed;
                this.bdMain.Visibility = Visibility.Visible;
            }
            else
            {
                this.txtWaterMark.Visibility = Visibility.Collapsed;
                List<ValCommon1> temp3 = new List<ValCommon1>();
                temp3 = CheckVal(this.txtInput.Text);
                //foreach(var item in temp3)
                //{
                //    this.TreeList.Add(item);
                //}
                if(temp3.Count > 0)
                {
                    this.bdNull.Visibility = Visibility.Collapsed;
                    this.bdMain.Visibility = Visibility.Visible;
                }
                else
                {
                    this.bdNull.Visibility = Visibility.Visible;
                    this.bdMain.Visibility = Visibility.Collapsed;
                }

                this.itemMain.ItemsSource = temp3;
                //this.pu.Visibility = Visibility.Visible;
                //this.pu.IsOpen = true;
                this.popupMenu.IsOpen = true;
            }
            if(this.SelectedVals.Count > 0)
            {
                this.txtWaterMark.Visibility = Visibility.Collapsed;
            }
            
                
        }

        private List<ValCommon1> CheckVal(string strInput)
        {
            List<ValCommon1> temp2 = new List<ValCommon1>();
            List<ValCommon2> temp3 = new List<ValCommon2>();
           
            foreach (HttpJobCatagaryCodeRECORD iRECORD in pJobCatagaryRoot.RECORDS.RECORD)
            {
                if(iRECORD.type_name.Contains(strInput))
                {
                    if (iRECORD.type_level == "2")
                    {
                        temp2.Add(new ValCommon1() { code = iRECORD.type_code, name = iRECORD.type_name, parentCode = iRECORD.parent_type_code });
                    }
                    if (iRECORD.type_level == "3")
                    {
                        temp3.Add(new ValCommon2() { code1 = iRECORD.type_code, name2 = iRECORD.type_name, parentCode1 = iRECORD.parent_type_code });
                        
                    }
                }               
            }
            foreach(var item3 in temp3)
            {
                bool flag = false;
                foreach (var item2 in temp2)
                {
              
                    if (item3.parentCode1 == item2.code)
                    {
                        flag = true;
                        break;
                    }
                    
                }
                if (!flag)
                {
                    foreach (HttpJobCatagaryCodeRECORD iRECORD in pJobCatagaryRoot.RECORDS.RECORD)
                    {
                        if(iRECORD.type_code == item3.parentCode1)
                        {
                            temp2.Add(new ValCommon1() { code = iRECORD.type_code, name = iRECORD.type_name, parentCode = iRECORD.parent_type_code });
                        }
                    }
                }

            }
       

            

            foreach(var item2 in temp2)
            {
                item2.ChildList = new List<ValCommon2>();
                foreach(var item3 in temp3)
                {
                    if(item2.code == item3.parentCode1)
                    {
                        item2.ChildList.Add(item3);
                    }
                }
            }

                return temp2;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ValCommon1 temp = ((sender as Button).DataContext as ValCommon1);
            this.SelectedVals.Remove(temp);
            if(this.SelectedVals.Count < 3)
            {
                this.txtInput.IsEnabled = true;
            }
            if(this.SelectedVals.Count == 0 && string.IsNullOrEmpty(this.txtInput.Text))
            {
                this.txtWaterMark.Visibility = Visibility.Visible;
            }
            this.txtInput.Focus();
        }

        private void txtInput_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtInput.Clear();
            this.bdNull.Visibility = Visibility.Collapsed;
            this.bdMain.Visibility = Visibility.Visible;
        }

        private void txtInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Down)
            //{
            //    this.popupMenu.Focus();
            //    this.itemMain.Focus();
            //    this.popupMenu.IsOpen = true;
            //}
        }

        private void itemMain_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    this.txtInput.Focusable = false;
            //    this.txtInput.Text = this.itemMain.SelectedValue.ToString();
            //    this.popupMenu.IsOpen = false;
            //}
                
        }

        private void ucLocal_GotFocus(object sender, RoutedEventArgs e)
        {
            bdMainInput.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
        }

        private void ucLocal_LostFocus(object sender, RoutedEventArgs e)
        {
            bdMainInput.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
        }
    }
}
