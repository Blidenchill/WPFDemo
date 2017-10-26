using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;


namespace MagicCube.BindingConvert
{
    public class URLToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string avatarUrl = value as string;
            if(string.IsNullOrEmpty(avatarUrl))
            {
                return "/MagicCube;component/Resources/Images/HRDefault.png";
            }
            if(!avatarUrl.StartsWith("http"))
            {
                return "/MagicCube;component/Resources/Images/HRDefault.png";
            }
            return avatarUrl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }



    public class URLToUploadImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string avatarUrl = value as string;
            if (string.IsNullOrEmpty(avatarUrl))
            {
                return "/MagicCube;component/Resources/ImageSingle/AddImage.png";
            }
            if (!avatarUrl.StartsWith("http"))
            {
                return "/MagicCube;component/Resources/ImageSingle/AddImage.png";
            }
            //if (avatarUrl == null)
            //    return "/MagicCube;component/Resources/Images/companydefult.png";
            //else if(avatarUrl == string.Empty)
            //    return "/MagicCube;component/Resources/Images/companydefult.png";
            return avatarUrl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class URLToUploadVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string avatarUrl = value as string;
            if (string.IsNullOrEmpty(avatarUrl))
            {
                return System.Windows.Visibility.Collapsed;
            }
            if (!avatarUrl.StartsWith("http"))
            {
                return System.Windows.Visibility.Collapsed;
            }
            //if (avatarUrl == null)
            //    return "/MagicCube;component/Resources/Images/companydefult.png";
            //else if(avatarUrl == string.Empty)
            //    return "/MagicCube;component/Resources/Images/companydefult.png";
            return System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    

    
}
