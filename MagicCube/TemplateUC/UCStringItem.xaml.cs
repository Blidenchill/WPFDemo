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
    /// MenuItem.xaml 的交互逻辑
    /// </summary>
    public partial class UCStringItem : UserControl
    {
        public UCStringItem()
        {
            InitializeComponent();
            this.DataContextChanged += UCStringItem_DataContextChanged;
        }
        public string TextColor
        {
            get { return (string)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(string), typeof(UCStringItem), new PropertyMetadata(null));
        public string RectangleColor
        {
            get { return (string)GetValue(RectangleColorProperty); }
            set { SetValue(RectangleColorProperty, value); }
        }

        public static readonly DependencyProperty RectangleColorProperty =
            DependencyProperty.Register("RectangleColor", typeof(string), typeof(UCStringItem), new PropertyMetadata(null));
        void UCStringItem_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            List<string> lstStr = this.DataContext as List<string>;
           
            if(lstStr!=null)
            {
                if(lstStr.Count>0)
                {
                    List<StringItem> lstStringItem= new List<StringItem>();
                    int i = 1;
                    foreach(string istr in lstStr)
                    {
                        StringItem pStringItem = new StringItem();
                        pStringItem.ItemContent = istr;
                        if(i==lstStr.Count())
                        {
                            pStringItem.ItemVisiblity = Visibility.Collapsed;
                        }
                        else
                        {
                            pStringItem.ItemVisiblity = Visibility.Visible;
                        }
                        i++;
                        lstStringItem.Add(pStringItem);
                    }

                    icContent.ItemsSource = lstStringItem;
                    icContent.Visibility = Visibility.Visible;
                    tb.Visibility = Visibility.Collapsed;
                }
                else
                {
                    icContent.Visibility = Visibility.Collapsed;
                    tb.Visibility = Visibility.Visible;
                }
            }
            else
            {
                icContent.Visibility = Visibility.Collapsed;
                tb.Visibility = Visibility.Visible;
            }
        }
    }

    public class StringItem
    {
        public string ItemContent
        {
            get;
            set;
        }
        public Visibility ItemVisiblity
        {
            get;
            set;
        }
    }
}
