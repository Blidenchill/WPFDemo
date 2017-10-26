using MagicCube.HttpModel;
using MagicCube.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCJobTree.xaml 的交互逻辑
    /// </summary>
    public partial class UCJobTree : UserControl
    {
        List<HttpJobTreeThree> m_HttpJobTreeThree;
        public Action<Val2> actionJobChoose;

        public Action<bool> ResultAction;
        ObservableCollection<Val> secondVal = new ObservableCollection<Val>();
        ObservableCollection<Val2> thridVal = new ObservableCollection<Val2>();

        public ObservableCollection<ValCommon1> SelectItems = new ObservableCollection<ValCommon1>();
        public UCJobTree()
        {
            InitializeComponent();
            this.Loaded += UCJobTree_Loaded;
        }
        public void iniJobTree()
        {
            if (m_HttpJobTreeThree == null)
            {
                string pResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "jobCatagary.json");
                HttpJobCatagaryCodeRoot pJobCatagaryRoot = DAL.JsonHelper.ToObject<HttpJobCatagaryCodeRoot>(pResult);
                m_HttpJobTreeThree = new List<HttpJobTreeThree>();
        
                foreach (HttpJobCatagaryCodeRECORD iRECORD in pJobCatagaryRoot.RECORDS.RECORD)
                {
                    if (iRECORD.type_level == "1")
                    {
                        m_HttpJobTreeThree.Add(new HttpJobTreeThree() { code = iRECORD.type_code, name = iRECORD.type_name });
                    }
                    if (iRECORD.type_level == "2")
                    {
                        secondVal.Add(new Val() { code = iRECORD.type_code, name = iRECORD.type_name, parentCode = iRECORD.parent_type_code });
                    }
                    if (iRECORD.type_level == "3")
                    {
                        thridVal.Add(new Val2() { code = iRECORD.type_code, name = iRECORD.type_name, parentCode = iRECORD.parent_type_code });
                    }
                }
                foreach (Val2 iVal2 in thridVal)
                {
                    foreach(var item in this.SelectItems)
                    {
                        if(item.code == iVal2.code)
                        {
                            iVal2.isChoose = true;
                        }
                    }
                    foreach (Val iVal in secondVal)
                    {
                        if (iVal2.parentCode == iVal.code)
                        {
                            if (iVal.val == null)
                                iVal.val = new List<Val2>();
                            iVal.val.Add(iVal2);
                            iVal2.parent = iVal;
                        }
                    }
                }
                foreach (Val iVal2 in secondVal)
                {
                    foreach(var item in this.SelectItems)
                    {
                        if(item.code == iVal2.code)
                        {
                            iVal2.isChoose = true;
                        }
                    }
                    foreach (HttpJobTreeThree iVal in m_HttpJobTreeThree)
                    {
                        if (iVal2.parentCode == iVal.code)
                        {
                            if (iVal.val == null)
                                iVal.val = new List<Val>();
                            iVal.val.Add(iVal2);
                            iVal2.parent = iVal;
                        }
                    }
                }

                icJobTree.ItemsSource = m_HttpJobTreeThree;
                if (m_HttpJobTreeThree.Count > 0)
                {
                    m_HttpJobTreeThree[0].IsCheck = true;
                    icJobContent.ItemsSource = m_HttpJobTreeThree[0].val;
                }
            }
            else
            {
                initialCheck();
            }
        }

        public void initialCheck()
        {
            foreach (Val2 iVal2 in thridVal)
            {
                iVal2.isChoose = false;
                foreach (var item in this.SelectItems)
                {
                    if (item.code == iVal2.code)
                    {
                        iVal2.isChoose = true;
                    }
                }
            }
            foreach (Val iVal2 in secondVal)
            {
                iVal2.isChoose = false;
                foreach (var item in this.SelectItems)
                {
                    if (item.code == iVal2.code)
                    {
                        iVal2.isChoose = true;
                    }
                }
            }
            icJobContent.ItemsSource = null;
            foreach (var item in m_HttpJobTreeThree)
            {
                if((bool)item.IsCheck)
                {
                    icJobContent.ItemsSource = item.val;
                }
            }
            //icJobContent.ItemsSource = m_HttpJobTreeThree[0].val;
            //icJobTree.ItemsSource = m_HttpJobTreeThree;
            //if (m_HttpJobTreeThree.Count > 0)
            //{
            //    foreach(var item in m_HttpJobTreeThree)
            //    {
            //        item.IsCheck = false;
            //    }
            //    m_HttpJobTreeThree[1].IsCheck = true;
            //    //icJobContent.ItemsSource = m_HttpJobTreeThree[0].val;
            //}
        }
        void UCJobTree_Loaded(object sender, RoutedEventArgs e)
        {
            iniJobTree();
            this.itemsChoosed.ItemsSource = SelectItems;
        }

        private void RadioButton_MouseEnter(object sender, MouseEventArgs e)
        {

            icJobContent.ItemsSource = ((sender as RadioButton).DataContext as HttpJobTreeThree).val;
            ((sender as RadioButton).DataContext as HttpJobTreeThree).IsCheck = true;
            svJobContent.ScrollToTop();
            foreach (HttpJobTreeThree iHttpJobTreeThree in m_HttpJobTreeThree)
            {
                if (iHttpJobTreeThree != ((sender as RadioButton).DataContext as HttpJobTreeThree))
                {
                    iHttpJobTreeThree.IsCheck = false;
                }
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Val2 pVal2 = ((sender as TextBlock).DataContext as Val2);

            if(this.SelectItems.Count < 3)
            {
               
                this.SelectItems.Add(new ValCommon1() { name = pVal2.name, code = pVal2.code });
                this.itemsChoosed.ItemsSource = SelectItems;
            }
            if (actionJobChoose!=null)
            {
                actionJobChoose(pVal2);
            }
        }
        private void TextBlock1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Val pVal2 = ((sender as TextBlock).DataContext as Val);

            if (this.SelectItems.Count < 3)
            {
                this.SelectItems.Add(new ValCommon1() { name = pVal2.name, code = pVal2.code });
                this.itemsChoosed.ItemsSource = SelectItems;
            }

        }

        private void DeleteTags_Click(object sender, RoutedEventArgs e)
        {
          
            ValCommon1 val = (sender as Button).DataContext as ValCommon1;
            if(val != null)
            {
                this.SelectItems.Remove(val);
            }
            initialCheck();

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResultAction != null)
                this.ResultAction(true);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if(this.ResultAction != null)
            {
                this.ResultAction(false);
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk == null)
                return;

            Val pVal2 = chk.DataContext as Val;
            if ((bool)chk.IsChecked)
            {
                pVal2.isChoose = true;
                if (this.SelectItems.Count < 3)
                {
                    this.SelectItems.Add(new ValCommon1() { name = pVal2.name, code = pVal2.code });
                    this.itemsChoosed.ItemsSource = SelectItems;
                }
            }
            else
            {
                pVal2.isChoose = false;
                foreach(var item in this.SelectItems)
                {
                    if (item.code == pVal2.code)
                    {
                        this.SelectItems.Remove(item);
                        this.itemsChoosed.ItemsSource = SelectItems;
                        break;
                    }

                }
               
            }



        }

        private void CheckBox2_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk == null)
                return;

            Val2 pVal2 = chk.DataContext as Val2;
            if ((bool)chk.IsChecked)
            {
                
                if (this.SelectItems.Count < 3)
                {
                    pVal2.isChoose = true;
                    this.SelectItems.Add(new ValCommon1() { name = pVal2.name, code = pVal2.code });
                    this.itemsChoosed.ItemsSource = SelectItems;
                }
                else
                {
                    chk.IsChecked = false;
                }
            }
            else
            {
                pVal2.isChoose = false;
                foreach (var item in this.SelectItems)
                {
                    if (item.code == pVal2.code)
                    {
                        this.SelectItems.Remove(item);
                        this.itemsChoosed.ItemsSource = SelectItems;
                        break;
                    }

                }

            }
        }
    }
}
