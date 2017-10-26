using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;

namespace MagicCube.BindingConvert
{
    /// <summary>
    /// 如果有5个标记了就不再显示增加按钮
    /// </summary>
    public class ListCount5ToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //IEnumerable<object> list = value as IEnumerable<object>;
                int count = (int)value;
                if (count == 5)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
         
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
    public class ListCount5ToCollapseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //IEnumerable<object> list = value as IEnumerable<object>;
                int count = (int)value;
                if (count == 5)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
            catch
            {
                return Visibility.Visible;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    public class ListCount0ToCollapseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //IEnumerable<object> list = value as IEnumerable<object>;
                int count = (int)value;
                if (count == 0)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
            catch
            {
                return Visibility.Visible;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    public class ListCount0ToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //IEnumerable<object> list = value as IEnumerable<object>;
                int count = (int)value;
                if (count == 0)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    public class ListCount1ToUnVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //IEnumerable<object> list = value as IEnumerable<object>;

                //if (list == null)
                //{
                //    count = 0;
                //}
                //else
                //    count = list.Count();
                //int count = (int)value;
                int count = (int)value;
                if (count <= 1)
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;
            }
            catch
            {
                return Visibility.Visible;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    public class ListCountNegativeToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //IEnumerable<object> list = value as IEnumerable<object>;
                int count = (int)value;
                if (count == -1)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


}
