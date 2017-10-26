using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using MagicCube.Common;

namespace MagicCube.BindingConvert 
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = string.Empty;
            if (value == null)
                return string.Empty;
            long time = (long)value;
            DateTime ago = new DateTime(1970, 1, 1);

            DateTime dt = ago.AddTicks(time * 10000);
            DateTime nowDate = DateTime.Now;

            return TimeHelper.ResumeTableTime(dt);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
   }
}
