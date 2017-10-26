using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using MagicCube.HttpModel;

namespace MagicCube.BindingConvert
{
    public class CompanyProvinceToCityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HttpProvinceCityCodes temp = value as HttpProvinceCityCodes;
            if (temp == null)
                return null;
            return temp.city;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class CompanyCityToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HttpCityCodes temp = value as HttpCityCodes;
            if (temp == null)
                return null;
            return temp.name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


}
