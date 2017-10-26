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
    public class DateTimeToStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = string.Empty;
            DateTime dt = (DateTime)value;
            DateTime nowDate = DateTime.Now;
            if(dt.Year >= nowDate.Year)
            {
                if(dt.Month >= nowDate.Month)
                {
                    if(dt.Day == nowDate.Day)
                    {
                        return dt.ToString("HH:mm");
                    }
                    else if(dt.Day +1 == nowDate.Day)
                    {
                        temp = "昨天";
                        return temp;
                    }
                }
                return dt.ToString("MM-dd");
            }
            return dt.ToString("yyyy-MM-dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class DateTimeToFormatConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = string.Empty;
            DateTime dt = (DateTime)value;
            return dt.ToString("yyyy-MM-dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
