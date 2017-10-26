
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Controls;
using MagicCube.HttpModel;
using MagicCube.Model;

namespace MagicCube.BindingConvert
{

    public class GrayTagColorConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString() == "True")
                {
                    return "#b4b4b4";
                }
                else
                {
                    return "#303342";
                }
            }
            else
                return "#303342";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
