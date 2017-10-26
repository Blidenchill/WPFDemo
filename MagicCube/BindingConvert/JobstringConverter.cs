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
    public class JobstringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as HttpJob) != null)
            {
                string jobname = (value as HttpJob).jobName;
                if (jobname != null)
                {
                    if (jobname.ToString() == "不限")
                    {
                        return "投递职位";
                    }
                    else
                    {
                        if (jobname.ToString().Length > 5)
                        {
                            return jobname.ToString().Substring(0, 4) + "...";
                        }
                        else
                        {
                            return jobname;
                        }
                    }
                }
                else
                {
                    return "投递职位";
                }
            }
            else
            {
                return "投递职位";
            }
         
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
