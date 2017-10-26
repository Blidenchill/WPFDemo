using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using MagicCube.Model;

namespace MagicCube.BindingConvert
{
    public class InterviewWaitingOKToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            InterviewProcess temp = (InterviewProcess)value;
            if (temp == InterviewProcess.WaitingOK)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
         
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class InterviewCheckInToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            InterviewProcess temp = (InterviewProcess)value;
            if (temp == InterviewProcess.CheckIn)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class InterviewToVisibilityConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            InterviewProcess temp = (InterviewProcess)value;
            if (temp == InterviewProcess.Interview)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
