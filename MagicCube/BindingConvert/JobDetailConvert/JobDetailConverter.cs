using MagicCube.Common;
using MagicCube.HttpModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace MagicCube.BindingConvert
{
    public class JobDetailWorkYearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "经验不限";
            string name = value.ToString();
            if (name != null)
            {
                name = MinExpHelper.GetName(name);
                if (name == "不限")
                {
                    return "经验不限";
                }
            }
            return name;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class JobDetailDegreeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "学历不限";
            string name = value.ToString();
            if (name != null)
            {
                name = MinDegreeHelper.GetName(name);
                if (name == "不限")
                {
                    return "学历不限";
                }
            }
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class JobDetailCharactConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string name = value.ToString();
            if (name != null)
            {
                name = workCharactHelper.GetName(name);
            }
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class JobDetailCityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;
            string name = value.ToString();
            if (name != null)
            {
                name = CityCodeHelper.GetCityName(name);
            }
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class JobDetailTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string time = value.ToString();
            if (time != null)
            {
                return time;
            }
            else
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
