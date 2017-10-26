using MagicCube.Common;
using MarkupConverter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace MagicCube.BindingConvert
{
    public class HTML2RTFConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = value as string;
            if (temp == null)
                return null;
            try
            {
                return HtmlToRtfConverter.ConvertHtmlToRtf(temp.Replace("xa0;"," "));

            }
            catch
            {
                return temp.Replace("xa0;", " ");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = value as string;
            if (temp == null)
                return null;
            return RtfToHtmlConverter.ConvertRtfToHtml(temp);
        }
    }
}
