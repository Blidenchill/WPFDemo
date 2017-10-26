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
using System.Globalization;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCHintComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class UCHintComboBox : ComboBox
    {
        public UCHintComboBox()
        {
            InitializeComponent();
        }
        public string hintText
        {
            get { return (string)GetValue(hintTextProperty); }
            set { SetValue(hintTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for hintText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty hintTextProperty =
            DependencyProperty.Register("hintText", typeof(string), typeof(UCHintComboBox), new PropertyMetadata(null));

    }

    public class HintTextVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string temp = value as string;
            if (temp == null)
            {
                return Visibility.Visible;
            }
            if (temp == string.Empty)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
