using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using System.Windows;

namespace MagicCube.BindingConvert
{
    public class GenderToUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return AppDomain.CurrentDomain.BaseDirectory + "UserDefault.png";
            if (value.ToString() == "0")
            {
                return AppDomain.CurrentDomain.BaseDirectory + "UserDefaultFemale.png";
            }
            else if(value.ToString() == "女")
            {
                return AppDomain.CurrentDomain.BaseDirectory + "UserDefaultFemale.png";
            }
            else
            {
                return AppDomain.CurrentDomain.BaseDirectory + "UserDefault.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
