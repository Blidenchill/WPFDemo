using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MagicCube.Common
{
    public static class VTHelper
    {
        public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        public static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }

        //目前用于Linq结果转ObservableCollection
        public static ObservableCollection<T> ConvertLinq<T>(IEnumerable<T> original)
        {
            return new ObservableCollection<T>(original);
        }

        public static string Phone(string phonenum)
        {
            if (phonenum == null)
                return string.Empty;
            string result = string.Empty;
            if (phonenum.Length == 11)
            {
                result = phonenum.Insert(3, "****").Remove(7, 4);
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

    }
}
