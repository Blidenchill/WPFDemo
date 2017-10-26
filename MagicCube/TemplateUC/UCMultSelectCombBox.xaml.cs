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

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCMultSelectCombBox.xaml 的交互逻辑
    /// </summary>
    public partial class UCMultSelectCombBox : UserControl
    {
        public UCMultSelectCombBox()
        {
            InitializeComponent();
        }

    



        public string SelectContent
        {
            get { return (string)GetValue(SelectContentProperty); }
            set { SetValue(SelectContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectContentProperty =
            DependencyProperty.Register("SelectContent", typeof(string), typeof(UCMultSelectCombBox), new PropertyMetadata(null));



        public List<string> SelectItemsList
        {
            get { return (List<string>)GetValue(SelectItemsListProperty); }
            set { SetValue(SelectItemsListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectItemsList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectItemsListProperty =
            DependencyProperty.Register("SelectItemsList", typeof(List<string>), typeof(UCMultSelectCombBox), new PropertyMetadata(null));





        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            string temp = string.Empty;
            foreach (UIElement element in this.stkChkList.Children)
            {
                if(element is CheckBox)
                {
                   if((bool)(element as CheckBox).IsChecked)
                    {
                        temp += (element as CheckBox).Content + "、";
                    }
                }
            }
            this.SelectContent = temp.TrimEnd(new char[] { '、' });
        }

        private void WhiteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
