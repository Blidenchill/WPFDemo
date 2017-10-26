using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;


namespace MagicCube.BindingConvert
{
    public class CompanyLogoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string avatarUrl = value as string;
            if (string.IsNullOrEmpty(avatarUrl))
            {
                return "/MagicCube;component/Resources/Images/companydefult.png";
            }
            if (!avatarUrl.StartsWith("http"))
            {
                return "/MagicCube;component/Resources/Images/companydefult.png";
            }
            return avatarUrl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
