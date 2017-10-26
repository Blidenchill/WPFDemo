using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace MagicCube.BindingConvert
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> p = value as List<string>;

            if (p != null)
            {
                if (p.Count > 0)
                {
                    string t = p[0];
                    for (int i = 1; i < p.Count; i++)
                    {
                        t += "/" + p[i];
                    }
                    return t;
                }
                else
                {
                    return "不限";
                }
            }
            else
            {
                string p2 = value as string;
                if (p2 != null)
                    return p2;
                else
                    return "不限";
            }
                


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
