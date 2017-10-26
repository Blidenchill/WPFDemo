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
    public class WindowStateConverter :  IValueConverter
    {
         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WindowState temp = (WindowState)value;
            bool isCheck = false;
             switch(temp)
             {
                 case WindowState.Normal:
                     isCheck = true;
                     break;
                 case WindowState.Maximized:
                     isCheck = false;
                     break;
             }
             return isCheck;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
